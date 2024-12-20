﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Backgrounds;
using StardustSandbox.Core.Controllers.Background;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Managers
{
    public sealed class SBackgroundManager(ISGame gameInstance) : SManager(gameInstance)
    {
        public Color SolidColor => this.solidColor;

        private Color solidColor;
        private SBackground selectedBackground;

        private SCloudController cloudController;

        public override void Initialize()
        {
            this.cloudController = new(this.SGameInstance);

            SetColor(new Color(64, 116, 155));
            SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("ocean_1"));
        }

        public override void Update(GameTime gameTime)
        {
            this.cloudController.Update(gameTime);
            this.selectedBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
