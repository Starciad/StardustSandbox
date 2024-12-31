using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Background.Handlers;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Background.Handlers
{
    internal sealed class SSkyHandler(ISGame gameInstance) : SGameObject(gameInstance), ISSkyHandler
    {
        public bool IsActive { get; set; } = true;
        public Texture2D Texture => this.texture;
        public Effect Effect => this.effect;
        public SGradientColorMap[] GradientColorMap => this.gradientColorMap;

        private Texture2D texture;
        private Effect effect;
        private readonly SGradientColorMap[] gradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(6, 0, 0),  // Dawn
                InitialColor = SColorPalette.NavyBlue,
                FinalColor = SColorPalette.OrangeRed,
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(12, 0, 0), // Noon
                InitialColor = SColorPalette.SkyBlue,
                FinalColor = SColorPalette.NavyBlue
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(18, 0, 0),  // Dusk
                InitialColor = SColorPalette.NavyBlue,
                FinalColor = SColorPalette.OrangeRed,
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(0, 0, 0),    // Midnight
                InitialColor = new(8, 20, 38, 255),
                FinalColor = SColorPalette.DarkGray
            },
        ];

        public override void Initialize()
        {
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("background_4");
            this.effect = this.SGameInstance.AssetDatabase.GetEffect("effect_2");
        }

        public void Reset()
        {

        }
    }
}
