using artiso.Fischertechnik.TxtController.Lib.Configuration;
using artiso.Fischertechnik.TxtController.Lib.ControllerDriver;
using artiso.Fischertechnik.TxtController.Lib.Messages;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace TxtControllerLibTests
{
   using artiso.Fischertechnik.TxtController.Lib.Contracts;

   [TestClass]
    public class TcpControllerDriverTests
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var tracer = new TraceAppender();
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.AddAppender(tracer);
            var patternLayout = new PatternLayout { ConversionPattern = "%m%n" };
            tracer.Layout = patternLayout;
            hierarchy.Configured = true;
        }

        [TestMethod]
        public void ReceiveQueryStatusTcpControllerDriver()
        {
            using (var tcpControllerDriver = PrepareTcpControllerDriver())
            {
                var message = new QueryStatusCommandMessage();
                var bytes = new TcpControllerDriver(Communication.USB).GetBytesOfMessage(message);
                Assert.AreEqual(4, bytes.Length);

                var queryStatusResponseMessage = tcpControllerDriver.SendCommand<QueryStatusCommandMessage, QueryStatusResponseMessage>(message);

                Assert.IsNotNull(queryStatusResponseMessage);
                Assert.AreEqual("TX2013", queryStatusResponseMessage.Name);
                Assert.AreEqual(new Version(4, 2, 3, 0), queryStatusResponseMessage.Version);
            }
        }

        [TestMethod]
        public void StartOnline()
        {
            using (var tcpControllerDriver = PrepareTcpControllerDriver())
            {
                var message = new StartOnlineCommandMessage();
                var bytes = new TcpControllerDriver(Communication.USB).GetBytesOfMessage(message);
                Assert.AreEqual(68, bytes.Length);

                tcpControllerDriver.SendCommand(message);
            }
        }

        [TestMethod]
        public void StopOnline()
        {
            using (var tcpControllerDriver = PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
            }
        }

        [TestMethod]
        public void UpdateConfig()
        {
            StartOnline();
            using (var tcpControllerDriver = PrepareTcpControllerDriver())
            {
                try
                {
                    var message = new UpdateConfigCommandMessage
                    {
                        UpdateConfigSequence = 0,
                        MotorModes = new[] { MotorMode.M1, MotorMode.M1, MotorMode.M1, MotorMode.M1 },
                        InputConfigurations = Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8).ToArray(),
                        CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
                    };
                    var bytes = new TcpControllerDriver(Communication.USB).GetBytesOfMessage(message);
                    Assert.AreEqual(96, bytes.Length);

                    tcpControllerDriver.SendCommand(message);
                }
                finally
                {
                    tcpControllerDriver.SendCommand(new StopOnlineCommandMessage());
                }
            }
        }

        [TestMethod]
        public void TurnRobotLeftAndRight()
        {
            using (var tcpControllerDriver = PrepareTcpControllerDriver())
            {
                tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());

                try
                {
                    tcpControllerDriver.SendCommand(new UpdateConfigCommandMessage
                    {
                        UpdateConfigSequence = 0,
                        MotorModes = new[] { MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2 },
                        InputConfigurations = Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8).ToArray(),
                        CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
                    });

                    var endTime = DateTime.Now.AddSeconds(3);
                    //for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    //{
                    //    tcpControllerDriver.SendCommand(new StartMotorLeft(0, 256, 0);
                    //}
                    //tcpControllerDriver.StopMotor(0);

                    //Thread.Sleep(TimeSpan.FromSeconds(1));

                    //endTime = DateTime.Now.AddSeconds(3);
                    //for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    //{
                    //    tcpControllerDriver.StartMotorRight(0, 256, 0);
                    //}
                    //tcpControllerDriver.StopMotor(0);
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
            var tcpControllerDriver = new TcpControllerDriver(Communication.USB);
            tcpControllerDriver.StartCommunication();
            return tcpControllerDriver;
        }
    }
}