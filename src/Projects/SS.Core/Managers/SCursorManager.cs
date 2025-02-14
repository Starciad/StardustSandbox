using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SCursorManager(ISGame gameInstance) : SManager(gameInstance), ISCursorManager
    {
        public Vector2 Scale => this.scale;

        private readonly Texture2D[] cursorTextures = new Texture2D[2];
        private static readonly Rectangle[] cursorClipAreas = [
            new Rectangle(new Point(0, 0), new Point(36, 36)),
            new Rectangle(new Point(0, 36), new Point(36, 36)),
        ];

        private Vector2 position;
        private Vector2 scale;
        private Color color;

        private Vector2 backgroundPosition;
        private Color backgroundColor;

        private readonly int cursorTextureSelected = 0;

        private readonly ISAssetDatabase _assetDatabase = gameInstance.AssetDatabase;
        private readonly ISInputManager _inputManager = gameInstance.InputManager;

        public override void Initialize()
        {
            this.cursorTextures[0] = this._assetDatabase.GetTexture("texture_cursor_1");
            this.cursorTextures[1] = this._assetDatabase.GetTexture("texture_cursor_2");

            ApplySettings();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = this._inputManager.MouseState.Position.ToVector2();

            this.backgroundPosition = pos;
            this.position = pos;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D cursorSelectedTexture = this.cursorTextures[this.cursorTextureSelected];

            spriteBatch.Draw(cursorSelectedTexture, this.backgroundPosition, cursorClipAreas[1], this.backgroundColor, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cursorSelectedTexture, this.position, cursorClipAreas[0], this.color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
        }

        public void ApplySettings()
        {
            SCursorSettings cursorSettings = SSettingsHandler.LoadSettings<SCursorSettings>();

            this.color = cursorSettings.Color;
            this.backgroundColor = cursorSettings.BackgroundColor;
            this.scale = new(cursorSettings.Scale);
        }

        public void Reset()
        {
            return;
        }
    }
}
