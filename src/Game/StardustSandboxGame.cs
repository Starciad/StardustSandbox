using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Audio;
using StardustSandbox.Camera;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Managers;
using StardustSandbox.Screenshot;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox
{
    internal sealed class StardustSandboxGame : Game
    {
        // Rendering
        private SpriteBatch spriteBatch;

        // Core
        private readonly World world;
        private readonly InputController inputController;

        // Managers
        private readonly ActorManager actorManager;
        private readonly AmbientManager ambientManager;
        private readonly CursorManager cursorManager;
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
                PreferredBackBufferWidth = ScreenConstants.SCREEN_WIDTH,
                PreferredBackBufferHeight = ScreenConstants.SCREEN_HEIGHT,
                SynchronizeWithVerticalRetrace = true,
                HardwareModeSwitch = true,
                IsFullScreen = false,
                PreferHalfPixelOffset = true,
                PreferMultiSampling = false,
                PreferredDepthStencilFormat = DepthFormat.None,
                SupportedOrientations = DisplayOrientation.Default
            });

            // Load Settings
            VideoSettings videoSettings = SettingsSerializer.Load<VideoSettings>();

            if (videoSettings.Width == 0 || videoSettings.Height == 0)
            {
                videoSettings = videoSettings.UpdateResolution(this.videoManager.GraphicsDevice);
                SettingsSerializer.Save(videoSettings);
            }

            // Initialize Content
            this.Content.RootDirectory = IOConstants.ASSETS_DIRECTORY;

            // Configure the game's window
            this.Window.IsBorderless = videoSettings.Borderless;
            this.Window.Title = GameConstants.GetTitleAndVersionString();
            this.Window.AllowUserResizing = true;
            this.videoManager.SetGameWindow(this.Window);

            // Configure game settings
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / videoSettings.Framerate);
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = true;

            // Managers
            this.inputController = new();
            this.effectsManager = new();
            this.uiManager = new();
            this.cursorManager = new();
            this.ambientManager = new();

            // Core
            this.world = new(this.inputController);

            // Actor Manager
            this.actorManager = new(this.world);

            // Apply video settings
            this.videoManager.ApplySettings(videoSettings);
        }

        protected override void Initialize()
        {
            SSCamera.Initialize(this.videoManager);

            SongEngine.Initialize();
            SoundEngine.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Input
            Input.Initialize(this.videoManager);

            // Databases
            AssetDatabase.Load(this.Content, this.GraphicsDevice);
            ElementDatabase.Load();
            CatalogDatabase.Load();
            UIDatabase.Load(this.actorManager, this.ambientManager, this.cursorManager, this.Window, this.GraphicsDevice, this.inputController, this.uiManager, this.videoManager, this.world);
            BackgroundDatabase.Load();
            ToolDatabase.Load();
            ActorDatabase.Load(this.actorManager, this.world);

            // Managers
            this.effectsManager.Initialize();
            this.cursorManager.Initialize();
            this.ambientManager.Initialize(this.world);

            // Controllers
            this.inputController.Initialize(this.actorManager, this.world);

            // Renderer
            GameRenderer.Initialize(this.videoManager);
            this.spriteBatch = new(this.GraphicsDevice);

            // Screenshot
            ElementScreenshot.Load();
        }

        protected override void BeginRun()
        {
            if (Parameters.CreateException)
            {
                throw new Exception("This is a test exception created by the --create-exception parameter.");
            }

            GameHandler.RemoveState(GameStates.IsPaused);
            GameHandler.RemoveState(GameStates.IsSimulationPaused);

            if (Parameters.SkipIntro)
            {
                GameHandler.StartGame(this.actorManager, this.ambientManager, this.inputController, this.uiManager, this.world);
            }
            else
            {
                this.uiManager.OpenGUI(UIIndex.MainMenu);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (!GameHandler.HasState(GameStates.IsFocused) || GameHandler.HasState(GameStates.IsPaused))
            {
                return;
            }

            Input.Update();

            // Controllers
            this.inputController.Update();
            SSCamera.Update(this.world);

            // Managers
            this.effectsManager.Update(gameTime, this.world.Time.CurrentTime);
            this.uiManager.Update(gameTime);
            this.cursorManager.Update();

            if (!GameHandler.HasState(GameStates.IsSimulationPaused) && !GameHandler.HasState(GameStates.IsCriticalMenuOpen))
            {
                this.world.Update(gameTime);
                this.actorManager.Update(gameTime);
            }

            this.ambientManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameRenderer.Draw(
                this.actorManager,
                this.ambientManager,
                this.cursorManager,
                this.inputController,
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
            ElementScreenshot.Unload();

            base.UnloadContent();
        }

        // Event occurs when the game window returns to focus.
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            GameHandler.SetState(GameStates.IsFocused);
            MediaPlayer.Resume();
        }

        // Event occurs when the game window stops having focus.
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            GameHandler.RemoveState(GameStates.IsFocused);
            MediaPlayer.Pause();
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
