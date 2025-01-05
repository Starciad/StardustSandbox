using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_WorldSettings : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D smallIconTexture;
        private readonly Texture2D mediumSmallIconTexture;
        private readonly Texture2D mediumIconTexture;
        private readonly Texture2D mediumLargeIconTexture;
        private readonly Texture2D largeIconTexture;
        private readonly Texture2D veryLargeIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;
        private readonly SButton[] sizeButtons;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_WorldSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_9");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.smallIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_38");
            this.mediumSmallIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_39");
            this.mediumIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_40");
            this.mediumLargeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_41");
            this.largeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_42");
            this.veryLargeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_43");

            this.tooltipBoxElement = tooltipBoxElement;

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sizeButtons = [
                new(this.smallIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Description, () => { SetWorldSizeButtonAction(new SSize2(40, 23)); }),
                new(this.mediumSmallIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Description, () => { SetWorldSizeButtonAction(new SSize2(80, 46)); }),
                new(this.mediumIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Description, () => { SetWorldSizeButtonAction(new SSize2(120, 69)); }),
                new(this.mediumLargeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Description, () => { SetWorldSizeButtonAction(new SSize2(160, 92)); }),
                new(this.largeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Description, () => { SetWorldSizeButtonAction(new SSize2(240, 138)); }),
                new(this.veryLargeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Description, () => { SetWorldSizeButtonAction(new SSize2(320, 184)); }),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.sizeButtonSlots = new SSlot[this.sizeButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateSizeButtons();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                SSlot slot = this.sizeButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.sizeButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.sizeButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.sizeButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }
    }
}
