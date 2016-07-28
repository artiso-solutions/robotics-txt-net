namespace artiso.Fischertechnik.TxtController.Lib.Messages.Base
{
    public class DeserializationContext
    {
        public DeserializationContext(byte[] buffer)
        {
            Buffer = buffer;
            CurrentPosition = 0;
        }

        public byte[] Buffer { get; }

        public int CurrentPosition { get; set; }
    }
}