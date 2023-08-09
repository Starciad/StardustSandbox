using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Reflection;

using PixelDust.Core.Managers;
using PixelDust.Core.Scenes;
using PixelDust.Core.Engine;
using PixelDust.Core.Elements;

namespace PixelDust.Core
{
    public abstract class PEngineInstance : Game
    {
        public Assembly Assembly { get; private set; }

        public PEngineInstance()
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

        protected override void Update(GameTime gameTime)
        {
            PInput.Update();
            PManagerPool.Update();
            PSceneManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float scale = 1f / (PScreen.DefaultWidth / PGraphics.Viewport.Height);

            // DRAW GAME ELEMENTS
            PGraphics.GraphicsDevice.SetRenderTarget(PGraphics.RenderTarget);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred);
            PSceneManager.Draw();
            PGraphics.SpriteBatch.End();

            // DRAW RENDER TARGET
            PGraphics.GraphicsDevice.SetRenderTarget(null);
            PGraphics.GraphicsDevice.Clear(Color.Black);

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred);
            PGraphics.SpriteBatch.Draw(PGraphics.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            PGraphics.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected virtual void OnAwake() { return; }
        protected virtual void OnStartup() { return; }
    }
}
