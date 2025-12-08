using StardustSandbox.Catalog;
using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Elements.Solids.Immovables.Pushers;
using StardustSandbox.Elements.Solids.Immovables.Wools;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Elements.Solids.Movables.Explosives;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Items;
using StardustSandbox.Localization;
using StardustSandbox.Tools;

using System;

namespace StardustSandbox.Databases
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
                                    associatedType: typeof(Dirt),
                                    name: Localization_Elements.Solid_Movable_Dirt_Name,
                                    description: Localization_Elements.Solid_Movable_Dirt_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 0, 32, 32)
                                ),

                                // [1] Mud
                                new(
                                    associatedType: typeof(Mud),
                                    name: Localization_Elements.Solid_Movable_Mud_Name,
                                    description: Localization_Elements.Solid_Movable_Mud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 0, 32, 32)
                                ),

                                // [2] Stone
                                new(
                                    associatedType: typeof(Stone),
                                    name: Localization_Elements.Solid_Movable_Stone_Name,
                                    description: Localization_Elements.Solid_Movable_Stone_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 0, 32, 32)
                                ),

                                // [3] Grass
                                new(
                                    associatedType: typeof(Grass),
                                    name: Localization_Elements.Solid_Movable_Grass_Name,
                                    description: Localization_Elements.Solid_Movable_Grass_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 0, 32, 32)
                                ),

                                // [4] Ice
                                new(
                                    associatedType: typeof(Ice),
                                    name: Localization_Elements.Solid_Movable_Ice_Name,
                                    description: Localization_Elements.Solid_Movable_Ice_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 0, 32, 32)
                                ),

                                // [5] Sand
                                new(
                                    associatedType: typeof(Sand),
                                    name: Localization_Elements.Solid_Movable_Sand_Name,
                                    description: Localization_Elements.Solid_Movable_Sand_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 0, 32, 32)
                                ),

                                // [6] Snow
                                new(
                                    associatedType: typeof(Snow),
                                    name: Localization_Elements.Solid_Movable_Snow_Name,
                                    description: Localization_Elements.Solid_Movable_Snow_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 0, 32, 32)
                                ),

                                // [7] Corruption
                                new(
                                    associatedType: typeof(MovableCorruption),
                                    name: Localization_Elements.Solid_Movable_Corruption_Name,
                                    description: Localization_Elements.Solid_Movable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 0, 32, 32)
                                ),

                                // [8] Salt
                                new(
                                    associatedType: typeof(Salt),
                                    name: Localization_Elements.Solid_Movable_Salt_Name,
                                    description: Localization_Elements.Solid_Movable_Salt_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 64, 32, 32)
                                ),

                                // [9] Ash
                                new(
                                    associatedType: typeof(Ash),
                                    name: Localization_Elements.Solid_Movable_Ash_Name,
                                    description: Localization_Elements.Solid_Movable_Ash_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 96, 32, 32)
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
                                    associatedType: typeof(Water),
                                    name: Localization_Elements.Liquid_Water_Name,
                                    description: Localization_Elements.Liquid_Water_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 0, 32, 32)
                                ),

                                // [1] Lava
                                new(
                                    associatedType: typeof(Lava),
                                    name: Localization_Elements.Liquid_Lava_Name,
                                    description: Localization_Elements.Liquid_Lava_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 0, 32, 32)
                                ),

                                // [2] Acid
                                new(
                                    associatedType: typeof(Acid),
                                    name: Localization_Elements.Liquid_Acid_Name,
                                    description: Localization_Elements.Liquid_Acid_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 32, 32, 32)
                                ),

                                // [3] Corruption
                                new(
                                    associatedType: typeof(LiquidCorruption),
                                    name: Localization_Elements.Liquid_Corruption_Name,
                                    description: Localization_Elements.Liquid_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 32, 32, 32)
                                ),

                                // [4] Oil
                                new(
                                    associatedType: typeof(Oil),
                                    name: Localization_Elements.Liquid_Oil_Name,
                                    description: Localization_Elements.Liquid_Oil_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 64, 32, 32)
                                ),

                                // [5] Saltwater
                                new(
                                    associatedType: typeof(Saltwater),
                                    name: Localization_Elements.Liquid_Saltwater_Name,
                                    description: Localization_Elements.Liquid_Saltwater_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 64, 32, 32)
                                )
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
                                    associatedType: typeof(GasCorruption),
                                    name: Localization_Elements.Gas_Corruption_Name,
                                    description: Localization_Elements.Gas_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 32, 32, 32)
                                ),

                                // [1] Steam
                                new(
                                    associatedType: typeof(Steam),
                                    name: Localization_Elements.Gas_Steam_Name,
                                    description: Localization_Elements.Gas_Steam_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 32, 32, 32)
                                ),

                                // [2] Smoke
                                new(
                                    associatedType: typeof(Smoke),
                                    name: Localization_Elements.Gas_Smoke_Name,
                                    description: Localization_Elements.Gas_Smoke_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 32, 32, 32)
                                ),

                                // [3] Anti-Corruption
                                new(
                                    associatedType: typeof(AntiCorruption),
                                    name: Localization_Elements.Gas_AntiCorruption_Name,
                                    description: Localization_Elements.Gas_AntiCorruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 96, 32, 32)
                                ),

                                // [4] Cloud
                                new(
                                    associatedType: typeof(Cloud),
                                    name: Localization_Elements.Gas_Cloud_Name,
                                    description: Localization_Elements.Gas_Cloud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 128, 32, 32)
                                ),

                                // [5] Charged Cloud
                                new(
                                    associatedType: typeof(ChargedCloud),
                                    name: Localization_Elements.Gas_ChargedCloud_Name,
                                    description: Localization_Elements.Gas_ChargedCloud_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 128, 32, 32)
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
                                    associatedType: typeof(Glass),
                                    name: Localization_Elements.Solid_Immovable_Glass_Name,
                                    description: Localization_Elements.Solid_Immovable_Glass_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 32, 32, 32)
                                ),

                                // [1] Iron
                                new(
                                    associatedType: typeof(Iron),
                                    name: Localization_Elements.Solid_Immovable_Iron_Name,
                                    description: Localization_Elements.Solid_Immovable_Iron_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 32, 32, 32)
                                ),

                                // [2] Wall
                                new(
                                    associatedType: typeof(Wall),
                                    name: Localization_Elements.Solid_Immovable_Wall_Name,
                                    description: Localization_Elements.Solid_Immovable_Wall_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 32, 32, 32)
                                ),

                                // [3] Wood
                                new(
                                    associatedType: typeof(Wood),
                                    name: Localization_Elements.Solid_Immovable_Wood_Name,
                                    description: Localization_Elements.Solid_Immovable_Wood_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 32, 32, 32)
                                ),

                                // [4] Corruption
                                new(
                                    associatedType: typeof(ImmovableCorruption),
                                    name: Localization_Elements.Solid_Immovable_Corruption_Name,
                                    description: Localization_Elements.Solid_Immovable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 32, 32, 32)
                                ),

                                // [5] Red Brick
                                new(
                                    associatedType: typeof(RedBrick),
                                    name: Localization_Elements.Solid_Immovable_RedBrick_Name,
                                    description: Localization_Elements.Solid_Immovable_RedBrick_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 32, 32, 32)
                                ),

                                // [6] Tree Leaf
                                new(
                                    associatedType: typeof(TreeLeaf),
                                    name: Localization_Elements.Solid_Immovable_TreeLeaf_Name,
                                    description: Localization_Elements.Solid_Immovable_TreeLeaf_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 64, 32, 32)
                                ),

                                // [7] Mounting Block
                                new(
                                    associatedType: typeof(MountingBlock),
                                    name: Localization_Elements.Solid_Immovable_MountingBlock_Name,
                                    description: Localization_Elements.Solid_Immovable_MountingBlock_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 64, 32, 32)
                                ),

                                // [8] Lamp
                                new(
                                    associatedType: typeof(Lamp),
                                    name: Localization_Elements.Solid_Immovable_Lamp_Name,
                                    description: Localization_Elements.Solid_Immovable_Lamp_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 64, 32, 32)
                                ),

                                // [9] Dry Sponge
                                new(
                                    associatedType: typeof(DrySponge),
                                    name: Localization_Elements.Solid_Immovable_DrySponge_Name,
                                    description: Localization_Elements.Solid_Immovable_DrySponge_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(32, 96, 32, 32)
                                ),

                                // [10] Wet Sponge
                                new(
                                    associatedType: typeof(WetSponge),
                                    name: Localization_Elements.Solid_Immovable_WetSponge_Name,
                                    description: Localization_Elements.Solid_Immovable_WetSponge_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 96, 32, 32)
                                ),

                                // [11] Gold
                                new(
                                    associatedType: typeof(Gold),
                                    name: Localization_Elements.Solid_Immovable_Gold_Name,
                                    description: Localization_Elements.Solid_Immovable_Gold_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(96, 96, 32, 32)
                                ),

                                // [12] Dry Black Wool
                                new(
                                    associatedType: typeof(DryBlackWool),
                                    name: Localization_Elements.Solid_Immovable_DryBlackWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBlackWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 64, 32, 32)
                                ),

                                // [13] Dry White Wool
                                new(
                                    associatedType: typeof(DryWhiteWool),
                                    name: Localization_Elements.Solid_Immovable_DryWhiteWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryWhiteWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 64, 32, 32)
                                ),

                                // [14] Dry Red Wool
                                new(
                                    associatedType: typeof(DryRedWool),
                                    name: Localization_Elements.Solid_Immovable_DryRedWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryRedWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(192, 64, 32, 32)
                                ),

                                // [15] Dry Orange Wool
                                new(
                                    associatedType: typeof(DryOrangeWool),
                                    name: Localization_Elements.Solid_Immovable_DryOrangeWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryOrangeWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(224, 64, 32, 32)
                                ),

                                // [16] Dry Yellow Wool
                                new(
                                    associatedType: typeof(DryYellowWool),
                                    name: Localization_Elements.Solid_Immovable_DryYellowWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryYellowWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 64, 32, 32)
                                ),

                                // [17] Dry Green Wool
                                new(
                                    associatedType: typeof(DryGreenWool),
                                    name: Localization_Elements.Solid_Immovable_DryGreenWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryGreenWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 64, 32, 32)
                                ),

                                // [18] Dry Cyan Wool
                                new(
                                    associatedType: typeof(DryCyanWool),
                                    name: Localization_Elements.Solid_Immovable_DryCyanWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryCyanWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 64, 32, 32)
                                ),

                                // [19] Dry Blue Wool
                                new(
                                    associatedType: typeof(DryBlueWool),
                                    name: Localization_Elements.Solid_Immovable_DryBlueWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBlueWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(352, 64, 32, 32)
                                ),

                                // [20] Dry Violet Wool
                                new(
                                    associatedType: typeof(DryVioletWool),
                                    name: Localization_Elements.Solid_Immovable_DryVioletWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryVioletWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(384, 64, 32, 32)
                                ),

                                // [21] Dry Brown Wool
                                new(
                                    associatedType: typeof(DryBrownWool),
                                    name: Localization_Elements.Solid_Immovable_DryBrownWool_Name,
                                    description: Localization_Elements.Solid_Immovable_DryBrownWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(416, 64, 32, 32)
                                ),

                                // [22] Wet Black Wool
                                new(
                                    associatedType: typeof(WetBlackWool),
                                    name: Localization_Elements.Solid_Immovable_WetBlackWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBlackWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(448, 64, 32, 32)
                                ),

                                // [23] Wet White Wool
                                new(
                                    associatedType: typeof(WetWhiteWool),
                                    name: Localization_Elements.Solid_Immovable_WetWhiteWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetWhiteWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(480, 64, 32, 32)
                                ),

                                // [24] Wet Red Wool
                                new(
                                    associatedType: typeof(WetRedWool),
                                    name: Localization_Elements.Solid_Immovable_WetRedWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetRedWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(512, 64, 32, 32)
                                ),

                                // [25] Wet Orange Wool
                                new(
                                    associatedType: typeof(WetOrangeWool),
                                    name: Localization_Elements.Solid_Immovable_WetOrangeWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetOrangeWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(544, 64, 32, 32)
                                ),

                                // [26] Wet Yellow Wool
                                new(
                                    associatedType: typeof(WetYellowWool),
                                    name: Localization_Elements.Solid_Immovable_WetYellowWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetYellowWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(576, 64, 32, 32)
                                ),

                                // [27] Wet Green Wool
                                new(
                                    associatedType: typeof(WetGreenWool),
                                    name: Localization_Elements.Solid_Immovable_WetGreenWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetGreenWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(608, 64, 32, 32)
                                ),

                                // [28] Wet Cyan Wool
                                new(
                                    associatedType: typeof(WetCyanWool),
                                    name: Localization_Elements.Solid_Immovable_WetCyanWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetCyanWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(640, 64, 32, 32)
                                ),

                                // [29] Wet Blue Wool
                                new(
                                    associatedType: typeof(WetBlueWool),
                                    name: Localization_Elements.Solid_Immovable_WetBlueWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBlueWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(672, 64, 32, 32)
                                ),

                                // [30] Wet Violet Wool
                                new(
                                    associatedType: typeof(WetVioletWool),
                                    name: Localization_Elements.Solid_Immovable_WetVioletWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetVioletWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(704, 64, 32, 32)
                                ),

                                // [31] Wet Brown Wool
                                new(
                                    associatedType: typeof(WetBrownWool),
                                    name: Localization_Elements.Solid_Immovable_WetBrownWool_Name,
                                    description: Localization_Elements.Solid_Immovable_WetBrownWool_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(736, 64, 32, 32)
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
                                    associatedType: typeof(Fire),
                                    name: Localization_Elements.Energy_Fire_Name,
                                    description: Localization_Elements.Energy_Fire_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 64, 32, 32)
                                ),

                                // [1] Thunder
                                new(
                                    associatedType: typeof(ThunderHead),
                                    name: Localization_Elements.Energy_Thunder_Name,
                                    description: Localization_Elements.Energy_Thunder_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(64, 128, 32, 32)
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
                                    associatedType: typeof(Bomb),
                                    name: Localization_Elements.Solid_Movable_Bomb_Name,
                                    description: Localization_Elements.Solid_Movable_Bomb_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 64, 32, 32)
                                ),

                                // [1] Dynamite
                                new(
                                    associatedType: typeof(Dynamite),
                                    name: Localization_Elements.Solid_Movable_Dynamite_Name,
                                    description: Localization_Elements.Solid_Movable_Dynamite_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 64, 32, 32)
                                ),

                                // [2] TNT
                                new(
                                    associatedType: typeof(Tnt),
                                    name: Localization_Elements.Solid_Movable_TNT_Name,
                                    description: Localization_Elements.Solid_Movable_TNT_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(0, 96, 32, 32)
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
                                    associatedType: typeof(Heater),
                                    name: Localization_Elements.Solid_Immovable_Heater_Name,
                                    description: Localization_Elements.Solid_Immovable_Heater_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 96, 32, 32)
                                ),

                                // [1] Freezer
                                new(
                                    associatedType: typeof(Freezer),
                                    name: Localization_Elements.Solid_Immovable_Freezer_Name,
                                    description: Localization_Elements.Solid_Immovable_Freezer_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 96, 32, 32)
                                ),

                                // [2] Upward Pusher
                                new(
                                    associatedType: typeof(UpwardPusher),
                                    name: Localization_Elements.Solid_Immovable_UpwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_UpwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 96, 32, 32)
                                ),

                                // [3] Rightward Pusher
                                new(
                                    associatedType: typeof(RightwardPusher),
                                    name: Localization_Elements.Solid_Immovable_RightwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_RightwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 96, 32, 32)
                                ),

                                // [4] Downward Pusher
                                new(
                                    associatedType: typeof(DownwardPusher),
                                    name: Localization_Elements.Solid_Immovable_DownwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_DownwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(288, 128, 32, 32)
                                ),

                                // [5] Leftward Pusher
                                new(
                                    associatedType: typeof(LeftwardPusher),
                                    name: Localization_Elements.Solid_Immovable_LeftwardPusher_Name,
                                    description: Localization_Elements.Solid_Immovable_LeftwardPusher_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(320, 128, 32, 32)
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
                                    associatedType: typeof(Elements.Solids.Immovables.Void),
                                    name: Localization_Elements.Solid_Immovable_Void_Name,
                                    description: Localization_Elements.Solid_Immovable_Void_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(128, 64, 32, 32)
                                ),

                                // [1] Clone
                                new(
                                    associatedType: typeof(Clone),
                                    name: Localization_Elements.Solid_Immovable_Clone_Name,
                                    description: Localization_Elements.Solid_Immovable_Clone_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(160, 64, 32, 32)
                                ),

                                // [2] Devourer
                                new(
                                    associatedType: typeof(Devourer),
                                    name: Localization_Elements.Solid_Immovable_Devourer_Name,
                                    description: Localization_Elements.Solid_Immovable_Devourer_Description,
                                    contentType: ItemContentType.Element,
                                    textureIndex: TextureIndex.IconElements,
                                    sourceRectangle: new(256, 96, 32, 32)
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
                                    associatedType: typeof(HeatTool),
                                    name: Localization_Tools.Environment_Heat_Name,
                                    description: Localization_Tools.Environment_Heat_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(0, 0, 32, 32)
                                ),

                                // [1] Freeze Tool
                                new(
                                    associatedType: typeof(FreezeTool),
                                    name: Localization_Tools.Environment_Freeze_Name,
                                    description: Localization_Tools.Environment_Freeze_Description,
                                    contentType: ItemContentType.Tool,
                                    textureIndex: TextureIndex.IconTools,
                                    sourceRectangle: new(32, 0, 32, 32)
                                ),
                            ]
                        ),
                    ]
                )
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
