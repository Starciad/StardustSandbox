using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PGraphicsManager(GraphicsDeviceManager gdm) : PGameObject
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this._gdm;
        public GraphicsDevice GraphicsDevice => this._gdm.GraphicsDevice;

        public RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        public RenderTarget2D GuiRenderTarget => this.guiRenderTarget;
        public RenderTarget2D BackgroundRenderTarget => this.backgroundRenderTarget;
        public RenderTarget2D WorldRenderTarget => this.worldRenderTarget;
        public RenderTarget2D LightingRenderTarget => this.lightingRenderTarget;

        private readonly GraphicsDeviceManager _gdm = gdm;

        // ENGINE
        private RenderTarget2D screenRenderTarget;

        // SCENE
        private RenderTarget2D guiRenderTarget;
        private RenderTarget2D backgroundRenderTarget;
        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D lightingRenderTarget;

        protected override void OnAwake()
        {
            this.screenRenderTarget = new(this.GraphicsDevice, PScreenConstants.SCREEN_WIDTH, PScreenConstants.SCREEN_HEIGHT);
            this.guiRenderTarget = new(this.GraphicsDevice, PScreenConstants.SCREEN_WIDTH, PScreenConstants.SCREEN_HEIGHT);
            this.backgroundRenderTarget = new(this.GraphicsDevice, PScreenConstants.SCREEN_WIDTH, PScreenConstants.SCREEN_HEIGHT);
            this.worldRenderTarget = new(this.GraphicsDevice, PScreenConstants.SCREEN_WIDTH, PScreenConstants.SCREEN_HEIGHT);
            this.lightingRenderTarget = new(this.GraphicsDevice, PScreenConstants.SCREEN_WIDTH, PScreenConstants.SCREEN_HEIGHT);
        }
    }
}
