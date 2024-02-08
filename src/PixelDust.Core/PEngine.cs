using System;
using System.Reflection;
using System.Threading;

namespace PixelDust.Core
{
    /// <summary>
    /// Static class of PixelDust's main engine, responsible for controlling and storing important states of the game, in addition to having methods for general manipulation of the game's execution.
    /// </summary>
    public static class PEngine
    {
        /// <summary>
        /// Current project assembly where the <see cref="PEngine"/> class is located.
        /// </summary>
        public static Assembly Assembly => _assembly;

        /// <summary>
        /// Main instance of the <see cref="PGame"/> class that is being executed and manipulated by the engine.
        /// </summary>
        public static PGame Instance => _instance;

        /// <summary>
        /// Responsible for containing information related to the current running state of the game. If game stop is requested, the cancellation token will be activated.
        /// </summary>
        public static CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private static Assembly _assembly;
        private static PGame _instance;
        private static readonly CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Configure (based on a generic type that inherits from the <see cref="PGame"/> class) the class that will be instantiated and manipulated by <see cref="PEngine"/> throughout the life cycle of the game process.
        /// </summary>
        /// <typeparam name="T">Generic type that inherits from the <see cref="PGame"/> class that will be used by the <see cref="PEngine"/>.</typeparam>
        public static void SetGameInstance<T>() where T : PGame, new()
        {
            _instance = new T();
            _assembly = typeof(PEngine).Assembly;
        }

        /// <summary>
        /// Makes a request for the engine to start the instance defined and created in <see cref="SetGameInstance{T}"/> method.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        public static void Start()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("The Start method was invoked correctly, however it was not possible to find any defined instance to execute. Did you remember to call the SetGameInstance<T>() method before executing Start()?");
            }

            _instance.Run();
        }

        /// <summary>
        /// Makes a request to close the currently running game instance.
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        public static void Stop()
        {
            if (_instance == null || !_instance.IsActive)
            {
                throw new InvalidOperationException("A game stop request was initiated, but no instance was created or executed to complete this action.");
            }

            _cancellationTokenSource.Cancel();
            _instance.Exit();
        }
    }
}
