using System;
using System.Reactive.Linq;
using System.Threading;
using artiso.Fischertechnik.TxtController.Lib.Commands;
using artiso.Fischertechnik.TxtController.Lib.Components;
using artiso.Fischertechnik.TxtController.Lib.Contracts;
using log4net;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TxtControllerLibTests
{
    [TestClass]
    public class BreakOutStation
    {
        private ILog logger;

        public BreakOutStation()
        {
            this.logger = LogManager.GetLogger(typeof(ControllerCommunicatorTest));
        }

        [TestMethod]
        public void BreakOutBothInterfaces()
        {
            var communicator = new ControllerCommunicator();

            communicator.Start();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            logger.DebugExt("Send StartMotor Two");
            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.One, true);
            logger.DebugExt("Send StopMotor Two");
            communicator.QueueCommand(new StopMotorCommand(Motor.Two));

            logger.DebugExt("Send StartMotor One Left");
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Left));
            WaitForInput(communicator, DigitalInput.Three, false);
            Thread.Sleep(TimeSpan.FromMilliseconds(900));
            logger.DebugExt("Send StartMotor One Right");
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.Three, true);
            logger.DebugExt("Send StopMotor One");
            communicator.QueueCommand(new StopMotorCommand(Motor.One));

            logger.DebugExt("Send StartMotor Two");
            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.Two, true);
            logger.DebugExt("Send StopMotor Two");
            communicator.QueueCommand(new StopMotorCommand(Motor.Two));

            logger.DebugExt("Send StartMotor One Left");
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Left));
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            logger.DebugExt("Send StartMotor One Right");
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.Three, true);


            logger.DebugExt("Send StartMotor Two");
            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.Three, false);

            logger.DebugExt("Send StopMotor One");
            communicator.QueueCommand(new StopMotorCommand(Motor.One));
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            logger.DebugExt("Send StopMotor Two");
            communicator.QueueCommand(new StopMotorCommand(Motor.Two));


            Thread.Sleep(TimeSpan.FromSeconds(5));

            communicator.Stop();
        }

        private void WaitForInput(ControllerCommunicator communicator, DigitalInput digitalInput, bool expectedValue)
        {
            var waitHandle = new ManualResetEvent(false);

            logger.DebugExt("Wait for input 1");
            communicator.UniversalInputs[(int)digitalInput].StateChanges.Where(b => b == expectedValue).Subscribe(b =>
            {
                logger.DebugExt($"Got input {digitalInput}");
                waitHandle.Set();
            });

            if (!waitHandle.WaitOne(TimeSpan.FromSeconds(10)))
            {
                Assert.Fail("Did not finish movement");
            }
        }
    }
}
