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
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_EnvironmentSettings : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D frozenTimeIconTexture;
        private readonly Texture2D timeProgressIconTexture;
        private readonly Texture2D midnightIconTexture;
        private readonly Texture2D dawnIconTexture;
        private readonly Texture2D morningIconTexture;
        private readonly Texture2D noonIconTexture;
        private readonly Texture2D afternoonIconTexture;
        private readonly Texture2D duskIconTexture;
        private readonly Texture2D eveningIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        private readonly SButton[] timeStateButtons;
        private readonly SButton[] timeButtons;

        private readonly ISWorld world;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_EnvironmentSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_7");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.world = gameInstance.World;

            this.tooltipBoxElement = tooltipBoxElement;

            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.frozenTimeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_29");
            this.timeProgressIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_30");
            this.midnightIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_31");
            this.dawnIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_32");
            this.morningIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_33");
            this.noonIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_34");
            this.afternoonIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_35");
            this.duskIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_36");
            this.eveningIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_37");

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.timeStateButtons = [
                new SButton(this.frozenTimeIconTexture, SLocalization_Statements.Disable, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Disable_Description, () => SetTimeFreezeState(true)),
                new SButton(this.timeProgressIconTexture, SLocalization_Statements.Enable, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Enable_Description, () => SetTimeFreezeState(false)),
            ];

            this.timeButtons = [
                new SButton(this.midnightIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Description, () => SetTimeButtonAction(new TimeSpan(0, 0, 0))),
                new SButton(this.dawnIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Description, () => SetTimeButtonAction(new TimeSpan(6, 0, 0))),
                new SButton(this.morningIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Description, () => SetTimeButtonAction(new TimeSpan(9, 0, 0))),
                new SButton(this.noonIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Description, () => SetTimeButtonAction(new TimeSpan(12, 0, 0))),
                new SButton(this.afternoonIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Description, () => SetTimeButtonAction(new TimeSpan(15, 0, 0))),
                new SButton(this.duskIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Description, () => SetTimeButtonAction(new TimeSpan(18, 0, 0))),
                new SButton(this.eveningIconTexture, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Title, SLocalization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Description, () => SetTimeButtonAction(new TimeSpan(21, 0, 0))),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.timeStateButtonSlots = new SSlot[this.timeStateButtons.Length];
            this.timeButtonSlots = new SSlot[this.timeButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateTimeStateButtons();
            UpdateTimeButtons();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.menuButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateTimeStateButtons()
        {
            for (int i = 0; i < this.timeStateButtons.Length; i++)
            {
                SSlot slot = this.timeStateButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.timeStateButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.timeStateButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.timeStateButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }

            if (this.world.Time.IsFrozen)
            {
                this.timeStateButtonSlots[0].BackgroundElement.Color = SColorPalette.SelectedColor;
            }
            else
            {
                this.timeStateButtonSlots[1].BackgroundElement.Color = SColorPalette.SelectedColor;
            }
        }

        private void UpdateTimeButtons()
        {
            for (int i = 0; i < this.timeButtons.Length; i++)
            {
                SSlot slot = this.timeButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.timeButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.timeButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.timeButtons[i].Description;

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
