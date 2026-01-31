/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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

namespace StardustSandbox.Core.Colors.Palettes
{
    /// <summary>
    /// Provides the AAP-64 color palette as static <see cref="Color"/> properties.
    /// </summary>
    internal static class AAP64ColorPalette
    {
        #region Palette Colors

        /// <summary>
        /// Very dark gray. Hex: #060608
        /// </summary>
        internal static Color DarkGray => new(6, 6, 8, 255);

        /// <summary>
        /// Charcoal gray. Hex: #141013
        /// </summary>
        internal static Color Charcoal => new(20, 16, 19, 255);

        /// <summary>
        /// Deep maroon. Hex: #3b1725
        /// </summary>
        internal static Color Maroon => new(59, 23, 37, 255);

        /// <summary>
        /// Dark red. Hex: #73172d
        /// </summary>
        internal static Color DarkRed => new(115, 23, 45, 255);

        /// <summary>
        /// Crimson red. Hex: #b4202a
        /// </summary>
        internal static Color Crimson => new(180, 32, 42, 255);

        /// <summary>
        /// Orange red. Hex: #df3e23
        /// </summary>
        internal static Color OrangeRed => new(223, 62, 35, 255);

        /// <summary>
        /// Bright orange. Hex: #fa6a0a
        /// </summary>
        internal static Color Orange => new(250, 106, 10, 255);

        /// <summary>
        /// Amber yellow. Hex: #f9a31b
        /// </summary>
        internal static Color Amber => new(249, 163, 27, 255);

        /// <summary>
        /// Gold yellow. Hex: #ffd541
        /// </summary>
        internal static Color Gold => new(255, 213, 65, 255);

        /// <summary>
        /// Lemon yellow. Hex: #fffc40
        /// </summary>
        internal static Color LemonYellow => new(255, 252, 64, 255);

        /// <summary>
        /// Lime green. Hex: #d6f264
        /// </summary>
        internal static Color LimeGreen => new(214, 242, 100, 255);

        /// <summary>
        /// Grass green. Hex: #9cdb43
        /// </summary>
        internal static Color GrassGreen => new(156, 219, 67, 255);

        /// <summary>
        /// Forest green. Hex: #59c135
        /// </summary>
        internal static Color ForestGreen => new(89, 193, 53, 255);

        /// <summary>
        /// Emerald green. Hex: #14a02e
        /// </summary>
        internal static Color EmeraldGreen => new(20, 160, 46, 255);

        /// <summary>
        /// Dark green. Hex: #1a7a3e
        /// </summary>
        internal static Color DarkGreen => new(26, 122, 62, 255);

        /// <summary>
        /// Moss green. Hex: #24523b
        /// </summary>
        internal static Color MossGreen => new(36, 82, 59, 255);

        /// <summary>
        /// Dark teal. Hex: #122020
        /// </summary>
        internal static Color DarkTeal => new(18, 32, 32, 255);

        /// <summary>
        /// Navy blue. Hex: #143464
        /// </summary>
        internal static Color NavyBlue => new(20, 52, 100, 255);

        /// <summary>
        /// Royal blue. Hex: #285cc4
        /// </summary>
        internal static Color RoyalBlue => new(40, 92, 196, 255);

        /// <summary>
        /// Sky blue. Hex: #249fde
        /// </summary>
        internal static Color SkyBlue => new(36, 159, 222, 255);

        /// <summary>
        /// Cyan. Hex: #20d6c7
        /// </summary>
        internal static Color Cyan => new(32, 214, 199, 255);

        /// <summary>
        /// Mint. Hex: #a6fcdb
        /// </summary>
        internal static Color Mint => new(166, 252, 219, 255);

        /// <summary>
        /// Pure white. Hex: #ffffff
        /// </summary>
        internal static Color White => new(255, 255, 255, 255);

        /// <summary>
        /// Pale yellow. Hex: #fef3c0
        /// </summary>
        internal static Color PaleYellow => new(254, 243, 192, 255);

        /// <summary>
        /// Peach. Hex: #fad6b8
        /// </summary>
        internal static Color Peach => new(250, 214, 184, 255);

        /// <summary>
        /// Salmon. Hex: #f5a097
        /// </summary>
        internal static Color Salmon => new(245, 160, 151, 255);

        /// <summary>
        /// Rose. Hex: #e86a73
        /// </summary>
        internal static Color Rose => new(232, 106, 115, 255);

        /// <summary>
        /// Magenta. Hex: #bc4a9b
        /// </summary>
        internal static Color Magenta => new(188, 74, 155, 255);

        /// <summary>
        /// Violet. Hex: #793a80
        /// </summary>
        internal static Color Violet => new(121, 58, 128, 255);

        /// <summary>
        /// Purple gray. Hex: #403353
        /// </summary>
        internal static Color PurpleGray => new(64, 51, 83, 255);

        /// <summary>
        /// Dark purple. Hex: #242234
        /// </summary>
        internal static Color DarkPurple => new(36, 34, 52, 255);

        /// <summary>
        /// Cocoa brown. Hex: #221c1a
        /// </summary>
        internal static Color Cocoa => new(34, 28, 26, 255);

