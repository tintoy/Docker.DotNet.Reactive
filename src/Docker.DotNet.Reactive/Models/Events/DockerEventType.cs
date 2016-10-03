using System.Runtime.Serialization;

namespace Docker.DotNet.Models.Events
{
	/// <summary>
    ///		Well-known event types used by the Docker API. 
    /// </summary>
	public enum DockerEventType
	{
		/// <summary>
		///		An unknown event type.
		/// </summary>
		Unknown = 0,

		// TODO: Document these.
		
		[EnumMember(Value = "attach")]
		Attach,

		[EnumMember(Value = "commit")]
		Commit,

		[EnumMember(Value = "connect")]
		Connect,

		[EnumMember(Value = "copy")]
		Copy,
		
		[EnumMember(Value = "create")]
		Create,
		
		[EnumMember(Value = "delete")]
		Delete,
		
		[EnumMember(Value = "destroy")]
		Destroy,
		
		[EnumMember(Value = "detach")]
		Detach,
		
		[EnumMember(Value = "die")]		
		Die,
		
		[EnumMember(Value = "disconnect")]
		Disconnect,
		
		[EnumMember(Value = "exec_create")]
		ExecCreate,
		
		[EnumMember(Value = "exec_detach")]
		ExecDetach,
		
		[EnumMember(Value = "exec_start")]
		ExecStart,
		
		[EnumMember(Value = "export")]
		Export,
		
		[EnumMember(Value = "health_status")]
		HealthStatus,
		
		[EnumMember(Value = "import")]
		Import,
		
		[EnumMember(Value = "kill")]		
		Kill,
		
		[EnumMember(Value = "load")]
		Load,
		
		[EnumMember(Value = "mount")]
		Mount,
		
		[EnumMember(Value = "oom")]
		OutOfMemory,
		
		[EnumMember(Value = "pause")]
		Pause,
		
		[EnumMember(Value = "pull")]
		Pull,
		
		[EnumMember(Value = "push")]
		Push,
		
		[EnumMember(Value = "reload")]
		Reload,
		
		[EnumMember(Value = "rename")]
		Rename,
		
		[EnumMember(Value = "resize")]
		Resize,
		
		[EnumMember(Value = "restart")]
		Restart,
		
		[EnumMember(Value = "save")]
		Save,
		
		[EnumMember(Value = "start")]		
		Start,
		
		[EnumMember(Value = "stop")]
		Stop,
		
		[EnumMember(Value = "tag")]
		Tag,
		
		[EnumMember(Value = "top")]		
		Top,
		
		[EnumMember(Value = "unmount")]
		Unmount,
		
		[EnumMember(Value = "unpause")]
		Unpause,
		
		[EnumMember(Value = "untag")]
		Untag,
		
		[EnumMember(Value = "update")]
		Update
	}
}
