using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PCursorManager(PAssetDatabase assetDatabase, PInputManager inputManager) : PGameObject
    {
        private readonly Texture2D[] cursorTextures = new Texture2D[2];

        private readonly int cursorTextureSelected = 0;
        private Vector2 mousePosition = Vector2.Zero;

        protected override void OnAwake()
        {
            this.cursorTextures[0] = assetDatabase.GetTexture("cursor_1");
            this.cursorTextures[1] = assetDatabase.GetTexture("cursor_2");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.mousePosition = inputManager.MouseState.Position.ToVector2();
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.cursorTextures[this.cursorTextureSelected], this.mousePosition, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
