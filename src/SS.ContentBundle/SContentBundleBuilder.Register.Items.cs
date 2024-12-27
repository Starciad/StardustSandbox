using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Items;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterItems(ISGame game, ISItemDatabase itemDatabase)
        {
            #region Categories
            SCategory[] categories = [
                // [0]
                new(
                    identifier: "powders",
                    displayName: SLocalization.Item_Category_Powders,
                    description: SLocalization.Item_Category_Powders_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
                ),

                // [1]
                new(
                    identifier: "liquids",
                    displayName: SLocalization.Item_Category_Liquids,
                    description: SLocalization.Item_Category_Liquids_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
                ),

                // [2]
                new(
                    identifier: "gases",
                    displayName: SLocalization.Item_Category_Gases,
                    description: SLocalization.Item_Category_Gases_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
                ),

                // [3]
                new(
                    identifier: "solids",
                    displayName: SLocalization.Item_Category_Solids,
                    description: SLocalization.Item_Category_Solids_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
                ),

                // [4]
                new(
                    identifier: "walls",
                    displayName: SLocalization.Item_Category_Walls,
                    description: SLocalization.Item_Category_Walls_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
                ),

                // [5]
                new(
                    identifier: "energies",
                    displayName: SLocalization.Item_Category_Energies,
                    description: SLocalization.Item_Category_Energies_Description,
                    iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
                ),
            ];

            for (int i = 0; i < categories.Length; i++)
            {
                itemDatabase.RegisterCategory(categories[i]);
            }
            #endregion

            #region Items

            #region Elements
            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.DIRT,
                displayName: SLocalization.Element_Solid_Movable_Dirt_Name,
                description: SLocalization.Element_Solid_Movable_Dirt_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.MUD,
                displayName: SLocalization.Element_Solid_Movable_Mud_Name,
                description: SLocalization.Element_Solid_Movable_Mud_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_2")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.WATER,
                displayName: SLocalization.Element_Liquid_Water_Name,
                description: SLocalization.Element_Liquid_Water_Description,
                contentType: SItemContentType.Element,
                category: categories[1],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.STONE,
                displayName: SLocalization.Element_Solid_Movable_Stone_Name,
                description: SLocalization.Element_Solid_Movable_Stone_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_4")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.GRASS,
                displayName: SLocalization.Element_Solid_Movable_Grass_Name,
                description: SLocalization.Element_Solid_Movable_Grass_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_5")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.ICE,
                displayName: SLocalization.Element_Solid_Movable_Ice_Name,
                description: SLocalization.Element_Solid_Movable_Ice_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_6")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.SAND,
                displayName: SLocalization.Element_Solid_Movable_Sand_Name,
                description: SLocalization.Element_Solid_Movable_Sand_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_7")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.SNOW,
                displayName: SLocalization.Element_Solid_Movable_Snow_Name,
                description: SLocalization.Element_Solid_Movable_Snow_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_8")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.MOVABLE_CORRUPTION,
                displayName: SLocalization.Element_Solid_Movable_Corruption_Name,
                description: SLocalization.Element_Solid_Movable_Corruption_Description,
                contentType: SItemContentType.Element,
                category: categories[0],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_9")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.LAVA,
                displayName: SLocalization.Element_Liquid_Lava_Name,
                description: SLocalization.Element_Liquid_Lava_Description,
                contentType: SItemContentType.Element,
                category: categories[1],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_10")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.ACID,
                displayName: SLocalization.Element_Liquid_Acid_Name,
                description: SLocalization.Element_Liquid_Acid_Description,
                contentType: SItemContentType.Element,
                category: categories[1],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_11")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.GLASS,
                displayName: SLocalization.Element_Solid_Immovable_Glass_Name,
                description: SLocalization.Element_Solid_Immovable_Glass_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_12")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.METAL,
                displayName: SLocalization.Element_Solid_Immovable_Metal_Name,
                description: SLocalization.Element_Solid_Immovable_Metal_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.WALL,
                displayName: SLocalization.Element_Solid_Immovable_Wall_Name,
                description: SLocalization.Element_Solid_Immovable_Wall_Description,
                contentType: SItemContentType.Element,
                category: categories[4],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.WOOD,
                displayName: SLocalization.Element_Solid_Immovable_Wood_Name,
                description: SLocalization.Element_Solid_Immovable_Wood_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.GAS_CORRUPTION,
                displayName: SLocalization.Element_Gas_Corruption_Name,
                description: SLocalization.Element_Gas_Corruption_Description,
                contentType: SItemContentType.Element,
                category: categories[2],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_16")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.LIQUID_CORRUPTION,
                displayName: SLocalization.Element_Liquid_Corruption_Name,
                description: SLocalization.Element_Liquid_Corruption_Description,
                contentType: SItemContentType.Element,
                category: categories[1],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_17")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.IMMOVABLE_CORRUPTION,
                displayName: SLocalization.Element_Solid_Immovable_Corruption_Name,
                description: SLocalization.Element_Solid_Immovable_Corruption_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_18")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.STEAM,
                displayName: SLocalization.Element_Gas_Steam_Name,
                description: SLocalization.Element_Gas_Steam_Description,
                contentType: SItemContentType.Element,
                category: categories[2],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_19")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.SMOKE,
                displayName: SLocalization.Element_Gas_Smoke_Name,
                description: SLocalization.Element_Gas_Smoke_Description,
                contentType: SItemContentType.Element,
                category: categories[2],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.RED_BRICK,
                displayName: SLocalization.Element_Solid_Immovable_RedBrick_Name,
                description: SLocalization.Element_Solid_Immovable_RedBrick_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_21")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.TREE_LEAF,
                displayName: SLocalization.Element_Solid_Immovable_TreeLeaf_Name,
                description: SLocalization.Element_Solid_Immovable_TreeLeaf_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_22")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.MOUNTING_BLOCK,
                displayName: SLocalization.Element_Solid_Immovable_MountingBlock_Name,
                description: SLocalization.Element_Solid_Immovable_MountingBlock_Description,
                contentType: SItemContentType.Element,
                category: categories[3],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_23")
            ));

            itemDatabase.RegisterItem(new(
                identifier: SElementIdentifierConstants.FIRE,
                displayName: SLocalization.Element_Energy_Fire_Name,
                description: SLocalization.Element_Energy_Fire_Description,
                contentType: SItemContentType.Element,
                category: categories[5],
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            ));
            #endregion

            #endregion
        }
    }
}
