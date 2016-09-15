using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    internal class DigitalInputInfo
    {
        private readonly Subject<bool> stateChangesSubject;

        public DigitalInputInfo()
        {
            this.stateChangesSubject = new Subject<bool>();
        }

        public bool CurrentState { get; set; }

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