using System;
using artiso.Fischertechnik.TxtController.Lib.Messages;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    public class ResponseProcessor
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
