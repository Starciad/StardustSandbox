using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core
{
    public sealed partial class SGame
    {
        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            // BACKGROUND
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(this.backgroundManager.SolidColor);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.backgroundManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();

            // WORLD
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(gameTime, this.spriteBatch);
            this.entityManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();

            // LIGHTING
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.LightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.spriteBatch.End();

            // GUI
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.guiManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(Color.Black);
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.graphicsManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.LightingRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.GuiRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (FINAL)
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, this.graphicsManager.GetScreenScaleFactor(), SpriteEffects.None, 0f);
            this.cursorManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
