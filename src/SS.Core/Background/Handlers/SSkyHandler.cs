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
        public SSkyGradientColorMap[] GradientColorMap => this.gradientColorMap;

        private Texture2D texture;
        private Effect effect;
        private readonly SSkyGradientColorMap[] gradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(6, 0, 0),  // Dawn
                Color1 = (SColorPalette.NavyBlue, SColorPalette.OrangeRed),
                Color2 = (SColorPalette.SkyBlue, SColorPalette.NavyBlue),
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(12, 0, 0), // Noon
                Color1 = (SColorPalette.SkyBlue, SColorPalette.NavyBlue),
                Color2 = (SColorPalette.NavyBlue, SColorPalette.OrangeRed)
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(18, 0, 0),  // Dusk
                Color1 = (SColorPalette.NavyBlue, SColorPalette.OrangeRed),
                Color2 = (new(8, 20, 38, 255), SColorPalette.DarkGray),
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(23, 59, 59), // Midnight
                Color1 = (new(8, 20, 38, 255), SColorPalette.DarkGray),
                Color2 = (SColorPalette.NavyBlue, SColorPalette.OrangeRed)
            },
        ];

        public override void Initialize()
        {
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("background_4");
            this.effect = this.SGameInstance.AssetDatabase.GetEffect("effect_1");
        }

        public void Reset()
        {

        }
    }
}
