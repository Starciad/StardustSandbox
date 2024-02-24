using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;

namespace PixelDust.Game.GUI.Common.Menus.ElementExplorer
{
    public sealed partial class PGUI_ElementExplorer
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _root;

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._root = layout.RootElement;

            // Build Header
            // Build Body
        }
    }
}
