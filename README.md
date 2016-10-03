# Docker.DotNet.Reactive
Reactive Extensions (Rx) support for the .NET Docker client.

## Usage example

```csharp
DockerClient client =
	new DockerClientConfiguration(
		new Uri("unix:///var/run/docker.sock")
	)
	.CreateClient();

// You could also use ObserveEvents or ObserveEventsJson.
IObservable<string> allEvents =
	client.Miscellaneous.Reactive()
		.ObserveEventsRaw();

var subscription1 = allEvents.Subscribe(
	eventJson => Console.WriteLine("Subscription1: {0}", eventJson),
	error => Console.WriteLine("Subscription1: ERROR - {0}", error),
	() => Console.WriteLine("Subscription1: Completed")
);
Console.WriteLine("Subscription1: Running");

var subscription2 = allEvents.Subscribe(
	eventJson => Console.WriteLine("Subscription2: {0}", eventJson),
	error => Console.WriteLine("Subscription2: ERROR - {0}", error),
	() => Console.WriteLine("Subscription2: Completed")
);
Console.WriteLine("Subscription2: Running");

using (subscription1)
{
	using (subscription2)
	{
		Console.ReadLine();
	}
	Console.WriteLine("S2: Dispose");

	Console.ReadLine();
}
Console.WriteLine("S1: Dispose");
```

Go perform some actions in Docker and watch the events show up. Each call to `IObservable.Subscribe` will result in another call to the underlying Docker API client's `MonitorEventsAsync` method.

Disposing of the subscription will terminate that event stream.
