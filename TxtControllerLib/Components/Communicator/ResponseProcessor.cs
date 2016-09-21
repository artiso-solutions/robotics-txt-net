using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Components.Communicator
{
    internal class ResponseProcessor
    {
        public void ProcessResponse(ExchangeDataResponseMessage responseMessage, DigitalInputInfo[] universalInputs, MotorDistanceInfo[] motorDistanceInfos)
        {
            for (int i = 0; i < 8; i++)
            {
                var newState = responseMessage.UniversalInputs[i] == 0;
                universalInputs[i].SetNewState(newState);
            }

            for (int i = 0; i < 4; i++)
            {
                motorDistanceInfos[i].SetCurrentDistanceValue(responseMessage.CounterValue[i], responseMessage.MotorCommandId[i], responseMessage.CounterCommandId[i]);
            }
        }
    }
}
