using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Background;
using StardustSandbox.Game.Controllers.Background;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SBackgroundManager(SGame gameInstance) : SGameObject(gameInstance)
    {
        public Color SolidColor => this.solidColor;

        private Color solidColor;
        private SBackground selectedBackground;

        private SCloudController cloudController;
        //private SCelestialBodyController celestialBodyController;

        public override void Initialize()
        {
            this.cloudController = new(this.SGameInstance);

            //this.celestialBodyController = new(this.SGameInstance,
            //    this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_1"),
            //    this.SGameInstance.AssetDatabase.GetTexture("bgo_celestial_body_1"),
            //    this.SGameInstance.AssetDatabase.GetTexture("background_2"),
            //100f);

            SetColor(new Color(64, 116, 155));
            SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("ocean_1"));
        }

        public override void Update(GameTime gameTime)
        {
            //this.celestialBodyController.Update(gameTime);
            this.cloudController.Update(gameTime);
            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //this.celestialBodyController.Draw(gameTime, spriteBatch);
            this.cloudController.Draw(gameTime, spriteBatch);
            this.selectedBackground.Draw(gameTime, spriteBatch);
        }

        public void SetColor(Color value)
        {
            this.solidColor = value;
        }

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }
    }
}
