using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.DotNet.Reactive
{
    using Models;
    using Models.Converters;
    using Models.Events;

    /// <summary>
    ///		<see cref="IObservable{T}"/>-related extension methods for the Docker API client.  
    /// </summary>
    public static class DockerReactiveExtensions
	{
		/// <summary>
        ///		Default JSON serialisation settings for parsing event data. 
        /// </summary>
		static readonly JsonSerializerSettings EventSerializerSettings = new JsonSerializerSettings
        {
			Converters =
			{
				new DockerEventConverter(),
				new DockerDateConverter(),
                new StringEnumConverter()
			}
		};

		/// <summary>
        ///		Observe events from the docker API.
        /// </summary>
        /// <param name="client">
		/// 	The Docker miscellaneous operations API. 
		/// </param>
        /// <param name="parameters">
		/// 	Optional <see cref="ContainerEventsParameters"/> that control the events returned by the API.
		/// </param>
        /// <returns>
		/// 	A sequence of <see cref="DockerEvent"/>s representing the events.
		/// </returns>
		public static IObservable<DockerEvent> ObserveEvents(this IMiscellaneousOperations client, ContainerEventsParameters parameters = null)
		{
			return client.ObserveEventsRaw(parameters).Select(rawEventData =>
				JsonConvert.DeserializeObject<DockerEvent>(rawEventData, EventSerializerSettings)
			);
		}

		/// <summary>
        ///		Observe parsed JSON representing events from the docker API.
        /// </summary>
        /// <param name="client">
		/// 	The Docker miscellaneous operations API. 
		/// </param>
        /// <param name="parameters">
		/// 	Optional <see cref="ContainerEventsParameters"/> that control the events returned by the API.
		/// </param>
        /// <returns>
		/// 	A sequence of <see cref="JObject"/>s containing event data.
		/// </returns>
		public static IObservable<JObject> ObserveEventsJson(this IMiscellaneousOperations client, ContainerEventsParameters parameters = null)
		{
			return client.ObserveEventsRaw(parameters).Select(rawEventData =>
				JsonConvert.DeserializeObject<JObject>(rawEventData, EventSerializerSettings)
			);
		}

		/// <summary>
        ///		Observe raw JSON representing events from the docker API.
        /// </summary>
        /// <param name="client">
		/// 	The Docker miscellaneous operations API. 
		/// </param>
        /// <param name="parameters">
		/// 	Optional <see cref="ContainerEventsParameters"/> that control the events returned by the API.
		/// </param>
        /// <returns>
		/// 	A sequence of strings containing JSON event data.
		/// </returns>
		public static IObservable<string> ObserveEventsRaw(this IMiscellaneousOperations client, ContainerEventsParameters parameters = null)
		{
			if (parameters == null)
				parameters = new ContainerEventsParameters();

			return Observable.Create<string>(async (subscriber, cancellation) =>
			{
				StreamReader reader = new StreamReader(
					await client.MonitorEventsAsync(parameters, cancellation)
				);
				using (reader)
				{
					cancellation.Register(
						() => reader.Dispose() // Close the connection when the subscription is terminated.
					);

					await StreamReadLinePumpAsync(reader, subscriber, cancellation);
				}

				return Disposable.Empty; // Unused; disposal is handled by the outer cancellation from Observable.Create.
			});
		}

		/// <summary>
        /// 	Asynchronously read lines from a stream publish them to a subscriber.
        /// </summary>
        /// <param name="streamReader">
		/// 	The <see cref="StreamReader"/> used to read lines from the stream.
		/// </param>
        /// <param name="subscriber">
		///		The subscriber that the lines will be published to.
		/// </param>
        /// <param name="cancellation">
		/// 	A <see cref="CancellationToken"/> that can be used to cancel the operation.
		/// </param>
        /// <returns>
		/// 	A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
		static async Task StreamReadLinePumpAsync(StreamReader streamReader, IObserver<string> subscriber, CancellationToken cancellation)
		{
			await subscriber.PublishAndCompleteAsync(async () =>
			{
				while (!streamReader.EndOfStream && !cancellation.IsCancellationRequested)
				{
					try
					{
						string currentLine = await streamReader.ReadLineAsync();
						if (currentLine == null)
							break;

						subscriber.OnNext(currentLine);
					}
					catch (EndOfStreamException)
					{
						break; // Either way, we're done here.
					}
				}
			});
		}

		/// <summary>
        /// 	Asynchronously publish notifications to a subscriber, then complete the subscription.
        /// </summary>
		/// <typeparam name="T">
		/// 	The type of element in notification stream. 
		/// </typeparam>
        /// <param name="subscriber">
		/// 	The subscriber that will receive notifications.
		/// </param>
        /// <param name="publishAsync">
		/// 	The asynchronous delegate that publishes the notifications.
		/// </param>
        /// <returns>
		/// 	A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
		static async Task PublishAndCompleteAsync<T>(this IObserver<T> subscriber, Func<Task> publishAsync)
		{
			try
			{
				await publishAsync();
			}
			catch (Exception exception)
			{
				subscriber.OnError(exception);
			}
			finally
			{
				subscriber.OnCompleted();
			}
		}
	}
}
