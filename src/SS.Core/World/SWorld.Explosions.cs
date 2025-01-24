using Microsoft.Xna.Framework;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        private readonly Queue<SExplosion> instantiatedExplosions = new(SExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT);
        private readonly SObjectPool explosionPool = new();

        public void InstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiateExplosion(explosionBuilder);
        }

        public bool TryInstantiateExplosion(SExplosionBuilder explosionBuilder)
        {
            if (!InsideTheWorldDimensions(explosionBuilder.Position) && this.instantiatedExplosions.Count >= SExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT)
            {
                return false;
            }

            SExplosion explosion = this.explosionPool.TryGet(out ISPoolableObject pooledObject)
                ? (SExplosion)pooledObject
                : new();

            explosion.Build(explosionBuilder);
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
            HashSet<Point> visitedPoints = [];
            int radiusCeil = (int)explosion.Radius;

            for (int dx = -radiusCeil; dx <= radiusCeil; dx++)
            {
                for (int dy = -radiusCeil; dy <= radiusCeil; dy++)
                {
                    Point target = new(explosion.Position.X + dx, explosion.Position.Y + dy);

                    if (!visitedPoints.Contains(target) && TryAffectPoint(target, explosion.Power))
                    {
                        _ = visitedPoints.Add(target);
                    }
                }
            }
        }

        private bool TryAffectPoint(Point position, float power)
        {
            if (!InsideTheWorldDimensions(position))
            {
                return false;
            }

            SWorldSlot worldSlot = GetWorldSlot(position);

            if (worldSlot.IsEmpty)
            {
                return false;
            }

            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Foreground), SWorldLayer.Foreground, worldSlot.Position, power);
            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Background), SWorldLayer.Background, worldSlot.Position, power);

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
