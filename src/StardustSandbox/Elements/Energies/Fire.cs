using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class Fire : Energy
    {
        internal Fire(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Single;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsExplosionImmune | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 500;
            this.defaultDensity = 0;
        }

        protected override void OnBeforeStep()
        {
            if (SSRandom.Chance(ElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR))
            {
                this.Context.DestroyElement();

                if (SSRandom.Chance(ElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE))
                {
                    this.Context.InstantiateElement(ElementIndex.Smoke);
                }
            }
        }

        protected override void OnStep()
        {
            Point targetPosition = new(this.Context.Slot.Position.X + SSRandom.Range(-1, 1), this.Context.Slot.Position.Y - 1);

            if (this.Context.IsEmptySlot(targetPosition))
            {
                if (this.Context.TrySetPosition(targetPosition, this.Context.Layer))
                {
                    return;
                }
            }
            else
            {
                Element targetElement = this.Context.GetElement(targetPosition, this.Context.Layer);

                if (targetElement != null && (targetElement.Category == ElementCategory.MovableSolid || targetElement.Category == ElementCategory.Liquid || targetElement.Category == ElementCategory.Gas))
                {
                    this.Context.SwappingElements(targetPosition);
                }
            }
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                if (!neighbor.ForegroundLayer.HasState(ElementStates.IsEmpty))
                {
                    IgniteElement(neighbor, neighbor.GetLayer(LayerType.Foreground), LayerType.Foreground);
                }

                if (!neighbor.BackgroundLayer.HasState(ElementStates.IsEmpty))
                {
                    IgniteElement(neighbor, neighbor.GetLayer(LayerType.Background), LayerType.Background);
                }
            }
        }

        private void IgniteElement(Slot slot, SlotLayer worldSlotLayer, LayerType layer)
        {
            // Increase neighboring temperature by fire's heat value
            this.Context.SetElementTemperature((short)(worldSlotLayer.Temperature + ElementConstants.FIRE_HEAT_VALUE));

            // Check if the element is flammable
            if (worldSlotLayer.Element.HasCharacteristic(ElementCharacteristics.IsFlammable))
            {
                // Adjust combustion chance based on the element's flammability resistance
                int combustionChance = ElementConstants.CHANCE_OF_COMBUSTION;
                bool isAbove = slot.Position.Y < this.Context.Slot.Position.Y;

                // Increase chance of combustion if the element is directly above
                if (isAbove)
                {
                    combustionChance += 10;
                }

                // Attempt combustion based on flammabilityResistance
                if (SSRandom.Chance(combustionChance, 100 + worldSlotLayer.Element.DefaultFlammabilityResistance))
                {
                    this.Context.ReplaceElement(slot.Position, layer, ElementIndex.Fire);
                }
            }
        }
    }
}
