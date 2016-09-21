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

        public void SetCurrentDistanceValue(short distanceValue, short commandId, short counterCommandId)
        {
            if (distanceValue == 0 && currentDistanceValue > 0)
            {
                currentDistanceValue = 0;
                logger.Info("reset currentDistance");
                return;
            }
            
            if (currentDistanceValue == distanceValue && currentCommandId == commandId)
            {
                return;
            }
            
            var difference = distanceValue - currentDistanceValue;

            logger.Info($"d={distanceValue:000} - diff={difference:000} - c={commandId:000}");

            currentDistanceValue = distanceValue;
            distanceChangesSubject.OnNext(difference);

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