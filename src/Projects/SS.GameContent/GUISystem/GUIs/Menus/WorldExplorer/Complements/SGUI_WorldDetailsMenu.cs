using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.IO.Files.Saving;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldExplorer.Complements
{
    internal sealed partial class SGUI_WorldDetailsMenu : SGUISystem
    {
        private SSaveFile worldSaveFile;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButton3Texture;
        private readonly Texture2D returnIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly SButton[] worldButtons;

        internal SGUI_WorldDetailsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("texture_particle_1");
            this.guiButton3Texture = this.SGameInstance.AssetDatabase.GetTexture("texture_gui_button_1");
            this.returnIconTexture = this.SGameInstance.AssetDatabase.GetTexture("texture_icon_gui_16");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

            this.worldButtons = [
                new(null, "Return", string.Empty, ReturnButtonAction),
                new(null, "Delete", string.Empty, DeleteButtonAction),
                new(null, "Play", string.Empty, PlayButtonAction),
            ];

            this.worldButtonElements = new SGUILabelElement[this.worldButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            // Buttons
            for (int i = 0; i < this.worldButtonElements.Length; i++)
            {
                SGUILabelElement slotInfoElement = this.worldButtonElements[i];

                SSize2 buttonSize = slotInfoElement.GetStringSize() / 2;
                Vector2 buttonPosition = new(slotInfoElement.Position.X + buttonSize.Width, slotInfoElement.Position.Y - (buttonSize.Height / 4));

                if (this.GUIEvents.OnMouseClick(buttonPosition, buttonSize))
                {
                    this.worldButtons[i].ClickAction?.Invoke();
                }

                slotInfoElement.Color = this.GUIEvents.OnMouseOver(buttonPosition, buttonSize) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        internal void SetWorldSaveFile(SSaveFile worldSaveFile)
        {
            this.worldSaveFile = worldSaveFile;
            UpdateDisplay(worldSaveFile);
        }

        private void UpdateDisplay(SSaveFile worldSaveFile)
        {
            this.worldThumbnailElement.Texture = worldSaveFile.Header.ThumbnailTexture;
            this.worldTitleElement.SetTextualContent(worldSaveFile.Header.Metadata.Name);
            this.worldDescriptionElement.SetTextualContent(worldSaveFile.Header.Metadata.Description);
            this.worldVersionElement.SetTextualContent(string.Concat('v', worldSaveFile.Header.Information.SaveVersion));
            this.worldCreationTimestampElement.SetTextualContent(worldSaveFile.Header.Information.CreationTimestamp.ToString());
        }
    }
}
