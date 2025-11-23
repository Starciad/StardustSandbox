using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

using System;

namespace StardustSandbox.AmbientSystem
{
    internal sealed class SkyHandler
    {
        internal bool IsActive { get; set; } = true;
        internal Texture2D SkyTexture => this.skyTexture;
        internal Effect GradientTransitionEffect => this.gradientTransitionEffect;

        private readonly Texture2D skyTexture = AssetDatabase.GetTexture(TextureIndex.BackgroundSky);
        private readonly Effect gradientTransitionEffect = AssetDatabase.GetEffect(EffectIndex.GradientTransition);

        private readonly GradientColorMap[] skyGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(3, 0, 0),  // Late Night
                Color1 = (AAP64ColorPalette.DarkPurple, AAP64ColorPalette.NavyBlue),
                Color2 = (AAP64ColorPalette.NavyBlue, AAP64ColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(3, 0, 0), // Late Night
                EndTime = new(6, 0, 0),  // Dawn
                Color1 = (AAP64ColorPalette.NavyBlue, AAP64ColorPalette.DarkTeal),
                Color2 = (AAP64ColorPalette.DarkTeal, AAP64ColorPalette.OrangeRed),
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(8, 0, 0),  // Early Morning
                Color1 = (AAP64ColorPalette.DarkTeal, AAP64ColorPalette.OrangeRed),
                Color2 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Orange),
            },

            new()
            {
                StartTime = new(8, 0, 0), // Early Morning
                EndTime = new(12, 0, 0), // Noon
                Color1 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Orange),
                Color2 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.LemonYellow),
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(15, 0, 0),  // Early Afternoon
                Color1 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.LemonYellow),
                Color2 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Gold),
            },

            new()
            {
                StartTime = new(15, 0, 0), // Early Afternoon
                EndTime = new(18, 0, 0),  // Dusk
                Color1 = (AAP64ColorPalette.SkyBlue, AAP64ColorPalette.Gold),
                Color2 = (AAP64ColorPalette.OrangeRed, AAP64ColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(20, 0, 0), // Evening
                Color1 = (AAP64ColorPalette.OrangeRed, AAP64ColorPalette.DarkTeal),
                Color2 = (AAP64ColorPalette.DarkTeal, AAP64ColorPalette.NavyBlue),
            },

            new()
            {
                StartTime = new(20, 0, 0), // Evening
                EndTime = new(23, 59, 59), // Midnight
                Color1 = (AAP64ColorPalette.DarkTeal, AAP64ColorPalette.NavyBlue),
                Color2 = (AAP64ColorPalette.DarkPurple, AAP64ColorPalette.NavyBlue),
            },
        ];

        private readonly GradientColorMap[] backgroundGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(3, 0, 0),  // Late Night
                Color1 = (new Color(60, 40, 90, 255), new Color(30, 30, 60, 255)),
                Color2 = (new Color(30, 30, 60, 255), new Color(20, 25, 50, 255)),
            },

            new()
            {
                StartTime = new(3, 0, 0), // Late Night
                EndTime = new(6, 0, 0),  // Dawn
                Color1 = (new Color(30, 30, 60, 255), new Color(20, 25, 50, 255)),
                Color2 = (new Color(70, 80, 100, 255), new Color(100, 70, 50, 255)),
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(8, 0, 0),  // Early Morning
                Color1 = (new Color(70, 80, 100, 255), new Color(100, 70, 50, 255)),
                Color2 = (new Color(110, 130, 170, 255), new Color(190, 160, 120, 255)),
            },

            new()
            {
                StartTime = new(8, 0, 0), // Early Morning
                EndTime = new(12, 0, 0), // Noon
                Color1 = (new Color(110, 130, 170, 255), new Color(190, 160, 120, 255)),
                Color2 = (new Color(160, 200, 250, 255), new Color(220, 180, 130, 255)),
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(15, 0, 0),  // Early Afternoon
                Color1 = (new Color(160, 200, 250, 255), new Color(220, 180, 130, 255)),
                Color2 = (new Color(200, 220, 250, 255), new Color(250, 200, 130, 255)),
            },

            new()
            {
                StartTime = new(15, 0, 0), // Early Afternoon
                EndTime = new(18, 0, 0),  // Dusk
                Color1 = (new Color(200, 220, 250, 255), new Color(250, 200, 130, 255)),
                Color2 = (new Color(220, 130, 100, 255), new Color(120, 70, 50, 255)),
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(20, 0, 0), // Evening
                Color1 = (new Color(220, 130, 100, 255), new Color(120, 70, 50, 255)),
                Color2 = (new Color(80, 40, 50, 255), new Color(40, 30, 60, 255)),
            },

            new()
            {
                StartTime = new(20, 0, 0), // Evening
                EndTime = new(23, 59, 59), // Midnight
                Color1 = (new Color(80, 40, 50, 255), new Color(40, 30, 60, 255)),
                Color2 = (new Color(60, 40, 90, 255), new Color(30, 30, 60, 255)),
            },
        ];

        internal GradientColorMap GetBackgroundGradientByTime(TimeSpan currentTime)
        {
            return Array.Find(this.backgroundGradientColorMap, x =>
            {
                return currentTime >= x.StartTime && currentTime < x.EndTime;
            });
        }

        internal GradientColorMap GetSkyGradientByTime(TimeSpan currentTime)
        {
            return Array.Find(this.skyGradientColorMap, x =>
            {
                return currentTime >= x.StartTime && currentTime < x.EndTime;
            });
        }
    }
}
