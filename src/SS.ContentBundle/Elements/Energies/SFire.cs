using Microsoft.Xna.Framework;

using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Energies;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Energies
{
    internal sealed class SFire : SEnergy
    {
        internal SFire(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Amber;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_24");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(new SAnimation(gameInstance, [
                new(new(new(00, 00), new(SSpritesConstants.SPRITE_SCALE)), 200),
                new(new(new(32, 00), new(SSpritesConstants.SPRITE_SCALE)), 200),
                new(new(new(64, 00), new(SSpritesConstants.SPRITE_SCALE)), 200),
                new(new(new(96, 00), new(SSpritesConstants.SPRITE_SCALE)), 200),
            ])));
            this.enableNeighborsAction = true;
            this.defaultTemperature = 500;
        }

        protected override void OnBeforeStep()
        {
            if (SRandomMath.Chance(SElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR, SElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR_TOTAL))
            {
                this.Context.DestroyElement(this.Context.Layer);

                if (SRandomMath.Chance(SElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE, SElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE_TOTAL))
                {
                    this.Context.InstantiateElement(this.Context.Layer, SElementConstants.IDENTIFIER_SMOKE);
                }
            }
        }

        protected override void OnStep()
        {
            Point targetPosition = new(this.Context.Slot.Position.X + SRandomMath.Range(-1, 2), this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptyWorldSlot(targetPosition))
            {
                if (this.Context.TrySetPosition(targetPosition, this.Context.Layer))
                {
                    return;
                }
            }
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                if (!neighbor.ForegroundLayer.IsEmpty)
                {
                    IgniteElement(neighbor, neighbor.GetLayer(SWorldLayer.Foreground), SWorldLayer.Foreground);
                }

                if (!neighbor.BackgroundLayer.IsEmpty)
                {
                    IgniteElement(neighbor, neighbor.GetLayer(SWorldLayer.Background), SWorldLayer.Background);
                }
            }
        }

        private void IgniteElement(SWorldSlot slot, SWorldSlotLayer worldSlotLayer, SWorldLayer worldLayer)
        {
            // Increase neighboring temperature by fire's heat value
            this.Context.SetElementTemperature((short)(worldSlotLayer.Temperature + SElementConstants.FIRE_HEAT_VALUE));

            // Check if the element is flammable
            if (worldSlotLayer.Element.EnableFlammability)
            {
                // Adjust combustion chance based on the element's flammability resistance
                int combustionChance = SElementConstants.CHANCE_OF_COMBUSTION;
                bool isAbove = slot.Position.Y < this.Context.Slot.Position.Y;

                // Increase chance of combustion if the element is directly above
                if (isAbove)
                {
                    combustionChance += 10;
                }

                // Attempt combustion based on flammabilityResistance
                if (SRandomMath.Chance(combustionChance, 100 + worldSlotLayer.Element.DefaultFlammabilityResistance))
                {
                    this.Context.ReplaceElement(slot.Position, worldLayer, SElementConstants.IDENTIFIER_FIRE);
                }
            }
        }
    }
}
