using PixelDust.Game.Constants.GUI;
using PixelDust.Game.GUI.Events;

namespace PixelDust.Game.GUI.Common.Menus.ElementExplorer
{
    public sealed partial class PGUI_ElementExplorer(PGUIEvents events, PGUILayoutPool layoutPool) : PGUISystem(events, layoutPool)
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = PGUIConstants.ELEMENT_EXPLORER;
        }
    }
}
