using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        #region Initialize
        public override void Initialize()
        {
            this.componentContainer.Initialize();
        }
        #endregion


        #region Update
        public override void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            this.Time.Update(gameTime);

            UpdateWorld(gameTime);
            UpdateEntities(gameTime);
        }

        private void UpdateWorld(GameTime gameTime)
        {
            if (this.currentFrameUpdateSlotsDelay > 0)
            {
                this.currentFrameUpdateSlotsDelay--;
                return;
            }

            this.currentFrameUpdateSlotsDelay = this.totalFramesUpdateSlotsDelay;
            this.componentContainer.Update(gameTime);
        }

        private void UpdateEntities(GameTime gameTime)
        {
            this.instantiatedEntities.ForEach(entity =>
            {
                if (entity == null)
                {
                    return;
                }

                entity.Update(gameTime);
            });
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsVisible)
            {
                return;
            }

            DrawWorld(gameTime, spriteBatch);
            DrawEntities(gameTime, spriteBatch);
        }

        private void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.componentContainer.Draw(gameTime, spriteBatch);
        }

        private void DrawEntities(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.instantiatedEntities.ForEach(entity =>
            {
                if (entity == null)
                {
                    return;
                }

                entity.Draw(gameTime, spriteBatch);
            });
        }
        #endregion
    }
}
