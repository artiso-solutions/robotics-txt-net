using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    public class MotorConfiguration
    {
        public Motor Motor { get; set; }

        public DigitalInput ReferencingInput { get; set; }

        public Direction ReferencingDirection { get; set; }

        public Speed ReferencingSpeed { get; set; }

        public bool ReferencingInputState { get; set; }
    }
}