using System.Threading.Tasks;
using log4net;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Components
{
    public class SequenceCommandLogic
    {
        private readonly ControllerSequencer controllerSequencer;
        private readonly MotorPositionController backwardForwardPositionController;
        private readonly MotorPositionController upDownPositionController;
        private readonly MotorPositionController turnLeftRightPositionController;
        private readonly MotorPositionController openCloseClampPositionController;
        private readonly ILog logger;

        public SequenceCommandLogic(ControllerSequencer controllerSequencer,
                                    MotorPositionController backwardForwardPositionController,
                                    MotorPositionController upDownPositionController,
                                    MotorPositionController turnLeftRightPositionController,
                                    MotorPositionController openCloseClampPositionController)
        {
            this.controllerSequencer = controllerSequencer;
            this.backwardForwardPositionController = backwardForwardPositionController;
            this.upDownPositionController = upDownPositionController;
            this.turnLeftRightPositionController = turnLeftRightPositionController;
            this.openCloseClampPositionController = openCloseClampPositionController;

            this.logger = LogManager.GetLogger(typeof(SequenceCommandLogic));
        }

        public async Task RunPickUpBatchSequenceAsync(int batchIndex)
        {
            var waitingPositionName = $"Warteposition {batchIndex}";
            var pickUpPositionName = $"Abholposition {batchIndex}";
            var removalPositionName = $"Entnahmeposition {batchIndex}";

            this.logger.Info("Starting sequence...");

            //await this.turnLeftRightPositionController.StartMotorAndMoveDistanceAsync(Speed.Fast, Direction.Left, 1000);

            await this.controllerSequencer.MoveToPositionAsync(waitingPositionName);
            this.logger.Info("Waiting position reached.");

            await this.openCloseClampPositionController.StartMotorAndMoveDistanceAsync(Speed.Fast, Direction.Right, 8);
            this.logger.Info("Clamp pre-closed.");

            await this.controllerSequencer.MoveToPositionAsync(pickUpPositionName);
            this.logger.Info("Pickup position reached.");

            await this.openCloseClampPositionController.StartMotorAndMoveDistanceAsync(Speed.Quick, Direction.Right, 14);
            this.logger.Info("Clamp closed.");

            await this.controllerSequencer.MoveToPositionAsync(removalPositionName);
            this.logger.Info("Removal position reached.");

            await this.controllerSequencer.MoveToPositionAsync("Übergabeposition");
            this.logger.Info("Delivery position reached.");
        }
    }
}
