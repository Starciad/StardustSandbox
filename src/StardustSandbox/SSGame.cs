using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.AudioSystem;
using StardustSandbox.Colors;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Settings;
using StardustSandbox.Managers;
using StardustSandbox.UI;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox
{
    internal sealed class SSGame : Game
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
        private readonly CameraManager cameraManager;
        private readonly CursorManager cursorManager;
        private readonly GameManager gameManager;
        private readonly InputManager inputManager;
        private readonly ShaderManager shaderManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        internal SSGame()
        {
            // Graphics
            this.videoManager = new(new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,
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
            VideoSettings videoSettings = SettingsHandler.LoadSettings<VideoSettings>();

            if (videoSettings.Width == 0 || videoSettings.Height == 0)
            {
                videoSettings.UpdateResolution(this.GraphicsDevice);
                SettingsHandler.SaveSettings(videoSettings);
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
            this.shaderManager = new();
            this.uiManager = new();
            this.cursorManager = new();
            this.ambientManager = new();

            // Core
            this.cameraManager = new(this.videoManager);
            this.world = new(this.cameraManager, this.inputController, this.gameManager);
        }

        #region ROUTINE

        protected override void Initialize()
        {
            this.gameplaySettings = SettingsHandler.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsHandler.LoadSettings<VolumeSettings>();

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
            BackgroundDatabase.Load(this.cameraManager);
            ToolDatabase.Load();

            // Managers
            this.gameManager.Initialize(this.ambientManager, this.cameraManager, this.inputController, this.uiManager, this.world);
            this.videoManager.Initialize();
            this.shaderManager.Initialize();
            this.inputManager.Initialize(this.videoManager);
            this.cursorManager.Initialize(this.inputManager);
            this.ambientManager.Initialize(this.cameraManager, this.gameManager, this.world);

            // Controllers
            this.inputController.Initialize(this.cameraManager, this.gameManager, this.inputManager, this.world);

            // ============================= //

            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this.uiManager.OpenGUI(UIIndex.MainMenu);

            this.gameManager.RemoveState(GameStates.IsPaused);
            this.gameManager.RemoveState(GameStates.IsSimulationPaused);
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
            this.gameManager.Update();
            this.shaderManager.Update(gameTime);
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
            #region RENDERING (ELEMENTS)
            DrawAmbient();
            DrawWorld();
            DrawGUI();
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.videoManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(AAP64ColorPalette.DarkGray);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.videoManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.videoManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            DrawCursorPenActionArea();
            this.spriteBatch.Draw(this.videoManager.UIRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (FINAL)
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(AAP64ColorPalette.DarkGray);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.videoManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, this.videoManager.GetScreenScaleFactor(), SpriteEffects.None, 0f);
            this.cursorManager.Draw(this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            AssetDatabase.Unload();
            base.UnloadContent();
        }

        #endregion

        #region RENDERING

        private void DrawAmbient()
        {
            this.GraphicsDevice.SetRenderTarget(this.videoManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(this.ambientManager.BackgroundHandler.SolidColor);

            DrawAmbientSky();
            DrawAmbientDetails();
            DrawAmbientBackground();
        }

        private void DrawAmbientSky()
        {
            if (!this.ambientManager.SkyHandler.IsActive)
            {
                return;
            }

            GradientColorMap skyGradientColorMap = this.ambientManager.SkyHandler.GetSkyGradientByTime(this.world.Time.CurrentTime);
            float interpolation = skyGradientColorMap.GetInterpolationFactor(this.world.Time.CurrentTime);

            Effect skyEffect = this.ambientManager.SkyHandler.GradientTransitionEffect;
            skyEffect.Parameters["StartColor1"].SetValue(skyGradientColorMap.Color1.Start.ToVector4());
            skyEffect.Parameters["StartColor2"].SetValue(skyGradientColorMap.Color2.Start.ToVector4());
            skyEffect.Parameters["EndColor1"].SetValue(skyGradientColorMap.Color1.End.ToVector4());
            skyEffect.Parameters["EndColor2"].SetValue(skyGradientColorMap.Color2.End.ToVector4());
            skyEffect.Parameters["TimeNormalized"].SetValue(interpolation);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, skyEffect, null);
            this.spriteBatch.Draw(this.ambientManager.SkyHandler.SkyTexture, Vector2.Zero, null, AAP64ColorPalette.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
        }

        private void DrawAmbientDetails()
        {
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.ambientManager.CelestialBodyHandler.Draw(this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawAmbientBackground()
        {
            Effect backgroundEffect = null;

            if (this.ambientManager.BackgroundHandler.SelectedBackground.IsAffectedByLighting)
            {
                GradientColorMap backgroundGradientColorMap = this.ambientManager.SkyHandler.GetBackgroundGradientByTime(this.world.Time.CurrentTime);
                float interpolation = backgroundGradientColorMap.GetInterpolationFactor(this.world.Time.CurrentTime);

                backgroundEffect = this.ambientManager.SkyHandler.GradientTransitionEffect;
                backgroundEffect.Parameters["StartColor1"].SetValue(backgroundGradientColorMap.Color1.Start.ToVector4());
                backgroundEffect.Parameters["StartColor2"].SetValue(backgroundGradientColorMap.Color2.Start.ToVector4());
                backgroundEffect.Parameters["EndColor1"].SetValue(backgroundGradientColorMap.Color1.End.ToVector4());
                backgroundEffect.Parameters["EndColor2"].SetValue(backgroundGradientColorMap.Color2.End.ToVector4());
                backgroundEffect.Parameters["TimeNormalized"].SetValue(interpolation);
            }

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, backgroundEffect, null);
            this.ambientManager.CloudHandler.Draw(this.spriteBatch);
            this.ambientManager.BackgroundHandler.Draw(this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawWorld()
        {
            this.GraphicsDevice.SetRenderTarget(this.videoManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawGUI()
        {
            this.GraphicsDevice.SetRenderTarget(this.videoManager.UIRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.uiManager.Draw(this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawCursorPenActionArea()
        {
            PenTool penTool = this.inputController.Pen.Tool;

            if (penTool == PenTool.Visualization || penTool == PenTool.Fill)
            {
                return;
            }

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();
            Vector2 worldMousePosition = this.cameraManager.ScreenToWorld(mousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / WorldConstants.GRID_SIZE),
                (int)Math.Floor(worldMousePosition.Y / WorldConstants.GRID_SIZE)
            );

            foreach (Point point in this.inputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * WorldConstants.GRID_SIZE,
                    point.Y * WorldConstants.GRID_SIZE
                );

                Vector2 screenPosition = this.cameraManager.WorldToScreen(worldPosition);

                this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ShapeSquares), screenPosition, new(110, 0, 32, 32), this.gameplaySettings.PreviewAreaColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        #endregion

        #region EVENTS

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

        #endregion

        internal void Quit()
        {
            Exit();
        }
    }
}