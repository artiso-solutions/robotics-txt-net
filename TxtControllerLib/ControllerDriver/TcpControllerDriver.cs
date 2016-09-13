using artiso.Fischertechnik.TxtController.Lib.Messages.Base;
using JetBrains.Annotations;
using log4net;
using log4net.Util;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace artiso.Fischertechnik.TxtController.Lib.ControllerDriver
{
   using artiso.Fischertechnik.TxtController.Lib.Contracts;

   public class TcpControllerDriver : IDisposable
   {
      private readonly IPAddress ipAddress;
      private readonly ILog logger = LogManager.GetLogger(typeof(TcpControllerDriver));
      private Socket socket;

      public TcpControllerDriver(Communication communication)
      {
         switch (communication)
         {
            case Communication.USB:
               ipAddress = IPAddress.Parse("192.168.7.2");
               break;

            case Communication.Wifi:
               ipAddress = IPAddress.Parse("192.168.8.2");
               break;

            case Communication.Bluetooth:
               ipAddress = IPAddress.Parse("192.168.9.2");
               break;

            default:
               throw new ArgumentOutOfRangeException(nameof(communication), communication, "Unknow value");
         }

      }

      public void Dispose()
      {
         if (socket?.Connected == true)
         {
            socket.Close();
            socket.Dispose();
            socket = null;
         }
      }

      public void StartCommunication()
      {
         socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
         socket.Connect(ipAddress, 65000);
      }

      public void SendCommand<TCmdMessage>([NotNull] TCmdMessage command)
          where TCmdMessage : CommandMessage
      {
         var cmdBytes = GetBytesOfMessage(command);
         logger.DebugExt($"Sending {cmdBytes.Length} bytes");
         socket.Send(cmdBytes);

         using (var receiveStream = new MemoryStream())
         {
            var receiveBuffer = new byte[256];
            int length;
            do
            {
               length = socket.Receive(receiveBuffer);
               receiveStream.Write(receiveBuffer, 0, length);
            } while (length == receiveBuffer.Length);

            var receviedBytes = receiveStream.ToArray();
            if (receviedBytes.Length <= 0)
            {
               throw new CommunicationFailedException("Failed to receive response");
            }

            var responseMessageId = GetMessageId(receviedBytes);
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
         var cmdBytes = GetBytesOfMessage(command);
         socket.Send(cmdBytes);

         using (var receiveStream = new MemoryStream())
         {
            var receiveBuffer = new byte[256];
            int length;
            do
            {
               length = socket.Receive(receiveBuffer);
               receiveStream.Write(receiveBuffer, 0, length);
            } while (length == receiveBuffer.Length);

            var receviedBytes = receiveStream.ToArray();
            if (receviedBytes.Length <= 0)
            {
               throw new CommunicationFailedException("Failed to receive response");
            }

            var responseMessage = GetMessageOfBytes<TResponseMessage>(receviedBytes);
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
         logger.DebugExt($"Serialize message of type {message.GetType().FullName}");
         using (var memoryStream = new MemoryStream())
         {
            long oldLength;
            long newLength;
            foreach (var propertySerializationInfo in message.SerializationProperties)
            {
               oldLength = memoryStream.Length;
               propertySerializationInfo.WriteValue(memoryStream);
               newLength = memoryStream.Length;
               logger.DebugExt(() => $"  {propertySerializationInfo.Name} -> {newLength - oldLength} added -> {string.Join("|", memoryStream.ToArray().Skip((int)oldLength))}");
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
         logger.DebugExt($"Deserialize message of type {responseMessage.GetType().FullName}");
         var deserializationContext = new DeserializationContext(bytes);
         int oldPosition;
         int newPosition;
         foreach (var propertyDeserializationInfo in responseMessage.DeserializationProperties)
         {
            oldPosition = deserializationContext.CurrentPosition;
            propertyDeserializationInfo.ReadValue(deserializationContext);
            newPosition = deserializationContext.CurrentPosition;
            logger.DebugExt(() => $"  {propertyDeserializationInfo.Name} -> {newPosition - oldPosition} read -> {string.Join("|", deserializationContext.Buffer.Skip(oldPosition).Take(newPosition - oldPosition))}");
         }

         return responseMessage;
      }
   }
}
