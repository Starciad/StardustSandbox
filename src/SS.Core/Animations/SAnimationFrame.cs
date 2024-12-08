using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.Animations
{
    public struct SAnimationFrame(Rectangle textureClipArea, uint duration)
    {
        public Rectangle TextureClipArea { readonly get; private set; } = textureClipArea;
        public uint Duration { readonly get; private set; } = duration;
    }
}
