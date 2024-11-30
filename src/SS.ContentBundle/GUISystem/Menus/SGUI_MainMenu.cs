using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.GUISystem.Elements;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu : SGUISystem
    {
        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D buttonBackgroundTexture;

        public SGUI_MainMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.buttonBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_2");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                (SGUISliceImageElement buttonBackground, SGUILabelElement buttonLabel) = this.menuButtons[i];

                if (!buttonBackground.IsVisible)
                {
                    continue;
                }

                // Highlight when mouse is over slot.
                if (this.GUIEvents.OnMouseOver(buttonBackground.Position, buttonBackground.Size))
                {
                    buttonBackground.SetColor(Color.DarkGray);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    buttonBackground.SetColor(Color.White);
                }
            }
        }
    }
}
