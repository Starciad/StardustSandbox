using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SClone : SImmovableSolid
    {
        private static readonly List<Point> positionBuffer = [];
        private static readonly List<SWorldSlotLayer> layerBuffer = [];

        internal SClone(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Amber;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_27");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.enableNeighborsAction = true;
            this.isExplosionImmune = true;
            this.defaultDensity = 3000;
        }

        protected override void OnBeforeStep()
        {
            positionBuffer.Clear();
            layerBuffer.Clear();
        }

        protected override void OnAfterStep()
        {
            TryInstantiateStoredElement();
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            TryDefineStoredElement(neighbors);
        }

        private void TryInstantiateStoredElement()
        {
            if (this.Context.SlotLayer.StoredElement == null || !TryGetValidPosition(out Point validPositon))
            {
                return;
            }

            this.Context.InstantiateElement(validPositon, this.Context.Layer, this.Context.SlotLayer.StoredElement);
        }

        private bool TryGetValidPosition(out Point validPosition)
        {
            foreach (Point position in this.Context.Slot.Position.GetNeighboringCardinalPoints())
            {
                if (this.Context.IsEmptyWorldSlotLayer(position, this.Context.Layer))
                {
                    positionBuffer.Add(position);
                }
            }

            if (positionBuffer.Count == 0)
            {
                validPosition = Point.Zero;
                return false;
            }

            validPosition = positionBuffer.GetRandomItem();
            return true;
        }

        private void TryDefineStoredElement(IEnumerable<SWorldSlot> neighbors)
        {
            if (this.Context.SlotLayer.StoredElement != null)
            {
                return;
            }

            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer layer = neighbor.GetLayer(this.Context.Layer);

                if (layer.IsEmpty ||
                    layer.Element is SClone or SWall or SVoid)
                {
                    continue;
                }

                layerBuffer.Add(layer);
            }

            if (layerBuffer.Count == 0)
            {
                return;
            }

            this.Context.SetStoredElement(layerBuffer.GetRandomItem().Element);
        }
    }
}
