using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Settings;

namespace StardustSandbox.Managers
{
    internal sealed class VideoManager
    {
        internal GraphicsDeviceManager GraphicsDeviceManager => this._graphicsDeviceManager;
        internal GraphicsDevice GraphicsDevice => this._graphicsDeviceManager.GraphicsDevice;
        internal GameWindow GameWindow { get; private set; }

        internal Viewport Viewport => this.GraphicsDevice.Viewport;

        internal RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        internal RenderTarget2D UIRenderTarget => this.uiRenderTarget;
        internal RenderTarget2D BackgroundRenderTarget => this.backgroundRenderTarget;
        internal RenderTarget2D WorldRenderTarget => this.worldRenderTarget;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private RenderTarget2D backgroundRenderTarget;
        private RenderTarget2D screenRenderTarget;
        private RenderTarget2D uiRenderTarget;
        private RenderTarget2D worldRenderTarget;

        internal VideoManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            this._graphicsDeviceManager = graphicsDeviceManager;
            ApplySettings();
        }

        internal void Initialize()
        {
            int width = ScreenConstants.SCREEN_WIDTH;
            int height = ScreenConstants.SCREEN_HEIGHT;

            this.screenRenderTarget = new(this.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
            this.uiRenderTarget = new(this.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
            this.backgroundRenderTarget = new(this.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
            this.worldRenderTarget = new(this.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);
        }

        internal void ApplySettings()
        {
            VideoSettings videoSettings = SettingsHandler.LoadSettings<VideoSettings>();

            if (this.GameWindow != null)
            {
                this.GameWindow.IsBorderless = videoSettings.Borderless;
            }

            if (videoSettings.Width == 0 || videoSettings.Height == 0)
            {
                this._graphicsDeviceManager.PreferredBackBufferWidth = ScreenConstants.SCREEN_WIDTH;
                this._graphicsDeviceManager.PreferredBackBufferHeight = ScreenConstants.SCREEN_HEIGHT;
            }
            else
            {
                this._graphicsDeviceManager.PreferredBackBufferWidth = videoSettings.Width;
                this._graphicsDeviceManager.PreferredBackBufferHeight = videoSettings.Height;
            }

            this._graphicsDeviceManager.IsFullScreen = videoSettings.FullScreen;
            this._graphicsDeviceManager.SynchronizeWithVerticalRetrace = videoSettings.VSync;
            this._graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            this._graphicsDeviceManager.ApplyChanges();
        }

        internal void SetGameWindow(GameWindow gameWindow)
        {
            this.GameWindow = gameWindow;
        }

        internal Vector2 GetScreenScaleFactor()
        {
            return new(
                this._graphicsDeviceManager.PreferredBackBufferWidth / (float)ScreenConstants.SCREEN_WIDTH,
                this._graphicsDeviceManager.PreferredBackBufferHeight / (float)ScreenConstants.SCREEN_HEIGHT
            );
        }
    }
}
