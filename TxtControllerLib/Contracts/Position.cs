using System.Collections.Generic;

namespace RoboticsTxt.Lib.Contracts
{
    public class Position
    {
        public Position()
        {
            this.MotorPositionInfos = new List<MotorPositionInfo>();
        }

        public string PositionName { get; set; }

        public List<MotorPositionInfo> MotorPositionInfos { get; set; }
    }
}
