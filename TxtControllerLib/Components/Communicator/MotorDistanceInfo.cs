using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using log4net;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Communicator
{
    internal class MotorDistanceInfo
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(MotorDistanceInfo));
        private readonly Subject<short> commandIdChangesSubject;
        private readonly Subject<int> distanceDifferencesSubject;

        private short currentCommandId;
        private short currentDistanceValue;

        public MotorDistanceInfo(Motor motor)
        {
            this.Motor = motor;
            this.IsTracking = true;

            distanceDifferencesSubject = new Subject<int>();
            commandIdChangesSubject = new Subject<short>();
        }

        public Motor Motor { get; }

        public bool IsTracking { get; set; }

        // TODO think about rename
        public IObservable<int> DistanceDifferences => distanceDifferencesSubject.AsObservable();

        public IObservable<short> CommandIdChanges => commandIdChangesSubject.AsObservable();

        public void SetCurrentDistanceValue(short distanceValue, short commandId)
        {
            if (!this.IsTracking)
            {
                this.currentDistanceValue = distanceValue;
                return;
            }

            if ((distanceValue < currentDistanceValue))
            {
                currentDistanceValue = 0;
                logger.Info("reset currentDistance");
            }

            if (currentDistanceValue == distanceValue)
            {
                return;
            }

            var difference = distanceValue - currentDistanceValue;

            logger.Info($"d={distanceValue:000} - diff={difference:000} - c={commandId:000}");

            currentDistanceValue = distanceValue;
            this.distanceDifferencesSubject.OnNext(difference);

            currentCommandId = commandId;
            commandIdChangesSubject.OnNext(commandId);
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