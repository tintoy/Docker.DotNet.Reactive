using System;
using Newtonsoft.Json.Linq;

namespace Docker.DotNet.Models.Converters
{
	class DockerEventConverter
        : JsonCreationConverter<DockerEvent>
    {
        protected override DockerEvent Create(Type objectType, JObject json)
        {
			string eventTargetValue = (string)json.GetValue("Type");
			DockerEventTarget eventTarget;
			if (!Enum.TryParse(eventTargetValue, out eventTarget))
				throw new InvalidOperationException($"Unsupported event target ('Type' == '{eventTargetValue}').");

			string eventTypeValue = (string)json.GetValue("Type");
			DockerEventType eventType;
			if (!Enum.TryParse(eventTypeValue, out eventType))
				throw new InvalidOperationException($"Unsupported event type ('status' == '{eventTypeValue}').");

			DockerEvent dockerEvent;
			switch (eventTarget)
			{
				case DockerEventTarget.Container:
				{
					dockerEvent = CreateContainerEvent(eventType);

					break;
				}
				case DockerEventTarget.Image:
				{
					dockerEvent = CreateImageEvent(eventType);

					break;
				}
				default:
				{
					throw new InvalidOperationException($"Unsupported event target ({eventTarget}).");
				}
			}

			if (dockerEvent == null)
				throw new InvalidOperationException($"Unsupported event type ({eventType}).");

            return dockerEvent;
        }

		ContainerEvent CreateContainerEvent(DockerEventType eventType)
		{
			switch (eventType)
			{
				case DockerEventType.Create:
				{
					return new ContainerCreated();
				}
				default:
				{
					return null;
				}
			}
		}

		ImageEvent CreateImageEvent(DockerEventType eventType)
		{
			switch (eventType)
			{
				case DockerEventType.Pull:
				{
					return new ImagePulled();
				}
				case DockerEventType.Push:
				{
					return new ImagePushed();
				}
				default:
				{
					return null;
				}
			}
		}
    }
}