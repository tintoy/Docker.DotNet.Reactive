namespace Docker.DotNet
{
	using Reactive;

	/// <summary>
    ///		Extension methods for <see cref="IDockerClient"/>. 
    /// </summary>
	public static class DockerClientExtensions
	{
		/// <summary>
        ///		Get the reactive version of the Docker client. 
        /// </summary>
        /// <param name="client">
		/// 	The Docker client.
		/// </param>
        /// <returns>
		/// 	A <see cref="DockerClientReactive"/> representing the reactive Docker client.
		/// </returns>
		public static DockerClientReactive Reactive(this IDockerClient client)
		{
			return new DockerClientReactive(client);
		}

		/// <summary>
        ///		Get the reactive version of the Docker miscellaneous operations API. 
        /// </summary>
        /// <param name="miscellaneousOperations">
		/// 	The Docker miscellaneous operations API.
		/// </param>
        /// <returns>
		/// 	A <see cref="MiscellaneousOperationsReactive"/> representing the reactive Docker client.
		/// </returns>
		public static MiscellaneousOperationsReactive Reactive(this IMiscellaneousOperations miscellaneousOperations)
		{
			return new MiscellaneousOperationsReactive(miscellaneousOperations);
		}
	}
}