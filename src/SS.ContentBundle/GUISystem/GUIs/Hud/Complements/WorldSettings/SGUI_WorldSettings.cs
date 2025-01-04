using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
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
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D iconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;
        private readonly SButton[] sizeButtons;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_WorldSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");

            this.tooltipBoxElement = tooltipBoxElement;

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_GUIs.Button_Exit_Name, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sizeButtons = [
                new(this.iconTexture, "Small", "40x23 - Fits entirely within the player's camera", () => { SetWorldSizeButtonAction(new SSize2(40, 23)); }),
                new(this.iconTexture, "Medium-Small", "80x46 - Allows limited free movement", () => { SetWorldSizeButtonAction(new SSize2(80, 46)); }),
                new(this.iconTexture, "Medium", "120x69 - Balanced for most gameplay scenarios", () => { SetWorldSizeButtonAction(new SSize2(120, 69)); }),
                new(this.iconTexture, "Medium-Large", "160x92 - Provides ample space for exploration", () => { SetWorldSizeButtonAction(new SSize2(160, 92)); }),
                new(this.iconTexture, "Large", "240x138 - Designed for expansive gameplay areas. May impact performance on lower-end systems.", () => { SetWorldSizeButtonAction(new SSize2(240, 138)); }),
                new(this.iconTexture, "Very Large", "320x184 - Best for epic-scale worlds. May impact performance on lower-end systems.", () => { SetWorldSizeButtonAction(new SSize2(320, 184)); }),
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
