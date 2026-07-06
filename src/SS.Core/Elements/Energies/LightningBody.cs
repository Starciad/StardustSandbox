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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class LightningBody : Energy
    {
        private readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 2.0f,
            Power = 5.0f,
            Heat = TemperatureConstants.MAX_CELSIUS_VALUE,

            AffectsWater = true,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke
            ]
        };

        internal LightningBody(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = TemperatureConstants.MAX_CELSIUS_VALUE;
            this.BaseDensity = 0.0f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsExplosionImmune = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);

                if (slot.GetElement(context.Layer).Category is ElementCategory.Gas)
                {
                    continue;
                }

                switch (slot.GetElementIndex(context.Layer))
                {
                    case ElementIndex.LightningBody:
                    case ElementIndex.LightningHead:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.Wall:
                    case ElementIndex.Fire:
                        continue;

                    case ElementIndex.Water:
                    case ElementIndex.Snow:
                    case ElementIndex.Ice:
                        if (slot.GetFallingState(context.Layer))
                        {
                            continue;
                        }

                        break;

                    default:
                        break;
                }

                context.Instantiate(this.explosionBuilder);
            }
        }

        protected override void OnStep(ElementContext context)
        {
            context.Remove();
        }
    }
}
