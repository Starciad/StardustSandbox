using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static class that contains information related to the screen/window of the game.
    /// </summary>
    public static class PScreen
    {
        public static Vector2 DefaultResolution { get; private set; }
        public static Vector2 CurrentResolution { get; private set; }

        public static int DefaultWidth => 1280;
        public static int DefaultHeight => 720;

        internal static Vector2[] Resolutions => resolutions;

        // 16:9 Aspect Ratio
        private static readonly Vector2[] resolutions = new Vector2[]
        {
            new(640, 360), // nHD
            new(854, 480), // FWVGA
            new(960, 540), // qHD
            new(1280, 720), // SD / HD ready (720p)
            new(1366, 768), // WXGA
            new(1600, 900), // HD+
            new(1920, 1080), // FHD / Full HD (1080p)
            new(2560, 1440), // WQHD
            new(3200, 1800), // QHD+
            new(3840, 2160), // 4K UHD
            new(5120, 2880), // 5K
            new(7680, 4320), // 8K UHD
            new(15360, 8640) // 16K UHD
        };

        private static Matrix _ScaleMatrix;
        private static bool _FullScreen = false;
        private static bool _dirtyMatrix = true;

        internal static void Initialize()
        {
            DefaultResolution = resolutions[3];
            CurrentResolution = new(PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth, PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight);

            _dirtyMatrix = true;
            ApplyResolutionSettings();
        }

        public static void SetResolution(Vector2 size, bool fullScreen)
        {
            CurrentResolution = size;
            _FullScreen = fullScreen;

            ApplyResolutionSettings();
        }

        public static void SetDefaultResolution(Vector2 size)
        {
            DefaultResolution = size;
            _dirtyMatrix = true;
        }

        private static void ApplyResolutionSettings()
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (_FullScreen == false)
            {
                if ((CurrentResolution.X <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (CurrentResolution.Y <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth = (int)CurrentResolution.X;
                    PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight = (int)CurrentResolution.Y;
                    PGraphics.GraphicsDeviceManager.IsFullScreen = _FullScreen;
                    PGraphics.GraphicsDeviceManager.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set. To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == CurrentResolution.X) && (dm.Height == CurrentResolution.Y))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth = (int)CurrentResolution.X;
                        PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight = (int)CurrentResolution.Y;
                        PGraphics.GraphicsDeviceManager.IsFullScreen = _FullScreen;
                        PGraphics.GraphicsDeviceManager.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;
            CurrentResolution = new(PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth, PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Sets the device to use the draw pump
        /// Sets correct aspect ratio
        /// </summary>
        public static void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            PGraphics.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            PGraphics.GraphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        private static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _ScaleMatrix = Matrix.CreateScale(
                           PGraphics.GraphicsDevice.Viewport.Width / DefaultResolution.X,
                           PGraphics.GraphicsDevice.Viewport.Width / DefaultResolution.Y,
                           1f);
        }


        public static void FullViewport()
        {
            Viewport vp = new();
            vp.X = vp.Y = 0;
            vp.Width = (int)CurrentResolution.X;
            vp.Height = (int)CurrentResolution.Y;
            PGraphics.GraphicsDevice.Viewport = vp;
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public static float GetAspectRatio()
        {
            return DefaultResolution.X / DefaultResolution.Y;
        }

        public static void ResetViewport()
        {
            float targetAspectRatio = GetAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            int width = PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + .5f);
            bool changed = false;

            if (height > PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight)
            {
                height = PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport viewport = new()
            {
                X = (PGraphics.GraphicsDeviceManager.PreferredBackBufferWidth / 2) - (width / 2),
                Y = (PGraphics.GraphicsDeviceManager.PreferredBackBufferHeight / 2) - (height / 2),
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };

            if (changed)
            {
                _dirtyMatrix = true;
            }

            PGraphics.GraphicsDevice.Viewport = viewport;
        }

        public static Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix) RecreateScaleMatrix();
            return _ScaleMatrix;
        }
    }
}
