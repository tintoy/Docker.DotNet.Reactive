using Newtonsoft.Json;

namespace Docker.DotNet.Models
{
	/// <summary>
	/// 	Represents an event relating to a network. 
	/// </summary>
	public abstract class NetworkEvent
		: DockerEvent
	{
		/// <summary>
		///		Initialise the <see cref="NetworkEvent"/>. 
		/// </summary>
		protected NetworkEvent()
		{
		}

		/// <summary>
		/// 	The Id of the network that the event relates to.
		/// </summary>
		[JsonIgnore]
		public string Id => Actor.Id;

		/// <summary>
		///		The name of the network that the event relates to.
		/// </summary>
		[JsonIgnore]
		public string Name => GetActorAttribute("name");

		/// <summary>
		///		The type of network that the event relates to.
		/// </summary>
		[JsonIgnore]
		public string NetworkType => GetActorAttribute("type");

		/// <summary>
		///		The type of entity (e.g. image, container, etc) that the event relates to. 
		/// </summary>
		[JsonProperty("Type")]
		public override DockerEventTarget TargetType => DockerEventTarget.Network;
	}

	/// <summary>
	///		Model for the event raised when a container has been successfully connected to a network.
	/// </summary>
	public class NetworkConnected
		: ImageEvent
	{
		/// <summary>
		///		Create a new <see cref="NetworkConnected"/> event model.
		/// </summary>
		public NetworkConnected()
		{
		}

		/// <summary>
		///		The Id of the container that was connected to the network.
		/// </summary>
		[JsonIgnore]
		public string NetworkType => GetActorAttribute("type");

		/// <summary>
		///		The event type.
		/// </summary>
		public sealed override DockerEventType EventType => DockerEventType.Connect;
	}

	/// <summary>
	///		Model for the event raised when a container has been successfully disconnected from a network.
	/// </summary>
	public class NetworkDisconnected
		: ImageEvent
	{
		/// <summary>
		///		Create a new <see cref="NetworkDisconnected"/> event model.
		/// </summary>
		public NetworkDisconnected()
		{
		}

		/// <summary>
		///		The Id of the container that was disconnected from the network.
		/// </summary>
		[JsonIgnore]
		public string NetworkType => GetActorAttribute("type");

		/// <summary>
		///		The event type.
		/// </summary>
		public sealed override DockerEventType EventType => DockerEventType.Disconnect;
	}
}