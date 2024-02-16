using Microsoft.Xna.Framework;

using PixelDust.Game.Constants.Elements;

namespace PixelDust.Game.Elements.Rendering.Common
{
    public sealed partial class PElementBlobRenderingMechanism : PElementRenderingMechanism
    {
        private static readonly Rectangle[] spriteKeyPoints = [
            // Full
            new Rectangle(location: new Point(00, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(00, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Corner
            new Rectangle(location: new Point(32, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(32, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Vertical Edge
            new Rectangle(location: new Point(64, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(64, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Horizontal Border
            new Rectangle(location: new Point(096, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(096, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Gaps
            new Rectangle(location: new Point(128, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(128, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
        ];
    }
}
