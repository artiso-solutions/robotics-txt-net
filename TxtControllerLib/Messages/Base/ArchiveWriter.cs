using System;
using System.IO;

namespace RoboticsTxt.Lib.Messages.Base
{
    internal class ArchiveWriter
    {
        public static void WriteInt32(Stream stream, uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteInt32(Stream stream, int value)
        {
            var buffer = BitConverter.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteInt16(Stream stream, ushort[] values)
        {
            foreach (var value in values)
            {
                var buffer = BitConverter.GetBytes(value);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void WriteInt16(Stream stream, ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteInt16(Stream stream, short[] values)
        {
            foreach (var value in values)
            {
                var buffer = BitConverter.GetBytes(value);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void WriteInt16(Stream stream, short value)
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

            stream.Write(new byte[length - value.Length*2], 0, length - value.Length*2);
        }

        public static void WriteAppendedString(Stream stream, string value)
        {
            // Trenner
            WriteInt16(stream, (ushort)0);

            // String Inhalt
            foreach (var c in value)
            {
                var buffer = BitConverter.GetBytes(c);
                stream.Write(buffer, 0, buffer.Length);
            }

            // Null Terminierung
            WriteInt16(stream, (ushort)0);
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
            WriteInt16(stream, (ushort)0);

            // Max Length auffüllen
            for (var i = value.Length; i < maxStringLength; i++)
            {
                WriteInt16(stream, (ushort)0);
            }
        }

        public static void WriteBytes(Stream stream, params byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}