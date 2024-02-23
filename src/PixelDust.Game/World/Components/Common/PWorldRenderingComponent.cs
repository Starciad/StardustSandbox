using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Managers;
using PixelDust.Game.Mathematics;

using System.Collections.Generic;

namespace PixelDust.Game.World.Components.Common
{
    public sealed class PWorldRenderingComponent(PElementDatabase elementDatabase, PCameraManager cameraManager) : PWorldComponent
    {
        private readonly List<Point> _slotsCapturedForRendering = [];

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GetAllElementsForRendering();
            DrawAllCapturedElements(gameTime, spriteBatch);
        }

        private void GetAllElementsForRendering()
        {
            this._slotsCapturedForRendering.Clear();

            for (int y = 0; y < this.World.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.World.Infos.Size.Width; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    Size2 targetSize = new(PWorldConstants.GRID_SCALE);

                    if (cameraManager.InsideCameraBounds(targetPosition * PWorldConstants.GRID_SCALE, targetSize, PWorldConstants.GRID_SCALE))
                    {
                        this._slotsCapturedForRendering.Add(targetPosition.ToPoint());
                    }
                }
            }
        }

        private void DrawAllCapturedElements(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Point position in this._slotsCapturedForRendering)
            {
                if (!this.World.IsEmptyElementSlot(position))
                {
                    PElement element = elementDatabase.GetElementById(this.World.GetElementSlot(position).Id);

                    element.Context = new PElementContext(this.World, elementDatabase, this.World.GetElementSlot(position), position);
                    element.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
