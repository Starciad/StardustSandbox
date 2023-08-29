using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;

using System;
using System.Collections.Generic;

namespace PixelDust.Core.TileSet
{
    [Flags]
    internal enum BitwiseTileIndex
    {
        None = 0,
        North = 1,
        Northeast = 2,
        East = 4,
        Southeast = 8,
        South = 16,
        Soutwest = 32,
        West = 64,
        Northwest = 128,
    }

    public sealed class PTileSet
    {
        internal static byte Size => PWorld.Scale / 2;
        internal Texture2D Texture { get; set; }

        private static readonly Rectangle[] rules = new Rectangle[]
        {
            // 0
            // X##
            // ###
            // ###
            new(new(0 * Size, 0 * Size), new(Size)),

            // 1
            // #X#
            // ###
            // ###
            new(new(1 * Size, 0 * Size), new(Size)),

            // 2
            // ##X
            // ###
            // ###
            new(new(2 * Size, 0 * Size), new(Size)),
            
            // 3
            // ###
            // X##
            // ###
            new(new(0 * Size, 1 * Size), new(Size)),
            
            // 4
            // ###
            // #X#
            // ###
            new(new(1 * Size, 1 * Size), new(Size)),
            
            // 5
            // ###
            // ##X
            // ###
            new(new(2 * Size, 1 * Size), new(Size)),
            
            // 6
            // ###
            // ###
            // X##
            new(new(0 * Size, 2 * Size), new(Size)),
            
            // 7
            // ###
            // ###
            // #X#
            new(new(1 * Size, 2 * Size), new(Size)),
            
            // 8
            // ###
            // ###
            // ##X
            new(new(2 * Size, 2 * Size), new(Size)),

            // 9
            // X#
            // ##
            new(new(3 * Size, 0 * Size), new(Size)),

            // 10
            // #X
            // ##
            new(new(4 * Size, 0 * Size), new(Size)),

            // 11
            // ##
            // X#
            new(new(3 * Size, 1 * Size), new(Size)),

            // 12
            // ##
            // #X
            new(new(4 * Size, 1 * Size), new(Size)),
        };

        public PTileSet(Texture2D texture)
        {
            Texture = texture;
        }

        internal PTileSprite[] BuildTileSpriteByContext(PElementContext context)
        {
            List<PTileSprite> PTile = new(4);
            for (int i = 0; i < 4; i++)
            {
                PTileSprite temp = GetTileSprite(context, i);
                if (temp != null)
                {
                    PTile.Add(temp);
                }
            }

            if (PTile.Count == 0)
                return Array.Empty<PTileSprite>();

            return PTile.ToArray();
        }
        private PTileSprite GetTileSprite(PElementContext context, int targetId)
        {
            BitwiseTileIndex index = BitwiseTileIndex.None;
            Vector2 pos;

            PTileSprite sprite = new(Texture);

            // X#
            // ##
            if (targetId == 0)
            {
                pos = new(context.Position.X * PWorld.Scale, context.Position.Y * PWorld.Scale);

                if (!context.IsEmpty(new(context.Position.X    , context.Position.Y - 1))) index |= BitwiseTileIndex.North;
                if (!context.IsEmpty(new(context.Position.X - 1, context.Position.Y    ))) index |= BitwiseTileIndex.West;
                if (!context.IsEmpty(new(context.Position.X - 1, context.Position.Y - 1))) index |= BitwiseTileIndex.Northwest;

                return (int)index switch
                {
                    // Nothing
                    0 => sprite.Build(pos, rules[0]),
                    128 => sprite.Build(pos, rules[0]),

                    // Rules
                    1 => sprite.Build(pos, rules[3]),
                    129 => sprite.Build(pos, rules[3]),
                    64 => sprite.Build(pos, rules[1]),
                    192 => sprite.Build(pos, rules[1]),

                    // Curve
                    65 => sprite.Build(pos, rules[12]),

                    // All
                    193 => sprite.Build(pos, rules[4]),

                    // Others
                    _ => sprite.Build(pos, rules[4]),
                };
            }

            // #X
            // ##
            if (targetId == 1)
            {
                pos = new(context.Position.X * PWorld.Scale + Size, context.Position.Y * PWorld.Scale);

                if (!context.IsEmpty(new(context.Position.X    , context.Position.Y - 1))) index |= BitwiseTileIndex.North;
                if (!context.IsEmpty(new(context.Position.X + 1, context.Position.Y    ))) index |= BitwiseTileIndex.East;
                if (!context.IsEmpty(new(context.Position.X + 1, context.Position.Y - 1))) index |= BitwiseTileIndex.Northeast;

                return (int)index switch
                {
                    // Nothing
                    0 => sprite.Build(pos, rules[2]),
                    2 => sprite.Build(pos, rules[2]),

                    // Rules
                    1 => sprite.Build(pos, rules[5]),
                    3 => sprite.Build(pos, rules[5]),
                    4 => sprite.Build(pos, rules[1]),
                    6 => sprite.Build(pos, rules[1]),

                    // Curve
                    5 => sprite.Build(pos, rules[11]),

                    // All
                    7 => sprite.Build(pos, rules[4]),

                    // Others
                    _ => sprite.Build(pos, rules[4]),
                };
            }

            // ##
            // X#
            if (targetId == 2)
            {
                pos = new(context.Position.X * PWorld.Scale, context.Position.Y * PWorld.Scale + Size);

                if (!context.IsEmpty(new(context.Position.X    , context.Position.Y + 1))) index |= BitwiseTileIndex.South;
                if (!context.IsEmpty(new(context.Position.X - 1, context.Position.Y    ))) index |= BitwiseTileIndex.West;
                if (!context.IsEmpty(new(context.Position.X - 1, context.Position.Y + 1))) index |= BitwiseTileIndex.Soutwest;

                return (int)index switch
                {
                    // Nothing
                    0 => sprite.Build(pos, rules[6]),
                    32 => sprite.Build(pos, rules[6]),

                    // Rules
                    16 => sprite.Build(pos, rules[3]),
                    48 => sprite.Build(pos, rules[3]),
                    64 => sprite.Build(pos, rules[7]),
                    96 => sprite.Build(pos, rules[7]),

                    // Curve
                    80 => sprite.Build(pos, rules[10]),

                    // All
                    112 => sprite.Build(pos, rules[4]),

                    // Others
                    _ => sprite.Build(pos, rules[4]),
                };
            }

            // ##
            // #X
            if (targetId == 3)
            {
                pos = new(context.Position.X * PWorld.Scale + Size, context.Position.Y * PWorld.Scale + Size);

                if (!context.IsEmpty(new(context.Position.X    , context.Position.Y + 1))) index |= BitwiseTileIndex.South;
                if (!context.IsEmpty(new(context.Position.X + 1, context.Position.Y    ))) index |= BitwiseTileIndex.East;
                if (!context.IsEmpty(new(context.Position.X + 1, context.Position.Y + 1))) index |= BitwiseTileIndex.Southeast;

                return (int)index switch
                {
                    // Nothing
                    0 => sprite.Build(pos, rules[8]),
                    8 => sprite.Build(pos, rules[8]),

                    // Rules
                    4 => sprite.Build(pos, rules[7]),
                    12 => sprite.Build(pos, rules[7]),
                    16 => sprite.Build(pos, rules[5]),
                    24 => sprite.Build(pos, rules[5]),

                    // Curve
                    20 => sprite.Build(pos, rules[9]),

                    // All
                    28 => sprite.Build(pos, rules[4]),

                    // Others
                    _ => sprite.Build(pos, rules[4]),
                };
            }

            return default;
        }
    }
}