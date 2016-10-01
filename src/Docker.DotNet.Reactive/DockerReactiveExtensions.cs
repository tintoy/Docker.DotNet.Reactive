using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Docker.DotNet
{
    using Models;
    using Models.Converters;

    /// <summary>
    ///		<see cref="IObservable{T}"/>-related extension methods for the Docker API client.  
    /// </summary>
    public static class DockerReactiveExtensions
	{
		/// <summary>
        ///		Default JSON serialisation settings for parsing event data. 
        /// </summary>
		static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
			Converters =
			{
				new DockerDateConverter(),
                new StringEnumConverter()
			}
		};

		// TODO: Implement JSON contracts (and custom converter) for event payloads.

		/// <summary>
        ///		Observe parsed JSON representing events from the docker API.
        /// </summary>
        /// <param name="client">
		/// 	The Docker miscellaneous operations API. 
		/// </param>
        /// <param name="parameters">
		/// 	<see cref="ContainerEventsParameters"/> that control the events returned by the API.
		/// </param>
        /// <returns>
		/// 	A sequence of <see cref="JObject"/>s containing event data.
		/// </returns>
		public static IObservable<JObject> ObserveEventsJson(this IMiscellaneousOperations client, ContainerEventsParameters parameters)
		{
			return client.ObserveEventsRaw(parameters).Select(rawEventData =>
				JsonConvert.DeserializeObject<JObject>(rawEventData, DefaultSerializerSettings)
			);
		}

		/// <summary>
        ///		Observe raw JSON representing events from the docker API.
        /// </summary>
        /// <param name="client">
		/// 	The Docker miscellaneous operations API. 
		/// </param>
        /// <param name="parameters">
		/// 	<see cref="ContainerEventsParameters"/> that control the events returned by the API.
		/// </param>
        /// <returns>
		/// 	A sequence of strings containing JSON event data.
		/// </returns>
		public static IObservable<string> ObserveEventsRaw(this IMiscellaneousOperations client, ContainerEventsParameters parameters)
		{
			return Observable.Create<string>(async (subscriber, cancellation) =>
			{
				StreamReader reader = new StreamReader(
					await client.MonitorEventsAsync(parameters, cancellation)
				);
				cancellation.Register(
					() => reader.Dispose() // Close the connection when the subscription is terminated.
				);

				while (!reader.EndOfStream && !cancellation.IsCancellationRequested)
				{
					string currentLine = null;
					try
					{
						currentLine = await reader.ReadLineAsync();
						if (currentLine == null)
							break;

						subscriber.OnNext(currentLine);
					}
					catch (EndOfStreamException)
					{
						break; // Either way, we're done here.
					}
					catch (Exception exception)
					{
						subscriber.OnError(exception);

						break;
					}
				}

				subscriber.OnCompleted();

				return Disposable.Empty; // Unused; disposal is handled by the outer cancellation from Observable.Create.
			});
		}
	}
}