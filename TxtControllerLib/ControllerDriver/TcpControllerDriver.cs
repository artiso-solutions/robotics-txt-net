using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using log4net;
using log4net.Util;
using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.ControllerDriver
{
    internal class TcpControllerDriver : IDisposable
    {
        private readonly IPAddress ipAddress;
        private readonly ILog logger = LogManager.GetLogger(typeof(TcpControllerDriver));
        private Socket socket;

        public TcpControllerDriver(IPAddress ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public void Dispose()
        {
            if (this.socket?.Connected == true)
            {
                this.socket.Close();
                this.socket.Dispose();
                this.socket = null;
            }
        }

        public void StartCommunication()
        {
            this.socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(this.ipAddress, 65000);
        }

        public void SendCommand<TCmdMessage>([NotNull] TCmdMessage command)
            where TCmdMessage : CommandMessage
        {
            var cmdBytes = this.GetBytesOfMessage(command);
            this.logger.DebugExt($"Sending {cmdBytes.Length} bytes");
            this.socket.Send(cmdBytes);

            using (var receiveStream = new MemoryStream())
            {
                var receiveBuffer = new byte[256];
                int length;
                do
                {
                    length = this.socket.Receive(receiveBuffer);
                    receiveStream.Write(receiveBuffer, 0, length);
                } while (length == receiveBuffer.Length);

                var receivedBytes = receiveStream.ToArray();
                if (receivedBytes.Length <= 0)
                {
                    throw new CommunicationFailedException("Failed to receive response");
                }

                var responseMessageId = this.GetMessageId(receivedBytes);
                if (command.ExpectedResponseId != responseMessageId)
                {
                    throw new CommunicationFailedException(
                        $"Did not receive expected respone id {command.ExpectedResponseId}. Received response message of type {responseMessageId.GetType().FullName} instead.");
                }
            }
        }

        [NotNull]
        public TResponseMessage SendCommand<TCmdMessage, TResponseMessage>([NotNull] TCmdMessage command)
            where TCmdMessage : CommandMessage
            where TResponseMessage : ResponseMessage, new()
        {
            var cmdBytes = this.GetBytesOfMessage(command);
            this.socket.Send(cmdBytes);

            using (var receiveStream = new MemoryStream())
            {
                var receiveBuffer = new byte[256];
                int length;
                do
                {
                    length = this.socket.Receive(receiveBuffer);
                    receiveStream.Write(receiveBuffer, 0, length);
                } while (length == receiveBuffer.Length);

                var receviedBytes = receiveStream.ToArray();
                if (receviedBytes.Length <= 0)
                {
                    throw new CommunicationFailedException("Failed to receive response");
                }

                var responseMessage = this.GetMessageOfBytes<TResponseMessage>(receviedBytes);
                if (command.ExpectedResponseId != responseMessage.CommandId)
                {
                    throw new CommunicationFailedException(
                        $"Did not receive expected respone id {command.ExpectedResponseId}. Received response message of type {responseMessage.GetType().FullName} instead.");
                }

                return (TResponseMessage)responseMessage;
            }
        }

        [NotNull]
        public byte[] GetBytesOfMessage([NotNull] CommandMessage message)
        {
            this.logger.DebugExt($"Serialize message of type {message.GetType().FullName}");
            using (var memoryStream = new MemoryStream())
            {
                long oldLength;
                long newLength;
                foreach (var propertySerializationInfo in message.SerializationProperties)
                {
                    oldLength = memoryStream.Length;
                    propertySerializationInfo.WriteValue(memoryStream);
                    newLength = memoryStream.Length;
                    this.logger.DebugExt(() => $"  {propertySerializationInfo.Name} -> {newLength - oldLength} added -> {string.Join("|", memoryStream.ToArray().Skip((int)oldLength))}");
                }

                return memoryStream.ToArray();
            }
        }

        private uint GetMessageId([NotNull] byte[] bytes)
        {
            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(bytes, ref startIndex);

            return responseId;
        }

        [NotNull]
        private ControllerMessage GetMessageOfBytes<TResponseMessage>([NotNull] byte[] bytes)
            where TResponseMessage : ResponseMessage, new()
        {
            var responseMessage = new TResponseMessage();
            this.logger.DebugExt($"Deserialize message of type {responseMessage.GetType().FullName}");
            var deserializationContext = new DeserializationContext(bytes);
            int oldPosition;
            int newPosition;
            foreach (var propertyDeserializationInfo in responseMessage.DeserializationProperties)
            {
                oldPosition = deserializationContext.CurrentPosition;
                propertyDeserializationInfo.ReadValue(deserializationContext);
                newPosition = deserializationContext.CurrentPosition;
                this.logger.DebugExt(() => $"  {propertyDeserializationInfo.Name} -> {newPosition - oldPosition} read -> {string.Join("|", deserializationContext.Buffer.Skip(oldPosition).Take(newPosition - oldPosition))}");
            }

            return responseMessage;
        }
    }
}
