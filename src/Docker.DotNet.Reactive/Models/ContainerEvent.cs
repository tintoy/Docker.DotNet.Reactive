using Newtonsoft.Json;
using System;

namespace Docker.DotNet.Models
{
	/// <summary>
    /// 	Represents an event relating to a container. 
    /// </summary>
	public abstract class ContainerEvent
		: DockerEvent
	{
		/// <summary>
        ///		Initialise the <see cref="ContainerEvent"/>. 
        /// </summary>
		protected ContainerEvent()
		{
		}

		/// <summary>
		/// 	The name of the container that the event relates to.
		/// </summary>
		[JsonIgnore]
		public string Name => GetActorAttribute("name");

		/// <summary>
		/// 	The image used to the container that the event relates to.
		/// </summary>
		[JsonIgnore]
		public string Image => GetActorAttribute("image");
		
		/// <summary>
        ///		The type of entity (e.g. image, container, etc) that the event relates to. 
        /// </summary>
		[JsonProperty("Type")]
		public sealed override DockerEventTarget TargetType => DockerEventTarget.Container;
	}

	/// <summary>
    ///		Event raised when a container is created. 
    /// </summary>
	public class ContainerCreated
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerCreated"/> event model. 
        /// </summary>
		public ContainerCreated()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Create;
	}

	/// <summary>
    ///		Event raised when a container is started. 
    /// </summary>
	public class ContainerStarted
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerStarted"/> event model. 
        /// </summary>
		public ContainerStarted()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Start;
	}

	/// <summary>
    ///		Event raised when a container is stopped. 
    /// </summary>
	public class ContainerStopped
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerStopped"/> event model. 
        /// </summary>
		public ContainerStopped()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Stop;
	}

	/// <summary>
    ///		Model for the event raised when a container has been terminated. 
    /// </summary>
	public class ContainerDied
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerDied"/> event model. 
        /// </summary>
		public ContainerDied()
		{
		}

		/// <summary>
        ///		Does container exit code represent success (i.e. is 0)?
        /// </summary>
		public bool IsSuccessExitCode => ExitCode == Byte.MinValue;

		/// <summary>
        ///		The container exit code.
        /// </summary>
		/// <remarks>
        ///		Will be <see cref="Byte.MaxValue"/> if the exit code is not available.
        /// </remarks>
		public byte ExitCode
		{
			get
			{
				byte exitCode;
				if (!Byte.TryParse(GetActorAttribute("exitCode"), out exitCode))
					return Byte.MaxValue;

				return exitCode;
			}
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Die;
	}

	/// <summary>
    ///		Model for the event raised when a container has been destroyed. 
    /// </summary>
	public class ContainerDestroyed
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerDestroyed"/> event model. 
        /// </summary>
		public ContainerDestroyed()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Destroy;
	}

	/// <summary>
    ///		Model for the event raised when a container is resizeed. 
    /// </summary>
	public class ContainerResized
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerResized"/> event model. 
        /// </summary>
		public ContainerResized()
		{
		}

		/// <summary>
        ///		The new container "height".
        /// </summary>
		public long Height
		{
			get
			{
				long height;
				if (!Int64.TryParse(GetActorAttribute("height"), out height))
					return Int64.MinValue;

				return height;
			}
		}

		/// <summary>
        ///		The new container "width".
        /// </summary>
		public long Width
		{
			get
			{
				long width;
				if (!Int64.TryParse(GetActorAttribute("width"), out width))
					return Int64.MinValue;

				return width;
			}
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Resize;
	}

	/// <summary>
    ///		Model for the event raised when a container is attached to a TTY. 
    /// </summary>
	public class ContainerAttached
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerAttached"/> event model. 
        /// </summary>
		public ContainerAttached()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Attach;
	}

	/// <summary>
    ///		Model for the event raised when a container is detached from a TTY. 
    /// </summary>
	public class ContainerDetached
		: ContainerEvent
	{
		/// <summary>
        ///		Create a new <see cref="ContainerDetached"/> event model. 
        /// </summary>
		public ContainerDetached()
		{
		}

		/// <summary>
        /// 	The event type.
        /// </summary>
		[JsonProperty("status")]
		public override DockerEventType EventType => DockerEventType.Detach;
	}
}
