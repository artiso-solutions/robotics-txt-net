using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using RoboticsTxt.Lib.Configuration;
using RoboticsTxt.Lib.ControllerDriver;
using RoboticsTxt.Lib.Messages;
using TxtControllerLibTests.Properties;

namespace TxtControllerLibTests
{
    [TestClass]
    public class TcpControllerDriverTests
    {
        [TestMethod]
        public void ReceiveQueryStatusTcpControllerDriver()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                var message = new QueryStatusCommandMessage();
                var bytes = new TcpControllerDriver(IPAddress.Parse(Settings.Default.TestDeviceIpAddress)).GetBytesOfMessage(message);
                Assert.AreEqual(4, bytes.Length);

                var queryStatusResponseMessage =
                    tcpControllerDriver.SendCommand<QueryStatusCommandMessage, QueryStatusResponseMessage>(message);

                Assert.IsNotNull(queryStatusResponseMessage);
                Assert.AreEqual("TX2013", queryStatusResponseMessage.Name);
                Assert.AreEqual(new Version(4, 2, 3, 0), queryStatusResponseMessage.Version);
            }
        }

        [TestMethod]
        public void StartOnline()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                var message = new StartOnlineCommandMessage();
                var bytes = new TcpControllerDriver(IPAddress.Parse(Settings.Default.TestDeviceIpAddress)).GetBytesOfMessage(message);
                Assert.AreEqual(68, bytes.Length);

                tcpControllerDriver.SendCommand(message);
            }
        }

        [TestMethod]
        public void StopOnline()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
            }
        }

        [TestMethod]
        public void UpdateConfig()
        {
            this.StartOnline();
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                try
                {
                    this.SetConfigM1(tcpControllerDriver);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        [TestMethod]
        public void TurnRobotLeftAndRightTime()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());

                try
                {
                    this.SetConfigO1O2(tcpControllerDriver);

                    var startMotorLeftCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 256, 0, 512, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 1, 1, 0, 0 }
                    };

                    var stopMotorCommand1 = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 2, 2, 0, 0 }
                    };

                    var startMotorRightCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 256, 0, 512, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 3, 3, 0, 0 }
                    };

                    var stopMotorCommand2 = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 4, 4, 0, 0 }
                    };

                    var test = tcpControllerDriver.GetBytesOfMessage(startMotorLeftCommand);
                    Assert.AreEqual(60, tcpControllerDriver.GetBytesOfMessage(startMotorLeftCommand).Length);

                    var endTime = DateTime.Now.AddSeconds(1.2);

                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        tcpControllerDriver.SendCommand(startMotorLeftCommand);
                    }
                    tcpControllerDriver.SendCommand(stopMotorCommand1);

                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    endTime = DateTime.Now.AddSeconds(1);
                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        tcpControllerDriver.SendCommand(startMotorRightCommand);
                    }
                    tcpControllerDriver.SendCommand(stopMotorCommand2);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        [TestMethod]
        public void TurnRobotLeftAndRightDistance()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());

                try
                {
                    this.SetConfigO1O2(tcpControllerDriver);

                    var startMotorLeftCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 256, 0, 512, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 1, 1, 0, 0 },
                        MotorDistance = new short[] { 200, 200, 0, 0 }
                    };

                    var stopMotorCommand1 = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 2, 2, 0, 0 }
                    };

                    var startMotorRightCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 256, 0, 512, 0, 0, 0, 0 },
                        MotorDistance = new short[] { 200, 200, 0, 0 },
                        MotorCommandId = new short[] { 3, 3, 0, 0 }
                    };

                    var stopMotorCommand2 = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 4, 4, 0, 0 }
                    };

                    var test = tcpControllerDriver.GetBytesOfMessage(startMotorLeftCommand);
                    Assert.AreEqual(60, tcpControllerDriver.GetBytesOfMessage(startMotorLeftCommand).Length);

                    var endTime = DateTime.Now.AddSeconds(4);

                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        var response =
                            tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(
                                startMotorLeftCommand);
                        Debug.WriteLine(response.CounterValue[0]);
                    }
                    tcpControllerDriver.SendCommand(stopMotorCommand1);

                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    endTime = DateTime.Now.AddSeconds(4);
                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        var response =
                            tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(
                                startMotorRightCommand);
                        Debug.WriteLine(response.CounterValue[0]);
                    }
                    tcpControllerDriver.SendCommand(stopMotorCommand2);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        [TestMethod]
        public void TurnRobotWhileButtonPressed()
        {
            short commandId = 0;
            ExchangeDataResponseMessage response = new ExchangeDataResponseMessage();

            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());

                try
                {
                    this.SetConfigO1O2(tcpControllerDriver);

                    var Command = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { commandId, commandId, commandId, commandId }
                    };
                    commandId++;

                    while (response.UniversalInputs[0] == 0)
                    {
                        response =
                            tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(
                                Command);
                    }

                    Command = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 256, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { commandId, commandId, commandId, commandId }
                    };
                    commandId++;

                    while (response.UniversalInputs[0] == 1)
                    {
                        response =
                            tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(
                                Command);
                    }

                    Command = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { commandId, commandId, commandId, commandId }
                    };
                    commandId++;

                    response =
                        tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(Command);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        [TestMethod]
        public void MoveComponentCarrier()
        {
            using (var tcpControllerDriver = this.PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());

                try
                {
                    this.SetConfigO1O2(tcpControllerDriver);

                    var startMotorCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 256, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 0, 1, 0, 0 },
                        MotorDistance = new short[] { 0, 0, 0, 0 }
                    };

                    var stopMotorCommand = new ExchangeDataCommandMessage
                    {
                        PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                        MotorCommandId = new short[] { 0, 2, 0, 0 }
                    };

                    var endTime = DateTime.Now.AddSeconds(10);

                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        var response =
                            tcpControllerDriver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(
                                startMotorCommand);
                        Debug.WriteLine(response.CounterValue[0]);
                        Thread.Sleep(500);
                    }
                    tcpControllerDriver.SendCommand(stopMotorCommand);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        public TestContext TestContext { get; set; }

        private TcpControllerDriver PrepareTcpControllerDriver()
        {
            var logger = LogManager.GetLogger(typeof(TcpControllerDriverTests));
            logger.Info($"Preparing TcpControllerDriver");
            var tcpControllerDriver = new TcpControllerDriver(IPAddress.Parse(Settings.Default.TestDeviceIpAddress));
            tcpControllerDriver.StartCommunication();
            return tcpControllerDriver;
        }

        private void SetConfigM1(TcpControllerDriver tcpControllerDriver)
        {
            var message = new UpdateConfigCommandMessage
            {
                ConfigId = 0,
                MotorModes = new[] { MotorMode.M1, MotorMode.M1, MotorMode.M1, MotorMode.M1 },
                InputConfigurations =
                    Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8)
                        .ToArray(),
                CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
            };
            var bytes = new TcpControllerDriver(IPAddress.Parse(Settings.Default.TestDeviceIpAddress)).GetBytesOfMessage(message);
            Assert.AreEqual(96, bytes.Length);

            tcpControllerDriver.SendCommand(message);
        }

        private void SetConfigO1O2(TcpControllerDriver tcpControllerDriver)
        {
            tcpControllerDriver.SendCommand(new UpdateConfigCommandMessage
            {
                ConfigId = 0,
                MotorModes = new[] { MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2 },
                InputConfigurations =
                    Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8)
                        .ToArray(),
                CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
            });
        }
    }
}