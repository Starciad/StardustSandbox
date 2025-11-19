using Microsoft.Xna.Framework;

namespace StardustSandbox.AnimationSystem
{
    internal readonly struct AnimationFrame(Rectangle textureClipArea, uint duration)
    {
        internal readonly Rectangle TextureClipArea => textureClipArea;
        internal readonly uint Duration => duration;
    }
}
