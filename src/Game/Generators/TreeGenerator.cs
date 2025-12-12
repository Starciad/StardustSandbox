using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Generators
{
    internal static class TreeGenerator
    {
        internal static void Start(in ElementContext context, Point origin, int height, int trunkWidth, int leavesRadius)
        {
            // Generate trunk
            for (int y = 0; y < height; y++)
            {
                for (int x = -trunkWidth / 2; x <= trunkWidth / 2; x++)
                {
                    Point position = new(origin.X + x, origin.Y - y);

                    if (context.IsEmptySlotLayer(position))
                    {
                        context.InstantiateElement(position, ElementIndex.Wood);
                    }
                    else
                    {
                        context.ReplaceElement(position, ElementIndex.Wood);
                    }
                }
            }

            // Generate leaves
            int leavesStartY = origin.Y - height;

            for (int y = -leavesRadius; y <= leavesRadius; y++)
            {
                for (int x = -leavesRadius; x <= leavesRadius; x++)
                {
                    if ((x * x) + (y * y) <= leavesRadius * leavesRadius)
                    {
                        Point position = new(origin.X + x, leavesStartY + y);

                        if (context.IsEmptySlotLayer(position))
                        {
                            context.InstantiateElement(position, ElementIndex.TreeLeaf);
                        }
                        else
                        {
                            context.ReplaceElement(position, ElementIndex.TreeLeaf);
                        }
                    }
                }
            }
        }
    }
}
