using StardustSandbox.ContentBundle.Localization.Catalog;
using StardustSandbox.ContentBundle.Localization.Elements;
using StardustSandbox.ContentBundle.Localization.Tools;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle
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

            SCategory toolCategory = new(
                "tools",
                SLocalization_Catalog.Category_Tools_Name,
                SLocalization_Catalog.Category_Tools_Description,
                game.AssetDatabase.GetTexture("icon_gui_53")
            );

            catalogDatabase.RegisterCategory(elementCategory);
            catalogDatabase.RegisterCategory(toolCategory);
            #endregion

            #region Subcategories

            #region Elements
            SSubcategory elementPowderSubcategory = new(
                parent: elementCategory,
                identifier: "powders",
                name: SLocalization_Catalog.Subcategory_Elements_Powders_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Powders_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            );

            SSubcategory elementLiquidSubcategory = new(
                parent: elementCategory,
                identifier: "liquids",
                name: SLocalization_Catalog.Subcategory_Elements_Liquids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Liquids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            );

            SSubcategory elementGasSubcategory = new(
                parent: elementCategory,
                identifier: "gases",
                name: SLocalization_Catalog.Subcategory_Elements_Gases_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Gases_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            );

            SSubcategory elementSolidSubcategory = new(
                parent: elementCategory,
                identifier: "solids",
                name: SLocalization_Catalog.Subcategory_Elements_Solids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Solids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15")
            );

            SSubcategory elementEnergySubcategory = new(
                parent: elementCategory,
                identifier: "energies",
                name: SLocalization_Catalog.Subcategory_Elements_Energies_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Energies_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            );

            SSubcategory elementExplosiveSubcategory = new(
                parent: elementCategory,
                identifier: "explosives",
                name: SLocalization_Catalog.Subcategory_Elements_Explosives_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Explosives_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_33")
            );

            SSubcategory elementTechnologySubcategory = new(
                parent: elementCategory,
                identifier: "technologies",
                name: SLocalization_Catalog.Subcategory_Elements_Technologies_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Technologies_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_37")
            );

            SSubcategory elementSpecialSubcategory = new(
                parent: elementCategory,
                identifier: "specials",
                name: SLocalization_Catalog.Subcategory_Elements_Specials_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Specials_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_26")
            );

            elementCategory.AddSubcategory(elementPowderSubcategory);
            elementCategory.AddSubcategory(elementLiquidSubcategory);
            elementCategory.AddSubcategory(elementGasSubcategory);
            elementCategory.AddSubcategory(elementSolidSubcategory);
            elementCategory.AddSubcategory(elementEnergySubcategory);
            elementCategory.AddSubcategory(elementExplosiveSubcategory);
            elementCategory.AddSubcategory(elementTechnologySubcategory);
            elementCategory.AddSubcategory(elementSpecialSubcategory);
            #endregion

            #region Tools
            SSubcategory toolEnvironmentSubcategory = new(
                parent: toolCategory,
                identifier: "environment",
                name: SLocalization_Catalog.Subcategory_Tools_Environment_Name,
                description: SLocalization_Catalog.Subcategory_Tools_Environment_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_gui_54")
            );

            toolCategory.AddSubcategory(toolEnvironmentSubcategory);
            #endregion

            #endregion

            #region Items

            #region Elements
            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DIRT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Dirt_Name,
                description: SLocalization_Elements.Solid_Movable_Dirt_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MUD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Mud_Name,
                description: SLocalization_Elements.Solid_Movable_Mud_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_2")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WATER_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Water_Name,
                description: SLocalization_Elements.Liquid_Water_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.STONE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Stone_Name,
                description: SLocalization_Elements.Solid_Movable_Stone_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_4")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GRASS_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Grass_Name,
                description: SLocalization_Elements.Solid_Movable_Grass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_5")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.ICE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Ice_Name,
                description: SLocalization_Elements.Solid_Movable_Ice_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_6")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SAND_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Sand_Name,
                description: SLocalization_Elements.Solid_Movable_Sand_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_7")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SNOW_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Snow_Name,
                description: SLocalization_Elements.Solid_Movable_Snow_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_8")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Corruption_Name,
                description: SLocalization_Elements.Solid_Movable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_9")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LAVA_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Lava_Name,
                description: SLocalization_Elements.Liquid_Lava_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_10")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.ACID_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Acid_Name,
                description: SLocalization_Elements.Liquid_Acid_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_11")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GLASS_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Glass_Name,
                description: SLocalization_Elements.Solid_Immovable_Glass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_12")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IRON_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Iron_Name,
                description: SLocalization_Elements.Solid_Immovable_Iron_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WALL_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Wall_Name,
                description: SLocalization_Elements.Solid_Immovable_Wall_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WOOD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Wood_Name,
                description: SLocalization_Elements.Solid_Immovable_Wood_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GAS_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Gas_Corruption_Name,
                description: SLocalization_Elements.Gas_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_16")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LIQUID_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Corruption_Name,
                description: SLocalization_Elements.Liquid_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_17")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Corruption_Name,
                description: SLocalization_Elements.Solid_Immovable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_18")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.STEAM_IDENTIFIER,
                name: SLocalization_Elements.Gas_Steam_Name,
                description: SLocalization_Elements.Gas_Steam_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_19")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SMOKE_IDENTIFIER,
                name: SLocalization_Elements.Gas_Smoke_Name,
                description: SLocalization_Elements.Gas_Smoke_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.RED_BRICK_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_RedBrick_Name,
                description: SLocalization_Elements.Solid_Immovable_RedBrick_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_21")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.TREE_LEAF_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_TreeLeaf_Name,
                description: SLocalization_Elements.Solid_Immovable_TreeLeaf_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_22")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MOUNTING_BLOCK_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_MountingBlock_Name,
                description: SLocalization_Elements.Solid_Immovable_MountingBlock_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_23")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.FIRE_IDENTIFIER,
                name: SLocalization_Elements.Energy_Fire_Name,
                description: SLocalization_Elements.Energy_Fire_Description,
                contentType: SItemContentType.Element,
                subcategory: elementEnergySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LAMP_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Lamp_Name,
                description: SLocalization_Elements.Solid_Immovable_Lamp_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_25")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.VOID_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Void_Name,
                description: SLocalization_Elements.Solid_Immovable_Void_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSpecialSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_26")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.CLONE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Clone_Name,
                description: SLocalization_Elements.Solid_Immovable_Clone_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSpecialSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_27")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.OIL_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Oil_Name,
                description: SLocalization_Elements.Liquid_Oil_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_28")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SALT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Salt_Name,
                description: SLocalization_Elements.Solid_Movable_Salt_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_29")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SALTWATER_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Saltwater_Name,
                description: SLocalization_Elements.Liquid_Saltwater_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_30")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.BOMB_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Bomb_Name,
                description: SLocalization_Elements.Solid_Movable_Bomb_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_31")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DYNAMITE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Dynamite_Name,
                description: SLocalization_Elements.Solid_Movable_Dynamite_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_32")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.TNT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_TNT_Name,
                description: SLocalization_Elements.Solid_Movable_TNT_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_33")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DRY_SPONGE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_DrySponge_Name,
                description: SLocalization_Elements.Solid_Immovable_DrySponge_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_34")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WET_SPONGE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_WetSponge_Name,
                description: SLocalization_Elements.Solid_Immovable_WetSponge_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_35")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GOLD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Gold_Name,
                description: SLocalization_Elements.Solid_Immovable_Gold_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_36")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.HEATER_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Heater_Name,
                description: SLocalization_Elements.Solid_Immovable_Heater_Description,
                contentType: SItemContentType.Element,
                subcategory: elementTechnologySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_37")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.FREEZER_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Freezer_Name,
                description: SLocalization_Elements.Solid_Immovable_Freezer_Description,
                contentType: SItemContentType.Element,
                subcategory: elementTechnologySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_38")
            ));
            #endregion

            #region Tools
            catalogDatabase.RegisterItem(new(
                identifier: SToolConstants.HEAT_IDENTIFIER,
                name: SLocalization_Tools.Environment_Heat_Name,
                description: SLocalization_Tools.Environment_Heat_Description,
                contentType: SItemContentType.Tool,
                subcategory: toolEnvironmentSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_tool_1")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SToolConstants.FREEZE_IDENTIFIER,
                name: SLocalization_Tools.Environment_Freeze_Name,
                description: SLocalization_Tools.Environment_Freeze_Description,
                contentType: SItemContentType.Tool,
                subcategory: toolEnvironmentSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("icon_tool_2")
            ));
            #endregion

            #endregion
        }
    }
}
