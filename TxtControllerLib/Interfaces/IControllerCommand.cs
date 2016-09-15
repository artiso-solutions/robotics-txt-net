using artiso.Fischertechnik.TxtController.Lib.Messages;

namespace artiso.Fischertechnik.TxtController.Lib.Interfaces
{
    internal interface IControllerCommand
    {
        void ApplyMessageChanges(ExchangeDataCommandMessage message);
    }
}
