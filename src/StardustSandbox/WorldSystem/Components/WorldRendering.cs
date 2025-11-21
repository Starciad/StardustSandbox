using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.Managers;

using System;

namespace StardustSandbox.WorldSystem.Components
{
    internal sealed class WorldRendering(CameraManager cameraManager, InputController inputController, World world)
    {
        private Texture2D gridTexture;

        private readonly CameraManager cameraManager = cameraManager;
        private readonly ElementContext elementRenderingContext = new(world);
        private readonly InputController inputController = inputController;
        private readonly World world = world;

        internal void Initialize()
        {
            this.gridTexture = AssetDatabase.GetTexture("texture_shape_square_2");
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftWorld = this.cameraManager.ScreenToWorld(new Vector2(0, 0));
            Vector2 bottomRightWorld = this.cameraManager.ScreenToWorld(new Vector2(ScreenConstants.SCREEN_DIMENSIONS.X, ScreenConstants.SCREEN_DIMENSIONS.Y));

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
                        spriteBatch.Draw(this.gridTexture, targetPosition * WorldConstants.GRID_SIZE, null, new Color(AAP64ColorPalette.White, 124), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
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

            element.Context = this.elementRenderingContext;
            element.Draw(spriteBatch);
        }
    }
}
