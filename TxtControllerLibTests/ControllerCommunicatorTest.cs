using System;
using System.Diagnostics;
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

            Thread.Sleep(TimeSpan.FromSeconds(1));

            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Maximal, Movement.Right));

            Thread.Sleep(TimeSpan.FromSeconds(3));

            communicator.QueueCommand(new StopMotorCommand(Motor.Two));
            
            Thread.Sleep(TimeSpan.FromSeconds(3));

            communicator.Stop();
        }

        [TestMethod]
        public void LogInputStateChanges()
        {
            var communicator = new ControllerCommunicator();

            communicator.Start();

            for (int i = 0; i < 8; i++)
            {
                var inputDisplayIndex = i + 1;

                communicator.UniversalInputs[i].StateChanges.Subscribe(
                    state => Debug.WriteLine($"Input {inputDisplayIndex} - {DateTime.Now} - {state}"));
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            communicator.Stop();
        }
    }
}
