using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.World
{
    public sealed class SWorldRenderingComponent(ISGame gameInstance, ISWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementRenderingContext = new(worldInstance);

        private Texture2D _gridTexture;

        private readonly List<Point> _slotsCapturedForRendering = [];
        private readonly ISCameraManager _cameraManager = gameInstance.CameraManager;

        public override void Initialize()
        {
            this._gridTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_2");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GetAllSlotsForRendering(spriteBatch);
            DrawAllCapturedElements(gameTime, spriteBatch);
        }

        private void GetAllSlotsForRendering(SpriteBatch spriteBatch)
        {
            this._slotsCapturedForRendering.Clear();

            for (int y = 0; y < this.SWorldInstance.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.SWorldInstance.Infos.Size.Width; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    SSize2 targetSize = new(SWorldConstants.GRID_SCALE);

                    if (this._cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SCALE, targetSize, true, SWorldConstants.GRID_SCALE))
                    {
                        spriteBatch.Draw(this._gridTexture, targetPosition * SWorldConstants.GRID_SCALE, null, new(Color.White, 16), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

                        if (this.SWorldInstance.IsEmptyWorldSlot(targetPosition.ToPoint()))
                        {
                            continue;
                        }

                        this._slotsCapturedForRendering.Add(targetPosition.ToPoint());
                    }
                }
            }
        }

        private void DrawAllCapturedElements(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Point position in this._slotsCapturedForRendering)
            {
                ISWorldSlot worldSlot = this.SWorldInstance.GetWorldSlot(position);
                SWorldLayer worldLayer = SWorldLayer.Background;
                SElement element = (SElement)worldSlot.GetLayer(SWorldLayer.Background).Element;

                if (!worldSlot.ForegroundLayer.IsEmpty)
                {
                    worldLayer = SWorldLayer.Foreground;
                    element = (SElement)worldSlot.GetLayer(SWorldLayer.Foreground).Element;
                }

                this.elementRenderingContext.UpdateInformation(position, worldLayer, worldSlot);

                element.Context = this.elementRenderingContext;
                element.Draw(gameTime, spriteBatch);

                // [ DEBUG ]
                //spriteBatch.DrawString(this.SGameInstance.AssetDatabase.Fonts[0], this.SWorldInstance.GetElementSlot(position).Temperature.ToString(), new(position.X * 32, position.Y * 32), Color.Red, 0f, Vector2.Zero, new(0.05f), SpriteEffects.None, 0f, false);
            }
        }
    }
}
