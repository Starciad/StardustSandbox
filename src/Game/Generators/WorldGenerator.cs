using StardustSandbox.WorldSystem;

namespace StardustSandbox.Generators
{
    internal static class WorldGenerator
    {
        internal static void Start(World world)
        {
            int width = world.Information.Size.X;
            int height = world.Information.Size.Y;

            world.StartNew();
        }
    }
}
