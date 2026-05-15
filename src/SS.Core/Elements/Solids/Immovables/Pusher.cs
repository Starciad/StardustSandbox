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

using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class Pusher : ImmovableSolid
    {
        private readonly PusherDirection direction;

        internal Pusher(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, PusherDirection direction, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager, StatisticsManager statisticsManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager, statisticsManager)
        {
            this.direction = direction;
            this.BaseDensity = 2.0f;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            switch (this.direction)
            {
                case PusherDirection.Up:
                    PusherUtility.PushingNeighborsUp(context, neighbors, this.StatisticsManager);
                    break;

                case PusherDirection.Right:
                    PusherUtility.PushingNeighborsRight(context, neighbors, this.StatisticsManager);
                    break;

                case PusherDirection.Down:
                    PusherUtility.PushingNeighborsDown(context, neighbors, this.StatisticsManager);
                    break;

                case PusherDirection.Left:
                    PusherUtility.PushingNeighborsLeft(context, neighbors, this.StatisticsManager);
                    break;

                default:
                    break;
            }
        }
    }
}
