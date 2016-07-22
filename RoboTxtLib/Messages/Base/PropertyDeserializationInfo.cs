using System;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public class PropertyDeserializationInfo
    {
        public PropertyDeserializationInfo(string name, Action<DeserializationContext> readValue)
        {
            Name = name;
            ReadValue = readValue;
        }

        public string Name { get; }

        public Action<DeserializationContext> ReadValue { get; }
    }
}