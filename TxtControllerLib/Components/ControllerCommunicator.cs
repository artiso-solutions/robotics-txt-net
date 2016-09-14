using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using artiso.Fischertechnik.TxtController.Lib.Interfaces;
using artiso.Fischertechnik.TxtController.Lib.Messages;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    public class ControllerCommunicator
    {
        private readonly object commandMessageLock;
        private ExchangeDataCommandMessage currentCommandMessage;

        private readonly object commandQueueLock;
        private readonly Queue<IControllerCommand> commandQueue;

        private Task communicationLoopTask;
        private Task commandProcessingTask;

        public ControllerCommunicator()
        {
            this.commandMessageLock = new object();
            this.currentCommandMessage = new ExchangeDataCommandMessage();

            this.commandQueueLock = new object();
            this.commandQueue = new Queue<IControllerCommand>();

            this.communicationLoopTask = new Task(this.CommunicationLoop);
            this.commandProcessingTask = new Task(this.CommandProcessingLoop);
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void QueueCommand(IControllerCommand command)
        {
            lock (this.commandQueueLock)
            {
                this.commandQueue.Enqueue(command);
            }
        }

        private void CommunicationLoop()
        {
            throw new NotImplementedException();
        }

        private void CommandProcessingLoop()
        {
            throw new NotImplementedException();
        }
    }
}
