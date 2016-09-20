using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Communicator
{
    internal class MotorDistanceInfo
    {
        private readonly Subject<short> commandIdChangesSubject;
        private readonly Subject<int> distanceChangesSubject;

        private short currentCommandId;
        private short currentDistanceValue;

        public MotorDistanceInfo(Motor motor)
        {
            this.Motor = motor;

            distanceChangesSubject = new Subject<int>();
            commandIdChangesSubject = new Subject<short>();
        }

        public Motor Motor { get; }

        // TODO think about rename
        public IObservable<int> DistanceChanges => distanceChangesSubject.AsObservable();

        public IObservable<short> CommandIdChanges => commandIdChangesSubject.AsObservable();

        public void SetCurrentDistanceValue(short distanceValue)
        {
            if (currentDistanceValue == distanceValue)
            {
                return;
            }

            var difference = distanceValue - currentDistanceValue;
            currentDistanceValue = distanceValue;
            distanceChangesSubject.OnNext(difference);
        }

        public void SetCurrentCommandId(short commandId)
        {
            if (currentCommandId == commandId)
            {
                return;
            }

            currentCommandId = commandId;
            commandIdChangesSubject.OnNext(commandId);
        }
    }
}