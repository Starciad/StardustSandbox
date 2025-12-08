using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

using System;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class ThunderHead : Energy
    {
        private static void CreateBodyLine(in ElementContext context, Point start, Point end)
        {
            if (start == end)
            {
                context.InstantiateElement(end, ElementIndex.ThunderBody);
                return;
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

                context.InstantiateElement(new(currentX, currentY), ElementIndex.ThunderBody);
            }
        }

        protected override void OnInstantiated(in ElementContext context)
        {
            Point origin = context.Position;

            Point endPoint = new(
                origin.X + SSRandom.Range(-3, 3),
                origin.Y + SSRandom.Range(6, 12)
            );

            CreateBodyLine(context, origin, endPoint);
        }

        protected override void OnAfterStep(in ElementContext context)
        {
            context.RemoveElement();
        }
    }
}
