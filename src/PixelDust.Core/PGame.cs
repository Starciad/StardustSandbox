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
    /// Base game class used as a reference by the engine to run PixelDust.
    /// </summary>
    public abstract class PGame : Game
    {
        /// <summary>
        /// Assembly of the current class that is inheriting <see cref="PGame"/>.
        /// </summary>
        public Assembly Assembly { get; private set; }

        public PGame()
        {
            PGraphics.Build(new(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = PScreen.DefaultWidth,
                PreferredBackBufferHeight = PScreen.DefaultHeight,
                SynchronizeWithVerticalRetrace = false,
            });
            PContent.Build(Content);

            // Content
            Content.RootDirectory = "Content";

            // Assembly
            Assembly = GetType().Assembly;

            // Window
            Window.Title = "PixelDust - v0.0.1";
            Window.AllowUserResizing = true;
            Window.IsBorderless = false;

            // Settings
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
        }

        protected override void Initialize()
        {
            PManagerPool.Initialize();
            OnAwake();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            PGraphics.Load();
            PTextures.Load();
            PFonts.Load();
            PElementManager.Load();

            OnStartup();
        }

        protected override void BeginRun()
        {
            PWorld.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            PInput.Update();
            PManagerPool.Update();
            PSceneManager.Update();

            PWorld.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float scale = 1f / (PScreen.DefaultWidth / PGraphics.Viewport.Height);

            // ==================== //
            // RENDER TARGET
            PGraphics.GraphicsDevice.SetRenderTarget(PGraphics.DefaultRenderTarget);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred);
            PWorld.Draw();
            PSceneManager.Draw();
            PGraphics.SpriteBatch.End();

            // ==================== //
            // RENDER (RENDER TARGETS)

            PGraphics.GraphicsDevice.SetRenderTarget(null);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred);
            PGraphics.SpriteBatch.Draw(PGraphics.DefaultRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            PGraphics.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Invoked during program startup, after building sensitive aspects of the engine.
        /// </summary>
        protected virtual void OnAwake() { return; }

        /// <summary>
        /// Invoked right after engine initialization and main game asset loading. Called before the game's first frame refresh.
        /// </summary>
        protected virtual void OnStartup() { return; }
    }
}
