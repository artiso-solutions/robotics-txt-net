using artiso.Fischertechnik.RoboTxt.Lib.xxxObsoletexxx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace RoboTxtLibTests
{
    //[TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void ReceiveQueryStatus()
        {
            using (var communicationManager = PrepareCommunicationManager())
            {
                var controllerStatus = communicationManager.QueryStatus();
                Assert.IsNotNull(controllerStatus, "Expected controller status");
                Assert.AreEqual("TX2013", controllerStatus.Name);
                Assert.AreEqual(new Version(4, 2, 3, 0), controllerStatus.Version);
            }
        }

        [TestMethod]
        public void StartOnlineMode()
        {
            using (var communicationManager = PrepareCommunicationManager())
            {
                communicationManager.StartOnlineMode();
            }
        }

        [TestMethod]
        public void StopOnlineMode()
        {
            using (var communicationManager = PrepareCommunicationManager())
            {
                communicationManager.StopOnlineMode();
            }
        }

        [TestMethod]
        public void UpdateConfig()
        {
            using (var communicationManager = PrepareCommunicationManager())
            {
                communicationManager.StartOnlineMode();

                try
                {
                    communicationManager.UpdateConfig();
                }
                finally
                {
                    communicationManager.StopOnlineMode();
                }
            }
        }

        [TestMethod]
        public void TurnRobotLeftAndRight()
        {
            using (var communicationManager = PrepareCommunicationManager())
            {
                communicationManager.StartOnlineMode();

                try
                {
                    communicationManager.UpdateConfig();

                    var endTime = DateTime.Now.AddSeconds(3);
                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        communicationManager.StartMotorLeft(0, 256, 0);
                    }
                    communicationManager.StopMotor(0);

                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    endTime = DateTime.Now.AddSeconds(3);
                    for (var t = DateTime.Now; t < endTime; t = DateTime.Now)
                    {
                        communicationManager.StartMotorRight(0, 256, 0);
                    }
                    communicationManager.StopMotor(0);
                }
                finally
                {
                    communicationManager.StopOnlineMode();
                }
            }
        }


        private CommunicationManager PrepareCommunicationManager()
        {
            var communicationManager = new CommunicationManager();
            communicationManager.StartCommunication();
            return communicationManager;
        }
    }
}
