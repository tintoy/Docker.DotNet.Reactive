namespace Docker.DotNet.Models
{
	/// <summary>
    ///		Well-known event types used by the Docker API. 
    /// </summary>
	public enum EventType
	{
		/// <summary>
		///		An unknown event type.
		/// </summary>
		Unknown = 0,

		// TODO: Document these.
		
		Attach,
		Commit,
		Connect,
		Copy,
		Create,
		Delete,
		Destroy,
		Detach,
		Die,
		Disconnect,
		ExecCreate,
		ExecDetach,
		ExecStart,
		Export,
		HealthStatus,
		Import,
		Kill,
		Load,
		Mount,
		Oom,
		Pause,
		Pull,
		Push,
		Reload,
		Rename,
		Resize,
		Restart,
		Save,
		Start,
		Stop,
		Tag,
		Top,
		Unmount,
		Unpause,
		Untag,
		Update
	}
}