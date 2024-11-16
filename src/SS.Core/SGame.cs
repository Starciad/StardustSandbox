using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Models.Settings;
using StardustSandbox.Core.Plugins;
using StardustSandbox.Core.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core
{
    public sealed partial class SGame : Game, ISGame
    {
        public SAssetDatabase AssetDatabase => this._assetDatabase;
        public SElementDatabase ElementDatabase => this._elementDatabase;
        public SGUIDatabase GUIDatabase => this._guiDatabase;
        public SItemDatabase ItemDatabase => this._itemDatabase;
        public SBackgroundDatabase BackgroundDatabase => this._backgroundDatabase;

        public SInputManager InputManager => this._inputManager;
        public SGameInputManager GameInputManager => this._gameInputManager;
        public SCameraManager CameraManager => this._cameraManager;
        public SGraphicsManager GraphicsManager => this._graphicsManager;
        public SGUIManager GUIManager => this._guiManager;

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

        // Plugin System
        private readonly List<SPluginBuilder> pluginBuilders = [];

        // Status
        private bool isFocused;

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
            this._assetDatabase = new(this);
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

        public void RegisterPlugin(SPluginBuilder pluginBuilder)
        {
            this.pluginBuilders.Add(pluginBuilder);
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