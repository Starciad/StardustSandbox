using StardustSandbox.Core.Enums.World;

namespace StardustSandbox.Core.Extensions
{
    internal static class LayerExtension
    {
        internal static Layer GetOppositeLayer(this Layer layer)
        {
            return layer is Layer.Foreground ? Layer.Background : Layer.Foreground;
        }
    }
}
