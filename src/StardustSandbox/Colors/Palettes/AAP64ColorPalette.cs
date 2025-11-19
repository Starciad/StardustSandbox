// =================================================================== //
//                                                                     //
// Credits to the creator of the color palette used in the project.    //
//                                                                     //
// AAP-64 Color Palette                                                //
// Created by Adigun Polack                                            //
// Source: https://twitter.com/AdigunPolack                            //
// Reference: http://pixeljoint.com/pixelart/119466.htm                //
//                                                                     //
// =================================================================== //

using Microsoft.Xna.Framework;

namespace StardustSandbox.Colors.Palettes
{
    internal static class AAP64ColorPalette
    {
        internal static Color DarkGray => new(6, 6, 8, 255); // #060608
        internal static Color Charcoal => new(20, 16, 19, 255); // #141013
        internal static Color Maroon => new(59, 23, 37, 255); // #3b1725
        internal static Color DarkRed => new(115, 23, 45, 255); // #73172d
        internal static Color Crimson => new(180, 32, 42, 255); // #b4202a
        internal static Color OrangeRed => new(223, 62, 35, 255); // #df3e23
        internal static Color Orange => new(250, 106, 10, 255); // #fa6a0a
        internal static Color Amber => new(249, 163, 27, 255); // #f9a31b
        internal static Color Gold => new(255, 213, 65, 255); // #ffd541
        internal static Color LemonYellow => new(255, 252, 64, 255); // #fffc40
        internal static Color LimeGreen => new(214, 242, 100, 255); // #d6f264
        internal static Color GrassGreen => new(156, 219, 67, 255); // #9cdb43
        internal static Color ForestGreen => new(89, 193, 53, 255); // #59c135
        internal static Color EmeraldGreen => new(20, 160, 46, 255); // #14a02e
        internal static Color DarkGreen => new(26, 122, 62, 255); // #1a7a3e
        internal static Color MossGreen => new(36, 82, 59, 255); // #24523b
        internal static Color DarkTeal => new(18, 32, 32, 255); // #122020
        internal static Color NavyBlue => new(20, 52, 100, 255); // #143464
        internal static Color RoyalBlue => new(40, 92, 196, 255); // #285cc4
        internal static Color SkyBlue => new(36, 159, 222, 255); // #249fde
        internal static Color Cyan => new(32, 214, 199, 255); // #20d6c7
        internal static Color Mint => new(166, 252, 219, 255); // #a6fcdb
        internal static Color White => new(255, 255, 255, 255); // #ffffff
        internal static Color PaleYellow => new(254, 243, 192, 255); // #fef3c0
        internal static Color Peach => new(250, 214, 184, 255); // #fad6b8
        internal static Color Salmon => new(245, 160, 151, 255); // #f5a097
        internal static Color Rose => new(232, 106, 115, 255); // #e86a73
        internal static Color Magenta => new(188, 74, 155, 255); // #bc4a9b
        internal static Color Violet => new(121, 58, 128, 255); // #793a80
        internal static Color PurpleGray => new(64, 51, 83, 255); // #403353
        internal static Color DarkPurple => new(36, 34, 52, 255); // #242234
        internal static Color Cocoa => new(34, 28, 26, 255); // #221c1a
        internal static Color Umber => new(50, 43, 40, 255); // #322b28
        internal static Color Brown => new(113, 65, 59, 255); // #71413b
        internal static Color Rust => new(187, 117, 71, 255); // #bb7547
        internal static Color Sand => new(219, 164, 99, 255); // #dba463
        internal static Color Tan => new(244, 210, 156, 255); // #f4d29c
        internal static Color LightGrayBlue => new(218, 224, 234, 255); // #dae0ea
        internal static Color SteelBlue => new(179, 185, 209, 255); // #b3b9d1
        internal static Color Slate => new(139, 147, 175, 255); // #8b93af
        internal static Color Graphite => new(109, 117, 141, 255); // #6d758d
        internal static Color Gunmetal => new(74, 84, 98, 255); // #4a5462
        internal static Color Coal => new(51, 57, 65, 255); // #333941
        internal static Color DarkBrown => new(66, 36, 51, 255); // #422433
        internal static Color Burgundy => new(91, 49, 56, 255); // #5b3138
        internal static Color Clay => new(142, 82, 82, 255); // #8e5252
        internal static Color Terracotta => new(186, 117, 106, 255); // #ba756a
        internal static Color Blush => new(233, 181, 163, 255); // #e9b5a3
        internal static Color PaleBlue => new(227, 230, 255, 255); // #e3e6ff
        internal static Color LavenderBlue => new(185, 191, 251, 255); // #b9bffb
        internal static Color Periwinkle => new(132, 155, 228, 255); // #849be4
        internal static Color Cerulean => new(88, 141, 190, 255); // #588dbe
        internal static Color TealGray => new(71, 125, 133, 255); // #477d85
        internal static Color HunterGreen => new(35, 103, 78, 255); // #23674e
        internal static Color PineGreen => new(50, 132, 100, 255); // #328464
        internal static Color SeafoamGreen => new(93, 175, 141, 255); // #5daf8d
        internal static Color MintGreen => new(146, 220, 186, 255); // #92dcba
        internal static Color Aquamarine => new(205, 247, 226, 255); // #cdf7e2
        internal static Color Khaki => new(228, 210, 170, 255); // #e4d2aa
        internal static Color Beige => new(199, 176, 139, 255); // #c7b08b
        internal static Color Sepia => new(160, 134, 98, 255); // #a08662
        internal static Color Coffee => new(121, 103, 85, 255); // #796755
        internal static Color DarkBeige => new(90, 78, 68, 255); // #5a4e44
        internal static Color DarkTaupe => new(66, 57, 52, 255); // #423934

        // ================================= //
        // Utilities

        internal static Color HoverColor => LimeGreen;
        internal static Color SelectedColor => Orange;
    }
}
