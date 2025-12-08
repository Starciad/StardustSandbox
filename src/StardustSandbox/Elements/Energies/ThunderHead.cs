using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;
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

        private static void CreateBranchedThunder(in ElementContext context, Point origin, int length, int depth, int maxDepth)
        {
            if (length <= 0 || depth > maxDepth)
            {
                return;
            }

            int[] angles = [SSRandom.Range(-30, -10), SSRandom.Range(10, 30)];

            for (int i = 0; i < angles.Length; i++)
            {
                int angle = angles[i];

                float rad = MathF.PI * angle / 180.0f;
                int dx = (int)MathF.Round((MathF.Sin(rad) * length) + SSRandom.Range(-3, 3));
                int dy = (int)MathF.Round((MathF.Cos(rad) * length) + SSRandom.Range(2, 4));

                Point endPoint = new(
                    origin.X + dx,
                    origin.Y + dy
                );

                CreateBodyLine(context, origin, endPoint);

                CreateBranchedThunder(
                    context,
                    endPoint,
                    (int)(length * SSRandom.Range(0.5f, 0.7f)),
                    depth + 1,
                    maxDepth
                );
            }
        }

        protected override void OnInstantiated(in ElementContext context)
        {
            Point origin = context.Position;

            int initialLength = SSRandom.Range(3, 6);
            int maxDepth = SSRandom.Range(4, 6);

            CreateBranchedThunder(context, origin, initialLength, 1, maxDepth);
        }

        protected override void OnAfterStep(in ElementContext context)
        {
            context.RemoveElement();
        }
    }
}
