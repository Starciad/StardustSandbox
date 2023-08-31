using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Reflection;

using PixelDust.Core.Managers;
using PixelDust.Core.Scenes;
using PixelDust.Core.Engine;
using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;

namespace PixelDust.Core
{
    /// <summary>
    /// <see cref="PGame"/> is a PixelDust game base class that is configured and run as a core component by the <see cref="PEngine"/> class. In it, it is possible to find all the relevant information about the processing of the game, among other forms of configurations, executions and processing.
    /// </summary>
    /// <remarks>
    /// <see cref="PGame"/> is a wrapper of MonoGame's <see cref="Game"/> class that abstracts several functions and automates some things to favor the execution of the PixelDust game, with the creation of managers, definition of textures and fonts, among others.
    /// </remarks>
    public abstract class PGame : Game
    {
        /// <summary>
        /// Assembly that references the project that the <see cref="PGame"/> class is part of.
        /// </summary>
        public Assembly Assembly => _assembly;

        private readonly Assembly _assembly;

        /// <summary>
        /// It builds in a standardized and automated way the basic components for the instantiation and execution of <see cref="PGame"/>.
        /// </summary>
        public PGame()
        {
            PGraphics.Build(new(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = (int)PScreen.DefaultResolution.X,
                PreferredBackBufferHeight = (int)PScreen.DefaultResolution.Y,
                SynchronizeWithVerticalRetrace = false,
            });

            // Content
            PContent.Build(Services, "Content");

            // Assembly
            _assembly = GetType().Assembly;

            // Window
            Window.Title = "PixelDust - v0.0.1";
            Window.AllowUserResizing = false;
            Window.IsBorderless = false;

            // Settings
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / PGraphics.FPS);
        }

        protected override void Initialize()
        {
            PManagersHandler.Initialize();
            OnAwake();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Assets
            PGraphics.Load();
            PTextures.Load();
            PFonts.Load();
            PElementsHandler.Load();
            PEffects.Load();

            OnStartup();
        }
        protected override void Update(GameTime gameTime)
        {
            // Time
            PTime.Update(gameTime);

            // Shaders
            PEffects.Update();

            // Inputs
            PInput.Update();

            // Managers
            PManagersHandler.Update();

            // Scenes & World
            PScenesHandler.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            PTime.Draw(gameTime);

            // ==================== //
            // RENDER TARGET
            PGraphics.GraphicsDevice.SetRenderTarget(PGraphics.DefaultRenderTarget);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            // Managers
            PManagersHandler.Draw();

            // SCENE
            PScenesHandler.Draw();

            // ==================== //
            // RENDER (RENDER TARGETS)

            PGraphics.GraphicsDevice.SetRenderTarget(null);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            PGraphics.SpriteBatch.Draw(PGraphics.DefaultRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            PGraphics.SpriteBatch.End();

            base.Draw(gameTime);
        }
        protected override void UnloadContent()
        {
            PContent.Unload();
            PTextures.Unload();
        }

        /// <summary>
        /// Called before the <see cref="OnStartup"/> method is executed during game initialization.
        /// </summary>
        protected virtual void OnAwake() { return; }

        /// <summary>
        /// Called after the <see cref="OnAwake"/> method, it is executed after all engine settings and before the first game update.
        /// </summary>
        protected virtual void OnStartup() { return; }
    }
}
