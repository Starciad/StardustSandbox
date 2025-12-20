using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("GameplaySettings")]
    public readonly struct GameplaySettings : ISettingsModule
    {
        [XmlElement("PreviewAreaColorR", typeof(byte))]
        public readonly byte PreviewAreaColorR { get; init; }

        [XmlElement("PreviewAreaColorG", typeof(byte))]
        public readonly byte PreviewAreaColorG { get; init; }

        [XmlElement("PreviewAreaColorB", typeof(byte))]
        public readonly byte PreviewAreaColorB { get; init; }

        [XmlElement("PreviewAreaColorA", typeof(byte))]
        public readonly byte PreviewAreaColorA { get; init; }

        [XmlElement("ShowGrid", typeof(bool))]
        public readonly bool ShowGrid { get; init; }

        [XmlElement("GridOpacity", typeof(byte))]
        public readonly byte GridOpacity { get; init; }

        [XmlElement("ShowTemperatureColorVariations", typeof(bool))]
        public readonly bool ShowTemperatureColorVariations { get; init; }

        [XmlIgnore]
        public Color PreviewAreaColor
        {
            readonly get => new(this.PreviewAreaColorR, this.PreviewAreaColorG, this.PreviewAreaColorB, this.PreviewAreaColorA);

            init
            {
                this.PreviewAreaColorR = value.R;
                this.PreviewAreaColorG = value.G;
                this.PreviewAreaColorB = value.B;
                this.PreviewAreaColorA = value.A;
            }
        }

        public GameplaySettings()
        {
            this.PreviewAreaColor = AAP64ColorPalette.White;
            this.PreviewAreaColorA = 25;
            this.ShowGrid = true;
            this.GridOpacity = 124;
            this.ShowTemperatureColorVariations = true;
        }
    }
}
