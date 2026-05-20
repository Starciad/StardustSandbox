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

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem.Components;
using StardustSandbox.Core.WorldSystem.Slots;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.WorldSystem.Handlers
{
    internal sealed class ExplosionHandler
    {
        private readonly ObjectPool explosionPool = new();
        private readonly Queue<Explosion> instantiatedExplosions = new(ExplosionConstants.MAX_SIMULTANEOUS_EXPLOSIONS);

        private readonly TileMap tileMap;

        internal ExplosionHandler(TileMap tileMap)
        {

        }

        internal bool TryInstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            if (!IsWithinBounds(position) && this.instantiatedExplosions.Count >= ExplosionConstants.MAX_SIMULTANEOUS_EXPLOSIONS)
            {
                return false;
            }

            Explosion explosion = this.explosionPool.TryDequeue(out IPoolableObject pooledObject)
                ? (Explosion)pooledObject
                : new();

            explosion.Build(position, layer, explosionBuilder);
            this.instantiatedExplosions.Enqueue(explosion);

            return true;
        }

        internal void InstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiateExplosion(position, layer, explosionBuilder);
        }

        private void HandleExplosion(Explosion explosion)
        {
            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(explosion.Position, Convert.ToInt32(explosion.Radius)))
            {
                if (!IsWithinBounds(point))
                {
                    continue;
                }

                if (TryGetSlot(point, out Slot slot))
                {
                    TryAffectPoint(slot, point, explosion);
                }

                InstantiateElementIndex(point, explosion.Layer, explosion.ExplosionResidues.GetRandomItem());
            }

            this.achievementManager.Unlock(AchievementIndex.ACH_016);
        }

        internal void HandleExplosions()
        {
            while (this.instantiatedExplosions.TryDequeue(out Explosion value))
            {
                HandleExplosion(value);
                this.explosionPool.Enqueue(value);
            }
        }

        private void TryAffectSlotLayer(SlotLayer slotLayer, Layer layer, Point targetPosition, Explosion explosion)
        {
            if (slotLayer.IsEmpty || slotLayer.Element.IsExplosionImmune)
            {
                return;
            }

            if (slotLayer.Element.BaseExplosionResistance >= explosion.Power)
            {
                slotLayer.Temperature += explosion.Heat;
            }
            else
            {
                DestroyElement(targetPosition, layer);
            }
        }

        private void TryAffectPoint(Slot slot, Point targetPosition, Explosion explosion)
        {
            TryAffectSlotLayer(slot.GetLayer(Layer.Foreground), Layer.Foreground, targetPosition, explosion);
            TryAffectSlotLayer(slot.GetLayer(Layer.Background), Layer.Background, targetPosition, explosion);
        }
    }
}
