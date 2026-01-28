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

using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Enums.Tools;
using StardustSandbox.Core.Localization;

using System;

namespace StardustSandbox.Core.Databases
{
    internal static class CatalogDatabase
    {
        internal static Category[] Categories => categories;
        internal static int CategoryLength => categoryLength;
        internal static int ItemLength => itemLength;

        private static int categoryLength = 0;
        private static int itemLength = 0;

        private static Category[] categories;

        private static bool isLoaded;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(CatalogDatabase)} has already been loaded.");
            }

            CreateCatalogStructure();
            SetParentReferences();
            CalculateCatalogLengths();

            isLoaded = true;
        }

        private static void CreateCatalogStructure()
        {
            categories = [
                // [0] Elements
                new(
                    Localization_Catalog.Category_Elements_Name,
                    Localization_Catalog.Category_Elements_Description,
                    TextureIndex.IconElements,
                    sourceRectangle: new(32, 0, 32, 32),
                    [
                        // [0] Powders
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Powders_Name,
                            description: Localization_Catalog.Subcategory_Elements_Powders_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(32, 0, 32, 32),
                            [
                                // [0] Dirt
                                new(
                                    contentIndex: (int)ElementIndex.Dirt,
                                    name: Localization_Elements.Solid_Movable_Dirt_Name,
                                    description: Localization_Elements.Solid_Movable_Dirt_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 0, 32, 32)
                                ),

                                // [1] Mud
                                new(
                                    contentIndex: (int)ElementIndex.Mud,
                                    name: Localization_Elements.Solid_Movable_Mud_Name,
                                    description: Localization_Elements.Solid_Movable_Mud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 0, 32, 32)
                                ),

                                // [2] Stone
                                new(
                                    contentIndex: (int)ElementIndex.Stone,
                                    name: Localization_Elements.Solid_Movable_Stone_Name,
                                    description: Localization_Elements.Solid_Movable_Stone_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 0, 32, 32)
                                ),

                                // [3] Grass
                                new(
                                    contentIndex: (int)ElementIndex.Grass,
                                    name: Localization_Elements.Solid_Movable_Grass_Name,
                                    description: Localization_Elements.Solid_Movable_Grass_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 0, 32, 32)
                                ),

                                // [4] Ice
                                new(
                                    contentIndex: (int)ElementIndex.Ice,
                                    name: Localization_Elements.Solid_Movable_Ice_Name,
                                    description: Localization_Elements.Solid_Movable_Ice_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 0, 32, 32)
                                ),

                                // [5] Sand
                                new(
                                    contentIndex: (int)ElementIndex.Sand,
                                    name: Localization_Elements.Solid_Movable_Sand_Name,
                                    description: Localization_Elements.Solid_Movable_Sand_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 0, 32, 32)
                                ),

                                // [6] Snow
                                new(
                                    contentIndex: (int)ElementIndex.Snow,
                                    name: Localization_Elements.Solid_Movable_Snow_Name,
                                    description: Localization_Elements.Solid_Movable_Snow_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 0, 32, 32)
                                ),

                                // [7] Corruption
                                new(
                                    contentIndex: (int)ElementIndex.MovableCorruption,
                                    name: Localization_Elements.Solid_Movable_Corruption_Name,
                                    description: Localization_Elements.Solid_Movable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 0, 32, 32)
                                ),

                                // [8] Salt
                                new(
                                    contentIndex: (int)ElementIndex.Salt,
                                    name: Localization_Elements.Solid_Movable_Salt_Name,
                                    description: Localization_Elements.Solid_Movable_Salt_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 64, 32, 32)
                                ),

                                // [9] Ash
                                new(
                                    contentIndex: (int)ElementIndex.Ash,
                                    name: Localization_Elements.Solid_Movable_Ash_Name,
                                    description: Localization_Elements.Solid_Movable_Ash_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 96, 32, 32)
                                ),

                                // [10] Fertile Soil
                                new(
                                    contentIndex: (int)ElementIndex.FertileSoil,
                                    name: Localization_Elements.Solid_Movable_FertileSoil_Name,
                                    description: Localization_Elements.Solid_Movable_FertileSoil_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 128, 32, 32)
                                )
                            ]
                        ),

                        // [1] Liquids
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Liquids_Name,
                            description: Localization_Catalog.Subcategory_Elements_Liquids_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(96, 0, 32, 32),
                            [
                                // [0] Water
                                new(
                                    contentIndex: (int)ElementIndex.Water,
                                    name: Localization_Elements.Liquid_Water_Name,
                                    description: Localization_Elements.Liquid_Water_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 0, 32, 32)
                                ),

                                // [1] Lava
                                new(
                                    contentIndex: (int)ElementIndex.Lava,
                                    name: Localization_Elements.Liquid_Lava_Name,
                                    description: Localization_Elements.Liquid_Lava_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 0, 32, 32)
                                ),

                                // [2] Acid
                                new(
                                    contentIndex: (int)ElementIndex.Acid,
                                    name: Localization_Elements.Liquid_Acid_Name,
                                    description: Localization_Elements.Liquid_Acid_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 32, 32, 32)
                                ),

                                // [3] Corruption
                                new(
                                    contentIndex: (int)ElementIndex.LiquidCorruption,
                                    name: Localization_Elements.Liquid_Corruption_Name,
                                    description: Localization_Elements.Liquid_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 32, 32, 32)
                                ),

                                // [4] Oil
                                new(
                                    contentIndex: (int)ElementIndex.Oil,
                                    name: Localization_Elements.Liquid_Oil_Name,
                                    description: Localization_Elements.Liquid_Oil_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 64, 32, 32)
                                ),

                                // [5] Saltwater
                                new(
                                    contentIndex: (int)ElementIndex.Saltwater,
                                    name: Localization_Elements.Liquid_Saltwater_Name,
                                    description: Localization_Elements.Liquid_Saltwater_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 64, 32, 32)
                                ),

                                // [6] Black Paint
                                new(
                                    contentIndex: (int)ElementIndex.BlackPaint,
                                    name: Localization_Elements.Liquid_BlackPaint_Name,
                                    description: Localization_Elements.Liquid_BlackPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 160, 32, 32)
                                ),

                                // [7] White Paint
                                new(
                                    contentIndex: (int)ElementIndex.WhitePaint,
                                    name: Localization_Elements.Liquid_WhitePaint_Name,
                                    description: Localization_Elements.Liquid_WhitePaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 160, 32, 32)
                                ),

                                // [8] Red Paint
                                new(
                                    contentIndex: (int)ElementIndex.RedPaint,
                                    name: Localization_Elements.Liquid_RedPaint_Name,
                                    description: Localization_Elements.Liquid_RedPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 160, 32, 32)
                                ),

                                // [9] Orange Paint
                                new(
                                    contentIndex: (int)ElementIndex.OrangePaint,
                                    name: Localization_Elements.Liquid_OrangePaint_Name,
                                    description: Localization_Elements.Liquid_OrangePaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 160, 32, 32)
                                ),

                                // [10] Yellow Paint
                                new(
                                    contentIndex: (int)ElementIndex.YellowPaint,
                                    name: Localization_Elements.Liquid_YellowPaint_Name,
                                    description: Localization_Elements.Liquid_YellowPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 160, 32, 32)
                                ),

                                // [11] Green Paint
                                new(
                                    contentIndex: (int)ElementIndex.GreenPaint,
                                    name: Localization_Elements.Liquid_GreenPaint_Name,
                                    description: Localization_Elements.Liquid_GreenPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 192, 32, 32)
                                ),

                                // [12] Blue Paint
                                new(
                                    contentIndex: (int)ElementIndex.BluePaint,
                                    name: Localization_Elements.Liquid_BluePaint_Name,
                                    description: Localization_Elements.Liquid_BluePaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 192, 32, 32)
                                ),

                                // [13] Gray Paint
                                new(
                                    contentIndex: (int)ElementIndex.GrayPaint,
                                    name: Localization_Elements.Liquid_GrayPaint_Name,
                                    description: Localization_Elements.Liquid_GrayPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 192, 32, 32)
                                ),

                                // [14] Violet Paint
                                new(
                                    contentIndex: (int)ElementIndex.VioletPaint,
                                    name: Localization_Elements.Liquid_VioletPaint_Name,
                                    description: Localization_Elements.Liquid_VioletPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 192, 32, 32)
                                ),

                                // [15] Brown Paint
                                new(
                                    contentIndex: (int)ElementIndex.BrownPaint,
                                    name: Localization_Elements.Liquid_BrownPaint_Name,
                                    description: Localization_Elements.Liquid_BrownPaint_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 192, 32, 32)
                                ),

                                // [16] Mercury
                                new(
                                    contentIndex: (int)ElementIndex.Mercury,
                                    name: Localization_Elements.Liquid_Mercury_Name,
                                    description: Localization_Elements.Liquid_Mercury_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 192, 32, 32)
                                ),
                            ]
                        ),

                        // [2] Gases
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Gases_Name,
                            description: Localization_Catalog.Subcategory_Elements_Gases_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(288, 32, 32, 32),
                            [
                                // [0] Corruption
                                new(
                                    contentIndex: (int)ElementIndex.GasCorruption,
                                    name: Localization_Elements.Gas_Corruption_Name,
                                    description: Localization_Elements.Gas_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 32, 32, 32)
                                ),

                                // [1] Steam
                                new(
                                    contentIndex: (int)ElementIndex.Steam,
                                    name: Localization_Elements.Gas_Steam_Name,
                                    description: Localization_Elements.Gas_Steam_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 32, 32, 32)
                                ),

                                // [2] Smoke
                                new(
                                    contentIndex: (int)ElementIndex.Smoke,
                                    name: Localization_Elements.Gas_Smoke_Name,
                                    description: Localization_Elements.Gas_Smoke_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 32, 32, 32)
                                ),

                                // [3] Anti-Corruption
                                new(
                                    contentIndex: (int)ElementIndex.AntiCorruption,
                                    name: Localization_Elements.Gas_AntiCorruption_Name,
                                    description: Localization_Elements.Gas_AntiCorruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 96, 32, 32)
                                ),

                                // [4] Cloud
                                new(
                                    contentIndex: (int)ElementIndex.Cloud,
                                    name: Localization_Elements.Gas_Cloud_Name,
                                    description: Localization_Elements.Gas_Cloud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 128, 32, 32)
                                ),

                                // [5] Charged Cloud
                                new(
                                    contentIndex: (int)ElementIndex.ChargedCloud,
                                    name: Localization_Elements.Gas_ChargedCloud_Name,
                                    description: Localization_Elements.Gas_ChargedCloud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 128, 32, 32)
                                ),

                                // [6] Liquefied Petroleum Gas
                                new(
                                    contentIndex: (int)ElementIndex.LiquefiedPetroleumGas,
                                    name: Localization_Elements.Gas_LiquefiedPetroleumGas_Name,
                                    description: Localization_Elements.Gas_LiquefiedPetroleumGas_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 128, 32, 32)
                                ),
                            ]
                        ),

                        // [3] Solids
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Solids_Name,
                            description: Localization_Catalog.Subcategory_Elements_Solids_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(128, 32, 32, 32),
                            [
                                // [0] Glass
                                new(
                                    contentIndex: (int)ElementIndex.Glass,
                                    name: Localization_Elements.Solid_Immovable_Glass_Name,
                                    description: Localization_Elements.Solid_Immovable_Glass_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 32, 32, 32)
                                ),

                                // [1] Iron
                                new(
                                    contentIndex: (int)ElementIndex.Iron,
                                    name: Localization_Elements.Solid_Immovable_Iron_Name,
                                    description: Localization_Elements.Solid_Immovable_Iron_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 32, 32, 32)
                                ),

                                // [2] Wall
                                new(
                                    contentIndex: (int)ElementIndex.Wall,
                                    name: Localization_Elements.Solid_Immovable_Wall_Name,
                                    description: Localization_Elements.Solid_Immovable_Wall_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 32, 32, 32)
                                ),

                                // [3] Wood
                                new(
                                    contentIndex: (int)ElementIndex.Wood,
                                    name: Localization_Elements.Solid_Immovable_Wood_Name,
                                    description: Localization_Elements.Solid_Immovable_Wood_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 32, 32, 32)
                                ),

                                // [4] Corruption
                                new(
                                    contentIndex: (int)ElementIndex.ImmovableCorruption,
                                    name: Localization_Elements.Solid_Immovable_Corruption_Name,
                                    description: Localization_Elements.Solid_Immovable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 32, 32, 32)
                                ),

                                // [5] Red Brick
                                new(
                                    contentIndex: (int)ElementIndex.RedBrick,
                                    name: Localization_Elements.Solid_Immovable_RedBrick_Name,
                                    description: Localization_Elements.Solid_Immovable_RedBrick_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 32, 32, 32)
                                ),

                                // [6] Tree Leaf
                                new(
                                    contentIndex: (int)ElementIndex.TreeLeaf,
                                    name: Localization_Elements.Solid_Immovable_TreeLeaf_Name,
                                    description: Localization_Elements.Solid_Immovable_TreeLeaf_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 64, 32, 32)
                                ),

                                // [7] Mounting Block
                                new(
                                    contentIndex: (int)ElementIndex.MountingBlock,
                                    name: Localization_Elements.Solid_Immovable_MountingBlock_Name,
                                    description: Localization_Elements.Solid_Immovable_MountingBlock_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 64, 32, 32)
                                ),

                                // [9] Dry Sponge
                                new(
                                    contentIndex: (int)ElementIndex.DrySponge,
                                    name: Localization_Elements.Solid_Immovable_DrySponge_Name,
                                    description: Localization_Elements.Solid_Immovable_DrySponge_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 96, 32, 32)
                                ),

                                // [10] Wet Sponge
                                new(
                                    contentIndex: (int)ElementIndex.WetSponge,
                                    name: Localization_Elements.Solid_Immovable_WetSponge_Name,
                                    description: Localization_Elements.Solid_Immovable_WetSponge_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 96, 32, 32)
                                ),

                                // [11] Gold
                                new(
                                    contentIndex: (int)ElementIndex.Gold,
                                    name: Localization_Elements.Solid_Immovable_Gold_Name,
                                    description: Localization_Elements.Solid_Immovable_Gold_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 96, 32, 32)
                                ),

                                // [12] Dry Black Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryBlackWool,
                                    name: Localization_Elements.Solid_Immovable_DryBlackWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBlackWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 160, 32, 32)
                                ),

                                // [13] Dry White Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryWhiteWool,
                                    name: Localization_Elements.Solid_Immovable_DryWhiteWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryWhiteWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 160, 32, 32)
                                ),

                                // [14] Dry Red Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryRedWool,
                                    name: Localization_Elements.Solid_Immovable_DryRedWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryRedWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 160, 32, 32)
                                ),

                                // [15] Dry Orange Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryOrangeWool,
                                    name: Localization_Elements.Solid_Immovable_DryOrangeWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryOrangeWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 160, 32, 32)
                                ),

                                // [16] Dry Yellow Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryYellowWool,
                                    name: Localization_Elements.Solid_Immovable_DryYellowWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryYellowWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 160, 32, 32)
                                ),

                                // [17] Dry Green Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryGreenWool,
                                    name: Localization_Elements.Solid_Immovable_DryGreenWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryGreenWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 192, 32, 32)
                                ),

                                // [18] Dry Gray Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryGrayWool,
                                    name: Localization_Elements.Solid_Immovable_DryGrayWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryGrayWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 192, 32, 32)
                                ),

                                // [19] Dry Blue Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryBlueWool,
                                    name: Localization_Elements.Solid_Immovable_DryBlueWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBlueWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 192, 32, 32)
                                ),

                                // [20] Dry Violet Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryVioletWool,
                                    name: Localization_Elements.Solid_Immovable_DryVioletWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryVioletWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 192, 32, 32)
                                ),

                                // [21] Dry Brown Wool
                                new(
                                    contentIndex: (int)ElementIndex.DryBrownWool,
                                    name: Localization_Elements.Solid_Immovable_DryBrownWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBrownWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 192, 32, 32)
                                ),

                                // [22] Wet Black Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetBlackWool,
                                    name: Localization_Elements.Solid_Immovable_WetBlackWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBlackWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 224, 32, 32)
                                ),

                                // [23] Wet White Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetWhiteWool,
                                    name: Localization_Elements.Solid_Immovable_WetWhiteWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetWhiteWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 224, 32, 32)
                                ),

                                // [24] Wet Red Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetRedWool,
                                    name: Localization_Elements.Solid_Immovable_WetRedWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetRedWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 224, 32, 32)
                                ),

                                // [25] Wet Orange Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetOrangeWool,
                                    name: Localization_Elements.Solid_Immovable_WetOrangeWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetOrangeWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 224, 32, 32)
                                ),

                                // [26] Wet Yellow Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetYellowWool,
                                    name: Localization_Elements.Solid_Immovable_WetYellowWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetYellowWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 224, 32, 32)
                                ),

                                // [27] Wet Green Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetGreenWool,
                                    name: Localization_Elements.Solid_Immovable_WetGreenWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetGreenWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 256, 32, 32)
                                ),

                                // [28] Wet Gray Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetGrayWool,
                                    name: Localization_Elements.Solid_Immovable_WetGrayWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetGrayWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 256, 32, 32)
                                ),

                                // [29] Wet Blue Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetBlueWool,
                                    name: Localization_Elements.Solid_Immovable_WetBlueWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBlueWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 256, 32, 32)
                                ),

                                // [30] Wet Violet Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetVioletWool,
                                    name: Localization_Elements.Solid_Immovable_WetVioletWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetVioletWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 256, 32, 32)
                                ),

                                // [31] Wet Brown Wool
                                new(
                                    contentIndex: (int)ElementIndex.WetBrownWool,
                                    name: Localization_Elements.Solid_Immovable_WetBrownWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBrownWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 256, 32, 32)
                                ),

                                // [32] Obsidian
                                new(
                                    contentIndex: (int)ElementIndex.Obsidian,
                                    name: Localization_Elements.Solid_Immovable_Obsidian_Name,
                                    description: Localization_Elements.Solid_Immovable_Obsidian_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 160, 32, 32)
                                ),
                            ]
                        ),

                        // [4] Energies
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Energies_Name,
                            description: Localization_Catalog.Subcategory_Elements_Energies_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(64, 64, 32, 32),
                            [
                                // [0] Fire
                                new(
                                    contentIndex: (int)ElementIndex.Fire,
                                    name: Localization_Elements.Energy_Fire_Name,
                                    description: Localization_Elements.Energy_Fire_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 64, 32, 32)
                                ),

                                // [1] Lightning
                                new(
                                    contentIndex: (int)ElementIndex.LightningHead,
                                    name: Localization_Elements.Energy_Lightning_Name,
                                    description: Localization_Elements.Energy_Lightning_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 128, 32, 32)
                                ),

                                // [2] Electricity
                                new(
                                    contentIndex: (int)ElementIndex.Electricity,
                                    name: Localization_Elements.Energy_Electricity_Name,
                                    description: Localization_Elements.Energy_Electricity_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 0, 32, 32)
                                ),
                            ]
                        ),

                        // [5] Explosives
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Explosives_Name,
                            description: Localization_Catalog.Subcategory_Elements_Explosives_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(0, 96, 32, 32),
                            [
                                // [0] Bomb
                                new(
                                    contentIndex: (int)ElementIndex.Bomb,
                                    name: Localization_Elements.Solid_Movable_Bomb_Name,
                                    description: Localization_Elements.Solid_Movable_Bomb_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 64, 32, 32)
                                ),

                                // [1] Dynamite
                                new(
                                    contentIndex: (int)ElementIndex.Dynamite,
                                    name: Localization_Elements.Solid_Movable_Dynamite_Name,
                                    description: Localization_Elements.Solid_Movable_Dynamite_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 64, 32, 32)
                                ),

                                // [2] TNT
                                new(
                                    contentIndex: (int)ElementIndex.Tnt,
                                    name: Localization_Elements.Solid_Movable_TNT_Name,
                                    description: Localization_Elements.Solid_Movable_TNT_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 96, 32, 32)
                                ),

                                // [3] Gunpowder
                                new(
                                    contentIndex: (int)ElementIndex.Gunpowder,
                                    name: Localization_Elements.Solid_Movable_Gunpowder_Name,
                                    description: Localization_Elements.Solid_Movable_Gunpowder_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 128, 32, 32)
                                ),
                            ]
                        ),

                        // [6] Technologies
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Technologies_Name,
                            description: Localization_Catalog.Subcategory_Elements_Technologies_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(128, 96, 32, 32),
                            [
                                // [0] Heater
                                new(
                                    contentIndex: (int)ElementIndex.Heater,
                                    name: Localization_Elements.Solid_Immovable_Heater_Name,
                                    description: Localization_Elements.Solid_Immovable_Heater_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 96, 32, 32)
                                ),

                                // [1] Freezer
                                new(
                                    contentIndex: (int)ElementIndex.Freezer,
                                    name: Localization_Elements.Solid_Immovable_Freezer_Name,
                                    description: Localization_Elements.Solid_Immovable_Freezer_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 96, 32, 32)
                                ),

                                // [2] Upward Pusher
                                new(
                                    contentIndex: (int)ElementIndex.UpwardPusher,
                                    name: Localization_Elements.Solid_Immovable_UpwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_UpwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 96, 32, 32)
                                ),

                                // [3] Rightward Pusher
                                new(
                                    contentIndex: (int)ElementIndex.RightwardPusher,
                                    name: Localization_Elements.Solid_Immovable_RightwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_RightwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 96, 32, 32)
                                ),

                                // [4] Downward Pusher
                                new(
                                    contentIndex: (int)ElementIndex.DownwardPusher,
                                    name: Localization_Elements.Solid_Immovable_DownwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_DownwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 128, 32, 32)
                                ),

                                // [5] Leftward Pusher
                                new(
                                    contentIndex: (int)ElementIndex.LeftwardPusher,
                                    name: Localization_Elements.Solid_Immovable_LeftwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_LeftwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 128, 32, 32)
                                ),

                                // [6] Battery
                                new(
                                    contentIndex: (int)ElementIndex.Battery,
                                    name: Localization_Elements.Solid_Immovable_Battery_Name,
                                    description: Localization_Elements.Solid_Immovable_Battery_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 0, 32, 32)
                                ),

                                // [7] Lamp (on)
                                new(
                                    contentIndex: (int)ElementIndex.LampOn,
                                    name: Localization_Elements.Solid_Immovable_LampOn_Name,
                                    description: Localization_Elements.Solid_Immovable_LampOn_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 64, 32, 32)
                                ),

                                // [8] Lamp (off)
                                new(
                                    contentIndex: (int)ElementIndex.LampOff,
                                    name: Localization_Elements.Solid_Immovable_LampOff_Name,
                                    description: Localization_Elements.Solid_Immovable_LampOff_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 64, 32, 32)
                                ),
                            ]
                        ),

                        // [7] Specials
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Specials_Name,
                            description: Localization_Catalog.Subcategory_Elements_Specials_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(160, 64, 32, 32),
                            [
                                // [0] Void
                                new(
                                    contentIndex: (int)ElementIndex.Void,
                                    name: Localization_Elements.Solid_Immovable_Void_Name,
                                    description: Localization_Elements.Solid_Immovable_Void_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 64, 32, 32)
                                ),

                                // [1] Clone
                                new(
                                    contentIndex: (int)ElementIndex.Clone,
                                    name: Localization_Elements.Solid_Immovable_Clone_Name,
                                    description: Localization_Elements.Solid_Immovable_Clone_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 64, 32, 32)
                                ),

                                // [2] Devourer
                                new(
                                    contentIndex: (int)ElementIndex.Devourer,
                                    name: Localization_Elements.Solid_Immovable_Devourer_Name,
                                    description: Localization_Elements.Solid_Immovable_Devourer_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 96, 32, 32)
                                ),
                            ]
                        ),

                        // [8] Vegetation
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Vegetation_Name,
                            description: Localization_Catalog.Subcategory_Elements_Vegetation_Description,
                            textureIndex: TextureIndex.IconElements,
                            sourceRectangle: new(160, 128, 32, 32),
                            [
                                // [0] Seed
                                new(
                                    contentIndex: (int)ElementIndex.Seed,
                                    name: Localization_Elements.Solid_Movable_Seed_Name,
                                    description: Localization_Elements.Solid_Movable_Seed_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 128, 32, 32)
                                ),

                                // [1] Sapling
                                new(
                                    contentIndex: (int)ElementIndex.Sapling,
                                    name: Localization_Elements.Solid_Movable_Sapling_Name,
                                    description: Localization_Elements.Solid_Movable_Sapling_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 128, 32, 32)
                                ),

                                // [2] Moss
                                new(
                                    contentIndex: (int)ElementIndex.Moss,
                                    name: Localization_Elements.Solid_Immovable_Moss_Name,
                                    description: Localization_Elements.Solid_Immovable_Moss_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 128, 32, 32)
                                ),
                            ]
                        ),
                    ]
                ),

                // [1] Tools
                new(
                    Localization_Catalog.Category_Tools_Name,
                    Localization_Catalog.Category_Tools_Description,
                    TextureIndex.IconUI,
                    sourceRectangle: new(224, 160, 32, 32),
                    [
                        // [0] Environment
                        new(
                            name: Localization_Catalog.Subcategory_Tools_Environment_Name,
                            description: Localization_Catalog.Subcategory_Tools_Environment_Description,
                            textureIndex: TextureIndex.IconUI,
                            sourceRectangle: new(0, 192, 32, 32),
                            [
                                // [0] Heat Tool
                                new(
                                    contentIndex: (int)ToolIndex.HeatTool,
                                    name: Localization_Tools.Environment_Heat_Name,
                                    description: Localization_Tools.Environment_Heat_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(0, 0, 32, 32)
                                ),

                                // [1] Freeze Tool
                                new(
                                    contentIndex: (int)ToolIndex.FreezeTool,
                                    name: Localization_Tools.Environment_Freeze_Name,
                                    description: Localization_Tools.Environment_Freeze_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(32, 0, 32, 32)
                                ),
                            ]
                        ),

                        // [1] Inks
                        new(
                            name: Localization_Catalog.Subcategory_Tools_Inks_Name,
                            description: Localization_Catalog.Subcategory_Tools_Inks_Description,
                            textureIndex: TextureIndex.IconTools,
                            sourceRectangle: new(0, 32, 32, 32),
                            [
                                // [0] Black Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.BlackInkTool,
                                    name: Localization_Tools.Inks_Black_Name,
                                    description: Localization_Tools.Inks_Black_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(0, 32, 32, 32)
                                ),

                                // [1] White Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.WhiteInkTool,
                                    name: Localization_Tools.Inks_White_Name,
                                    description: Localization_Tools.Inks_White_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(32, 32, 32, 32)
                                ),

                                // [2] Red Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.RedInkTool,
                                    name: Localization_Tools.Inks_Red_Name,
                                    description: Localization_Tools.Inks_Red_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(64, 32, 32, 32)
                                ),

                                // [3] Orange Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.OrangeInkTool,
                                    name: Localization_Tools.Inks_Orange_Name,
                                    description: Localization_Tools.Inks_Orange_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(96, 32, 32, 32)
                                ),

                                // [4] Yellow Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.YellowInkTool,
                                    name: Localization_Tools.Inks_Yellow_Name,
                                    description: Localization_Tools.Inks_Yellow_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(128, 32, 32, 32)
                                ),

                                // [5] Green Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.GreenInkTool,
                                    name: Localization_Tools.Inks_Green_Name,
                                    description: Localization_Tools.Inks_Green_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(0, 64, 32, 32)
                                ),

                                // [6] Blue Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.BlueInkTool,
                                    name: Localization_Tools.Inks_Blue_Name,
                                    description: Localization_Tools.Inks_Blue_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(32, 64, 32, 32)
                                ),

                                // [7] Gray Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.GrayInkTool,
                                    name: Localization_Tools.Inks_Gray_Name,
                                    description: Localization_Tools.Inks_Gray_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(64, 64, 32, 32)
                                ),

                                // [8] Violet Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.VioletInkTool,
                                    name: Localization_Tools.Inks_Violet_Name,
                                    description: Localization_Tools.Inks_Violet_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(96, 64, 32, 32)
                                ),

                                // [9] Brown Ink Tool
                                new(
                                    contentIndex: (int)ToolIndex.BrownInkTool,
                                    name: Localization_Tools.Inks_Brown_Name,
                                    description: Localization_Tools.Inks_Brown_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(128, 64, 32, 32)
                                ),
                            ]
                        ),
                    ]
                ),

                // [2] Actors
                new(
                    Localization_Catalog.Category_Actors_Name,
                    Localization_Catalog.Category_Actors_Description,
                    TextureIndex.IconActors,
                    sourceRectangle: new(0, 0, 32, 32),
                    [
                        // [0] Creatures
                        new(
                            name: Localization_Catalog.Subcategory_Actors_Creatures_Name,
                            description: Localization_Catalog.Subcategory_Actors_Creatures_Description,
                            textureIndex: TextureIndex.IconActors,
                            sourceRectangle: new(0, 0, 32, 32),
                            [
                                // [0] Gul
                                new(
                                    contentIndex: (int)ActorIndex.Gul,
                                    name: Localization_Actors.Gul_Name,
                                    description: Localization_Actors.Gul_Description,
                                    contentType: ItemContentType.Actor,
                                    textureIndex: TextureIndex.IconActors,
                                    sourceRectangle: new(0, 0, 32, 32)
                                ),
                            ]
                        ),
                    ]
                ),
            ];
        }

        private static void SetParentReferences()
        {
            for (int i = 0; i < categories.Length; i++)
            {
                Category category = categories[i];

                for (int j = 0; j < category.Subcategories.Length; j++)
                {
                    Subcategory subcategory = category.Subcategories[j];
                    subcategory.SetParentCategory(category);

                    for (int k = 0; k < subcategory.Items.Length; k++)
                    {
                        Item item = subcategory.Items[k];
                        item.SetParentSubcategory(subcategory);
                    }
                }
            }
        }

        private static void CalculateCatalogLengths()
        {
            categoryLength = categories.Length;
            itemLength = 0;

            for (int i = 0; i < categories.Length; i++)
            {
                Category category = categories[i];

                for (int j = 0; j < category.Subcategories.Length; j++)
                {
                    Subcategory subcategory = category.Subcategories[j];
                    itemLength += subcategory.Items.Length;
                }
            }
        }

        internal static Category GetCategory(byte index)
        {
            return categories[index];
        }

        internal static Item GetItem(byte categoryIndex, byte subcategoryIndex, byte itemIndex)
        {
            return categories[categoryIndex].GetSubcategory(subcategoryIndex).GetItem(itemIndex);
        }

        internal static Item[] GetItems(uint amount)
        {
            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be at least 1.");
            }

            Item[] items = new Item[amount];

            uint currentIndex = 0;

            for (uint i = 0; i < categories.Length; i++)
            {
                Category category = categories[i];

                for (uint j = 0; j < category.Subcategories.Length; j++)
                {
                    Subcategory subcategory = category.Subcategories[j];

                    for (uint k = 0; k < subcategory.Items.Length; k++)
                    {
                        Item item = subcategory.Items[k];

                        if (currentIndex >= amount)
                        {
                            return items;
                        }

                        items[currentIndex] = item;

                        currentIndex++;
                    }
                }
            }

            return items;
        }
    }
}

