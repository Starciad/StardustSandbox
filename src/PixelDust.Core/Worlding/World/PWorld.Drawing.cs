using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements.Contexts;
using PixelDust.Core.Engine.Assets;
using PixelDust.Core.Engine.Components;
using PixelDust.Core.Worlding.Components;

namespace PixelDust.Core.Worlding
{
    public sealed partial class PWorld
    {
        public void Draw()
        {
            if (!this.States.IsActive)
            {
                return;
            }

            PGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, PWorldCamera.Camera.GetViewMatrix());

            // System
            DrawSlots();

            // Components
            foreach (PWorldComponent component in this._components)
            {
                component.Draw();
            }

            PGraphics.SpriteBatch.End();
        }

        private void DrawSlots()
        {
            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyElementSlot(new(x, y)))
                    {
                        PGraphics.SpriteBatch.Draw(PTextures.Pixel, new Vector2(x * Scale, y * Scale), null, Color.Black, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        this.Elements[x, y].Instance.Draw(new PElementContext(this, this.Elements[x, y], new(x, y)));
                    }
                }
            }
        }
    }
}
