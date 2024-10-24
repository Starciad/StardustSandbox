﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.IO;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Models.Settings;
using StardustSandbox.Game.World;

using System;

namespace StardustSandbox.Game
{
    public sealed class SGame : Microsoft.Xna.Framework.Game
    {
        // Databases
        public SAssetDatabase AssetDatabase => this._assetDatabase;
        public SElementDatabase ElementDatabase => this._elementDatabase;
        public SGUIDatabase GUIDatabase => this._guiDatabase;
        public SItemDatabase ItemDatabase => this._itemDatabase;
        public SBackgroundDatabase BackgroundDatabase => this._backgroundDatabase;

        // Managers
        public SInputManager InputManager => this._inputManager;
        public SGameInputManager GameInputManager => this._gameInputManager;
        public SCameraManager CameraManager => this._cameraManager;
        public SGUIManager GUIManager => this._guiManager;

        // Core
        public SWorld World => this._world;

        // ================================= //

        private SpriteBatch _sb;

        // Databases
        private readonly SAssetDatabase _assetDatabase;
        private readonly SElementDatabase _elementDatabase;
        private readonly SGUIDatabase _guiDatabase;
        private readonly SItemDatabase _itemDatabase;
        private readonly SBackgroundDatabase _backgroundDatabase;

        // Managers
        private readonly SCameraManager _cameraManager;
        private readonly SGraphicsManager _graphicsManager;
        private readonly SGameInputManager _gameInputManager;
        private readonly SShaderManager _shaderManager;
        private readonly SInputManager _inputManager;
        private readonly SGUIManager _guiManager;
        private readonly SCursorManager _cursorManager;
        private readonly SBackgroundManager _backgroundManager;

        // Core
        private readonly SWorld _world;

        // ================================= //

        public SGame()
        {
            this._graphicsManager = new(this, new GraphicsDeviceManager(this));

            // Initialize Content
            this.Content.RootDirectory = SDirectoryConstants.ASSETS;

            // Configure the game's window
            UpdateGameSettings();
            this.Window.Title = SGameConstants.GetTitleAndVersionString();

            // Configure game settings
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Database
            this._assetDatabase = new(this, this.Content);
            this._elementDatabase = new(this);
            this._guiDatabase = new(this);
            this._itemDatabase = new(this);
            this._backgroundDatabase = new(this);

            // Core
            this._cameraManager = new(this._graphicsManager);
            this._world = new(this);

            // Managers
            this._inputManager = new(this);
            this._shaderManager = new(this);
            this._gameInputManager = new(this);
            this._guiManager = new(this);
            this._cursorManager = new(this);
            this._backgroundManager = new(this);
        }

        protected override void Initialize()
        {
            #region Databases
            this._assetDatabase.Initialize();
            this._elementDatabase.Initialize();
            this._itemDatabase.Initialize();
            this._guiDatabase.Initialize();
            this._backgroundDatabase.Initialize();
            #endregion

            #region Managers
            this._graphicsManager.Initialize();
            this._gameInputManager.Initialize();
            this._shaderManager.Initialize();
            this._inputManager.Initialize();
            this._guiManager.Initialize();
            this._cursorManager.Initialize();
            this._backgroundManager.Initialize();
            #endregion

            #region Game
            this._world.Initialize();
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._sb = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this._guiManager.ShowGUI(SGUIConstants.HUD_NAME);
            // this._guiManager.ShowGUI(SGUIConstants.ELEMENT_EXPLORER_NAME);
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
            this._backgroundManager.Update(gameTime);

            // Core
            this._world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            // BACKGROUND
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(this._backgroundManager.SolidColor);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._backgroundManager.Draw(gameTime, this._sb);
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

            // GUI
            this.GraphicsDevice.SetRenderTarget(this._graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this._sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._guiManager.Draw(gameTime, this._sb);
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

        public void UpdateGameSettings()
        {
            SGraphicsSettings graphicsSettings = SSystemSettingsFile.GetGraphicsSettings();
            this.Window.AllowUserResizing = graphicsSettings.Resizable;
            this.Window.IsBorderless = graphicsSettings.Borderless;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / graphicsSettings.Framerate);
        }
    }
}