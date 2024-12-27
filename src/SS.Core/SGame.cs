using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Managers.IO;
using StardustSandbox.Core.Plugins;
using StardustSandbox.Core.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core
{
    public sealed partial class SGame : Game, ISGame
    {
        public ISAssetDatabase AssetDatabase => this.assetDatabase;
        public ISElementDatabase ElementDatabase => this.elementDatabase;
        public ISGUIDatabase GUIDatabase => this.guiDatabase;
        public ISCatalogDatabase CatalogDatabase => this.catalogDatabase;
        public ISBackgroundDatabase BackgroundDatabase => this.backgroundDatabase;
        public ISEntityDatabase EntityDatabase => this.entityDatabase;

        public ISInputManager InputManager => this.inputManager;
        public ISCameraManager CameraManager => this.cameraManager;
        public ISGraphicsManager GraphicsManager => this.graphicsManager;
        public ISGUIManager GUIManager => this.guiManager;
        public ISEntityManager EntityManager => this.entityManager;
        public ISGameManager GameManager => this.gameManager;
        public ISBackgroundManager BackgroundManager => this.backgroundManager;
        public ISCursorManager CursorManager => this.cursorManager;

        public ISWorld World => this.world;
        public SGameInputController GameInputController => this.gameInputController;

        // ================================= //

        private SpriteBatch spriteBatch;

        // Databases
        private readonly SAssetDatabase assetDatabase;
        private readonly SElementDatabase elementDatabase;
        private readonly SGUIDatabase guiDatabase;
        private readonly SCatalogDatabase catalogDatabase;
        private readonly SBackgroundDatabase backgroundDatabase;
        private readonly SEntityDatabase entityDatabase;

        // Managers
        private readonly SCameraManager cameraManager;
        private readonly SGraphicsManager graphicsManager;
        private readonly SShaderManager shaderManager;
        private readonly SInputManager inputManager;
        private readonly SGUIManager guiManager;
        private readonly SCursorManager cursorManager;
        private readonly SBackgroundManager backgroundManager;
        private readonly SEntityManager entityManager;
        private readonly SGameManager gameManager;

        // Core
        private readonly SWorld world;
        private readonly SGameInputController gameInputController;

        // Plugin System
        private readonly List<SPluginBuilder> pluginBuilders = [];

        // ================================= //

        public SGame()
        {
            // Graphics
            this.graphicsManager = new(this, new GraphicsDeviceManager(this));

            // Load Settings
            SVideoSettings videoSettings = SSettingsManager.LoadSettings<SVideoSettings>();

            if (videoSettings.ScreenWidth == 0 || videoSettings.ScreenHeight == 0)
            {
                videoSettings.UpdateResolution(this.GraphicsDevice);
                SSettingsManager.SaveSettings(videoSettings);
            }

            // Initialize Content
            this.Content.RootDirectory = SDirectoryConstants.ASSETS;

            // Configure the game's window
            this.Window.IsBorderless = videoSettings.Borderless;
            this.Window.Title = SGameConstants.GetTitleAndVersionString();
            this.Window.AllowUserResizing = true;
            this.graphicsManager.SetGameWindow(this.Window);

            // Configure game settings
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / SScreenConstants.DEFAULT_FRAME_RATE);
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Database
            this.assetDatabase = new(this);
            this.elementDatabase = new(this);
            this.guiDatabase = new(this);
            this.catalogDatabase = new(this);
            this.backgroundDatabase = new(this);
            this.entityDatabase = new(this);

            // Core
            this.cameraManager = new(this);
            this.world = new(this);

            // Managers
            this.gameManager = new(this);
            this.inputManager = new(this);
            this.gameInputController = new(this);
            this.shaderManager = new(this);
            this.guiManager = new(this);
            this.cursorManager = new(this);
            this.backgroundManager = new(this);
            this.entityManager = new(this);
        }

        public void RegisterPlugin(SPluginBuilder pluginBuilder)
        {
            this.pluginBuilders.Add(pluginBuilder);
        }

        public void Quit()
        {
            Exit();
        }
    }
}