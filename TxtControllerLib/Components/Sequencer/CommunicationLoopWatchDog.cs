using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    internal class CommunicationLoopWatchDog
    {
        private readonly Subject<object> communicationLoopBlockSubject;

        private readonly IObservable<object> communicationLoopReactionEvents;

        private bool loopBlocking;
        private readonly object loopBlockingLock;

        private readonly CancellationTokenSource cancellationTokenSource;

        private IDisposable cycleTimeChangesSubscription;
        private Timer watchDogTimer;

        public CommunicationLoopWatchDog(IObservable<object> communicationLoopReactionEvents)
        {
            this.communicationLoopReactionEvents = communicationLoopReactionEvents;
            communicationLoopBlockSubject = new Subject<object>();
            loopBlockingLock = new object();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public IObservable<object> CommunicationLoopBlockEvents => communicationLoopBlockSubject.AsObservable();

        public void StartWatching()
        {
            cycleTimeChangesSubscription = communicationLoopReactionEvents.Subscribe(OnCommunicationLoopReaction);
            watchDogTimer = new Timer(CheckIfTimedOut, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
        }

        public void StopWatching()
        {
            cancellationTokenSource.Cancel();
            watchDogTimer.Dispose();
            cycleTimeChangesSubscription.Dispose();
        }

        private void OnCommunicationLoopReaction(object parameter)
        {
            lock (loopBlockingLock)
            {
                loopBlocking = false;
            }
        }

        private void CheckIfTimedOut(object state)
        {
            lock (loopBlockingLock)
            {
                if (loopBlocking)
                {
                    Task.Run(() => communicationLoopBlockSubject.OnNext(null));
                }
                else
                {
                    loopBlocking = true;
                }
            }
        }
    }
}