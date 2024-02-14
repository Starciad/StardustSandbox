using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.IO;
using PixelDust.Game.Models.Settings;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PCursorManager(PAssetDatabase assetDatabase, PInputManager inputManager) : PGameObject
    {
        private readonly Texture2D[] cursorTextures = new Texture2D[2];
        private static readonly Rectangle[] cursorClipAreas = [
            new Rectangle(new Point(0, 0), new Point(36, 36)),
            new Rectangle(new Point(0, 36), new Point(36, 36)),
        ];

        private readonly int cursorTextureSelected = 0;

        private Vector2 cursorScale;

        private Color cursorBackgroundColor;
        private Color cursorColor;

        private Vector2 cursorBackgroundPosition;
        private Vector2 cursorPosition;

        protected override void OnAwake()
        {
            this.cursorTextures[0] = assetDatabase.GetTexture("cursor_1");
            this.cursorTextures[1] = assetDatabase.GetTexture("cursor_2");

            UpdateCursorSettings();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Vector2 pos = inputManager.MouseState.Position.ToVector2();

            this.cursorBackgroundPosition = pos;
            this.cursorPosition = pos;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D cursorSelectedTexture = this.cursorTextures[this.cursorTextureSelected];

            spriteBatch.Draw(cursorSelectedTexture, this.cursorBackgroundPosition, cursorClipAreas[1], this.cursorBackgroundColor, 0f, Vector2.Zero, this.cursorScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cursorSelectedTexture, this.cursorPosition, cursorClipAreas[0], this.cursorColor, 0f, Vector2.Zero, this.cursorScale, SpriteEffects.None, 0f);
        }

        public void UpdateCursorSettings()
        {
            PCursorSettings cursorSettings = PSystemSettingsFile.GetCursorSettings();

            this.cursorScale = new(cursorSettings.Scale);
            this.cursorColor = cursorSettings.Color;
            this.cursorBackgroundColor = cursorSettings.BackgroundColor;
        }
    }
}
