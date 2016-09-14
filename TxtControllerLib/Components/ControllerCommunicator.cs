﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
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
    public class ControllerCommunicator
    {
        private readonly ILog logger;

        private readonly CommandProcessor commandProcessor;
        private readonly ResponseProcessor responseProcessor;

        private readonly ConcurrentQueue<IControllerCommand> commandQueue;

        private Task communicationLoopTask;
        private CancellationTokenSource cancellationTokenSource;

        public ControllerCommunicator()
        {
            this.logger = LogManager.GetLogger(typeof (ControllerCommunicator));

            this.commandProcessor = new CommandProcessor();
            this.responseProcessor = new ResponseProcessor();

            this.commandQueue = new ConcurrentQueue<IControllerCommand>();
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
            this.commandQueue.Enqueue(command);
        }

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

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var command in this.commandQueue)
                {
                    try
                    {
                        this.logger.DebugExt(command.GetType());
                        this.commandProcessor.ProcessControllerCommand(command, currentCommandMessage);
                    }
                    catch (Exception)
                    {
                        // TODO ???
                        throw;
                    }
                }

                var response = driver.SendCommand<ExchangeDataCommandMessage, ExchangeDataResponseMessage>(currentCommandMessage);

                //this.responseProcessor.ProcessResponse(response);

                Thread.Sleep(100);
            }

            driver.SendCommand(new StopOnlineCommandMessage());
        }
    }
}
