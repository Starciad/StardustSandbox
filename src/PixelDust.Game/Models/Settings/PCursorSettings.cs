using MessagePack;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.Models.Settings
{
    [MessagePackObject]
    public struct PCursorSettings
    {
        [IgnoreMember]
        public Color Color
        {
            readonly get => new(this.ColorR, this.ColorG, this.ColorB, this.ColorA);

            set
            {
                this.ColorR = value.R;
                this.ColorG = value.G;
                this.ColorB = value.B;
                this.ColorA = value.A;
            }
        }

        [IgnoreMember]
        public Color BackgroundColor
        {
            readonly get => new(this.BackgroundColorR, this.BackgroundColorG, this.BackgroundColorB, this.BackgroundColorA);

            set
            {
                this.BackgroundColorR = value.R;
                this.BackgroundColorG = value.G;
                this.BackgroundColorB = value.B;
                this.BackgroundColorA = value.A;
            }
        }

        [Key(0)] public float Scale { get; set; }
        [Key(1)] public byte ColorR { get; set; }
        [Key(2)] public byte ColorG { get; set; }
        [Key(3)] public byte ColorB { get; set; }
        [Key(4)] public byte ColorA { get; set; }
        [Key(5)] public byte BackgroundColorR { get; set; }
        [Key(6)] public byte BackgroundColorG { get; set; }
        [Key(7)] public byte BackgroundColorB { get; set; }
        [Key(8)] public byte BackgroundColorA { get; set; }

        public PCursorSettings()
        {
            this.Scale = 1f;
            this.Color = Color.White;
            this.BackgroundColor = Color.Red;
        }
    }
}