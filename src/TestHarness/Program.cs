using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace ConsoleApplication
{
	/// <summary>
	///		Quick-and-dirty test harness for reactive Docker API extensions.
	/// </summary>
	public class Program
    {
		/// <summary>
        ///		The global logger factory. 
        /// </summary>
		static readonly ILoggerFactory Loggers = new LoggerFactory().AddConsole();

		/// <summary>
        ///		The root logger for the test harness. 
        /// </summary>
		static readonly ILogger RootLogger = Loggers.CreateLogger("TestHarness");
		
		/// <summary>
        ///		The logger for the first subscription. 
        /// </summary>
		static readonly ILogger Sub1Logger = Loggers.CreateLogger("S1");
		
		/// <summary>
        ///		The logger for the second subscription. 
        /// </summary>
		static readonly ILogger Sub2Logger = Loggers.CreateLogger("S2");

		/// <summary>
        ///		The main program entry-point. 
        /// </summary>
		public static void Main()
        {
			SynchronizationContext.SetSynchronizationContext(
				new SynchronizationContext()
			);
            try
			{
				IConfiguration configuration = new ConfigurationBuilder()
					.AddJsonFile(
						Path.Combine(
							Directory.GetCurrentDirectory(),
							"appsettings.json"
						)
					)
					.Build();

				DockerClient client =
					new DockerClientConfiguration(
						new Uri(configuration["DockerApiEndPoint"])
					)
					.CreateClient();

				IObservable<JObject> obs = client.Reactive().Miscellaneous.ObserveEvents(
					new ContainerEventsParameters()
				);
				var subscription1 = obs.Subscribe(
					eventData => Sub1Logger.LogInformation("{EventData}", eventData.ToString(Formatting.Indented)),
					error => Sub1Logger.LogError("{Error}", error),
					() => Sub1Logger.LogInformation("Completed")
				);
				Sub1Logger.LogInformation("Running");

				var subscription2 = obs.Subscribe(
					eventData => Sub2Logger.LogInformation("{EventData}", eventData.ToString(Formatting.Indented)),
					error => Sub2Logger.LogError("{Error}", error),
					() => Sub2Logger.LogInformation("Completed")
				);
				Sub2Logger.LogInformation("Running");

				using (subscription1)
				{
					using (subscription2)
					{
						Console.ReadLine();
						Sub1Logger.LogInformation("Dispose");
					}

					Console.ReadLine();
					Sub2Logger.LogInformation("Dispose");
				}
			}
			catch (Exception unexpectedError)
			{
				RootLogger.LogError(
					new EventId(500),
					unexpectedError,
					"Unexpected error: {Error}", unexpectedError
				);
			}
        }
    }
}
