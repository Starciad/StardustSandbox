using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.General;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Templates.Gases
{
    public abstract class SGas(ISGame gameInstance) : SElement(gameInstance)
    {
        public SGasMovementType MovementType => this.movementType;

        protected SGasMovementType movementType;

        private readonly List<Point> emptyPositionsCache = [];

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

                if (this.Context.TrySetPosition(this.Context.Layer, position))
                {
                    return;
                }

                SElementUtility.UpdateHorizontalPosition(this.Context, this.DefaultDispersionRate);
            }
        }

        private void SpreadMovementTypeUpdate()
        {
            this.emptyPositionsCache.Clear();

            foreach (Point point in SPointExtensions.GetNeighboringCardinalPoints(this.Context.Slot.Position))
            {
                if (this.Context.IsEmptyElementSlot(point))
                {
                    this.emptyPositionsCache.Add(point);
                }
            }

            if (this.emptyPositionsCache.Count == 0)
            {
                return;
            }
            else if (this.emptyPositionsCache.Count == 1)
            {
                this.Context.SetPosition(this.Context.Layer, this.emptyPositionsCache[0]);
            }
            else
            {
                this.Context.SetPosition(this.Context.Layer, this.emptyPositionsCache.GetRandomItem());
            }
        }
    }
}
