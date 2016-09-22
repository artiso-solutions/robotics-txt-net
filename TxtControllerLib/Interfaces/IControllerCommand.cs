using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Interfaces
{
    internal interface IControllerCommand
    {
        void Execute(ControllerCommunicator controllerCommunicator, ExchangeDataCommandMessage message);
    }
}