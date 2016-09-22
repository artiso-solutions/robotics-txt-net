using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Communicator
{
    internal class DigitalInputInfo
    {
        private readonly Subject<bool> stateChangesSubject;

        public DigitalInputInfo(DigitalInput digitalInput)
        {
            DigitalInput = digitalInput;
            this.stateChangesSubject = new Subject<bool>();
        }

        public DigitalInput DigitalInput { get; }

        public bool CurrentState { get; private set; }

        public IObservable<bool> StateChanges => this.stateChangesSubject.AsObservable();

        public void SetNewState(bool newState)
        {
            if (this.CurrentState != newState)
            {
                this.CurrentState = newState;
                this.stateChangesSubject.OnNext(newState);
            }
        }
    }
}