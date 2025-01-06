using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Ambient.Handlers;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Helpers;

namespace StardustSandbox.Core
{
    public sealed partial class SGame
    {
        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            DrawAmbient(gameTime);
            DrawWorld(gameTime);
            DrawWorldLighting(gameTime);
            DrawGUI(gameTime);
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(SColorPalette.DarkGray);

            // BACKGROUND
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();

            // SCENE & LIGHTING
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.WorldLightingRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
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

        private void DrawWorldLighting(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.WorldLightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
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

            switch (penTool)
            {
                case SPenTool.Visualization:
                case SPenTool.Fill:
                    return;
            }

            Vector2 mousePosition = this.inputManager.GetScaledMousePosition();

            foreach (Point offset in this.gameInputController.Pen.GetShapePoints(mousePosition.ToPoint()))
            {
                Vector2 position = new Vector2(offset.X, offset.Y) / SWorldConstants.SLOT_SIZE * SWorldConstants.SLOT_SIZE;

                this.spriteBatch.Draw(this.mouseActionSquareTexture, position, null, new(SColorPalette.White, 32), 0f, new(SWorldConstants.SLOT_SIZE / 2), Vector2.One, SpriteEffects.None, 0f);
            }
        }
    }
}
