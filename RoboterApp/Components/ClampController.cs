using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Components
{
    public class ClampController
    {
        private readonly MotorPositionController clampPositionController;

        public ClampController(MotorPositionController clampPositionController)
        {
            this.clampPositionController = clampPositionController;
        }

        public async Task CloseClamp()
        {
            this.clampPositionController.StartMotorAsync(Speed.Fast, Direction.Right);

            await this.clampPositionController.PositionChanges.Throttle(TimeSpan.FromSeconds(1)).FirstAsync();

            this.clampPositionController.StopMotor();
        }

        public async Task WaitForContainerRemoval()
        {
            this.clampPositionController.StartMotorAsync(Speed.Slow, Direction.Right);

            await this.clampPositionController.PositionChanges.FirstAsync();
        }
    }
}
