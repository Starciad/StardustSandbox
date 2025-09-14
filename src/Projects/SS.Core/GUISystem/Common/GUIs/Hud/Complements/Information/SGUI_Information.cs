using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.General;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Localization.GUIs;
using StardustSandbox.Core.Localization.Statements;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.Information
{
    internal sealed partial class SGUI_Information : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        private readonly ISWorld world;

        internal SGUI_Information(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_background_10");
            this.guiSmallButtonTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("texture_icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.infoElements = new SGUILabelElement[SGUI_InformationConstants.AMOUNT_OF_INFORMATION];

            this.world = gameInstance.World;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }
    }
}
