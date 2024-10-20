using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Background.Details
{
    public sealed class SCloud : SGameObject, ISReset
    {
        public Vector2 Position;
        public Texture2D Texture;
        public float Speed;
        public float Opacity;

        public SCloud(SGame gameInstance, Texture2D texture, float speed, float opacity) : base(gameInstance)
        {
            this.Texture = texture;
            this.Speed = speed;
            this.Opacity = opacity;
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            this.Position.X += this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White * this.Opacity;
            spriteBatch.Draw(this.Texture, this.Position, color);
        }

        public void Reset()
        {
            this.Position = new Vector2(this.Texture.Width, 0f);
            this.Speed = SRandomMath.Range(10, 50);
            this.Opacity = ((float)SRandomMath.RandomDouble() * 0.5f) + 0.5f;
        }
    }
}
