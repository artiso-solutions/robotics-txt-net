using System;
using System.IO;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TxtControllerLibTests
{
    [TestClass]
    public class AssemblyLevelInitialization
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var assemblyLocation = typeof(TcpControllerDriverTests).Assembly.Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                throw new NotSupportedException("Failed to get log4net config");
            }

            var folder = Path.GetDirectoryName(assemblyLocation);
            if (string.IsNullOrEmpty(folder))
            {
                throw new NotSupportedException("Failed to get log4net config");
            }

            var configFile = new FileInfo(Path.Combine(folder, "log4net.config"));
            if (!configFile.Exists)
            {
                throw new NotSupportedException($"Missing log4net.config in folder {folder}");
            }

            XmlConfigurator.ConfigureAndWatch(configFile);
        }
    }
}