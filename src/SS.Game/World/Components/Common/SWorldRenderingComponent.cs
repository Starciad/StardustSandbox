using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Mathematics;

using System.Collections.Generic;

namespace StardustSandbox.Game.World.Components.Common
{
    public sealed class SWorldRenderingComponent(SElementDatabase elementDatabase, SCameraManager cameraManager) : SWorldComponent
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
                    SSize2 targetSize = new(SWorldConstants.GRID_SCALE);

                    if (cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SCALE, targetSize, SWorldConstants.GRID_SCALE))
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
                    SElement element = elementDatabase.GetElementById(this.World.GetElementSlot(position).Id);

                    element.Context = new SElementContext(this.World, elementDatabase, this.World.GetElementSlot(position), position);
                    element.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
