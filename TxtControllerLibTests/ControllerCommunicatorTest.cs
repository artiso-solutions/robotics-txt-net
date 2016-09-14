using System;
using System.Threading;
using artiso.Fischertechnik.TxtController.Lib.Commands;
using artiso.Fischertechnik.TxtController.Lib.Components;
using artiso.Fischertechnik.TxtController.Lib.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TxtControllerLibTests
{
    [TestClass]
    public class ControllerCommunicatorTest
    {
        [TestMethod]
        public void StartStopMotor()
        {
            var communicator = new ControllerCommunicator();

            communicator.Start();

            Thread.Sleep(1000);

            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Maximal, Movement.Right));

            Thread.Sleep(3000);

            communicator.QueueCommand(new StopMotorCommand(Motor.Two));
            
            Thread.Sleep(1000);

            communicator.Stop();
        }
    }
}
