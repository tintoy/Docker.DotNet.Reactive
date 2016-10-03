using System;

namespace Docker.DotNet.Reactive
{
	/// <summary>
    ///		The reactive version of the Docker client. 
    /// </summary>
	public struct DockerClientReactive
		: IDockerClientReactive
	{
		/// <summary>
        ///		The underlying Docker client. 
        /// </summary>
		readonly IDockerClient _dockerClient;

		/// <summary>
        ///		Create a new reactive Docker client. 
        /// </summary>
        /// <param name="dockerClient">
		/// 	The underlying Docker client.
		/// </param>
		public DockerClientReactive(IDockerClient dockerClient)
		{
			if (dockerClient == null)
				throw new ArgumentNullException(nameof(dockerClient));

			_dockerClient = dockerClient;
		}

		/// <summary>
        /// 	The reactive version of the Docker miscellaneous operations API.
        /// </summary>
		public IMiscellaneousOperationsReactive Miscellaneous => new MiscellaneousOperationsReactive(_dockerClient.Miscellaneous);
	}
}