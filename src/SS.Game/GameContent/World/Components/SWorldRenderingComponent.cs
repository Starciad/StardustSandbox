﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.Mathematics.Primitives;
using StardustSandbox.Game.World;
using StardustSandbox.Game.World.Components;

using System.Collections.Generic;

namespace StardustSandbox.Game.GameContent.World.Components
{
    public sealed class SWorldRenderingComponent(SGame gameInstance, SWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementRenderingContext = new(worldInstance, gameInstance.ElementDatabase);

        private Texture2D _gridTexture;

        private readonly List<Point> _slotsCapturedForRendering = [];
        private readonly SCameraManager _cameraManager = gameInstance.CameraManager;

        public override void Initialize()
        {
            this._gridTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_2");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw2DWorldGrid(spriteBatch);
            GetAllElementsForRendering();
            DrawAllCapturedElements(gameTime, spriteBatch);
        }

        private void Draw2DWorldGrid(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < this.SWorldInstance.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.SWorldInstance.Infos.Size.Width; x++)
                {
                    Vector2 targetPosition = new(x, y);
                    SSize2 targetSize = new(SWorldConstants.GRID_SCALE);

                    if (this._cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SCALE, targetSize, true, SWorldConstants.GRID_SCALE))
                    {
                        spriteBatch.Draw(this._gridTexture, targetPosition * SWorldConstants.GRID_SCALE, null, new(Color.White, 16), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }
                }
            }
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

                    if (this._cameraManager.InsideCameraBounds(targetPosition * SWorldConstants.GRID_SCALE, targetSize, true, SWorldConstants.GRID_SCALE))
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
                    SElement element = this.SWorldInstance.GetElementSlot(position).Element;

                    this.elementRenderingContext.UpdateInformation(this.SWorldInstance.GetElementSlot(position), position);

                    element.Context = this.elementRenderingContext;
                    element.Draw(gameTime, spriteBatch);

                    //spriteBatch.DrawString(this.SGameInstance.AssetDatabase.Fonts[0], this.SWorldInstance.GetElementSlot(position).Temperature.ToString(), new(position.X * 32, position.Y * 32), Color.Red, 0f, Vector2.Zero, new(0.05f), SpriteEffects.None, 0f, false);
                }
            }
        }
    }
}
