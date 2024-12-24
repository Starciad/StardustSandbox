using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SScreenConstants.DEFAULT_SCREEN_SIZE,
                Color = new(SColorPalette.DarkGray, 160)
            };

            layoutBuilder.AddElement(guiBackground);
        }
    }
}
