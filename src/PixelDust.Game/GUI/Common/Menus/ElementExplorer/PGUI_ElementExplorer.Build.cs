using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.GUI.Common.Menus.ElementExplorer
{
    public sealed partial class PGUI_ElementExplorer
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _rootElement;

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._rootElement = layout.RootElement;

            BuildGUIBackground();
            BuildExplorer();
        }

        private void BuildGUIBackground()
        {
            PGUIImageElement guiBackground = this._layout.CreateElement<PGUIImageElement>();
            guiBackground.SetTexture(this.particleTexture);
            guiBackground.SetScale(this._rootElement.Size.ToVector2());
            guiBackground.SetSize(this._rootElement.Size);
            guiBackground.SetColor(new Color(Color.Black, 160));
        }

        private void BuildExplorer()
        {
            PGUISliceImageElement explorerBackground = this._layout.CreateElement<PGUISliceImageElement>();
            explorerBackground.SetTexture(this.guiBackgroundTexture);
            explorerBackground.SetSize(new Size2(32, 15));
            explorerBackground.SetMargin(new Vector2(128, 128));
            explorerBackground.SetColor(new Color(104, 111, 121, 255));
            explorerBackground.PositionRelativeToElement(this._rootElement);

            // ================================ //

            PGUILabelElement explorerTitleLabel = this._layout.CreateElement<PGUILabelElement>();
            explorerTitleLabel.SetTextContent("TITLE");
            explorerTitleLabel.SetFontFamily("font_1");
            explorerTitleLabel.PositionRelativeToElement(explorerBackground);
        }
    }
}
