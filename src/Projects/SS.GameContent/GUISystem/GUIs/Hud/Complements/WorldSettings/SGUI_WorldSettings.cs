using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.Confirm;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.Confirm;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Messages;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.WorldSettings
{
    internal sealed partial class SGUI_WorldSettings : SGUISystem
    {
        private SSize2 worldTargetSize;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
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

        private readonly SGUI_Confirm guiConfirm;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        private readonly SConfirmSettings changeWorldSizeConfirmSettings;

        internal SGUI_WorldSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Confirm guiConfirm, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_background_9");
            this.guiSmallButtonTexture = gameInstance.AssetDatabase.GetTexture("texture_gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_16");
            this.smallIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_38");
            this.mediumSmallIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_39");
            this.mediumIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_40");
            this.mediumLargeIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_41");
            this.largeIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_42");
            this.veryLargeIconTexture = gameInstance.AssetDatabase.GetTexture("texture_icon_gui_43");

            this.guiConfirm = guiConfirm;
            this.changeWorldSizeConfirmSettings = new()
            {
                Caption = SLocalization_Messages.Confirm_World_Resize_Title,
                Message = SLocalization_Messages.Confirm_World_Resize_Description,
                OnConfirmCallback = (SConfirmStatus status) =>
                {
                    if (status == SConfirmStatus.Confirmed)
                    {
                        this.SGameInstance.World.StartNew(this.worldTargetSize);
                    }

                    this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
                },
            };

            this.tooltipBoxElement = tooltipBoxElement;

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sizeButtons = [
                new(this.smallIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[0]); }),
                new(this.mediumSmallIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[1]); }),
                new(this.mediumIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[2]); }),
                new(this.mediumLargeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[3]); }),
                new(this.largeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[4]); }),
                new(this.veryLargeIconTexture, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Name, SLocalization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Description, () => { SetWorldSizeButtonAction(SWorldConstants.WORLD_SIZES_TEMPLATE[5]); }),
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

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                SSlot slot = this.sizeButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.GRID_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.sizeButtons[i].ClickAction?.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.sizeButtons[i].Name;
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
