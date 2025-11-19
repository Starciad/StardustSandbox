using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Settings;

namespace StardustSandbox.Managers
{
    internal sealed class CursorManager
    {
        internal Vector2 Position => this.position;
        internal Vector2 Scale => this.scale;
        internal Color Color => this.color;

        private Vector2 position;
        private Vector2 scale;
        private Color color;

        private Vector2 backgroundPosition;
        private Color backgroundColor;

        private InputManager inputManager;

        private readonly Texture2D[] cursorTextures = new Texture2D[2];
        private readonly int cursorTextureSelected = 0;

        private static readonly Rectangle[] cursorClipAreas = [
            new(new Point(0, 0), new Point(36, 36)),
            new(new Point(0, 36), new Point(36, 36)),
        ];

        internal void Initialize(InputManager inputManager)
        {
            this.cursorTextures[0] = AssetDatabase.GetTexture("texture_cursor_1");
            this.cursorTextures[1] = AssetDatabase.GetTexture("texture_cursor_2");

            this.inputManager = inputManager;

            ApplySettings();
        }

        internal void Update()
        {
            Vector2 pos = this.inputManager.MouseState.Position.ToVector2();

            this.backgroundPosition = pos;
            this.position = pos;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Texture2D cursorSelectedTexture = this.cursorTextures[this.cursorTextureSelected];

            spriteBatch.Draw(cursorSelectedTexture, this.backgroundPosition, cursorClipAreas[1], this.backgroundColor, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cursorSelectedTexture, this.position, cursorClipAreas[0], this.color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
        }

        internal void ApplySettings()
        {
            CursorSettings cursorSettings = SettingsHandler.LoadSettings<CursorSettings>();

            this.color = cursorSettings.Color;
            this.backgroundColor = cursorSettings.BackgroundColor;
            this.scale = new((float)cursorSettings.Scale);
        }
    }
}
