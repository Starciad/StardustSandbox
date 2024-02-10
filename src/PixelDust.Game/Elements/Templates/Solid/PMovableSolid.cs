using PixelDust.Game.Elements.Templates.Liquid;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Utilities;

namespace PixelDust.Game.Elements.Templates.Solid
{
    /// <summary>
    /// Base class for defining movable solid elements in PixelDust.
    /// </summary>
    public abstract class PMovableSolid : PSolid
    {
        public override void OnBehaviourStep()
        {
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;

            Vector2Int down = new(this.Context.Position.X, this.Context.Position.Y + 1);
            Vector2Int[] sides =
            [
                new(this.Context.Position.X + direction, this.Context.Position.Y),
                new(this.Context.Position.X + direction * -1, this.Context.Position.Y),
            ];

            if (TrySetPosition(down))
            { return; }

            foreach (Vector2Int side in sides)
            {
                if (TrySetPosition(new(side.X, side.Y + 1)))
                { return; }
            }
        }

        private bool TrySetPosition(Vector2Int pos)
        {
            if (this.Context.TrySetPosition(pos))
            {
                return true;
            }

            if (this.Context.TryGetElement(pos, out PElement value))
            {
                if (value is PLiquid)
                {
                    if (this.Context.TrySwappingElements(pos))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
