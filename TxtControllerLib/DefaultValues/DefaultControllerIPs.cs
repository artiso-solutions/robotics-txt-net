using System.Net;

namespace RoboticsTxt.Lib.DefaultValues
{
    public static class DefaultControllerIPs
    {
        public static IPAddress DefaultUSBAddress => IPAddress.Parse("192.168.7.2");

        public static IPAddress DefaultWLanAddress => IPAddress.Parse("192.168.8.2");

        public static IPAddress DefaultBluetoothAddress => IPAddress.Parse("192.168.9.2");
    }
}