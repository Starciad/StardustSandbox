using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Generators
{
    internal static class LightningGenerator
    {
        internal static void Start(in ElementContext context, Point origin)
        {
            CreateBranchedThunder(context, origin);
        }

        private static void CreateBranchedThunder(in ElementContext context, Point origin)
        {
            int length = SSRandom.Range(3, 6);

            CreateBranchedThunderBranch(context, origin, length, SSRandom.Range(-30, -10));
            CreateBranchedThunderBranch(context, origin, length, SSRandom.Range(10, 30));
        }

        private static void CreateBranchedThunderBranch(in ElementContext context, Point origin, int length, int angle)
        {
            float rad = MathF.PI * angle / 180.0f;
            int dx = (int)MathF.Round((MathF.Sin(rad) * length) + SSRandom.Range(-3, 3));
            int dy = (int)MathF.Round((MathF.Cos(rad) * length) + SSRandom.Range(2, 4));

            Point endPoint = new(
                origin.X + dx,
                origin.Y + dy
            );

            if (!TryCreateBodyLine(context, origin, endPoint))
            {
                return;
            }

            CreateBranchedThunder(context, endPoint);
        }

        private static bool TryCreateBodyLine(in ElementContext context, Point start, Point end)
        {
            if (start == end)
            {
                context.InstantiateElement(end, ElementIndex.LightningBody);
                return true;
            }

            int matrixX1 = start.X;
            int matrixY1 = start.Y;
            int matrixX2 = end.X;
            int matrixY2 = end.Y;

            int xDiff = matrixX1 - matrixX2;
            int yDiff = matrixY1 - matrixY2;

            bool xDiffIsLarger = MathF.Abs(xDiff) > MathF.Abs(yDiff);

            int xModifier = xDiff < 0 ? 1 : -1;
            int yModifier = yDiff < 0 ? 1 : -1;

            int longerSideLength = (int)MathF.Max(MathF.Abs(xDiff), MathF.Abs(yDiff));
            int shorterSideLength = (int)MathF.Min(MathF.Abs(xDiff), MathF.Abs(yDiff));

            float slope = (shorterSideLength == 0 || longerSideLength == 0) ? 0 : ((float)shorterSideLength / longerSideLength);

            int shorterSideIncrease;

            for (int i = 1; i <= longerSideLength; i++)
            {
                shorterSideIncrease = (int)MathF.Round(i * slope);
                int yIncrease, xIncrease;

                if (xDiffIsLarger)
                {
                    xIncrease = i;
                    yIncrease = shorterSideIncrease;
                }
                else
                {
                    yIncrease = i;
                    xIncrease = shorterSideIncrease;
                }

                int currentY = matrixY1 + (yIncrease * yModifier);
                int currentX = matrixX1 + (xIncrease * xModifier);

                Point position = new(currentX, currentY);

                if (context.TryGetSlot(position, out Slot slot) && !slot.GetLayer(context.Layer).HasState(ElementStates.IsEmpty))
                {
                    if (slot.GetLayer(context.Layer).Element.Category == ElementCategory.Gas)
                    {
                        continue;
                    }

                    switch (slot.GetLayer(context.Layer).Element.Index)
                    {
                        case ElementIndex.Water:
                        case ElementIndex.Ice:
                        case ElementIndex.Snow:
                            if (slot.HasState(context.Layer, ElementStates.IsFalling))
                            {
                                continue;
                            }

                            break;

                        default:
                            break;
                    }
                }

                // Attempt to instantiate element
                if (!context.TryInstantiateElement(position, ElementIndex.LightningBody))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
