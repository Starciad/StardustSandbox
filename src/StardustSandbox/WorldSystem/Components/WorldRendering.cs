using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.Managers;

using System;
using System.Collections.Generic;

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
            foreach (Slot worldSlot in GetAllSlotsForRendering(spriteBatch))
            {
                if (!worldSlot.BackgroundLayer.IsEmpty)
                {
                    DrawSlotLayer(spriteBatch, worldSlot.Position, LayerType.Background, worldSlot, worldSlot.GetLayer(LayerType.Background).Element);
                }

                if (!worldSlot.ForegroundLayer.IsEmpty)
                {
                    DrawSlotLayer(spriteBatch, worldSlot.Position, LayerType.Foreground, worldSlot, worldSlot.GetLayer(LayerType.Foreground).Element);
                }
            }
        }

        private IEnumerable<Slot> GetAllSlotsForRendering(SpriteBatch spriteBatch)
        {
            float gridScale = WorldConstants.GRID_SIZE * this.cameraManager.Zoom;

            Vector2 cameraTopLeft = this.cameraManager.ScreenToWorld(Vector2.Zero);
            Vector2 cameraBottomRight = this.cameraManager.ScreenToWorld(new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT));

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
            maxSlot.X = Math.Min(maxSlot.X, this.world.Information.Size.X - 1);
            maxSlot.Y = Math.Min(maxSlot.Y, this.world.Information.Size.Y - 1);

            for (int y = minSlot.Y; y <= maxSlot.Y; y++)
            {
                for (int x = minSlot.X; x <= maxSlot.X; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    Point targetSize = new(WorldConstants.GRID_SIZE);

                    if (this.cameraManager.InsideCameraBounds(targetPosition * WorldConstants.GRID_SIZE, targetSize, true, WorldConstants.GRID_SIZE))
                    {
                        if (this.inputController.Pen.Tool != PenTool.Visualization)
                        {
                            spriteBatch.Draw(this.gridTexture, targetPosition * WorldConstants.GRID_SIZE, null, new Color(AAP64ColorPalette.White, 124), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                        }

                        if (this.world.TryGetSlot(targetPosition.ToPoint(), out Slot value))
                        {
                            yield return value;
                        }
                    }
                }
            }
        }

        private void DrawSlotLayer(SpriteBatch spriteBatch, Point position, LayerType worldLayer, Slot worldSlot, Element element)
        {
            this.elementRenderingContext.UpdateInformation(position, worldLayer, worldSlot);

            element.Context = this.elementRenderingContext;
            element.Draw(spriteBatch);
        }
    }
}
