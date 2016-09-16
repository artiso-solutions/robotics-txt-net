using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            ProcessWorkpiece(true, false).Wait();
        }

        private async Task ProcessWorkpiece(bool breakoutFirstInterface, bool breakoutSecondInterface)
        {
            using (var sequencer = new ControllerSequencer())
            {
                await sequencer.StartMotorStopWithDigitalInputAsync(Motor.Two, Speed.Fast, Movement.Right, DigitalInput.One, true);

                if (breakoutFirstInterface)
                {
                    await BreakoutInterface(sequencer);
                }

                await sequencer.StartMotorStopWithDigitalInputAsync(Motor.Two, Speed.Fast, Movement.Right, DigitalInput.Two, true);

                if (breakoutSecondInterface)
                {
                    await BreakoutInterface(sequencer);
                }

                await sequencer.StartMotorStopAfterAsync(Motor.Two, Speed.Maximal, Movement.Right, TimeSpan.FromSeconds(2));
            }
        }

        private async Task BreakoutInterface(ControllerSequencer sequencer)
        {
            logger.InfoExt("Breakout interface");
            await sequencer.StartMotorStopAfterAsync(Motor.One, Speed.Fast, Movement.Left, TimeSpan.FromMilliseconds(900));
            await sequencer.StartMotorStopWithDigitalInputAsync(Motor.One, Speed.Fast, Movement.Right, DigitalInput.Three, true);
        }
    }
}
