using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
using StardustSandbox.ContentBundle.Localization.Colors;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools
{
    internal sealed partial class SGUI_ColorPicker : SGUISystem
    {
        private SColorPickerSettings colorPickerSettings;

        private readonly Texture2D particleTexture;
        private readonly Texture2D colorButtonTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly SButton[] menuButtons;
        private readonly SColorButton[] colorButtons;

        internal SGUI_ColorPicker(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.colorButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_4");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

            this.menuButtons = [
                new(null, SLocalization_Statements.Cancel, string.Empty, CancelButtonAction),
            ];

            this.colorButtons = [
                new(SLocalization_Colors.DarkGray, SColorPalette.DarkGray),
                new(SLocalization_Colors.Charcoal, SColorPalette.Charcoal),
                new(SLocalization_Colors.Maroon, SColorPalette.Maroon),
                new(SLocalization_Colors.DarkRed, SColorPalette.DarkRed),
                new(SLocalization_Colors.Crimson, SColorPalette.Crimson),
                new(SLocalization_Colors.OrangeRed, SColorPalette.OrangeRed),
                new(SLocalization_Colors.Orange, SColorPalette.Orange),
                new(SLocalization_Colors.Amber, SColorPalette.Amber),
                new(SLocalization_Colors.Gold, SColorPalette.Gold),
                new(SLocalization_Colors.LemonYellow, SColorPalette.LemonYellow),
                new(SLocalization_Colors.LimeGreen, SColorPalette.LimeGreen),
                new(SLocalization_Colors.GrassGreen, SColorPalette.GrassGreen),
                new(SLocalization_Colors.ForestGreen, SColorPalette.ForestGreen),
                new(SLocalization_Colors.EmeraldGreen, SColorPalette.EmeraldGreen),
                new(SLocalization_Colors.DarkGreen, SColorPalette.DarkGreen),
                new(SLocalization_Colors.MossGreen, SColorPalette.MossGreen),
                new(SLocalization_Colors.DarkTeal, SColorPalette.DarkTeal),
                new(SLocalization_Colors.NavyBlue, SColorPalette.NavyBlue),
                new(SLocalization_Colors.RoyalBlue, SColorPalette.RoyalBlue),
                new(SLocalization_Colors.SkyBlue, SColorPalette.SkyBlue),
                new(SLocalization_Colors.Cyan, SColorPalette.Cyan),
                new(SLocalization_Colors.Mint, SColorPalette.Mint),
                new(SLocalization_Colors.White, SColorPalette.White),
                new(SLocalization_Colors.PaleYellow, SColorPalette.PaleYellow),
                new(SLocalization_Colors.Peach, SColorPalette.Peach),
                new(SLocalization_Colors.Salmon, SColorPalette.Salmon),
                new(SLocalization_Colors.Rose, SColorPalette.Rose),
                new(SLocalization_Colors.Magenta, SColorPalette.Magenta),
                new(SLocalization_Colors.Violet, SColorPalette.Violet),
                new(SLocalization_Colors.PurpleGray, SColorPalette.PurpleGray),
                new(SLocalization_Colors.DarkPurple, SColorPalette.DarkPurple),
                new(SLocalization_Colors.Cocoa, SColorPalette.Cocoa),
                new(SLocalization_Colors.Umber, SColorPalette.Umber),
                new(SLocalization_Colors.Brown, SColorPalette.Brown),
                new(SLocalization_Colors.Rust, SColorPalette.Rust),
                new(SLocalization_Colors.Sand, SColorPalette.Sand),
                new(SLocalization_Colors.Tan, SColorPalette.Tan),
                new(SLocalization_Colors.LightGrayBlue, SColorPalette.LightGrayBlue),
                new(SLocalization_Colors.SteelBlue, SColorPalette.SteelBlue),
                new(SLocalization_Colors.Slate, SColorPalette.Slate),
                new(SLocalization_Colors.Graphite, SColorPalette.Graphite),
                new(SLocalization_Colors.Gunmetal, SColorPalette.Gunmetal),
                new(SLocalization_Colors.Coal, SColorPalette.Coal),
                new(SLocalization_Colors.DarkBrown, SColorPalette.DarkBrown),
                new(SLocalization_Colors.Burgundy, SColorPalette.Burgundy),
                new(SLocalization_Colors.Clay, SColorPalette.Clay),
                new(SLocalization_Colors.Terracotta, SColorPalette.Terracotta),
                new(SLocalization_Colors.Blush, SColorPalette.Blush),
                new(SLocalization_Colors.PaleBlue, SColorPalette.PaleBlue),
                new(SLocalization_Colors.LavenderBlue, SColorPalette.LavenderBlue),
                new(SLocalization_Colors.Periwinkle, SColorPalette.Periwinkle),
                new(SLocalization_Colors.Cerulean, SColorPalette.Cerulean),
                new(SLocalization_Colors.TealGray, SColorPalette.TealGray),
                new(SLocalization_Colors.HunterGreen, SColorPalette.HunterGreen),
                new(SLocalization_Colors.PineGreen, SColorPalette.PineGreen),
                new(SLocalization_Colors.SeafoamGreen, SColorPalette.SeafoamGreen),
                new(SLocalization_Colors.MintGreen, SColorPalette.MintGreen),
                new(SLocalization_Colors.Aquamarine, SColorPalette.Aquamarine),
                new(SLocalization_Colors.Khaki, SColorPalette.Khaki),
                new(SLocalization_Colors.Beige, SColorPalette.Beige),
                new(SLocalization_Colors.Sepia, SColorPalette.Sepia),
                new(SLocalization_Colors.Coffee, SColorPalette.Coffee),
                new(SLocalization_Colors.DarkBeige, SColorPalette.DarkBeige),
                new(SLocalization_Colors.DarkTaupe, SColorPalette.DarkTaupe),
            ];

            this.menuButtonElements = new SGUILabelElement[this.menuButtons.Length];
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
                SGUILabelElement labelElement = this.menuButtonElements[i];

                SSize2 size = labelElement.GetStringSize() / 2;
                Vector2 position = labelElement.Position;

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        // ====================================== //

        internal void Configure(SColorPickerSettings settings)
        {
            this.colorPickerSettings = settings;
            this.captionElement.SetTextualContent(settings.Caption);
        }
    }
}