        /// <summary>
        /// Umber brown. Hex: #322b28
        /// </summary>
        internal static Color Umber => new(50, 43, 40, 255);

        /// <summary>
        /// Brown. Hex: #71413b
        /// </summary>
        internal static Color Brown => new(113, 65, 59, 255);

        /// <summary>
        /// Rust brown. Hex: #bb7547
        /// </summary>
        internal static Color Rust => new(187, 117, 71, 255);

        /// <summary>
        /// Sand. Hex: #dba463
        /// </summary>
        internal static Color Sand => new(219, 164, 99, 255);

        /// <summary>
        /// Tan. Hex: #f4d29c
        /// </summary>
        internal static Color Tan => new(244, 210, 156, 255);

        /// <summary>
        /// Light gray blue. Hex: #dae0ea
        /// </summary>
        internal static Color LightGrayBlue => new(218, 224, 234, 255);

        /// <summary>
        /// Steel blue. Hex: #b3b9d1
        /// </summary>
        internal static Color SteelBlue => new(179, 185, 209, 255);

        /// <summary>
        /// Slate blue. Hex: #8b93af
        /// </summary>
        internal static Color Slate => new(139, 147, 175, 255);

        /// <summary>
        /// Graphite gray. Hex: #6d758d
        /// </summary>
        internal static Color Graphite => new(109, 117, 141, 255);

        /// <summary>
        /// Gunmetal gray. Hex: #4a5462
        /// </summary>
        internal static Color Gunmetal => new(74, 84, 98, 255);

        /// <summary>
        /// Coal gray. Hex: #333941
        /// </summary>
        internal static Color Coal => new(51, 57, 65, 255);

        /// <summary>
        /// Dark brown. Hex: #422433
        /// </summary>
        internal static Color DarkBrown => new(66, 36, 51, 255);

        /// <summary>
        /// Burgundy. Hex: #5b3138
        /// </summary>
        internal static Color Burgundy => new(91, 49, 56, 255);

        /// <summary>
        /// Clay. Hex: #8e5252
        /// </summary>
        internal static Color Clay => new(142, 82, 82, 255);

        /// <summary>
        /// Terracotta. Hex: #ba756a
        /// </summary>
        internal static Color Terracotta => new(186, 117, 106, 255);

        /// <summary>
        /// Blush. Hex: #e9b5a3
        /// </summary>
        internal static Color Blush => new(233, 181, 163, 255);

        /// <summary>
        /// Pale blue. Hex: #e3e6ff
        /// </summary>
        internal static Color PaleBlue => new(227, 230, 255, 255);

        /// <summary>
        /// Lavender blue. Hex: #b9bffb
        /// </summary>
        internal static Color LavenderBlue => new(185, 191, 251, 255);

        /// <summary>
        /// Periwinkle blue. Hex: #849be4
        /// </summary>
        internal static Color Periwinkle => new(132, 155, 228, 255);

        /// <summary>
        /// Cerulean blue. Hex: #588dbe
        /// </summary>
        internal static Color Cerulean => new(88, 141, 190, 255);

        /// <summary>
        /// Teal gray. Hex: #477d85
        /// </summary>
        internal static Color TealGray => new(71, 125, 133, 255);

        /// <summary>
        /// Hunter green. Hex: #23674e
        /// </summary>
        internal static Color HunterGreen => new(35, 103, 78, 255);

        /// <summary>
        /// Pine green. Hex: #328464
        /// </summary>
        internal static Color PineGreen => new(50, 132, 100, 255);

        /// <summary>
        /// Seafoam green. Hex: #5daf8d
        /// </summary>
        internal static Color SeafoamGreen => new(93, 175, 141, 255);

        /// <summary>
        /// Mint green. Hex: #92dcba
        /// </summary>
        internal static Color MintGreen => new(146, 220, 186, 255);

        /// <summary>
        /// Aquamarine. Hex: #cdf7e2
        /// </summary>
        internal static Color Aquamarine => new(205, 247, 226, 255);

        /// <summary>
        /// Khaki. Hex: #e4d2aa
        /// </summary>
        internal static Color Khaki => new(228, 210, 170, 255);

        /// <summary>
        /// Beige. Hex: #c7b08b
        /// </summary>
        internal static Color Beige => new(199, 176, 139, 255);

        /// <summary>
        /// Sepia. Hex: #a08662
        /// </summary>
        internal static Color Sepia => new(160, 134, 98, 255);

        /// <summary>
        /// Coffee brown. Hex: #796755
        /// </summary>
        internal static Color Coffee => new(121, 103, 85, 255);

        /// <summary>
        /// Dark beige. Hex: #5a4e44
        /// </summary>
        internal static Color DarkBeige => new(90, 78, 68, 255);

        /// <summary>
        /// Dark taupe. Hex: #423934
        /// </summary>
        internal static Color DarkTaupe => new(66, 57, 52, 255);

        #endregion

        #region Utility Colors

        /// <summary>
        /// Utility color for hover state. Uses <see cref="LimeGreen"/>.
        /// </summary>
        internal static Color HoverColor => LimeGreen;

        /// <summary>
        /// Utility color for selected state. Uses <see cref="Orange"/>.
        /// </summary>
        internal static Color SelectedColor => Orange;

        #endregion
    }
}
