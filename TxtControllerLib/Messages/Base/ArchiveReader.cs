using System;
using System.Text;

namespace RoboticsTxt.Lib.Messages.Base
{
    internal class ArchiveReader
    {
        public static uint ReadUInt32(byte[] bytes, ref int currentPosition)
        {
            var value = BitConverter.ToUInt32(bytes, currentPosition);
            currentPosition += 4;
            return value;
        }

        public static uint ReadUInt32(DeserializationContext deserializationContext)
        {
            var value = BitConverter.ToUInt32(deserializationContext.Buffer, deserializationContext.CurrentPosition);
            deserializationContext.CurrentPosition += 4;
            return value;
        }

        public static ushort ReadUInt16(byte[] bytes, ref int currentPosition)
        {
            var value = BitConverter.ToUInt16(bytes, currentPosition);
            currentPosition += 2;
            return value;
        }

        public static short ReadInt16(DeserializationContext deserializationContext)
        {
            var value = BitConverter.ToInt16(deserializationContext.Buffer, deserializationContext.CurrentPosition);
            deserializationContext.CurrentPosition += 2;
            return value;
        }

        public static short[] ReadInt16(DeserializationContext deserializationContext, int byteCount)
        {
            var result = new short[byteCount];

            for (int i=0; i<byteCount; i++)
            {
                var value = BitConverter.ToInt16(deserializationContext.Buffer, deserializationContext.CurrentPosition);
                result[i] = value;
                deserializationContext.CurrentPosition += 2;
            }
            return result;
        }

        public static string ReadString(byte[] bytes, int lengthOfString, ref int currentPosition)
        {
            var text = new string(Encoding.ASCII.GetChars(bytes, currentPosition, 16));
            currentPosition += lengthOfString;
            return text.TrimEnd('\0');
        }

        public static string ReadString(DeserializationContext deserializationContext, int lengthOfString)
        {
            var text = new string(Encoding.ASCII.GetChars(deserializationContext.Buffer, deserializationContext.CurrentPosition, 16));
            deserializationContext.CurrentPosition += lengthOfString;
            return text.TrimEnd('\0');
        }

        public static string ReadString(byte[] bytes, ref int currentPosition)
        {
            currentPosition += 3;
            var numberOfCharacters = bytes[currentPosition];
            currentPosition++;
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < numberOfCharacters; i++)
            {
                var c = BitConverter.ToChar(bytes, currentPosition);
                currentPosition += 2;
                stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }

        public static char ReadChar(byte[] bytes, ref int currentPosition)
        {
            var c = BitConverter.ToChar(bytes, currentPosition);
            currentPosition += 2;
            return c;
        }

        public static string ReadNullTerminatedString(byte[] bytes, ref int currentPosition, int maxStringLength = -1)
        {
            var startIndex = currentPosition;

            var stringBuilder = new StringBuilder();
            while (true)
            {
                if (currentPosition >= bytes.Length - 2)
                {
                    return null;
                }

                var c = BitConverter.ToChar(bytes, currentPosition);
                currentPosition += 2;

                if (c == 0 && BitConverter.ToChar(bytes, currentPosition) == 0)
                {
                    currentPosition += 2;
                    break;
                }
                stringBuilder.Append(c);
            }

            if (maxStringLength > 0)
            {
                currentPosition = startIndex + (maxStringLength + 1) * 2;
            }

            return stringBuilder.ToString();
        }

        public static Version ReadVersion(byte[] bytes, ref int currentPosition)
        {
            var version = new Version(bytes[currentPosition + 3], bytes[currentPosition + 2], bytes[currentPosition + 1],
                bytes[currentPosition]);
            currentPosition += 4;
            return version;
        }

        public static Version ReadVersion(DeserializationContext deserializationContext)
        {
            var version = new Version(
                deserializationContext.Buffer[deserializationContext.CurrentPosition + 3],
                deserializationContext.Buffer[deserializationContext.CurrentPosition + 2],
                deserializationContext.Buffer[deserializationContext.CurrentPosition + 1],
                deserializationContext.Buffer[deserializationContext.CurrentPosition]);
            deserializationContext.CurrentPosition += 4;
            return version;
        }
    }
}