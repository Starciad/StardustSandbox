using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Slots;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.World
{
    public sealed class SWorldRenderingComponent(ISGame gameInstance, ISWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementRenderingContext = new(worldInstance);

        private Texture2D gridTexture;

        private readonly ISCameraManager cameraManager = gameInstance.CameraManager;

        public override void Initialize()
        {
            this.gridTexture = this.SGameInstance.AssetDatabase.GetTexture("texture_shape_square_2");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SWorldSlot worldSlot in GetAllSlotsForRendering(spriteBatch))
            {
                if (!worldSlot.BackgroundLayer.IsEmpty)
                {
                    DrawWorldSlotLayer(gameTime, spriteBatch, worldSlot.Position, SWorldLayer.Background, worldSlot, worldSlot.GetLayer(SWorldLayer.Background).Element);
                }

                if (!worldSlot.ForegroundLayer.IsEmpty)
                {
                    DrawWorldSlotLayer(gameTime, spriteBatch, worldSlot.Position, SWorldLayer.Foreground, worldSlot, worldSlot.GetLayer(SWorldLayer.Foreground).Element);
                }
            }
        }

        private IEnumerable<SWorldSlot> GetAllSlotsForRendering(SpriteBatch spriteBatch)
        {
            float gridScale = SWorldConstants.GRID_SIZE * this.cameraManager.Zoom;

            Vector2 cameraTopLeft = this.cameraManager.ScreenToWorld(Vector2.Zero);
            Vector2 cameraBottomRight = this.cameraManager.ScreenToWorld(new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT));

            Point minSlot = new(
                (int)Math.Floor(cameraTopLeft.X / gridScale),
                (int)Math.Floor(cameraTopLeft.Y / gridScale)
            );

            Point maxSlot = new(
                (int)Math.Ceiling(cameraBottomRight.X / gridScale),
                (int)Math.Ceiling(cameraBottomRight.Y / gridScale)
            );

            minSlot.X = Math.Max(minSlot.X, 0);
            minSlot.Y = Math.Max(minSlot.Y, 0);
            maxSlot.X = Math.Min(maxSlot.X, this.SWorldInstance.Infos.Size.Width - 1);
            maxSlot.Y = Math.Min(maxSlot.Y, this.SWorldInstance.Infos.Size.Height - 1);

            for (int y = minSlot.Y; y <= maxSlot.Y; y++)
            {
                for (int x = minSlot.X; x <= maxSlot.X; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    SSize2 targetSize = new(SWorldConstants.GRID_SIZE);

                    if (this.cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SIZE, targetSize, true, SWorldConstants.GRID_SIZE))
                    {
                        if (this.SGameInstance.GameInputController.Pen.Tool != SPenTool.Visualization)
                        {
                            spriteBatch.Draw(this.gridTexture, targetPosition * SWorldConstants.GRID_SIZE, null, new Color(SColorPalette.White, 124), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                        }

                        if (this.SWorldInstance.TryGetWorldSlot(targetPosition.ToPoint(), out SWorldSlot value))
                        {
                            yield return value;
                        }
                    }
                }
            }
        }

        private void DrawWorldSlotLayer(GameTime gameTime, SpriteBatch spriteBatch, Point position, SWorldLayer worldLayer, SWorldSlot worldSlot, ISElement element)
        {
            this.elementRenderingContext.UpdateInformation(position, worldLayer, worldSlot);

            element.Context = this.elementRenderingContext;
            element.Draw(gameTime, spriteBatch);
        }
    }
}
