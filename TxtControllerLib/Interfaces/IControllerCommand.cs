using artiso.Fischertechnik.TxtController.Lib.Messages;

namespace artiso.Fischertechnik.TxtController.Lib.Interfaces
{
    public interface IControllerCommand
    {
        void ApplyMessageChanges(ExchangeDataCommandMessage message);
    }
}
