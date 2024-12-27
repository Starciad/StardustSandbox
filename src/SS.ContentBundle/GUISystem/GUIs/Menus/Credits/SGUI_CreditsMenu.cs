using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_CreditsMenu : SGUISystem
    {
        private enum SCreditContentType
        {
            Text,
            Title,
            Image
        }

        private struct SCreditSection(string title, SCreditContent[] contents)
        {
            internal readonly string Title => title;
            internal readonly SCreditContent[] Contents => contents;
        }

        private struct SCreditContent
        {
            internal SCreditContentType ContentType { get; set; }
            internal string Text { get; set; }
            internal Texture2D Texture { get; set; }
            internal Vector2 TextureScale { get; set; }
            internal Vector2 Margin { get; set; }

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

        internal SGUI_CreditsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.starciadCharacterTexture = gameInstance.AssetDatabase.GetTexture("character_1");
            this.monogameLogoTexture = gameInstance.AssetDatabase.GetTexture("third_party_1");
            this.xnaLogoTexture = gameInstance.AssetDatabase.GetTexture("third_party_2");
            this.creditsMenuSong = this.SGameInstance.AssetDatabase.GetSong("song_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8");
            this.world = gameInstance.World;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateUserInput();
            UpdateElementsPosition();
            CheckIfTheCreditsHaveFinished();
        }

        private void UpdateUserInput()
        {
            if (this.SGameInstance.InputManager.MouseState.LeftButton == ButtonState.Pressed ||
                this.SGameInstance.InputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.SGameInstance.GUIManager.CloseGUI();
            }
        }

        private void UpdateElementsPosition()
        {
            foreach (SGUIElement creditElement in this.creditElements)
            {
                creditElement.Position = new(creditElement.Position.X, creditElement.Position.Y - this.speed);
            }
        }

        private void CheckIfTheCreditsHaveFinished()
        {
            if ((((this.lastElement.Position.Y + this.lastElement.Size.Height) * this.lastElement.Scale.Y) + 16f) < 0f)
            {
                this.SGameInstance.GUIManager.CloseGUI();
            }
        }
    }
}
