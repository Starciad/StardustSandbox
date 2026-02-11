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
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.InputSystem.Game;
using StardustSandbox.Core.Interfaces.Notifiers;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    public sealed class StardustSandboxGame : Game
    {
        private IAchievementNotifier achievementNotifier;
        private IGameNotifier gameNotifier;

        private SpriteBatch spriteBatch;

        private readonly World world;
        private readonly InputController inputController;

        private readonly ActorManager actorManager;
        private readonly AmbientManager ambientManager;
        private readonly CursorManager cursorManager;
        private readonly EffectsManager effectsManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        public StardustSandboxGame(string[] args)
        {
            GameParameters.Start(args);

            // Graphics
            GraphicsDeviceManager gdm = new(this)
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

            this.videoManager = new(gdm);

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
            this.achievementNotifier = this.Services.GetService<IAchievementNotifier>();
            this.gameNotifier = this.Services.GetService<IGameNotifier>();

            GameScreen.Initialize(this.GraphicsDevice);
            Camera.Initialize(this.world);

            AchievementEngine.Initialize(this.achievementNotifier);
            SongEngine.Initialize();
            SoundEngine.Initialize();

            GameHandler.Initialize(this.Window);

            this.Window.ClientSizeChanged += OnClientSizeChanged;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Databases
            AchievementDatabase.Load();
            AssetDatabase.Load(this.Content, this.GraphicsDevice);
            ElementDatabase.Load();
            CatalogDatabase.Load();
            UIDatabase.Load(this.actorManager, this.ambientManager, this.cursorManager, this.Window, this.GraphicsDevice, this.inputController, this, this.uiManager, this.videoManager, this.world);
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
        }

        protected override void BeginRun()
        {
            if (GameParameters.CreateException)
            {
                throw new Exception("This is a test exception created by the --create-exception parameter.");
            }

            GameHandler.RemoveState(GameStates.IsPaused);
            GameHandler.RemoveState(GameStates.IsSimulationPaused);

            if (GameParameters.SkipIntro)
            {
                GameHandler.StartGame(this.actorManager, this.ambientManager, this.inputController, this.uiManager, this.world);
            }
            else
            {
                this.uiManager.OpenUI(UIIndex.MainMenu);
            }

            this.gameNotifier?.OnBeginRun();
        }

        protected override void Update(GameTime gameTime)
        {
            this.gameNotifier?.OnUpdate();

            if (!GameHandler.HasState(GameStates.IsFocused) || GameHandler.HasState(GameStates.IsPaused))
            {
                return;
            }

            Input.Update();

            // Controllers
            this.inputController.Update();
            Camera.Update(gameTime);

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

            base.UnloadContent();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            GameHandler.SetState(GameStates.IsFocused);
            MediaPlayer.Resume();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            GameHandler.RemoveState(GameStates.IsFocused);
            MediaPlayer.Pause();
        }

        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            this.Window.ClientSizeChanged -= OnClientSizeChanged;

            base.OnExiting(sender, args);
        }

        private void OnClientSizeChanged(object sender, EventArgs args)
        {
            this.uiManager.ResizeGUIs();
        }

        internal void Quit()
        {
            Exit();
        }
    }
}

