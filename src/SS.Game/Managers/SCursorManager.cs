using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.IO;
using StardustSandbox.Game.Models.Settings;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SCursorManager : SGameObject
    {
        private readonly Texture2D[] cursorTextures = new Texture2D[2];
        private static readonly Rectangle[] cursorClipAreas = [
            new Rectangle(new Point(0, 0), new Point(36, 36)),
            new Rectangle(new Point(0, 36), new Point(36, 36)),
        ];

        private Vector2 cursorScale;

        private Color cursorBackgroundColor;
        private Color cursorColor;

        private Vector2 cursorBackgroundPosition;
        private Vector2 cursorPosition;

        private readonly int cursorTextureSelected = 0;

        private readonly SAssetDatabase _assetDatabase;
        private readonly SInputManager _inputManager;

        public SCursorManager(SGame gameInstance, SAssetDatabase assetDatabase, SInputManager inputManager) : base(gameInstance)
        {
            this._assetDatabase = assetDatabase;
            this._inputManager = inputManager;
        }

        public override void Initialize()
        {
            this.cursorTextures[0] = this._assetDatabase.GetTexture("cursor_1");
            this.cursorTextures[1] = this._assetDatabase.GetTexture("cursor_2");

            UpdateCursorSettings();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = this._inputManager.MouseState.Position.ToVector2();

            this.cursorBackgroundPosition = pos;
            this.cursorPosition = pos;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D cursorSelectedTexture = this.cursorTextures[this.cursorTextureSelected];

            spriteBatch.Draw(cursorSelectedTexture, this.cursorBackgroundPosition, cursorClipAreas[1], this.cursorBackgroundColor, 0f, Vector2.Zero, this.cursorScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cursorSelectedTexture, this.cursorPosition, cursorClipAreas[0], this.cursorColor, 0f, Vector2.Zero, this.cursorScale, SpriteEffects.None, 0f);
        }

        public void UpdateCursorSettings()
        {
            SCursorSettings cursorSettings = SSystemSettingsFile.GetCursorSettings();

            this.cursorScale = new(cursorSettings.Scale);
            this.cursorColor = cursorSettings.Color;
            this.cursorBackgroundColor = cursorSettings.BackgroundColor;
        }
    }
}
