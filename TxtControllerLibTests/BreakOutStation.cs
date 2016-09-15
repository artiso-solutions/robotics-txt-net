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

            StartMotorStopWithDigitalInput(communicator, new StartMotorCommand(Motor.Two, Speed.Fast, Movement.Right), DigitalInput.One, true);

            BreakoutInterface(communicator);

            StartMotorStopWithDigitalInput(communicator, new StartMotorCommand(Motor.Two, Speed.Fast, Movement.Right), DigitalInput.Two, true);

            BreakoutInterface(communicator);

            StartMotorStopAfter(communicator, new StartMotorCommand(Motor.Two, Speed.Maximal, Movement.Right), TimeSpan.FromSeconds(2));
            
            Thread.Sleep(TimeSpan.FromSeconds(5));

            communicator.Stop();
        }

        private void BreakoutInterface(ControllerCommunicator communicator)
        {
            logger.InfoExt("Breakout interface");
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Left));
            Thread.Sleep(TimeSpan.FromMilliseconds(900));
            communicator.QueueCommand(new StartMotorCommand(Motor.One, Speed.Fast, Movement.Right));
            WaitForInput(communicator, DigitalInput.Three, true);
            communicator.QueueCommand(new StopMotorCommand(Motor.One));
        }

        private void StartMotorStopWithDigitalInput(ControllerCommunicator communicator, StartMotorCommand startMotorCommand, DigitalInput digitalInput, bool expectedInputState)
        {
            communicator.QueueCommand(startMotorCommand);
            WaitForInput(communicator, digitalInput, expectedInputState);
            communicator.QueueCommand(new StopMotorCommand(startMotorCommand.Motor));
        }

        private void StartMotorStopAfter(ControllerCommunicator communicator, StartMotorCommand startMotorCommand, TimeSpan stopAfterTimeSpan)
        {
            communicator.QueueCommand(startMotorCommand);
            Thread.Sleep(stopAfterTimeSpan);
            communicator.QueueCommand(new StopMotorCommand(startMotorCommand.Motor));
        }

        private void WaitForInput(ControllerCommunicator communicator, DigitalInput digitalInput, bool expectedValue)
        {
            communicator.UniversalInputs[(int)digitalInput].StateChanges.FirstAsync(b => b == expectedValue).Wait();
        }
    }
}
