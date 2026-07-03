/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.Notifiers;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Common.Progress;
using StardustSandbox.Core.Serialization.Common.Settings;
using StardustSandbox.Core.Serialization.Common.Settings.StorageModels;
using StardustSandbox.Core.Serialization.Common.Worlds;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Threading;

namespace StardustSandbox.Core
{
    public sealed partial class StardustSandboxGame : Game
    {
        private IAchievementNotifier achievementNotifier;
        private IGameNotifier gameNotifier;

        private SpriteBatch spriteBatch;

        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly GameLaunchOptions gameLaunchOptions;

        private readonly ProgressSerializer progressSerializer;
        private readonly SettingsSerializer settingsSerializer;
        private readonly WorldSerializer worldSerializer;

        private readonly GameEvents gameEvents;
        private readonly GameHandler gameHandler;
        private readonly GameScreen gameScreen;

        private readonly AchievementDatabase achievementDatabase;
        private readonly ActorDatabase actorDatabase;
        private readonly AssetDatabase assetDatabase;
        private readonly BackgroundDatabase backgroundDatabase;
        private readonly CatalogDatabase catalogDatabase;
        private readonly ElementDatabase elementDatabase;
        private readonly ToolDatabase toolDatabase;
        private readonly UIDatabase uiDatabase;

        private readonly AchievementManager achievementManager;
        private readonly ActorManager actorManager;
        private readonly AmbientManager ambientManager;
        private readonly CursorManager cursorManager;
        private readonly EffectsManager effectsManager;
        private readonly SongManager songManager;
        private readonly SoundEffectManager soundEffectManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        private readonly World world;
        private readonly PlayerInputController playerInputController;
        private readonly Camera2D camera;

        private readonly ControlStorageModel controlSettings;
        private readonly CursorStorageModel cursorSettings;
        private readonly GeneralStorageModel generalSettings;
        private readonly GameplayStorageModel gameplaySettings;
        private readonly VideoStorageModel videoSettings;
        private readonly VolumeStorageModel volumeSettings;

        public StardustSandboxGame(GameLaunchOptions options)
        {
            this.gameLaunchOptions = options;

            // Graphics
            this.graphicsDeviceManager = new(this)
            {
                GraphicsProfile = GraphicsProfile.Reach,
                PreferredBackBufferFormat = SurfaceFormat.Color,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = true,
                HardwareModeSwitch = true,
                IsFullScreen = false,
                PreferHalfPixelOffset = true,
                PreferMultiSampling = false,
                PreferredDepthStencilFormat = DepthFormat.None,
                SupportedOrientations = DisplayOrientation.Default
            };

            this.videoManager = new(this.graphicsDeviceManager, this.Window);

            // Serializers
            this.progressSerializer = new();
            this.settingsSerializer = new();

            this.controlSettings = this.settingsSerializer.Load<ControlStorageModel>();
            this.cursorSettings = this.settingsSerializer.Load<CursorStorageModel>();
            this.generalSettings = this.settingsSerializer.Load<GeneralStorageModel>();
            this.gameplaySettings = this.settingsSerializer.Load<GameplayStorageModel>();
            this.videoSettings = this.settingsSerializer.Load<VideoStorageModel>();
            this.volumeSettings = this.settingsSerializer.Load<VolumeStorageModel>();

            // Initialize Content
            this.Content.RootDirectory = IOConstants.ASSETS_DIRECTORY;

            // Configure the game's window
            this.Window.IsBorderless = this.videoSettings.Borderless;
            this.Window.Title = GameConstants.GetTitleAndVersionString();
            this.Window.AllowUserResizing = true;

            // Configure game settings
            SetFrameRate(this.videoSettings.Framerate);

            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            this.gameEvents = new();
            this.gameScreen = new(this.graphicsDeviceManager);

            // Database
            this.achievementDatabase = new();
            this.actorDatabase = new();
            this.assetDatabase = new(this.Content, this.graphicsDeviceManager);
            this.backgroundDatabase = new(this.assetDatabase);
            this.catalogDatabase = new();
            this.elementDatabase = new();
            this.toolDatabase = new();
            this.uiDatabase = new();

            // System
            this.songManager = new(this.assetDatabase, this.gameLaunchOptions, this.volumeSettings);
            this.soundEffectManager = new(this.assetDatabase, this.volumeSettings);

            // Core
            this.playerInputController = new();
            this.world = new(
                this.assetDatabase,
                this.elementDatabase,
                this.gameEvents,
                this.gameplaySettings,
                this.playerInputController,
                this.worldSerializer
            );
            this.camera = new(this.gameplaySettings, this.gameScreen);

            // Managers
            this.achievementManager = new(this.achievementDatabase, this.gameEvents, this.progressSerializer, this.world.TileMap);
            this.effectsManager = new(this.assetDatabase);
            this.uiManager = new(this.uiDatabase);
            this.cursorManager = new(this.assetDatabase, this.cursorSettings);
            this.ambientManager = new(this.assetDatabase, this.backgroundDatabase, this.gameScreen, this.world);
            this.actorManager = new(this.actorDatabase, this.world, this.worldSerializer);

            // Serializers
            this.worldSerializer = new(this.actorManager, this.graphicsDeviceManager, this.world);

            // Handlers
            this.gameHandler = new(
                this.actorManager,
                this.ambientManager,
                this.camera,
                this.Window,
                this.playerInputController,
                this.songManager,
                this,
                this.uiDatabase,
                this.uiManager,
                this.world
            );
        }

        internal void SetFrameRate(float framerate)
        {
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / framerate);
        }

        private static void InitializeDirectories()
        {
            IO.Directory.Initialize();
        }

