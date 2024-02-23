using PixelDust.Game.Constants;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUIRootElement : PGUIContainerElement
    {
        public PGUIRootElement()
        {
            this.ShouldUpdate = false;
            this.IsVisible = false;
        }
    }
}
