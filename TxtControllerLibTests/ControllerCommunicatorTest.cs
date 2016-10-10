using System;
using System.Net;
using System.Threading;
using log4net;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboticsTxt.Lib.Commands;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Contracts.Configuration;
using TxtControllerLibTests.Properties;

namespace TxtControllerLibTests
{
    [TestClass]
    public class ControllerCommunicatorTest
    {
        private ILog logger;

        public ControllerCommunicatorTest()
        {
            this.logger = LogManager.GetLogger(typeof (ControllerCommunicatorTest));
        }

        [TestMethod]
        public void StartStopMotor()
        {
            var communicator = new ControllerCommunicator(IPAddress.Parse(Settings.Default.TestDeviceIpAddress), new ControllerConfiguration());

            communicator.Start();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            communicator.QueueCommand(new StartMotorCommand(Motor.Two, Speed.Maximal, Direction.Right));

            Thread.Sleep(TimeSpan.FromSeconds(3));

            communicator.QueueCommand(new StopMotorCommand(Motor.Two));

            Thread.Sleep(TimeSpan.FromSeconds(3));

            communicator.Stop();
        }

        [TestMethod]
        public void RunStepMotor()
        {
            var communicator = new ControllerCommunicator(IPAddress.Parse(Settings.Default.TestDeviceIpAddress), new ControllerConfiguration());

            communicator.Start();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            //int[] stepsLeft = new int[4] { 5, 9, 10, 6 };
            int[] stepsLeft = new int[8] { 5, 1, 9, 8, 10, 2, 6, 4 };
            var rounds = 20;
            for (int i = 0; i < rounds * stepsLeft.Length * 50; i++)
            {
                var currentStep = stepsLeft[i % stepsLeft.Length];
                
                StartMotor(currentStep, 1, 2, Motor.One, communicator);
                StartMotor(currentStep, 4, 8, Motor.Two, communicator);

                Thread.Sleep(50);
            }

            StartMotor(0, 1, 2, Motor.One, communicator);
            StartMotor(0, 4, 8, Motor.Two, communicator);

            Thread.Sleep(TimeSpan.FromSeconds(3));

            communicator.Stop();
        }

        private static void StartMotor(int currentStep, int bitO1, int bitO2, Motor motor, ControllerCommunicator communicator)
        {
            communicator.QueueCommand(new SetMotorOutputCommand(motor, (currentStep & bitO1) > 0 ? Speed.Maximal : Speed.Off, (currentStep & bitO2) > 0 ? Speed.Maximal : Speed.Off));
        }

        [TestMethod]
        public void LogInputStateChanges()
        {
            var communicator = new ControllerCommunicator(IPAddress.Parse(Settings.Default.TestDeviceIpAddress), new ControllerConfiguration());

            communicator.Start();

            for (int i = 0; i < 8; i++)
            {
                var inputDisplayIndex = i + 1;

                communicator.UniversalInputs[i].StateChanges.Subscribe(
                    state => this.logger.DebugExt($"Input {inputDisplayIndex} - {state}"));
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            communicator.Stop();
        }
    }
}
