using System;
using System.Threading;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main()
        {
			SynchronizationContext.SetSynchronizationContext(
				new SynchronizationContext()
			);
            try
			{
				DockerClient client = new DockerClientConfiguration(
					new Uri("unix:///var/run/docker.sock")
				).CreateClient();

				IObservable<string> obs = client.Miscellaneous.ObserveEventsRaw(
					new ContainerEventsParameters()
				);
				var subscription1 = obs.Subscribe(
					line => Console.WriteLine("S1: {0}", line),
					error => Console.WriteLine("S1: ERROR - {0}", error),
					() => Console.WriteLine("S1: Completed")
				);
				Console.WriteLine("S1: Running");

				var subscription2 = obs.Subscribe(
					line => Console.WriteLine("S2: {0}", line),
					error => Console.WriteLine("S2: ERROR - {0}", error),
					() => Console.WriteLine("S2: Completed")
				);
				Console.WriteLine("S2: Running");

				using (subscription1)
				{
					using (subscription2)
					{
						Console.ReadLine();
						Console.WriteLine("S2: Dispose");
					}

					Console.ReadLine();
					Console.WriteLine("S1: Dispose");
				}
			}
			catch (Exception unexpectedError)
			{
				Console.WriteLine(unexpectedError);
			}
        }
    }
}
