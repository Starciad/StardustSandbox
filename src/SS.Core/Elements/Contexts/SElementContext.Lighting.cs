using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal sealed partial class SElementContext
    {
        public void InstantiateLightingSource(byte intensity, Color color)
        {
            InstantiateLightingSource(this.Position, this.worldLayer, intensity, color);
        }

        public void InstantiateLightingSource(SWorldLayer worldLayer, byte intensity, Color color)
        {
            InstantiateLightingSource(this.Position, worldLayer, intensity, color);
        }

        public void InstantiateLightingSource(Point position, SWorldLayer worldLayer, byte intensity, Color color)
        {
            this.world.InstantiateLightingSource(position, worldLayer, intensity, color);
        }
    }
}
