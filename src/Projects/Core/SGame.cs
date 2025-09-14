using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Ambient.Handlers;
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Controllers.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.IO.Handlers;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.World;

using System;

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
        public ISToolDatabase ToolDatabase => this.toolDatabase;

        public ISInputManager InputManager => this.inputManager;
        public ISCameraManager CameraManager => this.cameraManager;
        public ISGraphicsManager GraphicsManager => this.graphicsManager;
        public ISGUIManager GUIManager => this.guiManager;
        public ISGameManager GameManager => this.gameManager;
        public ISAmbientManager AmbientManager => this.ambientManager;
        public ISCursorManager CursorManager => this.cursorManager;

        public ISWorld World => this.world;
        public ISGameInputController GameInputController => this.gameInputController;

        // ================================= //

        private SpriteBatch spriteBatch;

        // Databases
        private readonly SAssetDatabase assetDatabase;
        private readonly SElementDatabase elementDatabase;
        private readonly SGUIDatabase guiDatabase;
        private readonly SCatalogDatabase catalogDatabase;
        private readonly SBackgroundDatabase backgroundDatabase;
        private readonly SEntityDatabase entityDatabase;
        private readonly SToolDatabase toolDatabase;

        // Managers
        private readonly SCameraManager cameraManager;
        private readonly SGraphicsManager graphicsManager;
        private readonly SShaderManager shaderManager;
        private readonly SInputManager inputManager;
        private readonly SGUIManager guiManager;
        private readonly SCursorManager cursorManager;
        private readonly SAmbientManager ambientManager;
        private readonly SGameManager gameManager;

        // Core
        private readonly SWorld world;
        private readonly SGameInputController gameInputController;

        // ================================= //

        // Others
        private Texture2D mouseActionSquareTexture;

        private SGameplaySettings gameplaySettings;
        private SVolumeSettings volumeSettings;

        // ================================= //

        public SGame()
        {
            // Graphics
            this.graphicsManager = new(this, new GraphicsDeviceManager(this));

            // Load Settings
            SVideoSettings videoSettings = SSettingsHandler.LoadSettings<SVideoSettings>();

            if (videoSettings.Resolution.Width == 0 || videoSettings.Resolution.Height == 0)
            {
                videoSettings.UpdateResolution(this.GraphicsDevice);
                SSettingsHandler.SaveSettings(videoSettings);
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
            this.toolDatabase = new(this);

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
            this.ambientManager = new(this);
        }

        #region ROUTINE

        protected override void Initialize()
        {
            this.gameplaySettings = SSettingsHandler.LoadSettings<SGameplaySettings>();
            this.volumeSettings = SSettingsHandler.LoadSettings<SVolumeSettings>();

            SSongEngine.Volume = this.volumeSettings.MusicVolume * this.volumeSettings.MasterVolume;
            SSoundEngine.Volume = this.volumeSettings.SFXVolume * this.volumeSettings.MasterVolume;

            SGameContent.Initialize(this, this.Content);

            // Databases
            this.assetDatabase.Initialize();
            this.elementDatabase.Initialize();
            this.catalogDatabase.Initialize();
            this.guiDatabase.Initialize();
            this.backgroundDatabase.Initialize();
            this.entityDatabase.Initialize();
            this.toolDatabase.Initialize();

            // Managers
            this.gameManager.Initialize();
            this.graphicsManager.Initialize();
            this.shaderManager.Initialize();
            this.inputManager.Initialize();
            this.guiManager.Initialize();
            this.cursorManager.Initialize();
            this.ambientManager.Initialize();

            // Controllers
            this.gameInputController.Initialize();

            // Core
            this.world.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new(this.GraphicsDevice);
            this.mouseActionSquareTexture = this.assetDatabase.GetTexture("texture_shape_square_3");
        }

        protected override void BeginRun()
        {
            this.guiManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
            this.gameManager.GameState.IsPaused = false;
            this.gameManager.GameState.IsSimulationPaused = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameManager.GameState.IsFocused || this.gameManager.GameState.IsPaused)
            {
                return;
            }

            // Controllers
            this.gameInputController.Update(gameTime);

            // Managers
            this.gameManager.Update(gameTime);
            this.graphicsManager.Update(gameTime);
            this.shaderManager.Update(gameTime);
            this.inputManager.Update(gameTime);
            this.guiManager.Update(gameTime);
            this.cursorManager.Update(gameTime);

            if (!this.gameManager.GameState.IsSimulationPaused && !this.gameManager.GameState.IsCriticalMenuOpen)
            {
                this.world.Update(gameTime);
            }

            this.ambientManager.Update(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region RENDERING

        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            DrawAmbient(gameTime);
            DrawWorld(gameTime);
            DrawGUI(gameTime);
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(SColorPalette.DarkGray);

            // BACKGROUND
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();

            // SCENE
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();

            // DETAILS
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            DrawCursorPenActionArea();
            this.spriteBatch.End();

            // GUI
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.GuiRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (FINAL)
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(SColorPalette.DarkGray);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, this.graphicsManager.GetScreenScaleFactor(), SpriteEffects.None, 0f);
            this.cursorManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

        private void DrawAmbient(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(this.ambientManager.BackgroundHandler.SolidColor);

            DrawAmbientSky();
            DrawAmbientDetails(gameTime);
            DrawAmbientBackground(gameTime);
        }

        private void DrawAmbientSky()
        {
            if (!this.ambientManager.SkyHandler.IsActive)
            {
                return;
            }

            SGradientColorMap skyGradientColorMap = this.ambientManager.SkyHandler.GetSkyGradientByTime(this.world.Time.CurrentTime);
            float interpolation = skyGradientColorMap.GetInterpolationFactor(this.world.Time.CurrentTime);

            Effect skyEffect = this.ambientManager.SkyHandler.Effect;
            skyEffect.Parameters["StartColor1"].SetValue(skyGradientColorMap.Color1.Start.ToVector4());
            skyEffect.Parameters["StartColor2"].SetValue(skyGradientColorMap.Color2.Start.ToVector4());
            skyEffect.Parameters["EndColor1"].SetValue(skyGradientColorMap.Color1.End.ToVector4());
            skyEffect.Parameters["EndColor2"].SetValue(skyGradientColorMap.Color2.End.ToVector4());
            skyEffect.Parameters["TimeNormalized"].SetValue(interpolation);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, skyEffect, null);
            this.spriteBatch.Draw(this.ambientManager.SkyHandler.Texture, Vector2.Zero, null, SColorPalette.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
        }

        private void DrawAmbientDetails(GameTime gameTime)
        {
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            ((SCelestialBodyHandler)this.ambientManager.CelestialBodyHandler).Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawAmbientBackground(GameTime gameTime)
        {
            Effect backgroundEffect = null;

            if (this.ambientManager.BackgroundHandler.SelectedBackground.IsAffectedByLighting)
            {
                SGradientColorMap backgroundGradientColorMap = this.ambientManager.SkyHandler.GetBackgroundGradientByTime(this.world.Time.CurrentTime);
                float interpolation = backgroundGradientColorMap.GetInterpolationFactor(this.world.Time.CurrentTime);

                backgroundEffect = this.ambientManager.SkyHandler.Effect;
                backgroundEffect.Parameters["StartColor1"].SetValue(backgroundGradientColorMap.Color1.Start.ToVector4());
                backgroundEffect.Parameters["StartColor2"].SetValue(backgroundGradientColorMap.Color2.Start.ToVector4());
                backgroundEffect.Parameters["EndColor1"].SetValue(backgroundGradientColorMap.Color1.End.ToVector4());
                backgroundEffect.Parameters["EndColor2"].SetValue(backgroundGradientColorMap.Color2.End.ToVector4());
                backgroundEffect.Parameters["TimeNormalized"].SetValue(interpolation);
            }

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, backgroundEffect, null);
            ((SCloudHandler)this.ambientManager.CloudHandler).Draw(gameTime, this.spriteBatch);
            ((SBackgroundHandler)this.ambientManager.BackgroundHandler).Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawWorld(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawGUI(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.guiManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawCursorPenActionArea()
        {
            SPenTool penTool = this.gameInputController.Pen.Tool;

            if (penTool == SPenTool.Visualization || penTool == SPenTool.Fill)
            {
                return;
            }

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();
            Vector2 worldMousePosition = this.cameraManager.ScreenToWorld(mousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / SWorldConstants.GRID_SIZE),
                (int)Math.Floor(worldMousePosition.Y / SWorldConstants.GRID_SIZE)
            );

            foreach (Point point in this.gameInputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * SWorldConstants.GRID_SIZE,
                    point.Y * SWorldConstants.GRID_SIZE
                );

                Vector2 screenPosition = this.cameraManager.WorldToScreen(worldPosition);

                this.spriteBatch.Draw(this.mouseActionSquareTexture, screenPosition, null, this.gameplaySettings.PreviewAreaColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        #endregion

        #region EVENTS

        // Event occurs when the game window returns to focus.
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            this.gameManager.GameState.IsFocused = true;
            SSongEngine.Resume();
        }

        // Event occurs when the game window stops having focus.
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            this.gameManager.GameState.IsFocused = false;
            SSongEngine.Pause();
        }

        // Event occurs when the game process is finished.
        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            base.OnExiting(sender, args);
        }

        #endregion

        public void Quit()
        {
            Exit();
        }
    }
}