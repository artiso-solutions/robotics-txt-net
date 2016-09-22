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
        private readonly MotorDistanceInfo motorDistanceInfo;
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

        public int AvailableDistance => this.MotorConfiguration.Limit - this.CurrentPosition;

        internal MotorPositionController(MotorConfiguration motorConfiguration, ControllerCommunicator controllerCommunicator, ControllerSequencer controllerSequencer)
        {
            this.controllerCommunicator = controllerCommunicator;
            this.controllerSequencer = controllerSequencer;
            MotorConfiguration = motorConfiguration;

            motorDistanceInfo = controllerCommunicator.MotorDistanceInfos.First(m => m.Motor == motorConfiguration.Motor);
            motorDistanceInfo.DistanceDifferences.Subscribe(diff =>
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
        /// Starts the <see cref="Motor"/> specified in the <see cref="MotorConfiguration"/> of the controller immediately.
        /// </summary>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to movement.</param>
        public void StartMotor(Speed speed, Direction direction)
        {
            this.StartMotorAndMoveDistance(speed, direction, (short) this.AvailableDistance);
        }

        /// <summary>
        /// Stops the <see cref="Motor"/> specified in the <see cref="MotorConfiguration"/> of the controller immediately.
        /// </summary>
        public void StopMotor()
        {
            this.controllerCommunicator.QueueCommand(new StopMotorCommand(MotorConfiguration.Motor));
        }

        /// <summary>
        /// Starts the <see cref="Motor"/> specified in the <see cref="MotorConfiguration"/> immediately and runs the specified <paramref name="distance"/>.
        /// </summary>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to start.</param>
        /// <param name="distance">The distance to run.</param>
        public void StartMotorAndMoveDistance(Speed speed, Direction direction, short distance)
        {
            if (direction != this.MotorConfiguration.ReferencingDirection)
            {
                if (this.AvailableDistance <= 0)
                {
                    return;
                }
                
                if (distance > this.AvailableDistance)
                {
                    distance = (short)this.AvailableDistance;
                }
            }

            currentDirection = direction;
            controllerCommunicator.QueueCommand(new MotorRunDistanceCommand(MotorConfiguration.Motor, speed, direction, distance));
        }

        /// <summary>
        /// Moves the <see cref="Motor"/> specified in the <see cref="MotorConfiguration"/> to the given <see cref="MotorPositionInfo"/>.
        /// </summary>
        /// <param name="motorPositionInfo">Target position.</param>
        public void MoveMotorToPosition([NotNull] MotorPositionInfo motorPositionInfo)
        {
            if (motorPositionInfo == null) throw new ArgumentNullException(nameof(motorPositionInfo));

            var targetPosition = motorPositionInfo.Position;

            if (targetPosition > this.MotorConfiguration.Limit)
            {
                throw new InvalidOperationException("Position would breach limit.");
            }

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

            this.StartMotorAndMoveDistance(speed, direction, (short)distanceToPosition);
        }

        /// <summary>
        /// Moves the <see cref="Motor"/> specified in the <see cref="MotorConfiguration"/> to the given reference position and zeroes the tracked position.
        /// </summary>
        public async Task MoveMotorToReferenceAsync()
        {
            this.motorDistanceInfo.IsTracking = false;

            if (controllerSequencer.GetDigitalInputState(MotorConfiguration.ReferencingInput) == MotorConfiguration.ReferencingInputState)
            {
                var freeRunMovement = MotorConfiguration.ReferencingDirection == Direction.Left ? Direction.Right : Direction.Left;
                await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, MotorConfiguration.ReferencingInput, !MotorConfiguration.ReferencingInputState);
                await controllerSequencer.StartMotorStopAfterTimeSpanInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, TimeSpan.FromMilliseconds(100));
            }

            await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, MotorConfiguration.ReferencingDirection, MotorConfiguration.ReferencingInput, MotorConfiguration.ReferencingInputState);

            Interlocked.Exchange(ref currentPosition, 0);
            this.OnPropertyChanged(nameof(CurrentPosition));

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            this.motorDistanceInfo.IsTracking = true;
        }

        /// <summary>
        /// Delivers the current <see cref="MotorPositionInfo"/> of the controller.
        /// </summary>
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