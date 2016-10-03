using Newtonsoft.Json.Linq;
using System;

namespace Docker.DotNet.Reactive
{
	using Models;

	/// <summary>
    ///		The reactive version of the Docker miscellaneous operations API. 
    /// </summary>
	public struct MiscellaneousOperationsReactive
		: IMiscellaneousOperationsReactive
	{
		readonly IMiscellaneousOperations _miscellaneousOperations;

		public MiscellaneousOperationsReactive(IMiscellaneousOperations miscellaneousOperations)
		{
			if (miscellaneousOperations == null)
				throw new ArgumentNullException(nameof(miscellaneousOperations));

			_miscellaneousOperations = miscellaneousOperations;
		}

		/// <summary>
        ///		Create an <see cref="IObservable{T}"/> that can be used to observe events.
        /// </summary>
        /// <param name="parameters">
		/// 	Optional <see cref="ContainerEventsParameters"/> that control the events returned by the API. 
		/// </param>
        /// <returns>
		/// 	An <see cref="IObservable{T}"/> sequence of <see cref="JObject"/>s representing the parsed JSON event data.
		/// </returns>
		public IObservable<JObject> ObserveEvents(ContainerEventsParameters parameters = null)
		{
			return _miscellaneousOperations.ObserveEventsJson(parameters);
		}
	}
}