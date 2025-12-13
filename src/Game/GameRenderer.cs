using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Camera;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.InputSystem;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox
{
    internal static class GameRenderer
    {
        internal static RenderTarget2D ScreenRenderTarget2D => screenRenderTarget2D;

        private static bool isInitialized;
        private static bool isUnloaded;

        private static RenderTarget2D screenRenderTarget2D;
        private static RenderTarget2D backgroundRenderTarget2D;
        private static RenderTarget2D uiRenderTarget2D;
        private static RenderTarget2D worldRenderTarget2D;

        private static GraphicsDevice graphicsDevice;

        internal static void Initialize(VideoManager videoManager)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} has already been initialized.");
            }

            graphicsDevice = videoManager.GraphicsDevice;

            int width = ScreenConstants.SCREEN_WIDTH;
            int height = ScreenConstants.SCREEN_HEIGHT;

            screenRenderTarget2D = CreateRenderTarget(width, height);
            uiRenderTarget2D = CreateRenderTarget(width, height);
            backgroundRenderTarget2D = CreateRenderTarget(width, height);
            worldRenderTarget2D = CreateRenderTarget(width, height);

            isInitialized = true;
            isUnloaded = false;
        }

        internal static void Unload()
        {
            if (isUnloaded)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} has already been unloaded.");
            }

            DisposeRenderTarget(ref screenRenderTarget2D);
            DisposeRenderTarget(ref uiRenderTarget2D);
            DisposeRenderTarget(ref backgroundRenderTarget2D);
            DisposeRenderTarget(ref worldRenderTarget2D);

            isUnloaded = true;
            isInitialized = false;
        }

        internal static void Draw(
            in AmbientManager ambientManager,
            in Color cursorPreviewAreaColor,
            in CursorManager cursorManager,
            in InputController inputController,
            in SpriteBatch spriteBatch,
            in UIManager uiManager,
            in VideoManager videoManager,
            in World world
        )
        {
            if (!isInitialized || isUnloaded)
            {
                throw new InvalidOperationException($"{nameof(GameRenderer)} is not properly initialized or has been unloaded.");
            }

            DrawAmbient(spriteBatch, ambientManager);
            DrawWorld(spriteBatch, world);
            DrawGUI(spriteBatch, uiManager);

            graphicsDevice.SetRenderTarget(screenRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(backgroundRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.Draw(worldRenderTarget2D, Vector2.Zero, Color.White);
            DrawCursorPenActionArea(spriteBatch, inputController, cursorPreviewAreaColor);
            spriteBatch.Draw(uiRenderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(screenRenderTarget2D, videoManager.AdjustRenderTargetOnScreen(screenRenderTarget2D), Color.White);
            cursorManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static RenderTarget2D CreateRenderTarget(int width, int height)
        {
            return new RenderTarget2D(
                graphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents,
                false
            );
        }

        private static void DisposeRenderTarget(ref RenderTarget2D renderTarget)
        {
            if (renderTarget is not null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }
        }

        private static void DrawAmbient(in SpriteBatch spriteBatch, in AmbientManager ambientManager)
        {
            graphicsDevice.SetRenderTarget(backgroundRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            // Sky
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, AssetDatabase.GetEffect(EffectIndex.GradientTransition), null);
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Pixel), Vector2.Zero, null, AAP64ColorPalette.White, 0f, Vector2.Zero, new Vector2(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT), SpriteEffects.None, 0f);
            spriteBatch.End();

            // Celestial Bodies
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            ambientManager.CelestialBodyHandler.Draw(spriteBatch);
            spriteBatch.End();

            // Background
            if (ambientManager.BackgroundHandler.IsAffectedByLighting)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.GradientTransition), null);
                ambientManager.CloudHandler.Draw(spriteBatch);
                ambientManager.BackgroundHandler.Draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
                ambientManager.CloudHandler.Draw(spriteBatch);
                ambientManager.BackgroundHandler.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        private static void DrawWorld(in SpriteBatch spriteBatch, in World world)
        {
            graphicsDevice.SetRenderTarget(worldRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, SSCamera.GetViewMatrix());
            world.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawGUI(in SpriteBatch spriteBatch, in UIManager uiManager)
        {
            graphicsDevice.SetRenderTarget(uiRenderTarget2D);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            uiManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawCursorPenActionArea(in SpriteBatch spriteBatch, in InputController inputController, in Color previewAreaColor)
        {
            PenTool penTool = inputController.Pen.Tool;

            if (penTool is PenTool.Visualization or PenTool.Fill)
            {
                return;
            }

            Vector2 mousePosition = Input.GetScaledMousePosition();
            Vector2 worldMousePosition = SSCamera.ScreenToWorld(mousePosition);

            Point alignedPosition = new(
                (int)Math.Floor(worldMousePosition.X / WorldConstants.GRID_SIZE),
                (int)Math.Floor(worldMousePosition.Y / WorldConstants.GRID_SIZE)
            );

            foreach (Point point in inputController.Pen.GetShapePoints(alignedPosition))
            {
                Vector2 worldPosition = new(
                    point.X * WorldConstants.GRID_SIZE,
                    point.Y * WorldConstants.GRID_SIZE
                );

                Vector2 screenPosition = SSCamera.WorldToScreen(worldPosition);

                spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.ShapeSquares),
                    screenPosition,
                    new Rectangle(110, 0, 32, 32),
                    previewAreaColor,
                    0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        internal static Vector2 CalculateScaledMousePosition(in Vector2 mousePosition, in VideoManager videoManager)
        {
            Rectangle adjustedScreen = videoManager.AdjustRenderTargetOnScreen(ScreenRenderTarget2D);

            float scale = adjustedScreen.Width / (float)ScreenRenderTarget2D.Width;

            float mouseX = (mousePosition.X - adjustedScreen.X) / scale;
            float mouseY = (mousePosition.Y - adjustedScreen.Y) / scale;

            mouseX = Math.Clamp(mouseX, 0, ScreenRenderTarget2D.Width - 1);
            mouseY = Math.Clamp(mouseY, 0, ScreenRenderTarget2D.Height - 1);

            return new Vector2(mouseX, mouseY);
        }
    }
}
