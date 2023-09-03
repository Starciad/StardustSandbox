using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public void Draw()
        {
            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PWorldCamera.Camera.GetViewMatrix());
            if (!States.IsActive)
            {
                PGraphics.SpriteBatch.End();
                return;
            }

            // System
            DrawSlots();

            // Components
            foreach (PWorldComponent component in _components)
            {
                component.Draw();
            }

            PGraphics.SpriteBatch.End();
        }

        private void DrawSlots()
        {
            for (int x = 0; x < Infos.Size.Width; x++)
            {
                for (int y = 0; y < Infos.Size.Height; y++)
                {
                    elementDrawContext.Update(Elements[x, y], new(x, y));

                    if (!IsEmptyElementSlot(new(x, y)))
                    {
                        Elements[x, y].Instance.Draw(elementDrawContext);
                    }
                    else
                    {
                        PGraphics.SpriteBatch.Draw(PTextures.Pixel, new Vector2(x * Scale, y * Scale), null, Color.Black, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
