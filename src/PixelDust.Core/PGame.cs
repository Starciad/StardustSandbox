using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Reflection;

using PixelDust.Core.Managers;
using PixelDust.Core.Scenes;
using PixelDust.Core.Engine;
using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;
using PixelDust.Core.GUI;

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
                PreferredBackBufferWidth = (int)PScreen.Resolutions[3].X,
                PreferredBackBufferHeight = (int)PScreen.Resolutions[3].Y,
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
            PCamera.Initialize();
            PScreen.Initialize();
            OnAwake();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Assets
            PGraphics.Load();
            PTextures.Load();
            PFonts.Load();
            PElementManager.Load();
            PEffects.Load();

            PGUIEngine.Initialize();
            OnStartup();
        }

        protected override void BeginRun()
        {
            PWorld.Initialize();
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
            PManagerPool.Update();

            // GUI
            PGUIEngine.Update();

            // Scenes & World
            PSceneManager.Update();
            PWorld.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            PTime.Draw(gameTime);
            PScreen.BeginDraw();

            // ==================== //
            // RENDER TARGET
            PGraphics.GraphicsDevice.SetRenderTarget(PGraphics.DefaultRenderTarget);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            // WORLD
            if (PWorld.States.IsActive)
            {
                PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PCamera.GetMatrix());
                PWorld.Draw();
                PGraphics.SpriteBatch.End();
            }

            // SCENE
            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PCamera.GetMatrix());
            PSceneManager.Draw();
            PGraphics.SpriteBatch.End();

            // GUI
            PGUIEngine.Draw();

            // ==================== //
            // RENDER (RENDER TARGETS)

            PGraphics.GraphicsDevice.SetRenderTarget(null);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, PEffects.Effects["Global"], PCamera.GetMatrix());
            PGraphics.SpriteBatch.Draw(PGraphics.DefaultRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            PGraphics.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            PWorld.Unload();
            PTextures.Unload();
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
