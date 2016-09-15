namespace RoboticsTxt.Lib.Messages
{
    public class CommandIds
    {
        public const uint SendQueryStatus = 0xDC21219A;
        public const uint ReceiveQueryStatus = 0xBAC9723E;

        public const uint SendStartOnline = 0x163FF61D;
        public const uint ReceiveStartOnline = 0xCA689F75;

        public const uint SendUpdateConfig = 0x060EF27E;
        public const uint ReceiveUpdateConfig = 0x9689A68C;

        public const uint SendExchangeData = 0xCC3597BA;
        public const uint ReceiveExchangeData = 0x4EEFAC41;

        public const uint SendExchangeDataCmpr = 0xFBC56F98;
        public const uint ReceiveExchangeDataCmpr = 0x6F3B54E6;

        public const uint SendStopOnline = 0x9BE5082C;
        public const uint ReceiveStopOnline = 0xFBF600D2;
    }
}