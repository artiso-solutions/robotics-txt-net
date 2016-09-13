using System;

namespace artiso.Fischertechnik.TxtController.Lib.Messages.Base
{
    public class PropertyDeserializationInfo
    {
        public PropertyDeserializationInfo(string name, Action<DeserializationContext> readValue)
        {
            this.Name = name;
            this.ReadValue = readValue;
        }

        public string Name { get; }

        public Action<DeserializationContext> ReadValue { get; }
    }
}