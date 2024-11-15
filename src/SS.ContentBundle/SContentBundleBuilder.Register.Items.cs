using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterItems(ISGame game, SItemDatabase itemDatabase)
        {
            // [ CATEGORIES ]
            itemDatabase.RegisterCategory(
                identifier: "powders",
                displayName: "Powders",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1")
            );

            itemDatabase.RegisterCategory(
                identifier: "liquids",
                displayName: "Liquids",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3")
            );

            itemDatabase.RegisterCategory(
                identifier: "gases",
                displayName: "Gases",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20")
            );

            itemDatabase.RegisterCategory(
                identifier: "solids",
                displayName: "Solids",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13")
            );

            itemDatabase.RegisterCategory(
                identifier: "walls",
                displayName: "Walls",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14")
            );

            itemDatabase.RegisterCategory(
                identifier: "energies",
                displayName: "Energies",
                description: string.Empty,
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24")
            );

            // [ ITEMS ]
            itemDatabase.RegisterItem(
                identifier: "element_dirt",
                displayName: "Dirt",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_1"),
                referencedType: typeof(SDirt)
            );

            itemDatabase.RegisterItem(
                identifier: "element_mud",
                displayName: "Mud",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_2"),
                referencedType: typeof(SMud)
            );

            itemDatabase.RegisterItem(
                identifier: "element_water",
                displayName: "Water",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_3"),
                referencedType: typeof(SWater)
            );

            itemDatabase.RegisterItem(
                identifier: "element_stone",
                displayName: "Stone",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_4"),
                referencedType: typeof(SStone)
            );

            itemDatabase.RegisterItem(
                identifier: "element_grass",
                displayName: "Grass",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_5"),
                referencedType: typeof(SGrass)
            );

            itemDatabase.RegisterItem(
                identifier: "element_ice",
                displayName: "Ice",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_6"),
                referencedType: typeof(SIce)
            );

            itemDatabase.RegisterItem(
                identifier: "element_sand",
                displayName: "Sand",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_7"),
                referencedType: typeof(SSand)
            );

            itemDatabase.RegisterItem(
                identifier: "element_snow",
                displayName: "Snow",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_8"),
                referencedType: typeof(SSnow)
            );

            itemDatabase.RegisterItem(
                identifier: "element_movable_corruption",
                displayName: "Corruption (Movable)",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "powders",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_9"),
                referencedType: typeof(SMCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_lava",
                displayName: "Lava",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_10"),
                referencedType: typeof(SLava)
            );

            itemDatabase.RegisterItem(
                identifier: "element_acid",
                displayName: "Acid",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_11"),
                referencedType: typeof(SAcid)
            );

            itemDatabase.RegisterItem(
                identifier: "element_glass",
                displayName: "Glass",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_12"),
                referencedType: typeof(SGlass)
            );

            itemDatabase.RegisterItem(
                identifier: "element_metal",
                displayName: "Metal",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_13"),
                referencedType: typeof(SMetal)
            );

            itemDatabase.RegisterItem(
                identifier: "element_wall",
                displayName: "Wall",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "walls",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_14"),
                referencedType: typeof(SWall)
            );

            itemDatabase.RegisterItem(
                identifier: "element_wood",
                displayName: "Wood",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_15"),
                referencedType: typeof(SWood)
            );

            itemDatabase.RegisterItem(
                identifier: "element_gas_corruption",
                displayName: "Corruption (Gas)",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_16"),
                referencedType: typeof(SGCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_liquid_corruption",
                displayName: "Corruption (Liquid)",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "liquids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_17"),
                referencedType: typeof(SLCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_immovable_corruption",
                displayName: "Corruption (Immovable)",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_18"),
                referencedType: typeof(SIMCorruption)
            );

            itemDatabase.RegisterItem(
                identifier: "element_steam",
                displayName: "Steam",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_19"),
                referencedType: typeof(SSteam)
            );

            itemDatabase.RegisterItem(
                identifier: "element_smoke",
                displayName: "Smoke",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "gases",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_20"),
                referencedType: typeof(SSmoke)
            );

            itemDatabase.RegisterItem(
                identifier: "element_red_brick",
                displayName: "Red Brick",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_21"),
                referencedType: typeof(SRedBrick)
            );

            itemDatabase.RegisterItem(
                identifier: "element_tree_leaf",
                displayName: "Tree Leaf",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_22"),
                referencedType: typeof(STreeLeaf)
            );

            itemDatabase.RegisterItem(
                identifier: "element_mounting_block",
                displayName: "Mounting Block",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "solids",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_23"),
                referencedType: typeof(SMountingBlock)
            );

            itemDatabase.RegisterItem(
                identifier: "element_fire",
                displayName: "Fire",
                description: string.Empty,
                contentType: SItemContentType.Element,
                categoryIdentifier: "energies",
                iconTexture: game.AssetDatabase.GetTexture("icon_element_24"),
                referencedType: typeof(SFire)
            );
        }
    }
}
