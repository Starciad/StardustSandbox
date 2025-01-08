using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
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
        private readonly (SButton, Color)[] colorButtons;

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
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkGray); }), SColorPalette.DarkGray),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Charcoal); }), SColorPalette.Charcoal),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Maroon); }), SColorPalette.Maroon),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkRed); }), SColorPalette.DarkRed),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Crimson); }), SColorPalette.Crimson),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.OrangeRed); }), SColorPalette.OrangeRed),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Orange); }), SColorPalette.Orange),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Amber); }), SColorPalette.Amber),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Gold); }), SColorPalette.Gold),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.LemonYellow); }), SColorPalette.LemonYellow),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.LimeGreen); }), SColorPalette.LimeGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.GrassGreen); }), SColorPalette.GrassGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.ForestGreen); }), SColorPalette.ForestGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.EmeraldGreen); }), SColorPalette.EmeraldGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkGreen); }), SColorPalette.DarkGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.MossGreen); }), SColorPalette.MossGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkTeal); }), SColorPalette.DarkTeal),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.NavyBlue); }), SColorPalette.NavyBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.RoyalBlue); }), SColorPalette.RoyalBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.SkyBlue); }), SColorPalette.SkyBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Cyan); }), SColorPalette.Cyan),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Mint); }), SColorPalette.Mint),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.White); }), SColorPalette.White),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.PaleYellow); }), SColorPalette.PaleYellow),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Peach); }), SColorPalette.Peach),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Salmon); }), SColorPalette.Salmon),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Rose); }), SColorPalette.Rose),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Magenta); }), SColorPalette.Magenta),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Violet); }), SColorPalette.Violet),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.PurpleGray); }), SColorPalette.PurpleGray),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkPurple); }), SColorPalette.DarkPurple),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Cocoa); }), SColorPalette.Cocoa),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Umber); }), SColorPalette.Umber),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Brown); }), SColorPalette.Brown),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Rust); }), SColorPalette.Rust),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Sand); }), SColorPalette.Sand),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Tan); }), SColorPalette.Tan),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.LightGrayBlue); }), SColorPalette.LightGrayBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.SteelBlue); }), SColorPalette.SteelBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Slate); }), SColorPalette.Slate),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Graphite); }), SColorPalette.Graphite),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Gunmetal); }), SColorPalette.Gunmetal),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Coal); }), SColorPalette.Coal),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkBrown); }), SColorPalette.DarkBrown),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Burgundy); }), SColorPalette.Burgundy),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Clay); }), SColorPalette.Clay),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Terracotta); }), SColorPalette.Terracotta),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Blush); }), SColorPalette.Blush),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.PaleBlue); }), SColorPalette.PaleBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.LavenderBlue); }), SColorPalette.LavenderBlue),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Periwinkle); }), SColorPalette.Periwinkle),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Cerulean); }), SColorPalette.Cerulean),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.TealGray); }), SColorPalette.TealGray),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.HunterGreen); }), SColorPalette.HunterGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.PineGreen); }), SColorPalette.PineGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.SeafoamGreen); }), SColorPalette.SeafoamGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.MintGreen); }), SColorPalette.MintGreen),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Aquamarine); }), SColorPalette.Aquamarine),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Khaki); }), SColorPalette.Khaki),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Beige); }), SColorPalette.Beige),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Sepia); }), SColorPalette.Sepia),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.Coffee); }), SColorPalette.Coffee),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkBeige); }), SColorPalette.DarkBeige),
                (new(null, string.Empty, string.Empty, () => { SelectColorButtonAction(SColorPalette.DarkTaupe); }), SColorPalette.DarkTaupe),
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
