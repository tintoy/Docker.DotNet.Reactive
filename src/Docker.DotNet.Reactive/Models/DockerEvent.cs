using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Docker.DotNet.Models
{
	/// <summary>
    ///		The base model for Docker event data. 
    /// </summary>
	[JsonConverter(typeof(Converters.DockerEventConverter))]
	public abstract class DockerEvent
	{
		protected DockerEvent()
		{
		}

		/// <summary>
        /// 	The Id of the entity that the event relates to.
        /// </summary>
		[JsonProperty("id")]
		public string Target { get; set; }

		/// <summary>
        /// 	The type of entity (e.g. image, container, etc) that the event relates to.
        /// </summary>
		public abstract DockerEventTarget TargetType { get; }
		
		/// <summary>
        /// 	The event type.
        /// </summary>
		public abstract DockerEventType EventType { get; }

		/// <summary>
        /// 	Information about the entity that the event relates to. 
        /// </summary>
		[JsonProperty("Actor", ObjectCreationHandling = ObjectCreationHandling.Reuse)]
		public DockerEventActor Actor { get; } = new DockerEventActor();

		/// <summary>
        /// 	The UTC date / time that the event was raised.
        /// </summary>
		[JsonProperty("time")]
		public DateTime TimestampUTC { get; set; }

		/// <summary>
        ///		Get the value of the event actor attribute with the specified name. 
        /// </summary>
        /// <param name="name">
		/// 	The attribute name.
		/// </param>
        /// <returns>
		/// 	The attribute value (or <c>null</c>, if the attribute is not defined).
		/// </returns>
		protected string GetActorAttribute(string name)
		{
			string value;
			Actor.Attributes.TryGetValue(name, out value);

			return value;
		}
	}

	/// <summary>
    ///		Information about an entity that a Docker event relates to. 
    /// </summary>
	public class DockerEventActor
	{
		/// <summary>
        ///		The entity Id. 
        /// </summary>
		[JsonProperty("ID")]
		public string Id { get; set; }

		/// <summary>
        ///		The entity attributes (if any). 
        /// </summary>
		[JsonProperty("ID", ObjectCreationHandling = ObjectCreationHandling.Reuse)]
		public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
	}
}