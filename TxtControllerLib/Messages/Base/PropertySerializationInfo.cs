using System;
using System.IO;

namespace RoboticsTxt.Lib.Messages.Base
{
    internal class PropertySerializationInfo
    {
        public PropertySerializationInfo(string name, Action<Stream> writeValue)
        {
            this.Name = name;
            this.WriteValue = writeValue;
        }

        public string Name { get; }

        public Action<Stream> WriteValue { get; }
    }
}