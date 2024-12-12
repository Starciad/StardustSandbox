using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_CreditsMenu : SGUISystem
    {
        private enum SCreditContentType
        {
            Text,
            Title,
            Image
        }

        private struct SCreditSection(string title, SCreditContent[] contents)
        {
            public readonly string Title => title;
            public readonly SCreditContent[] Contents => contents;
        }

        private struct SCreditContent
        {
            public SCreditContentType ContentType { get; set; }
            public string Text { get; set; }
            public Texture2D Texture { get; set; }
            public Vector2 TextureScale { get; set; }
            public Vector2 Margin { get; set; }

            public SCreditContent()
            {
                this.ContentType = SCreditContentType.Text;
                this.TextureScale = Vector2.One;
            }
        }

        private readonly float speed = 0.75f;

        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D starciadCharacterTexture;

        private readonly Texture2D monogameLogoTexture;
        private readonly Texture2D xnaLogoTexture;

        private readonly Song creditsMenuSong;
        private readonly SpriteFont digitalDiscoSpriteFont;

        private readonly ISWorld world;

        public SGUI_CreditsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.starciadCharacterTexture = gameInstance.AssetDatabase.GetTexture("character_1");
            this.monogameLogoTexture = gameInstance.AssetDatabase.GetTexture("third_party_1");
            this.xnaLogoTexture = gameInstance.AssetDatabase.GetTexture("third_party_2");
            this.creditsMenuSong = this.SGameInstance.AssetDatabase.GetSong("song_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.DIGITAL_DISCO);
            this.world = gameInstance.World;
        }

        protected override void OnOpened()
        {
            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("credits"));

            this.world.IsActive = false;
            this.world.IsVisible = false;

            SSongEngine.Play(this.creditsMenuSong);

            foreach (SGUIElement element in this.creditElements)
            {
                element.PositionRelativeToScreen();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateUserInput();
            UpdateElementsPosition();
        }

        private void UpdateUserInput()
        {
            if (this.SGameInstance.InputManager.MouseState.LeftButton == ButtonState.Pressed ||
                this.SGameInstance.InputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                CloseCredits();
            }
        }

        private void UpdateElementsPosition()
        {
            foreach (SGUIElement creditElement in this.creditElements)
            {
                creditElement.Position = new(creditElement.Position.X, creditElement.Position.Y - this.speed);
            }
        }

        private void CloseCredits()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
        }
    }
}
