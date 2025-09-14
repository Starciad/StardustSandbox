using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Enums.GUISystem.Tools.Confirm;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Common.Helpers.General;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.GUISystem.Common.Helpers.Tools.Settings;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Localization.Messages;
using StardustSandbox.Core.Localization.Statements;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.GameContent.GUISystem.GUIs.Tools.Confirm;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.Pause
{
    internal sealed partial class SGUI_Pause : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiLargeButtonTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        private readonly SGUI_Confirm guiConfirm;

        private readonly SConfirmSettings exitConfirmSettings;

        internal SGUI_Pause(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Confirm guiConfirm) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_background_14");
            this.guiLargeButtonTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_button_3");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.guiConfirm = guiConfirm;
            this.exitConfirmSettings = new()
            {
                Caption = SLocalization_Messages.Confirm_Simulation_Exit_Title,
                Message = SLocalization_Messages.Confirm_Simulation_Exit_Description,
                OnConfirmCallback = status =>
                {
                    if (status == SConfirmStatus.Confirmed)
                    {
                        this.SGameInstance.GUIManager.Reset();
                        this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
                    }
                }
            };

            this.menuButtons = [
                new(null, SLocalization_Statements.Resume, string.Empty, ResumeButtonAction),
                new(null, SLocalization_Statements.Options, string.Empty, OptionsButtonAction),
                new(null, SLocalization_Statements.Exit, string.Empty, ExitButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                SSize2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size.ToVector2();

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }
    }
}
