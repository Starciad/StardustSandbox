using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;

namespace StardustSandbox.Game.Elements.Rendering.Common
{
    public sealed partial class SElementBlobRenderingMechanism : SElementRenderingMechanism
    {
        private static readonly Rectangle[] spriteKeyPoints = [
            // Full 0, 1, 2, 3
            new Rectangle(location: new Point(00, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(00, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Corner 4, 5, 6, 7
            new Rectangle(location: new Point(32, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(32, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Vertical Edge 8, 9, 10, 11
            new Rectangle(location: new Point(64, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(64, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Horizontal Border 12, 13, 14, 15
            new Rectangle(location: new Point(096, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(096, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Gaps 16, 17, 18, 19
            new Rectangle(location: new Point(128, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 00), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(128, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 16), size: new Point(SElementRenderingConstants.SPRITE_SLICE_SIZE)),
        ];
    }
}
