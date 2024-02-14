using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.IO;
using PixelDust.Game.Models.Settings;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PGraphicsManager : PGameObject
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this._gdm;
        public GraphicsDevice GraphicsDevice => this._gdm.GraphicsDevice;

        public RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        public RenderTarget2D GuiRenderTarget => this.guiRenderTarget;
        public RenderTarget2D BackgroundRenderTarget => this.backgroundRenderTarget;
        public RenderTarget2D WorldRenderTarget => this.worldRenderTarget;
        public RenderTarget2D LightingRenderTarget => this.lightingRenderTarget;

        private readonly GraphicsDeviceManager _gdm;

        // ENGINE
        private RenderTarget2D screenRenderTarget;

        // SCENE
        private RenderTarget2D guiRenderTarget;
        private RenderTarget2D backgroundRenderTarget;
        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D lightingRenderTarget;

        public PGraphicsManager(GraphicsDeviceManager gdm)
        {
            this._gdm = gdm;
            UpdateSettings();
        }

        protected override void OnAwake()
        {
            int width = PScreenConstants.DEFAULT_SCREEN_WIDTH;
            int height = PScreenConstants.DEFAULT_SCREEN_HEIGHT;

            this.screenRenderTarget = new(this.GraphicsDevice, width, height);
            this.guiRenderTarget = new(this.GraphicsDevice, width, height);
            this.backgroundRenderTarget = new(this.GraphicsDevice, width, height);
            this.worldRenderTarget = new(this.GraphicsDevice, width, height);
            this.lightingRenderTarget = new(this.GraphicsDevice, width, height);
        }

        public void UpdateSettings()
        {
            PGraphicsSettings graphicsSettings = PSystemSettingsFile.GetGraphicsSettings();

            this._gdm.IsFullScreen = graphicsSettings.FullScreen;
            this._gdm.PreferredBackBufferWidth = graphicsSettings.ScreenWidth;
            this._gdm.PreferredBackBufferHeight = graphicsSettings.ScreenHeight;
            this._gdm.SynchronizeWithVerticalRetrace = graphicsSettings.VSync;
            this._gdm.GraphicsProfile = GraphicsProfile.HiDef;
            this._gdm.ApplyChanges();
        }
    }
}
