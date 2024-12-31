using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Backgrounds;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Controllers.Background;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;

using System;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SBackgroundManager(ISGame gameInstance) : SManager(gameInstance), ISBackgroundManager
    {
        public Color SolidColor { get; set; } = new(64, 116, 155);
        public Texture2D SkyTexture => this.skyTexture;
        public Effect SkyEffect => this.skyEffect;
        public SGradientColorMap[] SkyGradientColorMap => this.skyGradientColorMap;

        private SBackground selectedBackground;
        private SCloudController cloudController;

        private Texture2D skyTexture;
        private Effect skyEffect;
        private readonly SGradientColorMap[] skyGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(6, 0, 0),  // Dawn
                InitialColor = SColorPalette.DarkGray,
                FinalColor = SColorPalette.NavyBlue
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(12, 0, 0), // Noon
                InitialColor = SColorPalette.NavyBlue,
                FinalColor = SColorPalette.Gold
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(18, 0, 0),  // Dusk
                InitialColor = SColorPalette.Gold,
                FinalColor = SColorPalette.Orange
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(0, 0, 0),    // Midnight
                InitialColor = Color.Orange,
                FinalColor = Color.DarkGray
            },
        ];

        public override void Initialize()
        {
            this.skyTexture = this.SGameInstance.AssetDatabase.GetTexture("background_4");
            this.skyEffect = this.SGameInstance.AssetDatabase.GetEffect("effect_2");

            this.cloudController = new(this.SGameInstance);
            this.cloudController.Initialize();
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

        public void SetBackground(SBackground background)
        {
            this.selectedBackground = background;
        }

        public void EnableClouds()
        {
            this.cloudController.Enable();
        }

        public void DisableClouds()
        {
            this.cloudController.Disable();
            this.cloudController.Clear();
        }

        public void Reset()
        {
            this.cloudController.Reset();
        }
    }
}
