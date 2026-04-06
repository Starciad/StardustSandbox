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
using StardustSandbox.Core.Interfaces.Notifiers;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Common;
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
        private readonly PlayerInputController playerInputController;
        private readonly Camera2D camera;

        private readonly ActorManager actorManager;
        private readonly AmbientManager ambientManager;
        private readonly CursorManager cursorManager;
        private readonly EffectsManager effectsManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        private readonly VideoSettings videoSettings;

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        public StardustSandboxGame(string[] args)
        {
            GameParameters.Start(args);

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

            // Load Settings
            this.videoSettings = SettingsSerializer.Load<VideoSettings>();

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

            // Managers
            this.playerInputController = new();
            this.effectsManager = new();
            this.uiManager = new();
            this.cursorManager = new();
            this.ambientManager = new();

            // Core
            this.world = new(this.playerInputController);
            this.camera = new();

            // Actor Manager
            this.actorManager = new(this.world);
        }

        internal void SetFrameRate(float framerate)
        {
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / framerate);
        }

        protected override void Initialize()
        {
            this.achievementNotifier = this.Services.GetService<IAchievementNotifier>();
            this.gameNotifier = this.Services.GetService<IGameNotifier>();

            GameScreen.Initialize(this.GraphicsDevice);

            AchievementEngine.Initialize(this.achievementNotifier);
            SongEngine.Initialize();
            SoundEngine.Initialize();

            GameHandler.Initialize(this.Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Databases
            AssetDatabase.Load(this.Content, this.GraphicsDevice);
            AchievementDatabase.Load();
            ElementDatabase.Load();
            CatalogDatabase.Load();
            BackgroundDatabase.Load();
            ToolDatabase.Load();
            ActorDatabase.Load(this.actorManager, this.world);
            UIDatabase.Load(this.actorManager, this.ambientManager, this.camera, this.cursorManager, this.Window, this.GraphicsDevice, this.playerInputController, this, this.uiManager, this.videoManager, this.world);

            // Managers
            this.effectsManager.Initialize();
            this.cursorManager.Initialize();
            this.ambientManager.Initialize(this.world);

            // Controllers
            this.playerInputController.Initialize(this.actorManager, this.camera, this.videoManager, this.world);

            // Resolution
            if (this.videoSettings.Width == 0 || this.videoSettings.Height == 0)
            {
                this.videoSettings.UpdateResolution(this.GraphicsDevice);
                SettingsSerializer.Save(this.videoSettings);
            }

            this.Window.ClientSizeChanged += OnClientSizeChanged;
            this.videoManager.ApplySettings(this.videoSettings);

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
                GameHandler.StartGame(
                    this.actorManager,
                    this.ambientManager,
                    this.camera,
                    (HudUI)UIDatabase.GetUI(UIIndex.Hud),
                    (ItemExplorerUI)UIDatabase.GetUI(UIIndex.ItemExplorer),
                    this.playerInputController,
                    this.uiManager,
                    this.world
                );
            }
            else
            {
                this.uiManager.OpenUI(UIIndex.Main);
            }

            this.gameNotifier?.OnBeginRun();
            UIDatabase.ResizeUIs(GameScreen.GetViewport());
        }

        protected override void Update(GameTime gameTime)
        {
            this.gameNotifier?.OnUpdate();

            if (!GameHandler.HasState(GameStates.IsFocused) || GameHandler.HasState(GameStates.IsPaused))
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
                this.camera.ClampTargetPositionToBounds(new(0, 0, this.world.Size.X * WorldConstants.TILE_SIZE, this.world.Size.Y * WorldConstants.TILE_SIZE));
            }

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
                this.camera,
                this.cursorManager,
                this.playerInputController,
                this.spriteBatch,
                this.uiManager,
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

            UIDatabase.ResizeUIs(GameScreen.GetViewport());
        }
    }
}
