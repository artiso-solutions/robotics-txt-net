using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Components
{
    internal class ResponseProcessor
    {
        public void ProcessResponse(ExchangeDataResponseMessage responseMessage, DigitalInputInfo[] universalInputs)
        {
            for (int i = 0; i < 8; i++)
            {
                var newState = responseMessage.UniversalInputs[i] == 0;
                universalInputs[i].SetNewState(newState);
            }
        }
    }
}
