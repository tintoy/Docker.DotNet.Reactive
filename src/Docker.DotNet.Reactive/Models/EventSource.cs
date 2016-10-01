namespace Docker.DotNet.Models
{
	/// <summary>
    ///		Represents a source of events for the Docker API. 
    /// </summary>
	public enum EventSource
	{
		/// <summary>
		///		An unknown event source.
		/// </summary>
		Unknown = 0,

		// TODO: Document these.

		Container,
		Image,
		Volume,
		Network
	}
}