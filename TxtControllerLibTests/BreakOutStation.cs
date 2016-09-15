using System;
using System.Reactive.Linq;
using System.Threading;
using log4net;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

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
            ProcessWorkpiece(true, false);
        }

        private void ProcessWorkpiece(bool breakoutFirstInterface, bool breakoutSecondInterface)
        {
            using (var sequencer = new ControllerSequencer())
            {
                sequencer.StartMotorStopWithDigitalInput(Motor.Two, Speed.Fast, Movement.Right, DigitalInput.One, true);

                if (breakoutFirstInterface)
                {
                    BreakoutInterface(sequencer);
                }

                sequencer.StartMotorStopWithDigitalInput(Motor.Two, Speed.Fast, Movement.Right, DigitalInput.Two, true);

                if (breakoutSecondInterface)
                {
                    BreakoutInterface(sequencer);
                }

                sequencer.StartMotorStopAfter(Motor.Two, Speed.Maximal, Movement.Right, TimeSpan.FromSeconds(2));
            }
        }

        private void BreakoutInterface(ControllerSequencer sequencer)
        {
            logger.InfoExt("Breakout interface");
            sequencer.StartMotorStopAfter(Motor.One, Speed.Fast, Movement.Left, TimeSpan.FromMilliseconds(900));
            sequencer.StartMotorStopWithDigitalInput(Motor.One, Speed.Fast, Movement.Right, DigitalInput.Three, true);
        }
    }
}
