using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal abstract class Gas : Element
    {
        private readonly List<Point> emptyPositionsCache = [];
        private readonly List<Point> validPositionsCache = [];

        internal Gas(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.defaultDensity = 1;
        }

        protected override void OnBehaviourStep()
        {
            this.emptyPositionsCache.Clear();
            this.validPositionsCache.Clear();

            foreach (Point position in this.Context.Slot.Position.GetNeighboringCardinalPoints())
            {
                if (this.Context.IsEmptySlotLayer(position, this.Context.Layer))
                {
                    this.emptyPositionsCache.Add(position);
                }
                else if (this.Context.TryGetSlot(position, out Slot value))
                {
                    SlotLayer worldSlotLayer = value.GetLayer(this.Context.Layer);

                    if (worldSlotLayer.Element is Gas || worldSlotLayer.Element is Liquid)
                    {
                        if ((worldSlotLayer.Element.GetType() == GetType() && worldSlotLayer.Temperature > this.Context.SlotLayer.Temperature) || worldSlotLayer.Element.DefaultDensity < this.DefaultDensity)
                        {
                            this.validPositionsCache.Add(position);
                        }
                    }
                }
            }

            if (this.emptyPositionsCache.Count > 0)
            {
                this.Context.SetPosition(this.emptyPositionsCache.GetRandomItem());
            }
            else if (this.validPositionsCache.Count > 0)
            {
                Point targetPosition = this.validPositionsCache.GetRandomItem();
                _ = this.Context.TrySwappingElements(targetPosition);
            }
        }
    }
}
