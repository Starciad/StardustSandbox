using StardustSandbox.ContentBundle.Localization.Catalog;
using StardustSandbox.ContentBundle.Localization.Elements;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterCatalog(ISGame game, ISCatalogDatabase catalogDatabase)
        {
            #region Categories
            SCategory elementCategory = new(
                "elements",
                SLocalization_Catalog.Category_Elements_Name,
                SLocalization_Catalog.Category_Elements_Description,
                game.AssetDatabase.GetTexture("icon_element_1")
            );

            catalogDatabase.RegisterCategory(elementCategory);
            #endregion

            #region Subcategories

            #region Elements
            SSubcategory elementPowderSubcategory = new(
                parent: elementCategory,
                identifier: "powders",
                displayName: SLocalization_Catalog.Subcategory_Elements_Powders_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Powders_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            );

            SSubcategory elementLiquidSubcategory = new(
                parent: elementCategory,
                identifier: "liquids",
                displayName: SLocalization_Catalog.Subcategory_Elements_Liquids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Liquids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            );

            SSubcategory elementGasSubcategory = new(
                parent: elementCategory,
                identifier: "gases",
                displayName: SLocalization_Catalog.Subcategory_Elements_Gases_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Gases_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            );

            SSubcategory elementSolidSubcategory = new(
                parent: elementCategory,
                identifier: "solids",
                displayName: SLocalization_Catalog.Subcategory_Elements_Solids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Solids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            );

            SSubcategory elementWallSubcategory = new(
                parent: elementCategory,
                identifier: "walls",
                displayName: SLocalization_Catalog.Subcategory_Elements_Walls_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Walls_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            );

            SSubcategory elementEnergySubcategory = new(
                parent: elementCategory,
                identifier: "energies",
                displayName: SLocalization_Catalog.Subcategory_Elements_Energies_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Energies_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            );

            elementCategory.AddSubcategory(elementPowderSubcategory);
            elementCategory.AddSubcategory(elementLiquidSubcategory);
            elementCategory.AddSubcategory(elementGasSubcategory);
            elementCategory.AddSubcategory(elementSolidSubcategory);
            elementCategory.AddSubcategory(elementWallSubcategory);
            elementCategory.AddSubcategory(elementEnergySubcategory);
            #endregion

            #endregion

            #region Items

            #region Elements
            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_DIRT,
                displayName: SLocalization_Elements.Solid_Movable_Dirt_Name,
                description: SLocalization_Elements.Solid_Movable_Dirt_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_MUD,
                displayName: SLocalization_Elements.Solid_Movable_Mud_Name,
                description: SLocalization_Elements.Solid_Movable_Mud_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_2")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_WATER,
                displayName: SLocalization_Elements.Liquid_Water_Name,
                description: SLocalization_Elements.Liquid_Water_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_STONE,
                displayName: SLocalization_Elements.Solid_Movable_Stone_Name,
                description: SLocalization_Elements.Solid_Movable_Stone_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_4")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_GRASS,
                displayName: SLocalization_Elements.Solid_Movable_Grass_Name,
                description: SLocalization_Elements.Solid_Movable_Grass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_5")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_ICE,
                displayName: SLocalization_Elements.Solid_Movable_Ice_Name,
                description: SLocalization_Elements.Solid_Movable_Ice_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_6")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_SAND,
                displayName: SLocalization_Elements.Solid_Movable_Sand_Name,
                description: SLocalization_Elements.Solid_Movable_Sand_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_7")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_SNOW,
                displayName: SLocalization_Elements.Solid_Movable_Snow_Name,
                description: SLocalization_Elements.Solid_Movable_Snow_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_8")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_MOVABLE_CORRUPTION,
                displayName: SLocalization_Elements.Solid_Movable_Corruption_Name,
                description: SLocalization_Elements.Solid_Movable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_9")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_LAVA,
                displayName: SLocalization_Elements.Liquid_Lava_Name,
                description: SLocalization_Elements.Liquid_Lava_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_10")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_ACID,
                displayName: SLocalization_Elements.Liquid_Acid_Name,
                description: SLocalization_Elements.Liquid_Acid_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_11")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_GLASS,
                displayName: SLocalization_Elements.Solid_Immovable_Glass_Name,
                description: SLocalization_Elements.Solid_Immovable_Glass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_12")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_IRON,
                displayName: SLocalization_Elements.Solid_Immovable_Iron_Name,
                description: SLocalization_Elements.Solid_Immovable_Iron_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_WALL,
                displayName: SLocalization_Elements.Solid_Immovable_Wall_Name,
                description: SLocalization_Elements.Solid_Immovable_Wall_Description,
                contentType: SItemContentType.Element,
                subcategory: elementWallSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_WOOD,
                displayName: SLocalization_Elements.Solid_Immovable_Wood_Name,
                description: SLocalization_Elements.Solid_Immovable_Wood_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_GAS_CORRUPTION,
                displayName: SLocalization_Elements.Gas_Corruption_Name,
                description: SLocalization_Elements.Gas_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_16")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_LIQUID_CORRUPTION,
                displayName: SLocalization_Elements.Liquid_Corruption_Name,
                description: SLocalization_Elements.Liquid_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_17")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_IMMOVABLE_CORRUPTION,
                displayName: SLocalization_Elements.Solid_Immovable_Corruption_Name,
                description: SLocalization_Elements.Solid_Immovable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_18")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_STEAM,
                displayName: SLocalization_Elements.Gas_Steam_Name,
                description: SLocalization_Elements.Gas_Steam_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_19")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_SMOKE,
                displayName: SLocalization_Elements.Gas_Smoke_Name,
                description: SLocalization_Elements.Gas_Smoke_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_RED_BRICK,
                displayName: SLocalization_Elements.Solid_Immovable_RedBrick_Name,
                description: SLocalization_Elements.Solid_Immovable_RedBrick_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_21")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_TREE_LEAF,
                displayName: SLocalization_Elements.Solid_Immovable_TreeLeaf_Name,
                description: SLocalization_Elements.Solid_Immovable_TreeLeaf_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_22")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_MOUNTING_BLOCK,
                displayName: SLocalization_Elements.Solid_Immovable_MountingBlock_Name,
                description: SLocalization_Elements.Solid_Immovable_MountingBlock_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_23")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IDENTIFIER_FIRE,
                displayName: SLocalization_Elements.Energy_Fire_Name,
                description: SLocalization_Elements.Energy_Fire_Description,
                contentType: SItemContentType.Element,
                subcategory: elementEnergySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            ));
            #endregion

            #endregion
        }
    }
}
