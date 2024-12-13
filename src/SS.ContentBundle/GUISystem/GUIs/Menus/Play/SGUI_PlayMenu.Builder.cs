using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_PlayMenu
    {
        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildButtons(layoutBuilder);
        }

        private void BuildButtons(ISGUILayoutBuilder layoutBuilder)
        {
            BuildReturnButton(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
        }

        private void BuildReturnButton(ISGUILayoutBuilder layoutBuilder)
        {
            return;
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Color = SColorPalette.White,
                    Margin = margin,
                    Scale = new(0.15f),
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center,
                };

                labelElement.SetTextualContent(this.menuButtons[i].DisplayName);
                labelElement.PositionRelativeToScreen();
                layoutBuilder.AddElement(labelElement);

                margin.Y += labelElement.GetStringSize().Height + 16f;
            }
        }
    }
}
