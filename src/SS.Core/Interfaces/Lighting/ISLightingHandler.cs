using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;

namespace StardustSandbox.Core.Interfaces.Lighting
{
    public interface ISLightingHandler
    {
        void InstantiateLightingSource(Point position, SWorldLayer worldLayer, byte intensity, Color color);
    }
}
