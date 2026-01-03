using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Generators
{
    internal static class TreeGenerator
    {
        internal static void Start(World world, in Point origin, in int height, in int trunkWidth, in int leavesRadius)
        {
            // Generate trunk
            for (int y = 0; y < height; y++)
            {
                for (int x = -trunkWidth / 2; x <= trunkWidth / 2; x++)
                {
                    Point position = new(origin.X + x, origin.Y - y);

                    if (world.IsEmptySlotLayer(position, Layer.Foreground))
                    {
                        world.InstantiateElement(position, Layer.Foreground, ElementIndex.Wood);
                    }
                    else
                    {
                        world.ReplaceElement(position, Layer.Foreground, ElementIndex.Wood);
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

                        if (world.IsEmptySlotLayer(position, Layer.Foreground))
                        {
                            world.InstantiateElement(position, Layer.Foreground, ElementIndex.TreeLeaf);
                        }
                        else
                        {
                            world.ReplaceElement(position, Layer.Foreground, ElementIndex.TreeLeaf);
                        }
                    }
                }
            }
        }
    }
}
