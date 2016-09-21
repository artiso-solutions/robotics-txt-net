using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Commands;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    public class MotorPositionController : INotifyPropertyChanged, IDisposable
    {
        private readonly ControllerCommunicator controllerCommunicator;
        private readonly ControllerSequencer controllerSequencer;
        private MotorDistanceInfo motorDistanceInfo;
        private Direction? currentDirection;

        private int currentPosition;

        public MotorConfiguration MotorConfiguration { get; }

        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                if (currentPosition == value)
                {
                    return;
                }

                currentPosition = value;
                OnPropertyChanged();
            }
        }

        internal MotorPositionController(MotorConfiguration motorConfiguration, ControllerCommunicator controllerCommunicator, ControllerSequencer controllerSequencer)
        {
            this.controllerCommunicator = controllerCommunicator;
            this.controllerSequencer = controllerSequencer;
            MotorConfiguration = motorConfiguration;

            motorDistanceInfo = controllerCommunicator.MotorDistanceInfos.First(m => m.Motor == motorConfiguration.Motor);
            motorDistanceInfo.DistanceChanges.Subscribe(diff =>
            {
                if (currentDirection == null)
                {
                    return;
                }

                if (currentDirection.Value == motorConfiguration.ReferencingDirection)
                {
                    Interlocked.Add(ref currentPosition, -diff);
                    OnPropertyChanged(nameof(CurrentPosition));
                }
                else
                {
                    Interlocked.Add(ref currentPosition, diff);
                    OnPropertyChanged(nameof(CurrentPosition));
                }
            });
        }

        /// <summary>
        /// Starts the configured <see cref="MotorConfiguration"/> immediately and runs the specified <paramref name="distance"/>.
        /// </summary>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to start.</param>
        /// <param name="distance">The distance to run.</param>
        public void MotorRunDistance(Speed speed, Direction direction, short distance)
        {
            currentDirection = direction;
            controllerCommunicator.QueueCommand(new MotorRunDistanceCommand(MotorConfiguration.Motor, speed, direction, distance));
        }

        public void MoveMotorToPosition([NotNull] MotorPositionInfo motorPositionInfo)
        {
            if (motorPositionInfo == null) throw new ArgumentNullException(nameof(motorPositionInfo));

            var targetPosition = motorPositionInfo.Position;

            var distanceToPosition = targetPosition - this.CurrentPosition;

            if (distanceToPosition == 0)
            {
                return;
            }

            Direction direction;
            var positiveMovement = this.MotorConfiguration.ReferencingDirection == Direction.Left
                ? Direction.Right
                : Direction.Left;

            var negativeMovement = this.MotorConfiguration.ReferencingDirection == Direction.Left
                ? Direction.Left
                : Direction.Right;

            direction = distanceToPosition > 0 ? positiveMovement : negativeMovement;
            distanceToPosition = Math.Abs(distanceToPosition);

            var speed = Speed.Maximal;

            this.MotorRunDistance(speed, direction, (short)distanceToPosition);
        }

        /// <summary>
        /// Stops the specified <see cref="MotorConfiguration"/> immediately.
        /// </summary>
        public void StopMotor()
        {
            this.controllerCommunicator.QueueCommand(new StopMotorCommand(MotorConfiguration.Motor));
        }

        public async Task ReferenceAsync()
        {
            this.motorDistanceInfo.IsTracking = false;

            if (controllerSequencer.GetDigitalInputState(MotorConfiguration.ReferencingInput) == MotorConfiguration.ReferencingInputState)
            {
                var freeRunMovement = MotorConfiguration.ReferencingDirection == Direction.Left ? Direction.Right : Direction.Left;
                await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, MotorConfiguration.ReferencingInput, true);
                await controllerSequencer.StartMotorStopAfterTimeSpanInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, TimeSpan.FromMilliseconds(100));
            }

            await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, MotorConfiguration.ReferencingDirection, MotorConfiguration.ReferencingInput, MotorConfiguration.ReferencingInputState);

            Interlocked.Exchange(ref currentPosition, 0);
            this.OnPropertyChanged(nameof(CurrentPosition));

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            this.motorDistanceInfo.IsTracking = true;
        }

        public MotorPositionInfo GetPositionInfo()
        {
            return new MotorPositionInfo
            {
                Motor = this.MotorConfiguration.Motor,
                Position = this.CurrentPosition
            };
        }

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}