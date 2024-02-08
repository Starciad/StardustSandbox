using Microsoft.Xna.Framework;

namespace PixelDust.Core.Engine.Components
{
    /// <summary>
    /// Static class responsible for storing information related to project update and rendering times.
    /// </summary>
    public static class PTime
    {
        /// <summary>
        /// Gets the <see cref="GameTime"/> object representing the time spent during the update phase of the game loop.
        /// </summary>
        public static GameTime UpdateGameTime => _updateGameTime;

        /// <summary>
        /// Gets the <see cref="GameTime"/> object representing the time spent during the draw phase of the game loop.
        /// </summary>
        public static GameTime DrawGameTime => _drawGameTime;

        private static GameTime _updateGameTime;
        private static GameTime _drawGameTime;

        /// <summary>
        /// Internal method to update the GameTime for the update phase of the game loop.
        /// </summary>
        /// <param name="value">The GameTime object representing the update phase time.</param>
        internal static void Update(GameTime value)
        {
            _updateGameTime = value;
        }

        /// <summary>
        /// Internal method to update the GameTime for the draw phase of the game loop.
        /// </summary>
        /// <param name="value">The GameTime object representing the draw phase time.</param>
        internal static void Draw(GameTime value)
        {
            _drawGameTime = value;
        }
    }
}
