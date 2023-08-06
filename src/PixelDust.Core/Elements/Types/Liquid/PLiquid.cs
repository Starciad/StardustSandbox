using Microsoft.Xna.Framework;

using PixelDust.Core.Utilities;

namespace PixelDust.Core.Elements
{
    public abstract class PLiquid : PElement
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
                    if (ctx.TrySetPosition(targetPos)) return;
            }

            for (int i = 0; i < DefaultDispersionRate; i++)
            {
                if (!ctx.IsEmpty(new(ctx.Position.X + direction, ctx.Position.Y)) &&
                    !ctx.IsEmpty(new(ctx.Position.X + direction, ctx.Position.Y + 1)))
                    break;

                ctx.TrySetPosition(new(ctx.Position.X + direction, ctx.Position.Y));
            }
        }
    }
}