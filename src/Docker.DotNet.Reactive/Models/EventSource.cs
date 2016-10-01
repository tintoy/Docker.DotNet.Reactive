using System.Runtime.Serialization;

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

		/// <summary>
		///		Event relates to a container.
		/// </summary>
		[EnumMember(Value = "container")]
		Container,
		
		/// <summary>
		///		Event relates to an image.
		/// </summary>
		[EnumMember(Value = "image")]
		Image,

		/// <summary>
		///		Event relates to a network.
		/// </summary>
		[EnumMember(Value = "network")]
		Network,
		
		/// <summary>
		///		Event relates to a volume.
		/// </summary>
		[EnumMember(Value = "volume")]
		Volume
	}
}
