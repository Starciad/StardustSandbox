using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.World;
using StardustSandbox.Game.World.Components;

using System.Collections.Generic;

namespace StardustSandbox.Game.GameContent.World.Components
{
    public sealed class SWorldRenderingComponent : SWorldComponent
    {
        private readonly List<Point> _slotsCapturedForRendering = [];
        private readonly SElementDatabase elementDatabase;
        private readonly SCameraManager cameraManager;

        public SWorldRenderingComponent(SGame gameInstance, SWorld worldInstance, SElementDatabase elementDatabase, SCameraManager cameraManager) : base(gameInstance, worldInstance)
        {
            this.elementDatabase = elementDatabase;
            this.cameraManager = cameraManager;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GetAllElementsForRendering();
            DrawAllCapturedElements(gameTime, spriteBatch);
        }

        private void GetAllElementsForRendering()
        {
            this._slotsCapturedForRendering.Clear();

            for (int y = 0; y < this.SWorldInstance.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.SWorldInstance.Infos.Size.Width; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    SSize2 targetSize = new(SWorldConstants.GRID_SCALE);

                    if (this.cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SCALE, targetSize, SWorldConstants.GRID_SCALE))
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
                if (!this.SWorldInstance.IsEmptyElementSlot(position))
                {
                    SElement element = this.elementDatabase.GetElementById(this.SWorldInstance.GetElementSlot(position).Id);

                    element.Context = new SElementContext(this.SWorldInstance, this.elementDatabase, this.SWorldInstance.GetElementSlot(position), position);
                    element.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
