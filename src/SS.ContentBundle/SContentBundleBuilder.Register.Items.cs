using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterItems(ISGame game, ISItemDatabase itemDatabase)
        {
            // [ CATEGORIES ]
            itemDatabase.RegisterCategory(
                identifier: "powders",
                displayName: SLocalization.Item_Category_Powders,
                description: SLocalization.Item_Category_Powders_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            );

            itemDatabase.RegisterCategory(
                identifier: "liquids",
                displayName: SLocalization.Item_Category_Liquids,
                description: SLocalization.Item_Category_Liquids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            );

            itemDatabase.RegisterCategory(
                identifier: "gases",
                displayName: SLocalization.Item_Category_Gases,
                description: SLocalization.Item_Category_Gases_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            );

            itemDatabase.RegisterCategory(
                identifier: "solids",
                displayName: SLocalization.Item_Category_Solids,
                description: SLocalization.Item_Category_Solids_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            );

            itemDatabase.RegisterCategory(
                identifier: "walls",
                displayName: SLocalization.Item_Category_Walls,
                description: SLocalization.Item_Category_Walls_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            );

            itemDatabase.RegisterCategory(
                identifier: "energies",
                displayName: SLocalization.Item_Category_Energies,
                description: SLocalization.Item_Category_Energies_Description,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            );

            // [ ITEMS ]
            itemDatabase.RegisterItem(
                identifier: "element_dirt",
                displayName: SLocalization.Element_Solid_Movable_Dirt_Name,
                description: SLocalization.Element_Solid_Movable_Dirt_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1"),
                referencedType: typeof(SDirt)
            );

            itemDatabase.RegisterItem(
                identifier: "element_mud",
                displayName: SLocalization.Element_Solid_Movable_Mud_Name,
                description: SLocalization.Element_Solid_Movable_Mud_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_2"),
                referencedType: typeof(SMud)
            );

            itemDatabase.RegisterItem(
                identifier: "element_water",
                displayName: SLocalization.Element_Liquid_Water_Name,
                description: SLocalization.Element_Liquid_Water_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3"),
                referencedType: typeof(SWater)
            );

            itemDatabase.RegisterItem(
                identifier: "element_stone",
                displayName: SLocalization.Element_Solid_Movable_Stone_Name,
                description: SLocalization.Element_Solid_Movable_Stone_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_4"),
                referencedType: typeof(SStone)
            );

            itemDatabase.RegisterItem(
                identifier: "element_grass",
                displayName: SLocalization.Element_Solid_Movable_Grass_Name,
                description: SLocalization.Element_Solid_Movable_Grass_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_5"),
                referencedType: typeof(SGrass)
            );

            itemDatabase.RegisterItem(
                identifier: "element_ice",
                displayName: SLocalization.Element_Solid_Movable_Ice_Name,
                description: SLocalization.Element_Solid_Movable_Ice_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_6"),
                referencedType: typeof(SIce)
            );

            itemDatabase.RegisterItem(
                identifier: "element_sand",
                displayName: SLocalization.Element_Solid_Movable_Sand_Name,
                description: SLocalization.Element_Solid_Movable_Sand_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_7"),
                referencedType: typeof(SSand)
            );

            itemDatabase.RegisterItem(
                identifier: "element_snow",
                displayName: SLocalization.Element_Solid_Movable_Snow_Name,
                description: SLocalization.Element_Solid_Movable_Snow_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_8"),
                referencedType: typeof(SSnow)
            );

            itemDatabase.RegisterItem(
                identifier: "element_movable_corruption",
                displayName: SLocalization.Element_Solid_Movable_Corruption_Name,
                description: SLocalization.Element_Solid_Movable_Corruption_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_9"),
                referencedType: typeof(SMCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_lava",
                displayName: SLocalization.Element_Liquid_Lava_Name,
                description: SLocalization.Element_Liquid_Lava_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_10"),
                referencedType: typeof(SLava)
            );

            itemDatabase.RegisterItem(
                identifier: "element_acid",
                displayName: SLocalization.Element_Liquid_Acid_Name,
                description: SLocalization.Element_Liquid_Acid_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_11"),
                referencedType: typeof(SAcid)
            );

            itemDatabase.RegisterItem(
                identifier: "element_glass",
                displayName: SLocalization.Element_Solid_Immovable_Glass_Name,
                description: SLocalization.Element_Solid_Immovable_Glass_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_12"),
                referencedType: typeof(SGlass)
            );

            itemDatabase.RegisterItem(
                identifier: "element_metal",
                displayName: SLocalization.Element_Solid_Immovable_Metal_Name,
                description: SLocalization.Element_Solid_Immovable_Metal_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13"),
                referencedType: typeof(SMetal)
            );

            itemDatabase.RegisterItem(
                identifier: "element_wall",
                displayName: SLocalization.Element_Solid_Immovable_Wall_Name,
                description: SLocalization.Element_Solid_Immovable_Wall_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "walls",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14"),
                referencedType: typeof(SWall)
            );

            itemDatabase.RegisterItem(
                identifier: "element_wood",
                displayName: SLocalization.Element_Solid_Immovable_Wood_Name,
                description: SLocalization.Element_Solid_Immovable_Wood_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15"),
                referencedType: typeof(SWood)
            );

            itemDatabase.RegisterItem(
                identifier: "element_gas_corruption",
                displayName: SLocalization.Element_Gas_Corruption_Name,
                description: SLocalization.Element_Gas_Corruption_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_16"),
                referencedType: typeof(SGCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_liquid_corruption",
                displayName: SLocalization.Element_Liquid_Corruption_Name,
                description: SLocalization.Element_Liquid_Corruption_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_17"),
                referencedType: typeof(SLCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_immovable_corruption",
                displayName: SLocalization.Element_Solid_Immovable_Corruption_Name,
                description: SLocalization.Element_Solid_Immovable_Corruption_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_18"),
                referencedType: typeof(SIMCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_steam",
                displayName: SLocalization.Element_Gas_Steam_Name,
                description: SLocalization.Element_Gas_Steam_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_19"),
                referencedType: typeof(SSteam)
            );

            itemDatabase.RegisterItem(
                identifier: "element_smoke",
                displayName: SLocalization.Element_Gas_Smoke_Name,
                description: SLocalization.Element_Gas_Smoke_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20"),
                referencedType: typeof(SSmoke)
            );

            itemDatabase.RegisterItem(
                identifier: "element_red_brick",
                displayName: SLocalization.Element_Solid_Immovable_RedBrick_Name,
                description: SLocalization.Element_Solid_Immovable_RedBrick_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_21"),
                referencedType: typeof(SRedBrick)
            );

            itemDatabase.RegisterItem(
                identifier: "element_tree_leaf",
                displayName: SLocalization.Element_Solid_Immovable_TreeLeaf_Name,
                description: SLocalization.Element_Solid_Immovable_TreeLeaf_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_22"),
                referencedType: typeof(STreeLeaf)
            );

            itemDatabase.RegisterItem(
                identifier: "element_mounting_block",
                displayName: SLocalization.Element_Solid_Immovable_MountingBlock_Name,
                description: SLocalization.Element_Solid_Immovable_MountingBlock_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_23"),
                referencedType: typeof(SMountingBlock)
            );

            itemDatabase.RegisterItem(
                identifier: "element_fire",
                displayName: SLocalization.Element_Energy_Fire_Name,
                description: SLocalization.Element_Energy_Fire_Description,
                contentType: SItemContentType.Element,
                categoryIdentifier: "energies",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24"),
                referencedType: typeof(SFire)
            );
        }
    }
}
