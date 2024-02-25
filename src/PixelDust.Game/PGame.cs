using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Constants.GUI;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements.Common.Gases;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.GUI.Common.HUD;
using PixelDust.Game.GUI.Common.Menus.ItemExplorer;
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
        // Databases
        public PAssetDatabase AssetDatabase => this._assetDatabase;
        public PElementDatabase ElementDatabase => this._elementDatabase;
        public PGUIDatabase GUIDatabase => this._guiDatabase;
        public PItemDatabase ItemDatabase => this._itemDatabase;

        // Managers
        public PInputManager InputManager => this._inputManager;
        public PGameInputManager GameInputManager => this._gameInputManager;
        public PCameraManager CameraManager => this._cameraManager;
        public PGUIManager GUIManager => this._guiManager;

        // ================================= //

        private readonly Assembly _assembly;
        private SpriteBatch _sb;

        // Databases
        private readonly PAssetDatabase _assetDatabase;
        private readonly PElementDatabase _elementDatabase;
        private readonly PGUIDatabase _guiDatabase;
        private readonly PItemDatabase _itemDatabase;

        // Core
        private readonly PCameraManager _cameraManager;
        private readonly PWorld _world;

        // Managers
        private readonly PGraphicsManager _graphicsManager;
        private readonly PGameInputManager _gameInputManager;
        private readonly PShaderManager _shaderManager;
        private readonly PInputManager _inputManager;
        private readonly PGUIManager _guiManager;
        private readonly PCursorManager _cursorManager;
        private readonly PGameContentManager _gameContentManager;

        // ================================= //

        public PGame()
        {
            this._graphicsManager = new(new(this));

            // Assembly
            this._assembly = Assembly.GetExecutingAssembly();

            // Initialize Content
            this.Content.RootDirectory = PDirectoryConstants.ASSETS;

            // Configure the game's window
            UpdateGameSettings();
            this.Window.Title = PGameConstants.GetTitleAndVersionString();

            // Configure game settings
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Database
            this._assetDatabase = new(this.Content);
            this._elementDatabase = new();
            this._guiDatabase = new();
            this._itemDatabase = new();

            // Core
            this._cameraManager = new(this._graphicsManager);
            this._world = new(this._elementDatabase, this._assetDatabase, this._cameraManager);

            // Managers
            this._inputManager = new();
            this._shaderManager = new(this._assetDatabase);
            this._gameInputManager = new(this._cameraManager, this._world, this._inputManager);
            this._guiManager = new(this._guiDatabase, this._inputManager);
            this._cursorManager = new(this._assetDatabase, this._inputManager);
            this._gameContentManager = new(this._assembly);
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
            #region Databases
            this._assetDatabase.Initialize(this);
            this._elementDatabase.Initialize(this);
            this._itemDatabase.Initialize(this);
            this._guiDatabase.Initialize(this);
            this._gameContentManager.Initialize(this);

            this._gameContentManager.RegisterAllGameContent();

            this._guiDatabase.Build();
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
            // this._guiManager.ShowGUI(PGUIConstants.HUD_NAME);
            this._guiManager.ShowGUI(PGUIConstants.ELEMENT_EXPLORER_NAME);
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
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this._cameraManager.GetViewMatrix());
            this._world.Draw(gameTime, this._sb);
            this._sb.End();

            // LIGHTING
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.LightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this._cameraManager.GetViewMatrix());
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
        private void RegisterAllGameItems(PItemDatabase database)
        {

        }
    }
}