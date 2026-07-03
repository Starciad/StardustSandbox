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
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Events.Explosions;
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

        private readonly GameEvents gameEvents;
        private readonly TileMap tileMap;

        internal ExplosionHandler(GameEvents gameEvents, TileMap tileMap)
        {
            this.gameEvents = gameEvents;
            this.tileMap = tileMap;
        }

        internal bool TryInstantiate(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            if (!this.tileMap.IsWithinBounds(position) && this.instantiatedExplosions.Count >= ExplosionConstants.MAX_SIMULTANEOUS_EXPLOSIONS)
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

        internal void Instantiate(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiate(position, layer, explosionBuilder);
        }

        private void HandleExplosion(Explosion explosion)
        {
            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(explosion.Position, Convert.ToInt32(explosion.Radius)))
            {
                if (!this.tileMap.IsWithinBounds(point))
                {
                    continue;
                }

                if (this.tileMap.TryGetSlot(point, out Slot slot))
                {
                    TryAffectPoint(slot, point, explosion);
                }

                this.tileMap.Instantiate(point, explosion.Layer, explosion.ExplosionResidues.GetRandomItem());
            }

            this.gameEvents.Publish(new ExplosionEvent());
        }

        internal void HandleExplosions()
        {
            while (this.instantiatedExplosions.TryDequeue(out Explosion value))
            {
                HandleExplosion(value);
                this.explosionPool.Enqueue(value);
            }
        }

        private void TryAffectSlotLayer(Slot slot, Layer layer, Point targetPosition, Explosion explosion)
        {
            if (!slot.HasElement(layer))
            {
                return;
            }

            Element element = slot.GetElement(layer);

            if (element.IsExplosionImmune)
            {
                return;
            }

            if (element.BaseExplosionResistance >= explosion.Power)
            {
                slot.SetTemperature(layer, slot.GetTemperature(layer) + explosion.Heat);
            }
            else
            {
                this.tileMap.Destroy(targetPosition, layer);
            }
        }

        private void TryAffectPoint(Slot slot, Point targetPosition, Explosion explosion)
        {
            TryAffectSlotLayer(slot, Layer.Foreground, targetPosition, explosion);
            TryAffectSlotLayer(slot, Layer.Background, targetPosition, explosion);
        }
    }
}
