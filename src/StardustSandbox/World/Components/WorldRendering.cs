using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Camera;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.World;
using StardustSandbox.Inputs.Game;

using System;

namespace StardustSandbox.World.Components
{
    internal sealed class WorldRendering(InputController inputController, GameWorld world)
    {
        private readonly ElementContext elementRenderingContext = new(world);
        private readonly InputController inputController = inputController;
        private readonly GameWorld world = world;

        internal void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftWorld = SSCamera.ScreenToWorld(new Vector2(0, 0));
            Vector2 bottomRightWorld = SSCamera.ScreenToWorld(new Vector2(ScreenConstants.SCREEN_DIMENSIONS.X, ScreenConstants.SCREEN_DIMENSIONS.Y));

            int minTileX = (int)Math.Clamp(Math.Floor(topLeftWorld.X / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.X);
            int minTileY = (int)Math.Clamp(Math.Floor(topLeftWorld.Y / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.Y);
            int maxTileX = (int)Math.Clamp(Math.Ceiling(bottomRightWorld.X / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.X);
            int maxTileY = (int)Math.Clamp(Math.Ceiling(bottomRightWorld.Y / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.Y);

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    Vector2 targetPosition = new(x, y);

                    if (this.inputController.Pen.Tool != PenTool.Visualization)
                    {
                        spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ShapeSquares), targetPosition * WorldConstants.GRID_SIZE, new(32, 0, 32, 32), new Color(AAP64ColorPalette.White, 124), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }

                    if (this.world.TryGetSlot(targetPosition.ToPoint(), out Slot value))
                    {
                        if (!value.BackgroundLayer.HasState(ElementStates.IsEmpty))
                        {
                            DrawSlotLayer(spriteBatch, value.Position, LayerType.Background, value, value.GetLayer(LayerType.Background).Element);
                        }

                        if (!value.ForegroundLayer.HasState(ElementStates.IsEmpty))
                        {
                            DrawSlotLayer(spriteBatch, value.Position, LayerType.Foreground, value, value.GetLayer(LayerType.Foreground).Element);
                        }
                    }
                }
            }
        }

        private void DrawSlotLayer(SpriteBatch spriteBatch, Point position, LayerType layer, Slot value, Element element)
        {
            this.elementRenderingContext.UpdateInformation(position, layer, value);

            element.SetContext(this.elementRenderingContext);
            element.Draw(spriteBatch);
        }
    }
}
