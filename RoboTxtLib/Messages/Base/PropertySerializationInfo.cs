using System;
using System.IO;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public class PropertySerializationInfo
    {
        public PropertySerializationInfo(string name, Action<Stream> writeValue)
        {
            Name = name;
            WriteValue = writeValue;
        }

        public string Name { get; }

        public Action<Stream> WriteValue { get; }
    }
}