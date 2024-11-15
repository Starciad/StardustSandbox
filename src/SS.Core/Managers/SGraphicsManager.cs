using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.IO;
using StardustSandbox.Game.Models.Settings;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SGraphicsManager : SGameObject
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this._graphicsDeviceManager;
        public GraphicsDevice GraphicsDevice => this._graphicsDeviceManager.GraphicsDevice;

        public RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        public RenderTarget2D GuiRenderTarget => this.guiRenderTarget;
        public RenderTarget2D BackgroundRenderTarget => this.backgroundRenderTarget;
        public RenderTarget2D WorldRenderTarget => this.worldRenderTarget;
        public RenderTarget2D LightingRenderTarget => this.lightingRenderTarget;

        public Viewport Viewport => this.GraphicsDevice.Viewport;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        // ENGINE
        private RenderTarget2D screenRenderTarget;

        // SCENE
        private RenderTarget2D guiRenderTarget;
        private RenderTarget2D backgroundRenderTarget;
        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D lightingRenderTarget;

        public SGraphicsManager(ISGame gameInstance, GraphicsDeviceManager graphicsDeviceManager) : base(gameInstance)
        {
            this._graphicsDeviceManager = graphicsDeviceManager;
            UpdateSettings();
        }

        public override void Initialize()
        {
            int width = SScreenConstants.DEFAULT_SCREEN_WIDTH;
            int height = SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            this.screenRenderTarget = new(this.GraphicsDevice, width, height);
            this.guiRenderTarget = new(this.GraphicsDevice, width, height);
            this.backgroundRenderTarget = new(this.GraphicsDevice, width, height);
            this.worldRenderTarget = new(this.GraphicsDevice, width, height);
            this.lightingRenderTarget = new(this.GraphicsDevice, width, height);
        }

        public void UpdateSettings()
        {
            SGraphicsSettings graphicsSettings = SSystemSettingsFile.GetGraphicsSettings();

            this._graphicsDeviceManager.IsFullScreen = graphicsSettings.FullScreen;
            this._graphicsDeviceManager.PreferredBackBufferWidth = graphicsSettings.ScreenWidth;
            this._graphicsDeviceManager.PreferredBackBufferHeight = graphicsSettings.ScreenHeight;
            this._graphicsDeviceManager.SynchronizeWithVerticalRetrace = graphicsSettings.VSync;
            this._graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            this._graphicsDeviceManager.ApplyChanges();
        }
    }
}