        private void InitializeGameCulture()
        {
            GameCulture gameCulture = this.generalSettings.GetGameCulture();

            Thread.CurrentThread.CurrentCulture = gameCulture.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = gameCulture.CultureInfo;

            gameCulture.CultureInfo.ClearCachedData();
        }

        private void ResolveServices()
        {
            this.achievementNotifier = this.Services.GetService<IAchievementNotifier>();
            this.gameNotifier = this.Services.GetService<IGameNotifier>();
        }

        private void RegisterAchievementEvents()
        {
            if (this.achievementNotifier is not null)
            {
                this.achievementManager.AchievementUnlocked += this.achievementNotifier.OnAchievementUnlocked;
            }

            this.achievementManager.AchievementUnlocked += OnAchievementUnlocked;
        }

        protected override void Initialize()
        {
            InitializeDirectories();
            InitializeGameCulture();
            ResolveServices();
            RegisterAchievementEvents();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Databases
            this.assetDatabase.Load();
            this.actorDatabase.Load(this.actorManager, this.assetDatabase, this.elementDatabase, this.gameEvents, this.world);
            this.elementDatabase.Load(this.gameEvents);
            this.backgroundDatabase.Load();
            this.uiDatabase.Load(
                this.achievementDatabase,
                this.achievementManager,
                this.actorManager,
                this.ambientManager,
                this.assetDatabase,
                this.catalogDatabase,
                this.cursorManager,
                this.gameHandler,
                this.gameScreen,
                this.Window,
                this.GraphicsDevice,
                this.playerInputController,
                this.progressSerializer,
                this.settingsSerializer,
                this.songManager,
                this.soundEffectManager,
                this.uiManager,
                this.videoManager,
                this.world,
                this.worldSerializer
            );

            // Managers
            this.cursorManager.Load();
            this.effectsManager.Initialize();
            this.ambientManager.Initialize();

            // Controllers
            this.playerInputController.Initialize(
                this.actorManager,
                this.camera,
                this.controlSettings,
                this.gameEvents,
                this.gameHandler,
                this.soundEffectManager,
                this.toolDatabase,
                this.videoManager,
                this.world
            );

            // Resolution
            if (this.videoSettings.Resolution.X == 0 || this.videoSettings.Resolution.Y == 0)
            {
                this.videoSettings.UpdateResolution(this.GraphicsDevice);
                this.settingsSerializer.Save(this.videoSettings);
            }

            this.Window.ClientSizeChanged += OnClientSizeChanged;
            this.videoManager.ApplySettings(this.videoSettings);

            // Renderer
            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            if (this.gameLaunchOptions.CreateException)
            {
                throw new Exception("This is a test exception created by the --create-exception parameter.");
            }

            this.gameHandler.RemoveState(GameStates.IsPaused);
            this.gameHandler.RemoveState(GameStates.IsSimulationPaused);

            if (this.gameLaunchOptions.SkipIntro)
            {
                this.gameHandler.StartGame();
            }
            else
            {
                this.uiManager.OpenUI(UIIndex.Main);
            }

            this.gameNotifier?.OnBeginRun();
            this.uiDatabase.ResizeUIs();
        }

        protected override void Update(GameTime gameTime)
        {
            this.gameNotifier?.OnUpdate();

            if (!this.gameHandler.HasState(GameStates.IsFocused) ||
                 this.gameHandler.HasState(GameStates.IsPaused))
            {
                base.Update(gameTime);
                return;
            }

            InputEngine.Update();

            // Controllers
            this.playerInputController.Update();
            this.camera.Update(gameTime);

            if (this.world.CanUpdate || this.world.CanDraw)
            {
                this.camera.ClampTargetPositionToBounds(new(0, 0, this.world.TileMap.Width * WorldConstants.TILE_SIZE, this.world.TileMap.Height * WorldConstants.TILE_SIZE));
            }

            // Managers
            this.effectsManager.Update(gameTime, this.world.Time.CurrentTime);
            this.uiManager.Update(gameTime);
            this.cursorManager.Update();

            if (!this.gameHandler.HasState(GameStates.IsSimulationPaused) &&
                !this.gameHandler.HasState(GameStates.IsCriticalMenuOpen))
            {
                this.world.Update(gameTime);
                this.actorManager.Update(gameTime);
            }

            this.ambientManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            this.assetDatabase.Unload();
            base.UnloadContent();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            this.gameHandler.SetState(GameStates.IsFocused);
            MediaPlayer.Resume();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            this.gameHandler.RemoveState(GameStates.IsFocused);
            MediaPlayer.Pause();
        }

        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            this.Window.ClientSizeChanged -= OnClientSizeChanged;

            base.OnExiting(sender, args);
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            this.soundEffectManager.Play(SoundEffectIndex.GUI_World_Saved);
        }

        private void OnClientSizeChanged(object sender, EventArgs args)
        {
            Point minSize = ScreenConstants.RESOLUTIONS[0];
            Point newSize = new(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            if (newSize.X < minSize.X)
            {
                newSize.X = minSize.X;
            }

            if (newSize.Y < minSize.Y)
            {
                newSize.Y = minSize.Y;
            }

            if (newSize.X != this.graphicsDeviceManager.PreferredBackBufferWidth || newSize.Y != this.graphicsDeviceManager.PreferredBackBufferHeight)
            {
                this.graphicsDeviceManager.PreferredBackBufferWidth = newSize.X;
                this.graphicsDeviceManager.PreferredBackBufferHeight = newSize.Y;
                this.graphicsDeviceManager.ApplyChanges();
            }

            this.uiDatabase.ResizeUIs();
        }
    }
}
