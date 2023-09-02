using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

using System;

namespace PixelDust.Core.Scenes
{
    /// <summary>
    /// Static class responsible for handling scenes in the project.
    /// </summary>
    public static class PScenesHandler
    {
        private static PScene _sceneInstance;

        /// <summary>
        /// Loads a new instance of a specified scene type, unloading the previous scene if one exists.
        /// </summary>
        /// <typeparam name="T">The type of the scene to load.</typeparam>
        public static void Load<T>() where T : PScene
        {
            _sceneInstance?.Unload();

            _sceneInstance = Activator.CreateInstance<T>();
            _sceneInstance.Load();
        }

        /// <summary>
        /// Gets the currently active scene instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the scene to retrieve.</typeparam>
        /// <returns>The currently active scene instance.</returns>
        public static T GetCurrentScene<T>() where T : PScene
        {
            // Cast and return the current scene instance as the specified type
            return (T)_sceneInstance;
        }

        /// <summary>
        /// Updates the currently active scene, if one exists.
        /// </summary>
        internal static void Update()
        {
            _sceneInstance?.Update();
        }

        /// <summary>
        /// Draws the currently active scene, if one exists.
        /// </summary>
        internal static void Draw()
        {
            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            _sceneInstance?.Draw();
            PGraphics.SpriteBatch.End();
        }
    }
}