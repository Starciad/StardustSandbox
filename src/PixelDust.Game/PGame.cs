using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Worlding;
using PixelDust.Game.Constants;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Managers;
using PixelDust.Game.Scenes.Common;
using PixelDust.Game.Worlding;

using System.Reflection;

namespace PixelDust.Game
{
    public sealed class PGame : Microsoft.Xna.Framework.Game
    {
        public Assembly Assembly => this._assembly;
        public PGraphicsManager GraphicsManager => this._graphicsManager;
        public PGameInputManager GameInputManager => this._gameInputManager;
        public PScenesManager ScenesManager => this._scenesManager;
        public PShaderManager ShaderManager => this._shaderManager;
        public PAssetDatabase AssetDatabase => this._assetDatabase;
        public PElementDatabase ElementDatabase => this._elementDatabase;
        public PInputManager InputManager => this._inputManager;
        public PWorld World => this._world;

        // ================================= //

        private readonly Assembly _assembly;
        private SpriteBatch _sb;

        // Managers
        private readonly PGraphicsManager _graphicsManager;
        private readonly PGameInputManager _gameInputManager;
        private readonly PScenesManager _scenesManager;
        private readonly PShaderManager _shaderManager;
        private readonly PInputManager _inputManager;

        // Databases
        private readonly PAssetDatabase _assetDatabase;
        private readonly PElementDatabase _elementDatabase;

        // Core
        private readonly PWorld _world;

        // ================================= //

        public PGame()
        {
            this._graphicsManager = new(new(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = (int)PScreenConstants.SCREEN_WIDTH,
                PreferredBackBufferHeight = (int)PScreenConstants.SCREEN_HEIGHT,
                SynchronizeWithVerticalRetrace = false,
            });

            // Assembly
            this._assembly = GetType().Assembly;

            // Initialize Content
            this.Content.RootDirectory = PDirectoryConstants.ASSETS;

            // Configure the game's window
            this.Window.Title = PGameConstants.GetTitleAndVersionString();
            this.Window.AllowUserResizing = false;
            this.Window.IsBorderless = false;

            // Configure game settings
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = PGraphicsConstants.FramesPerSecond;

            // Database
            this._assetDatabase = new(this.Content);
            this._elementDatabase = new();

            // Managers
            this._inputManager = new();
            this._gameInputManager = new(this._inputManager, this._elementDatabase);
            this._scenesManager = new();
            this._shaderManager = new();

            // Game
            this._world = new(this._elementDatabase, this._assetDatabase.GetTexture("particle_1"));
        }

        protected override void Initialize()
        {
            #region Databases
            RegisterAllGameElements(this._elementDatabase);

            this._assetDatabase.Initialize(this);
            this._elementDatabase.Initialize(this);
            #endregion

            #region Managers
            this._graphicsManager.Initialize(this);
            this._gameInputManager.Initialize(this);
            this._scenesManager.Initialize(this);
            this._shaderManager.Initialize(this);
            #endregion

            #region Handlers
            this._inputManager.Initialize(this);
            #endregion

            #region Game
            this._world.Initialize(this);
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._sb = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this._scenesManager.Load<WorldScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            this._inputManager.Update(gameTime);
            this._world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            // GUI
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._sb.End();

            // BACKGROUND
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._sb.End();

            // WORLD
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PWorldCamera.Camera.GetViewMatrix());
            this._world.Draw(gameTime, this._sb);
            this._sb.End();

            // LIGHTING
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.LightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PWorldCamera.Camera.GetViewMatrix());
            this._sb.End();
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin();
            this._sb.Draw(this._graphicsManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this._sb.Draw(this._graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this._sb.Draw(this._graphicsManager.LightingRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this._sb.Draw(this._graphicsManager.GuiRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this._sb.End();
            #endregion

            #region RENDERING (FINAL)
            Vector2 scaleFactor = Vector2.One;

            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._sb.Draw(this._graphicsManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scaleFactor, SpriteEffects.None, 0f);
            this._sb.End();
            #endregion

            base.Draw(gameTime);
        }

        // Utilities
        private static void RegisterAllGameElements(PElementDatabase database)
        {
            database.AddElement<PDirt>();
            database.AddElement<PMud>();
            database.AddElement<PWater>();
            database.AddElement<PStone>();
            database.AddElement<PGrass>();
            database.AddElement<PIce>();
            database.AddElement<PSand>();
            database.AddElement<PSnow>();
            database.AddElement<PMCorruption>();
            database.AddElement<PLava>();
            database.AddElement<PAcid>();
            database.AddElement<PGlass>();
            database.AddElement<PMetal>();
            database.AddElement<PWall>();
            database.AddElement<PWood>();
            database.AddElement<PGCorruption>();
            database.AddElement<PLCorruption>();
            database.AddElement<PIMCorruption>();
            database.AddElement<PSteam>();
            database.AddElement<PSmoke>();
            database.AddElement<POil>();
        }
    }
}