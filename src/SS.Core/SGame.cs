using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Controllers.GameInput;
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
        public SAssetDatabase AssetDatabase => this.assetDatabase;
        public SElementDatabase ElementDatabase => this.elementDatabase;
        public SGUIDatabase GUIDatabase => this.guiDatabase;
        public SItemDatabase ItemDatabase => this.itemDatabase;
        public SBackgroundDatabase BackgroundDatabase => this.backgroundDatabase;
        public SEntityDatabase EntityDatabase => this.entityDatabase;

        public SInputManager InputManager => this.inputManager;
        public SCameraManager CameraManager => this.cameraManager;
        public SGraphicsManager GraphicsManager => this.graphicsManager;
        public SGUIManager GUIManager => this.guiManager;
        public SEntityManager EntityManager => this.entityManager;
        public SGameManager GameManager => this.gameManager;

        public SWorld World => this.world;
        public SGameInputController GameInputController => this.gameInputController;
        

        // ================================= //

        private SpriteBatch spriteBatch;

        // Databases
        private readonly SAssetDatabase assetDatabase;
        private readonly SElementDatabase elementDatabase;
        private readonly SGUIDatabase guiDatabase;
        private readonly SItemDatabase itemDatabase;
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
            this.graphicsManager = new(this, new GraphicsDeviceManager(this));

            // Initialize Content
            this.Content.RootDirectory = SDirectoryConstants.ASSETS;

            // Configure the game's window
            UpdateGameSettings();
            this.Window.Title = SGameConstants.GetTitleAndVersionString();

            // Configure game settings
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Database
            this.assetDatabase = new(this);
            this.elementDatabase = new(this);
            this.guiDatabase = new(this);
            this.itemDatabase = new(this);
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

        public void UpdateGameSettings()
        {
            SGraphicsSettings graphicsSettings = SSystemSettingsFile.GetGraphicsSettings();
            this.Window.AllowUserResizing = graphicsSettings.Resizable;
            this.Window.IsBorderless = graphicsSettings.Borderless;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / graphicsSettings.Framerate);
        }

        public void Quit()
        {
            Exit();
        }
    }
}