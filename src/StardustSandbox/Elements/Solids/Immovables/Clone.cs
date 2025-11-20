using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Clone : ImmovableSolid
    {
        private static readonly List<Point> positionBuffer = [];
        private static readonly List<SlotLayer> layerBuffer = [];

        internal Clone(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
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

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
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
                if (this.Context.IsEmptySlotLayer(position, this.Context.Layer))
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

        private void TryDefineStoredElement(IEnumerable<Slot> neighbors)
        {
            if (this.Context.SlotLayer.StoredElement != null)
            {
                return;
            }

            foreach (Slot neighbor in neighbors)
            {
                SlotLayer layer = neighbor.GetLayer(this.Context.Layer);

                if (layer.HasState(ElementStates.IsEmpty) ||
                    layer.Element is Clone or Wall or Void)
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
