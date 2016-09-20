using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Interfaces
{
    internal interface IControllerCommand
    {
        void ApplyMessageChanges(ExchangeDataCommandMessage message);
    }
}