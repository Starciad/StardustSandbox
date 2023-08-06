using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    public abstract class PMovableSolid : PSolid
    {
        protected override void OnDefaultBehaviourStep(PElementContext ctx)
        {
            int direction = PRandom.Range(0, 101) < 50 ? 1 : -1;
            Vector2[] targets = new Vector2[]
            {
                new(ctx.Position.X                   , ctx.Position.Y + 1),
                new(ctx.Position.X + direction       , ctx.Position.Y + 1),
                new(ctx.Position.X + direction * -1, ctx.Position.Y + 1),
            };

            foreach (Vector2 targetPos in targets)
            {
                if (ctx.IsEmpty(targetPos))
                {
                    if (ctx.TrySetPosition(targetPos)) return;
                }

                if (ctx.TryGetElement(targetPos, out PElement value))
                {
                    // Liquid Settings
                    if (value is PLiquid)
                        if (ctx.TrySwitchPosition(ctx.Position, targetPos)) return;
                }
            }
        }
    }
}
