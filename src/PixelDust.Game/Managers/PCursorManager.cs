using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.Extensions;
using PixelDust.Game.Objects;
using PixelDust.Game.Enums.General;

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

        private readonly Vector2 cursorScale = new(1);

        private readonly Color cursorBackgroundColor = Color.Red;
        private readonly Color cursorColor = Color.White;

        private Vector2 cursorBackgroundPosition = Vector2.Zero;
        private Vector2 cursorPosition = Vector2.Zero;

        protected override void OnAwake()
        {
            this.cursorTextures[0] = assetDatabase.GetTexture("cursor_1");
            this.cursorTextures[1] = assetDatabase.GetTexture("cursor_2");
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
    }
}
