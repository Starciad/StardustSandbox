using System.Threading;

namespace PixelDust.Core
{
    /// <summary>
    /// Static class that manages most of the aspects that govern the PixelDust game, with general functionalities of status, performances, information and higher level actions.
    /// </summary>
    public static class PEngine
    {
        /// <summary>
        /// Current instance of the game being managed and observed by the Engine.
        /// </summary>
        public static PGame Instance => _instance;

        /// <summary>
        /// Current game state token.
        /// </summary>
        /// <remarks>
        /// Indicates if the game is still running, otherwise the cancellation will be activated.
        /// </remarks>
        public static CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private static PGame _instance;
        private static readonly CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Configures, based on a generic type, the instance that will be used by the engine to create and run the base game class.
        /// </summary>
        /// <typeparam name="T">The generic game type that will be used by the engine.</typeparam>
        public static void SetGameInstance<T>() where T : PGame, new()
        {
            _instance = new T();
        }

        /// <summary>
        /// Initializes the game instance.
        /// </summary>
        /// <remarks>
        /// Before starting the instance, it is necessary to configure it with the <see cref="SetGameInstance{T}"/>.
        /// </remarks>
        public static void Start()
        {
            _instance?.Run();
        }

        /// <summary>
        /// Halts the game and starts the full game stop process.
        /// </summary>
        /// <remarks>
        /// Use it when you need to close the game.
        /// </remarks>
        public static void Stop()
        {
            _cancellationTokenSource.Cancel();
            _instance.Exit();
        }
    }
}
