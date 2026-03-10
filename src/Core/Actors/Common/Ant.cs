/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Saving.Data;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.Actors.Common
{
    internal sealed class Ant : Actor
    {
        private static readonly Rectangle[] sourceRectangleSprites =
        [
            // ================================ //
            // Holding Surfaces

            // [0] Holding: Below; Direction: Right;
            new(32, 0, 32, 32),

            // [1] Holding: Below; Direction: Left;
            new(32, 32, 32, 32),

            // [2] Holding: Up; Direction: Right;
            new(64, 0, 32, 32),

            // [3] Holding: Up; Direction: Left;
            new(64, 32, 32, 32),

            // [4] Holding: Left; Direction: Down;
            new(96, 0, 32, 32),

            // [5] Holding: Left; Direction: Up;
            new(96, 32, 32, 32),

            // [6] Holding: Right; Direction: Down;
            new(96, 0, 32, 32),

            // [7] Holding: Right; Direction: Up;
            new(96, 32, 32, 32),

            // ================================ //
            // Holding Background

            // [8] Holding: Background; Direction: Up;
            new(32, 64, 32, 32),

            // [9] Holding: Background; Direction: Right;
            new(64, 64, 32, 32),

            // [10] Holding: Background; Direction: Down;
            new(32, 96, 32, 32),

            // [11] Holding: Background; Direction: Left;
            new(64, 96, 32, 32),
        ];

        internal Ant(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {
            Reset();
        }

        private static Rectangle GetSourceRectangle()
        {
            return sourceRectangleSprites[0];
        }

        public override void Reset()
        {

        }

        internal override void Update(GameTime gameTime)
        {

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.Actors),
                new(this.PositionX * SpriteConstants.SPRITE_SCALE, this.PositionY * SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE, SpriteConstants.SPRITE_SCALE),
                GetSourceRectangle(),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f
            );
        }
        
        internal override ActorData Serialize()
        {
            return new()
            {
                Index = ActorIndex.Ant,
                Content = new Dictionary<string, object>()
                {
                    ["PositionX"] = this.PositionX,
                    ["PositionY"] = this.PositionY,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            SetPosition(new(
                data.GetOrDefault("PositionX", 0),
                data.GetOrDefault("PositionY", 0)
            ));
        }
    }
}
