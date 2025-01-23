using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Templates.Gases
{
    public abstract class SGas : SElement
    {
        private readonly List<Point> emptyPositionsCache = [];
        private readonly List<Point> validPositionsCache = [];

        public SGas(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.defaultDensity = 1;
        }

        protected override void OnBehaviourStep()
        {
            this.emptyPositionsCache.Clear();
            this.validPositionsCache.Clear();

            foreach (Point position in SPointExtensions.GetNeighboringCardinalPoints(this.Context.Slot.Position))
            {
                if (this.Context.IsEmptyWorldSlotLayer(position, this.Context.Layer))
                {
                    this.emptyPositionsCache.Add(position);
                }
                else if (this.Context.TryGetElement(position, this.Context.Layer, out ISElement value))
                {
                    if (value is SGas || value is SLiquid)
                    {
                        if (value.DefaultDensity < this.DefaultDensity)
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
