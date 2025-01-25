using Microsoft.Xna.Framework;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics;
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
                if (!InsideTheWorldDimensions(point) || !TryGetWorldSlot(point, out SWorldSlot worldSlot))
                {
                    continue;
                }

                TryAffectPoint(point, explosion);

                foreach (SExplosionResidue residue in explosion.ExplosionResidues)
                {
                    SWorldLayer targetLayer = SRandomMath.Chance(50, 100) ? SWorldLayer.Foreground : SWorldLayer.Background;

                    if (SRandomMath.Chance(residue.CreationChance, 100))
                    {
                        InstantiateElement(point, targetLayer, residue.Identifier);
                    }
                }

                // if (explosion.CreatesLight)
                // {
                //     AddLightSource(<position>, <lightIntensity>, <color>);
                // }
            }
        }

        private bool TryAffectPoint(Point targetPosition, SExplosion explosion)
        {
            if (!InsideTheWorldDimensions(explosion.Position) || IsEmptyWorldSlot(explosion.Position) || !TryGetWorldSlot(explosion.Position, out SWorldSlot worldSlot))
            {
                return false;
            }

            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Foreground), SWorldLayer.Foreground, targetPosition, explosion);
            TryAffectSlotLayer(worldSlot.GetLayer(SWorldLayer.Background), SWorldLayer.Background, targetPosition, explosion);

            return true;
        }

        private void TryAffectSlotLayer(SWorldSlotLayer worldSlotLayer, SWorldLayer worldLayer, Point targetPosition, SExplosion explosion)
        {
            if (worldSlotLayer.IsEmpty)
            {
                return;
            }

            ISElement element = worldSlotLayer.Element;

            if (element.IsExplosionImmune)
            {
                return;
            }

            if (element.DefaultExplosionResistance >= explosion.Power)
            {
                worldSlotLayer.SetTemperatureValue((short)(worldSlotLayer.Temperature + explosion.Heat));
                worldSlotLayer.SetColorModifier(worldSlotLayer.ColorModifier.Darken(2f));
            }
            else
            {
                DestroyElement(targetPosition, worldLayer);
            }
        }
    }
}
