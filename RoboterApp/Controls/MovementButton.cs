using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using RoboterApp.Commands;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Controls
{
    public class MovementButton : RepeatButton
    {
        static MovementButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MovementButton), new FrameworkPropertyMetadata(typeof(MovementButton)));
        }

        public static readonly DependencyProperty MoveAxisCommandProperty = DependencyProperty.Register(
            "MoveAxisCommand", typeof(ContinuousMoveAxisCommand), typeof(MovementButton), new PropertyMetadata(default(ContinuousMoveAxisCommand)));

        private Speed currentSpeed;
        private short currentDistance;

        public ContinuousMoveAxisCommand MoveAxisCommand
        {
            get { return (ContinuousMoveAxisCommand)GetValue(MoveAxisCommandProperty); }
            set { SetValue(MoveAxisCommandProperty, value); }
        }

        public MovementButton()
        {
            currentSpeed = Speed.Off;
            currentDistance = -1;

            Interval = 100;

            PreviewMouseMove += OnPreviewMouseMove;
            Click += ClickHandler;
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            var relative = position.Y / this.ActualHeight;

            if (relative < 0.5)
            {
                currentSpeed = Speed.Maximal;
                currentDistance = 50;
            }
            else
            {
                currentSpeed = Speed.Maximal;
                currentDistance = 0;
            }
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);

            if (!IsPressed)
            {
                MoveAxisCommand?.OnStop();
            }
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            MoveAxisCommand?.OnMove(this.currentSpeed, this.currentDistance);
        }
    }
}