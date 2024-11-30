using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private ISGUILayoutBuilder layout;
        private SGUIRootElement rootElement;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;
            this.rootElement = layout.RootElement;
            
            SGUIImageElement gameTitle = new(this.SGameInstance);
            gameTitle.SetTexture(this.gameTitleTexture);
            gameTitle.SetOriginPivot(SCardinalDirection.Northwest);
            gameTitle.SetScale(new Vector2(2));
            gameTitle.SetPositionAnchor(SCardinalDirection.West);
            gameTitle.SetSize(new SSize2(292, 112));

            layout.AddElement(gameTitle);
        }
    }
}
