using PixelDust.Core;
using Microsoft.Xna.Framework;

namespace PixelDust
{
    [PElementRegister]
    internal class Acid : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Acid";
            Description = string.Empty;
            Color = new(0, 255, 0);
        }

        protected override void OnBeforeStep(PElementContext ctx)
        {
            Vector2[] downTargets = new Vector2[]
            {
                new(ctx.Position.X    , ctx.Position.Y - 1),
                new(ctx.Position.X - 1, ctx.Position.Y - 1),
                new(ctx.Position.X + 1, ctx.Position.Y - 1),

                new(ctx.Position.X - 1, ctx.Position.Y),
                new(ctx.Position.X + 1, ctx.Position.Y),

                new(ctx.Position.X    , ctx.Position.Y + 1),
                new(ctx.Position.X - 1, ctx.Position.Y + 1),
                new(ctx.Position.X + 1, ctx.Position.Y + 1),
            };

            foreach (Vector2 targetPos in downTargets)
            {
                if (ctx.TryGetElement(targetPos, out PElement value))
                {
                    if (value is Acid ||
                        value is Wall)
                        continue;

                    ctx.TryDestroy(ctx.Position);
                    ctx.TryDestroy(targetPos);
                }
            }
        }
    }
}