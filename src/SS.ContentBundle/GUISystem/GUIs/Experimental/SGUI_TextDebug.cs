using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Experimental
{
    public sealed class SGUI_TextDebug(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            SGUITextElement textElement = new(this.SGameInstance)
            {
                Scale = new Vector2(0.1f),
                TextAreaSize = new SSize2F(550f, 300f),
                LineHeight = 1.5f,
                WordSpacing = 5f,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            textElement.SetSpriteFont(SFontFamilyConstants.COMIC_SANS_MS);
            textElement.SetTextualContent("Welcome to the Stardust Sandbox! This is an example of rendering text with word wrap and alignment.");
            textElement.SetAllBorders(true, Color.Black, new(2f));
            textElement.PositionRelativeToScreen();

            layout.AddElement(textElement);
        }
    }
}
