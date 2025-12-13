using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.InputSystem;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

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

        private Texture2D cursorTexture;

        private static readonly Rectangle[] cursorClipAreas = [
            new(0, 0, 36, 36),
            new(0, 36, 36, 36),
        ];

        internal void Initialize()
        {
            this.cursorTexture = AssetDatabase.GetTexture(TextureIndex.Cursors);

            ApplySettings();
        }

        internal void Update()
        {
            Vector2 position = Input.MouseState.Position.ToVector2();

            this.backgroundPosition = position;
            this.position = position;
        }

        internal void Draw(in SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.cursorTexture, this.backgroundPosition, cursorClipAreas[1], this.backgroundColor, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(this.cursorTexture, this.position, cursorClipAreas[0], this.color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
        }

        internal void ApplySettings()
        {
            CursorSettings cursorSettings = SettingsSerializer.LoadSettings<CursorSettings>();

            this.color = cursorSettings.Color;
            this.backgroundColor = cursorSettings.BackgroundColor;
            this.scale = new(cursorSettings.Scale);
        }
    }
}
