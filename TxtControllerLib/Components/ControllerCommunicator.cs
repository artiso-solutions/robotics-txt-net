using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using artiso.Fischertechnik.TxtController.Lib.Configuration;
using artiso.Fischertechnik.TxtController.Lib.Contracts;
using artiso.Fischertechnik.TxtController.Lib.ControllerDriver;
using artiso.Fischertechnik.TxtController.Lib.Interfaces;
using artiso.Fischertechnik.TxtController.Lib.Messages;
using log4net;
using log4net.Util;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    internal class ControllerCommunicator
    {
        private readonly ILog logger;

        private readonly CommandProcessor commandProcessor;
        private readonly ResponseProcessor responseProcessor;

        private readonly ConcurrentQueue<IControllerCommand> commandQueue;

        private Task communicationLoopTask;
        private CancellationTokenSource cancellationTokenSource;

        public ControllerCommunicator()
        {
            this.logger = LogManager.GetLogger(typeof(ControllerCommunicator));

            this.commandProcessor = new CommandProcessor();
            this.responseProcessor = new ResponseProcessor();

            this.commandQueue = new ConcurrentQueue<IControllerCommand>();

            this.UniversalInputs = new DigitalInputInfo[8];

            for (int i = 0; i < this.UniversalInputs.Length; i++)
            {
                this.UniversalInputs[i] = new DigitalInputInfo();
            }
        }

        public void Start()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            var token = this.cancellationTokenSource.Token;

            this.communicationLoopTask
                = new Task(() => this.CommunicationLoop(token), this.cancellationTokenSource.Token);

            this.communicationLoopTask.Start();
        }

        public void Stop()
        {
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource.Dispose();
        }

        public void QueueCommand(IControllerCommand command)
        {
            logger.DebugExt($"Queue command {command.GetType().Name}");
            this.commandQueue.Enqueue(command);
        }

        public DigitalInputInfo[] UniversalInputs { get; set; }

        private void CommunicationLoop(CancellationToken cancellationToken)
        {
            var driver = new TcpControllerDriver(Communication.USB); // TODO dynamic comm-mode?
            var currentCommandMessage = new ExchangeDataCommandMessage();

            driver.StartCommunication();
            driver.SendCommand(new StartOnlineCommandMessage());

            var configMessage = new UpdateConfigCommandMessage
            {
                ConfigId = 0,
                MotorModes = new[] { MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2 },
                InputConfigurations =
                    Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8)
                        .ToArray(),
                CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
            }; // TODO init config?
            driver.SendCommand(configMessage);

            while (!cancellationToken.IsCancellationRequested || !this.commandQueue.IsEmpty)
            {
                IControllerCommand command;
                while (this.commandQueue.TryDequeue(out command))
                {
                    try
                    {
                        this.logger.DebugExt($"Process {command.GetType().Name}");
                        this.commandProcessor.ProcessControllerCommand(command, currentCommandMessage, UniversalInputs);
                    }
                    catch (Exception exception)
                    {
                        logger.Error(exception);
                        // TODO ???
                        throw;
                    }
                }

                this.logger.DebugExt("Send current message to controller");
                var response = driver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(currentCommandMessage);

                this.responseProcessor.ProcessResponse(response, this.UniversalInputs);

                Thread.Sleep(10);
            }

            driver.SendCommand(new StopOnlineCommandMessage());
        }
    }
}
