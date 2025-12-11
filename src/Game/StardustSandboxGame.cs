using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Audio;
using StardustSandbox.Camera;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Inputs.Game;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.UI;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox
{
    internal sealed class StardustSandboxGame : Game
    {
        // Rendering
        private SpriteBatch spriteBatch;

        // Settings
        private GameplaySettings gameplaySettings;
        private VolumeSettings volumeSettings;

        // Core
        private readonly World world;
        private readonly InputController inputController;

        // Managers
        private readonly AmbientManager ambientManager;
        private readonly CursorManager cursorManager;
        private readonly GameManager gameManager;
        private readonly InputManager inputManager;
        private readonly EffectsManager effectsManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        internal StardustSandboxGame()
        {
            // Graphics
            this.videoManager = new(new GraphicsDeviceManager(this)
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
            });

            // Load Settings
            VideoSettings videoSettings = SettingsSerializer.LoadSettings<VideoSettings>();

            if (videoSettings.Width == 0 || videoSettings.Height == 0)
            {
                videoSettings.UpdateResolution(this.GraphicsDevice);
                SettingsSerializer.SaveSettings(videoSettings);
            }

            // Initialize Content
            this.Content.RootDirectory = IOConstants.ASSETS_DIRECTORY;

            // Configure the game's window
            this.Window.IsBorderless = videoSettings.Borderless;
            this.Window.Title = GameConstants.GetTitleAndVersionString();
            this.Window.AllowUserResizing = true;
            this.videoManager.SetGameWindow(this.Window);

            // Configure game settings
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / videoSettings.Framerate);
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Managers
            this.inputController = new();

            this.gameManager = new();
            this.inputManager = new();
            this.effectsManager = new();
            this.uiManager = new();
            this.cursorManager = new();
            this.ambientManager = new();

            // Core
            this.world = new(this.inputController, this.gameManager);
        }

        protected override void Initialize()
        {
            SSCamera.Initialize(this.videoManager);

            this.gameplaySettings = SettingsSerializer.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsSerializer.LoadSettings<VolumeSettings>();

            SongEngine.Volume = this.volumeSettings.MusicVolume * this.volumeSettings.MasterVolume;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Interaction.Initialize(this.inputManager);

            // Databases
            AssetDatabase.Load(this.Content, this.GraphicsDevice);
            ElementDatabase.Load();
            CatalogDatabase.Load();
            UIDatabase.Load(this.ambientManager, this.cursorManager, this.gameManager, this.Window, this.GraphicsDevice, this.inputController, this.inputManager, this.uiManager, this.videoManager, this.world);
            BackgroundDatabase.Load();
            ToolDatabase.Load();

            // Managers
            this.gameManager.Initialize(this.ambientManager, this.inputController, this.uiManager, this.world);
            this.effectsManager.Initialize();
            this.inputManager.Initialize(this.videoManager);
            this.cursorManager.Initialize(this.inputManager);
            this.ambientManager.Initialize(this.gameManager, this.world);

            // Controllers
            this.inputController.Initialize(this.gameManager, this.inputManager, this.world);

            // Renderer
            GameRenderer.Initialize(this.videoManager);

            // ============================= //

            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            if (Parameters.CreateException)
            {
                throw new Exception("This is a test exception created by the --create-exception parameter.");
            }

            this.gameManager.RemoveState(GameStates.IsPaused);
            this.gameManager.RemoveState(GameStates.IsSimulationPaused);

            if (Parameters.SkipIntro)
            {
                this.gameManager.StartGame();
            }
            else
            {
                this.uiManager.OpenGUI(UIIndex.MainMenu);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameManager.HasState(GameStates.IsFocused) || this.gameManager.HasState(GameStates.IsPaused))
            {
                return;
            }

            // Controllers
            this.inputController.Update();

            // Managers
            this.effectsManager.Update(gameTime, this.world.Time.CurrentTime);
            this.gameManager.Update();
            this.inputManager.Update();
            this.uiManager.Update(gameTime);
            this.cursorManager.Update();

            if (!this.gameManager.HasState(GameStates.IsSimulationPaused) && !this.gameManager.HasState(GameStates.IsCriticalMenuOpen))
            {
                this.world.Update(gameTime);
            }

            this.ambientManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameRenderer.Draw(
                this.ambientManager,
                this.gameplaySettings.PreviewAreaColor,
                this.cursorManager,
                this.inputController,
                this.inputManager,
                this.spriteBatch,
                this.uiManager,
                this.videoManager,
                this.world
            );
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            AssetDatabase.Unload();
            GameRenderer.Unload();

            base.UnloadContent();
        }

        // Event occurs when the game window returns to focus.
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            this.gameManager.SetState(GameStates.IsFocused);
            SongEngine.Resume();
        }

        // Event occurs when the game window stops having focus.
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            this.gameManager.RemoveState(GameStates.IsFocused);
            SongEngine.Pause();
        }

        // Event occurs when the game process is finished.
        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            base.OnExiting(sender, args);
        }

        internal void Quit()
        {
            Exit();
        }
    }
}
