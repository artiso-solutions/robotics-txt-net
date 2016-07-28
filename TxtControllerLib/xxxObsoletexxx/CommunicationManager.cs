using artiso.Fischertechnik.TxtController.Lib.Messages;
using artiso.Fischertechnik.TxtController.Lib.Messages.Base;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace artiso.Fischertechnik.TxtController.Lib.xxxObsoletexxx
{
    [Obsolete]
    public class CommunicationManager : IDisposable
    {
        private readonly IPAddress ipAddress;
        private readonly ushort[] motorConfigSequence = new ushort[4];
        private Socket socket;

        private ushort updateConfigSequence;

        public CommunicationManager()
        {
            ipAddress = IPAddress.Parse("192.168.7.2");
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

        public ControllerStatus QueryStatus()
        {
            var cmdBytes = BitConverter.GetBytes(CommandIds.SendQueryStatus);
            socket.Send(cmdBytes);

            var receiveBuffer = new byte[256];
            var length = socket.Receive(receiveBuffer);

            if (length <= 0)
            {
                throw new Exception("Failed to receive response");
            }

            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(receiveBuffer, ref startIndex);
            if (responseId != CommandIds.ReceiveQueryStatus)
            {
                throw new Exception("Did not get expected response");
            }

            var name = ArchiveReader.ReadString(receiveBuffer, 16, ref startIndex);
            var version = ArchiveReader.ReadVersion(receiveBuffer, ref startIndex);

            return new ControllerStatus
            {
                Name = name,
                Version = version
            };
        }

        public void StartOnlineMode()
        {
            byte[] cmdBytes;
            using (var memoryStream = new MemoryStream())
            {
                ArchiveWriter.WriteInt32(memoryStream, CommandIds.SendStartOnline);
                ArchiveWriter.WriteString(memoryStream, "Online", 64);

                cmdBytes = memoryStream.ToArray();
            }

            var receiveBuffer = ExecuteCommand(cmdBytes);

            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(receiveBuffer, ref startIndex);
            if (responseId != CommandIds.ReceiveStartOnline)
            {
                throw new Exception("Did not get expected response");
            }
        }

        public void StopOnlineMode()
        {
            ExecuteSimpleCommand(CommandIds.SendStopOnline, CommandIds.ReceiveStopOnline);
        }

        public void UpdateConfig()
        {
            byte[] sendBytes;
            using (var memoryStream = new MemoryStream())
            {
                ArchiveWriter.WriteInt32(memoryStream, CommandIds.SendUpdateConfig);
                ArchiveWriter.WriteInt16(memoryStream, ++updateConfigSequence);
                ArchiveWriter.WriteInt16(memoryStream, (ushort)0);

                // config dummy (Tx only)
                ArchiveWriter.WriteInt32(memoryStream, 0);

                // motor configuration
                ArchiveWriter.WriteBytes(memoryStream, 1, 1, 1, 1);

                // input configuration
                for (var i = 0; i < 8; i++)
                {
                    ArchiveWriter.WriteBytes(memoryStream, 1, 1, 0, 0);
                }

                // counter configuration
                for (var i = 0; i < 4; i++)
                {
                    ArchiveWriter.WriteBytes(memoryStream, 1, 0, 0, 0);
                }

                // dummy
                ArchiveWriter.WriteBytes(memoryStream, new byte[32]);

                sendBytes = memoryStream.ToArray();
            }

            var receiveBuffer = ExecuteCommand(sendBytes);
            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(receiveBuffer, ref startIndex);
            if (responseId != CommandIds.ReceiveUpdateConfig)
            {
                throw new Exception("Did not get expected response");
            }
        }

        public void StartMotorLeft(int motorIndex, ushort speed, int distance)
        {
            StartMotor(motorIndex, speed, 0, distance);
        }

        public void StartMotorRight(int motorIndex, ushort speed, int distance)
        {
            StartMotor(motorIndex, 0, speed, distance);
        }

        public void StopMotor(int motorIndex)
        {
            StartMotor(motorIndex, 0, 0, 0);
        }

        private void StartMotor(int motorIndex, ushort speedLeft, ushort speedRight, int distance)
        {
            byte[] sendBytes;
            using (var memoryStream = new MemoryStream())
            {
                ArchiveWriter.WriteInt32(memoryStream, CommandIds.SendExchangeData);

                // speed
                var motorSpeeds = new ushort[8];
                motorSpeeds[motorIndex * 2] = speedLeft;
                motorSpeeds[motorIndex * 2 + 1] = speedRight;
                ArchiveWriter.WriteInt16(memoryStream, motorSpeeds);

                // synchronization
                ArchiveWriter.WriteInt16(memoryStream, new ushort[4]);

                // distance
                var motorDistances = new ushort[4];
                motorDistances[motorIndex] = (ushort)distance;
                ArchiveWriter.WriteInt16(memoryStream, motorDistances);

                // command
                motorConfigSequence[motorIndex]++;
                ArchiveWriter.WriteInt16(memoryStream, motorConfigSequence);

                // counter reset
                ArchiveWriter.WriteInt16(memoryStream, new ushort[4]);

                // sound gedöns
                ArchiveWriter.WriteInt16(memoryStream, new ushort[4]);

                sendBytes = memoryStream.ToArray();
            }

            var receiveBuffer = ExecuteCommand(sendBytes);
            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(receiveBuffer, ref startIndex);
            if (responseId != CommandIds.ReceiveExchangeData)
            {
                throw new Exception("Did not get expected response");
            }
        }

        private void ExecuteSimpleCommand(uint command, uint expectedResponseId)
        {
            var cmdBytes = BitConverter.GetBytes(command);
            socket.Send(cmdBytes);

            var receiveBuffer = new byte[256];
            var length = socket.Receive(receiveBuffer);

            if (length <= 0)
            {
                throw new Exception("Failed to receive response");
            }

            var startIndex = 0;
            var responseId = ArchiveReader.ReadUInt32(receiveBuffer, ref startIndex);
            if (responseId != expectedResponseId)
            {
                throw new Exception("Did not get expected response");
            }
        }

        private byte[] ExecuteCommand(byte[] queryBuffer)
        {
            socket.Send(queryBuffer);

            var receiveBuffer = new byte[256];
            var length = socket.Receive(receiveBuffer);

            if (length <= 0)
            {
                throw new Exception("Failed to receive response");
            }

            return receiveBuffer.Take(length).ToArray();
        }
    }
}