﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics.Geometry;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        private readonly Queue<SExplosion> instantiatedExplosions = new(SExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT);
        private readonly SObjectPool explosionPool = new();

        public void InstantiateExplosion(Point position, SExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiateExplosion(position, explosionBuilder);
        }

        public bool TryInstantiateExplosion(Point position, SExplosionBuilder explosionBuilder)
        {
            if (!InsideTheWorldDimensions(position) && this.instantiatedExplosions.Count >= SExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT)
            {
                return false;
            }

            SExplosion explosion = this.explosionPool.TryGet(out ISPoolableObject pooledObject)
                ? (SExplosion)pooledObject
                : new();

            explosion.Build(position, explosionBuilder);
            this.instantiatedExplosions.Enqueue(explosion);

            return true;
        }

        private void HandleExplosions()
        {
            while (this.instantiatedExplosions.TryDequeue(out SExplosion value))
            {
                HandleExplosion(value);
                this.explosionPool.Add(value);
            }
        }

        private void HandleExplosion(SExplosion explosion)
        {
            foreach (Point point in SShapePointGenerator.GenerateCirclePoints(explosion.Position, explosion.Radius))
            {
                _ = TryAffectPoint(point, explosion.Power);
            }
        }

        private bool TryAffectPoint(Point position, float power)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptyWorldSlot(position) || !TryGetWorldSlot(position, out SWorldSlot worldSlot))
            {
                return false;
            }

            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Foreground), SWorldLayer.Foreground, position, power);
            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Background), SWorldLayer.Background, position, power);

            return true;
        }

        private void TryAffectSlotLayer(SWorldSlotLayer worldSlotLayer, SWorldLayer worldLayer, Point position, float power)
        {
            if (worldSlotLayer.IsEmpty)
            {
                // Create Explosion Spark
                return;
            }

            ISElement element = worldSlotLayer.Element;

            if (element.IsExplosionImmune)
            {
                return;
            }

            if (element.DefaultExplosionResistance >= power)
            {
                SetElementTemperature(position, worldLayer, (short)(element.DefaultTemperature + 300));
                return;
            }

            DestroyElement(position, worldLayer);
        }
    }
}
