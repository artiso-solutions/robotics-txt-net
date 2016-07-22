using System;
using System.IO;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public class ArchiveWriter
    {
        public static void WriteUInt32(Stream stream, uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteUInt16(Stream stream, ushort[] values)
        {
            foreach (var value in values)
            {
                var buffer = BitConverter.GetBytes(value);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void WriteUInt16(Stream stream, ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteString(Stream stream, string value, int length)
        {
            foreach (var c in value)
            {
                var buffer = BitConverter.GetBytes(c);
                stream.Write(buffer, 0, buffer.Length);
            }

            stream.Write(new byte[length - value.Length], 0, length - value.Length);
        }

        public static void WriteAppendedString(Stream stream, string value)
        {
            // Trenner
            WriteUInt16(stream, 0);

            // String Inhalt
            foreach (var c in value)
            {
                var buffer = BitConverter.GetBytes(c);
                stream.Write(buffer, 0, buffer.Length);
            }

            // Null Terminierung
            WriteUInt16(stream, 0);
        }

        public static void WriteNullTerminatedString(Stream stream, string value, int maxStringLength)
        {
            if (value.Length > maxStringLength)
            {
                value = value.Substring(0, maxStringLength);
            }

            foreach (var c in value)
            {
                var buffer = BitConverter.GetBytes(c);
                stream.Write(buffer, 0, buffer.Length);
            }

            // Null Terminierung
            WriteUInt16(stream, 0);

            // Max Length auffüllen
            for (var i = value.Length; i < maxStringLength; i++)
            {
                WriteUInt16(stream, 0);
            }
        }

        public static void WriteBytes(Stream stream, params byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}