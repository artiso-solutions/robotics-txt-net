﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    /// <summary>
    /// The <see cref="IControllerSequencer"/> provides high level methods for operation of the Fischertechnik ROBOTICS TXT controller.
    /// This includes the operation of motors and inputs of any kind.
    /// </summary>
    public interface IControllerSequencer : IDisposable
    {
        /// <summary>
        /// Starts the specified <paramref name="motor"/> immediately.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to start.</param>
        void StartMotor(Motor motor, Speed speed, Direction direction);

        /// <summary>
        /// Stops the specified <paramref name="motor"/> immediately.
        /// </summary>
        /// <param name="motor">The motor to stop.</param>
        void StopMotor(Motor motor);

        /// <summary>
        /// Starts the specified <paramref name="motor"/> and stops it on state trigger of the specified <paramref name="digitalInput"/>.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to start.</param>
        /// <param name="digitalInput">The digital input to trigger the stop.</param>
        /// <param name="expectedInputState">The expected value for the state trigger.</param>
        /// <returns>This method is async. The returned task will be completed as soon as the movement is finished.</returns>
        Task<bool> StartMotorStopWithDigitalInputAsync(Motor motor, Speed speed, Direction direction, DigitalInput digitalInput, bool expectedInputState, TimeSpan? timeout = null);

        /// <summary>
        /// Starts the specified <paramref name="motor"/> and stops it after the given time span <paramref name="stopAfterTimeSpan"/>.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="direction">The direction to start.</param>
        /// <param name="stopAfterTimeSpan">The time span which is used to stop the motor again.</param>
        /// <returns>This method is async. The returned task will be completed as soon as the movement is finished.</returns>
        Task StartMotorStopAfterTimeSpanAsync(Motor motor, Speed speed, Direction direction, TimeSpan stopAfterTimeSpan);

        /// <summary>
        /// Retrieves the current input state of the specified <paramref name="referenceInput"/>.
        /// </summary>
        /// <param name="referenceInput">The digital input to get the state from.</param>
        /// <returns><c>true</c> if the input is triggered, otherwise <c>false</c>.</returns>
        bool GetDigitalInputState(DigitalInput referenceInput);

        /// <summary>
        /// Gets the event stream for changes of the state of digital inputs. This can be used to observe the current state or react on state changes.
        /// </summary>
        /// <param name="digitalInput">The digital input to get the stream of changes.</param>
        /// <returns>Observable stream of events representing the current state of the digital input.</returns>
        IObservable<bool> GetDigitalInputStateChanges(DigitalInput digitalInput);

        /// <summary>
        /// Saves the current position of all saveable <see cref="MotorPositionController"/>s.
        /// </summary>
        /// <param name="positionName">Name of the position to be saved.</param>
        void SaveCurrentPosition(string positionName);

        /// <summary>
        /// Moves all <see cref="MotorPositionController"/>s to the positions given in the position.
        /// </summary>
        /// <param name="positionName">Name of the position to be applied.</param>
        Task MoveToPositionAsync(string positionName);

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="repetitions">Number of times to play the sound. 0 means indefinite.</param>
        void PlaySound(Sound sound, ushort repetitions);

        /// <summary>
        /// Stops the current sound.
        /// </summary>
        void StopSound();

        /// <summary>
        /// Cleanup of all resrouces. This also stops the communication to the controller.
        /// </summary>
        void Dispose();

        MotorPositionController ConfigureMotorPositionController(MotorConfiguration motorConfiguration);
        void ReleaseMotorPositionController(MotorPositionController motorPositionController);
        List<string> GetPositionNames();
    }
}