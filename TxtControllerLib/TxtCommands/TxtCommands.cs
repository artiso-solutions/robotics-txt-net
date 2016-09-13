
namespace artiso.Fischertechnik.TxtController.Lib.TxtCommands
{
   using System;
   using System.ComponentModel;
   using System.Linq;
   using System.Threading;

   using Configuration;
   using Contracts;
   using ControllerDriver;
   using Messages;

   public static class TxtCommands
   {
      #region Constants and Fields

      private static readonly BackgroundWorker[] motorBackgroundWorkers;

      private static TcpControllerDriver tcpControllerDriver;

      #endregion

      #region Constructors and Destructors

      static TxtCommands()
      {
         motorBackgroundWorkers = new BackgroundWorker[Enum.GetValues(typeof(Motor)).Length];
      }

      #endregion

      internal static TcpControllerDriver TcpControllerDriver
      {
         get
         {
            if (tcpControllerDriver != null)
            {
               return tcpControllerDriver;
            }

            tcpControllerDriver = new TcpControllerDriver(Communication.USB);
            tcpControllerDriver.StartCommunication();
            tcpControllerDriver.SendCommand(new StartOnlineCommandMessage());
            tcpControllerDriver.SendCommand(new UpdateConfigCommandMessage
            {
               ConfigId = 0,
               MotorModes = new[] { MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2, MotorMode.O1O2 },
               InputConfigurations = Enumerable.Repeat(new InputConfiguration { InputMode = InputMode.Resistance, IsDigital = true }, 8).ToArray(),
               CounterModes = new[] { CounterMode.Normal, CounterMode.Normal, CounterMode.Normal, CounterMode.Normal }
            });

            return tcpControllerDriver;
         }
      }

      #region Public Methods and Operators

      public static int[] ReadCounterValues()
      {
         throw new NotImplementedException();
      }

      public static int[] ReadInputValues()
      {
         throw new NotImplementedException();
      }

      public static void StartMotor(Motor motor, Speed speed, Movement movement)
      {
         var index = (int)motor;
         if (index > motorBackgroundWorkers.Length)
         {
            throw new ArgumentOutOfRangeException(nameof(motor), motor, "Motor is out of the defined range");
         }

         if (motorBackgroundWorkers[index] == null)
         {
            motorBackgroundWorkers[index] = new BackgroundWorker { WorkerSupportsCancellation = true };
            motorBackgroundWorkers[index].DoWork += StartMotorDoWork;
         }

         while (motorBackgroundWorkers[index].IsBusy)
         {
            motorBackgroundWorkers[index].CancelAsync();
            Thread.Sleep(10);
         }

         motorBackgroundWorkers[index].RunWorkerAsync(new MotorParameters { Motor = motor, Speed = speed, Movement = movement });
      }

      public static void StartMotorLeft(Motor motor, Speed speed)
      {
         StartMotor(motor, speed, Movement.Left);
      }

      public static void StartMotorRight(Motor motor, Speed speed)
      {
         StartMotor(motor, speed, Movement.Right);
      }

      public static void StopMotor(Motor motor)
      {
         var index = (int)motor;
         if (index > motorBackgroundWorkers.Length)
         {
            throw new ArgumentOutOfRangeException(nameof(motor), motor, "Motor is out of the defined range");
         }

         if (motorBackgroundWorkers[index] == null)
         {
            return;
         }

         while (motorBackgroundWorkers[index].IsBusy)
         {
            motorBackgroundWorkers[index].CancelAsync();
            Thread.Sleep(10);
         }
      }

      #endregion

      #region Methods

      private static void StartMotorDoWork(object sender, DoWorkEventArgs e)
      {
         var worker = sender as BackgroundWorker;
         if (worker == null)
         {
            return;
         }

         if (!(e.Argument is MotorParameters))
         {
            return;
         }

         var parameters = (MotorParameters)e.Argument;

         while (!worker.CancellationPending)
         {

         }
      }

      #endregion

      private struct MotorParameters
      {
         #region Constants and Fields

         internal Motor Motor;

         internal Speed Speed;

         internal Movement Movement;

         #endregion
      }
   }
}