namespace Docker.DotNet.Reactive
{
	/// <summary>
    ///		Represents the reactive version of the Docker client. 
    /// </summary>
	public interface IDockerClientReactive
	{
		/// <summary>
        /// 	The reactive version of the Docker miscellaneous operations API.
        /// </summary>
		IMiscellaneousOperationsReactive Miscellaneous { get; }
	}
}