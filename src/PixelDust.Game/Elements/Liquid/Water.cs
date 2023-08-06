using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Water : PLiquid
    {
        private static int InfiltrationLevel => 2;

        protected override void OnSettings()
        {
            Name = "Water";
            Description = string.Empty;
            Color = new(35, 137, 218);

            DefaultDispersionRate = 4;
        }

        protected override void OnStep(PElementContext ctx)
        {
            Vector2[] downTargets = new Vector2[]
            {
                new(ctx.Position.X    , ctx.Position.Y + 1),
                new(ctx.Position.X - 1, ctx.Position.Y + 1),
                new(ctx.Position.X + 1, ctx.Position.Y + 1),
            };

            // Soil infiltration
            for (int i = 0; i < InfiltrationLevel; i++)
            {
                foreach (Vector2 targetPos in downTargets)
                {
                    if (ctx.TryGetElement(targetPos, out PElement value))
                    {
                        if (value is Dirt)
                        {
                            ctx.TryReplace<Mud>(targetPos);
                            return;
                        }

                        if (value is Stone)
                        {
                            ctx.TryReplace<Sand>(targetPos);
                            return;
                        }
                    }
                }
            }
        }
    }
}