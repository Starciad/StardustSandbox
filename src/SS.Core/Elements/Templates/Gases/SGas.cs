using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Templates.Gases
{
    public abstract class SGas : SElement
    {
        public SGasMovementType MovementType => this.movementType;

        protected SGasMovementType movementType;

        private readonly List<Point> emptyPositionsCache = [];
        private readonly List<Point> validPositionsCache = [];

        public SGas(ISGame gameInstance) : base(gameInstance)
        {
            this.defaultDensity = 1;
        }

        protected override void OnBehaviourStep()
        {
            switch (this.movementType)
            {
                case SGasMovementType.Up:
                    UpMovementTypeUpdate();
                    break;

                case SGasMovementType.Spread:
                    SpreadMovementTypeUpdate();
                    break;

                default:
                    break;
            }
        }

        private void UpMovementTypeUpdate()
        {
            Point[] abovePositions = SElementUtility.GetRandomSidePositions(this.Context.Slot.Position, SDirection.Up);

            for (int i = 0; i < abovePositions.Length; i++)
            {
                Point position = abovePositions[i];

                if (TrySetPosition(position))
                {
                    return;
                }
            }

            SElementUtility.UpdateHorizontalPosition(this.Context, this.DefaultDispersionRate);
        }

        private void SpreadMovementTypeUpdate()
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

        private bool TrySetPosition(Point position)
        {
            if (this.Context.TrySetPosition(position))
            {
                return true;
            }

            if (this.Context.TryGetElement(position, this.Context.Layer, out ISElement value))
            {
                if (value is SGas || value is SLiquid)
                {
                    if (value.DefaultDensity < this.DefaultDensity && this.Context.TrySwappingElements(position))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
