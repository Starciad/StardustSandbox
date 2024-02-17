using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.GUI.Common;
using PixelDust.Game.IO;
using PixelDust.Game.Managers;
using PixelDust.Game.Models.Settings;
using PixelDust.Game.World;

using System;
using System.Reflection;

namespace PixelDust.Game
{
    public sealed class PGame : Microsoft.Xna.Framework.Game
    {
        public Assembly Assembly => this._assembly;
        public PInputManager InputManager => this._inputManager;
        public PGameInputManager GameInputManager => this._gameInputManager;
        public PAssetDatabase AssetDatabase => this._assetDatabase;
        public PElementDatabase ElementDatabase => this._elementDatabase;
        public PCameraManager OrthographicCamera => this._orthographicCamera;

        // ================================= //

        private readonly Assembly _assembly;
        private SpriteBatch _sb;

        // Managers
        private readonly PGraphicsManager _graphicsManager;
        private readonly PGameInputManager _gameInputManager;
        private readonly PShaderManager _shaderManager;
        private readonly PInputManager _inputManager;
        private readonly PScreenManager _screenManager;
        private readonly PGUIManager _guiManager;
        private readonly PCursorManager _cursorManager;

        // Databases
        private readonly PAssetDatabase _assetDatabase;
        private readonly PElementDatabase _elementDatabase;

        // Core
        private readonly PCameraManager _orthographicCamera;
        private readonly PWorld _world;

        // ================================= //

        public PGame()
        {
            this._graphicsManager = new(new(this));

            // Assembly
            this._assembly = GetType().Assembly;

            // Initialize Content
            this.Content.RootDirectory = PDirectoryConstants.ASSETS;

            // Configure the game's window
            UpdateGameSettings();
            this.Window.Title = PGameConstants.GetTitleAndVersionString();

            // Configure game settings
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Screen & Camera
            this._screenManager = new();
            this._orthographicCamera = new(this._screenManager);

            // Database
            this._assetDatabase = new(this.Content);
            this._elementDatabase = new();

            // Core
            this._world = new(this._elementDatabase, this._assetDatabase);

            // Managers
            this._inputManager = new();
            this._shaderManager = new(this._assetDatabase);
            this._gameInputManager = new(this._orthographicCamera, this._world, this._inputManager);
            this._guiManager = new(this._inputManager);
            this._cursorManager = new(this._assetDatabase, this._inputManager);
        }

        public void UpdateGameSettings()
        {
            PGraphicsSettings graphicsSettings = PSystemSettingsFile.GetGraphicsSettings();
            this.Window.AllowUserResizing = graphicsSettings.Resizable;
            this.Window.IsBorderless = graphicsSettings.Borderless;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / graphicsSettings.Framerate);
        }

        protected override void Initialize()
        {
            RegisterAllGameElements(this._elementDatabase);
            RegisterAllGameGUIs(this._guiManager);

            #region Databases
            this._assetDatabase.Initialize(this);
            this._elementDatabase.Initialize(this);
            #endregion

            #region Managers
            this._graphicsManager.Initialize(this);
            this._gameInputManager.Initialize(this);
            this._shaderManager.Initialize(this);
            this._inputManager.Initialize(this);
            this._guiManager.Initialize(this);
            this._cursorManager.Initialize(this);
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
            this._guiManager.ShowGUI<PGUI_HUD>();
        }

        protected override void Update(GameTime gameTime)
        {
            // Managers
            this._graphicsManager.Update(gameTime);
            this._gameInputManager.Update(gameTime);
            this._shaderManager.Update(gameTime);
            this._inputManager.Update(gameTime);
            this._guiManager.Update(gameTime);
            this._cursorManager.Update(gameTime);

            // Core
            this._world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            // GUI
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._guiManager.Draw(gameTime, this._sb);
            this._sb.End();

            // BACKGROUND
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._sb.End();

            // WORLD
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this._orthographicCamera.GetViewMatrix());
            this._world.Draw(gameTime, this._sb);
            this._sb.End();

            // LIGHTING
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.LightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this._orthographicCamera.GetViewMatrix());
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
            this._cursorManager.Draw(gameTime, this._sb);
            this._sb.End();
            #endregion

            base.Draw(gameTime);
        }

        // Utilities
        private static void RegisterAllGameGUIs(PGUIManager guiManager)
        {
            guiManager.RegisterGUISystem<PGUI_HUD>();
        }
        private static void RegisterAllGameElements(PElementDatabase database)
        {
            database.RegisterElement<PDirt>();
            database.RegisterElement<PMud>();
            database.RegisterElement<PWater>();
            database.RegisterElement<PStone>();
            database.RegisterElement<PGrass>();
            database.RegisterElement<PIce>();
            database.RegisterElement<PSand>();
            database.RegisterElement<PSnow>();
            database.RegisterElement<PMCorruption>();
            database.RegisterElement<PLava>();
            database.RegisterElement<PAcid>();
            database.RegisterElement<PGlass>();
            database.RegisterElement<PMetal>();
            database.RegisterElement<PWall>();
            database.RegisterElement<PWood>();
            database.RegisterElement<PGCorruption>();
            database.RegisterElement<PLCorruption>();
            database.RegisterElement<PIMCorruption>();
            database.RegisterElement<PSteam>();
            database.RegisterElement<PSmoke>();
        }
    }
}