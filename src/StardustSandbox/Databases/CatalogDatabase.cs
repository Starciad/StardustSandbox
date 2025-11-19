using StardustSandbox.Catalog;
using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Items;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.ToolSystem;

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

        internal static void Load()
        {
            CreateCatalogStructure();
            SetParentReferences();
            CalculateCatalogLengths();
        }

        private static void CreateCatalogStructure()
        {
            categories = [
                // Elements
                new(
                    Localization_Catalog.Category_Elements_Name,
                    Localization_Catalog.Category_Elements_Description,
                    AssetDatabase.GetTexture("texture_icon_element_1"),
                    [
                        // Powders
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Powders_Name,
                            description: Localization_Catalog.Subcategory_Elements_Powders_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_1"),
                            [
                                // [0] Dirt2
                                new(
                                    associatedType: typeof(Dirt),
                                    name: Localization_Elements.Solid_Movable_Dirt_Name,
                                    description: Localization_Elements.Solid_Movable_Dirt_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_1")
                                ),

                                // [1] Mud
                                new(
                                    associatedType: typeof(Mud),
                                    name: Localization_Elements.Solid_Movable_Mud_Name,
                                    description: Localization_Elements.Solid_Movable_Mud_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_2")
                                ),

                                // [2] Stone
                                new(
                                    associatedType: typeof(Stone),
                                    name: Localization_Elements.Solid_Movable_Stone_Name,
                                    description: Localization_Elements.Solid_Movable_Stone_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_4")
                                ),

                                // [3] Grass
                                new(
                                    associatedType: typeof(Grass),
                                    name: Localization_Elements.Solid_Movable_Grass_Name,
                                    description: Localization_Elements.Solid_Movable_Grass_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_5")
                                ),

                                // [4] Ice
                                new(
                                    associatedType: typeof(Ice),
                                    name: Localization_Elements.Solid_Movable_Ice_Name,
                                    description: Localization_Elements.Solid_Movable_Ice_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_6")
                                ),

                                // [5] Sand
                                new(
                                    associatedType: typeof(Sand),
                                    name: Localization_Elements.Solid_Movable_Sand_Name,
                                    description: Localization_Elements.Solid_Movable_Sand_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_7")
                                ),

                                // [6] Snow
                                new(
                                    associatedType: typeof(Snow),
                                    name: Localization_Elements.Solid_Movable_Snow_Name,
                                    description: Localization_Elements.Solid_Movable_Snow_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_8")
                                ),

                                // [7] Corruption
                                new(
                                    associatedType: typeof(MCorruption),
                                    name: Localization_Elements.Solid_Movable_Corruption_Name,
                                    description: Localization_Elements.Solid_Movable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_9")
                                ),

                                // [8] Salt
                                new(
                                    associatedType: typeof(Salt),
                                    name: Localization_Elements.Solid_Movable_Salt_Name,
                                    description: Localization_Elements.Solid_Movable_Salt_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_29")
                                ),

                                // [9] Ash
                                new(
                                    associatedType: typeof(Ash),
                                    name: Localization_Elements.Solid_Movable_Ash_Name,
                                    description: Localization_Elements.Solid_Movable_Ash_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_39")
                                )
                            ]
                        ),

                        // Liquids
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Liquids_Name,
                            description: Localization_Catalog.Subcategory_Elements_Liquids_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_3"),
                            [
                                // [0] Water
                                new(
                                    associatedType: typeof(Water),
                                    name: Localization_Elements.Liquid_Water_Name,
                                    description: Localization_Elements.Liquid_Water_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_3")
                                ),

                                // [1] Lava
                                new(
                                    associatedType: typeof(Lava),
                                    name: Localization_Elements.Liquid_Lava_Name,
                                    description: Localization_Elements.Liquid_Lava_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_10")
                                ),

                                // [2] Acid
                                new(
                                    associatedType: typeof(Acid),
                                    name: Localization_Elements.Liquid_Acid_Name,
                                    description: Localization_Elements.Liquid_Acid_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_11")
                                ),

                                // [3] Corruption
                                new(
                                    associatedType: typeof(LCorruption),
                                    name: Localization_Elements.Liquid_Corruption_Name,
                                    description: Localization_Elements.Liquid_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_17")
                                ),

                                // [4] Oil
                                new(
                                    associatedType: typeof(Oil),
                                    name: Localization_Elements.Liquid_Oil_Name,
                                    description: Localization_Elements.Liquid_Oil_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_28")
                                ),

                                // [5] Saltwater
                                new(
                                    associatedType: typeof(Saltwater),
                                    name: Localization_Elements.Liquid_Saltwater_Name,
                                    description: Localization_Elements.Liquid_Saltwater_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_30")
                                )
                            ]
                        ),

                        // Gases
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Gases_Name,
                            description: Localization_Catalog.Subcategory_Elements_Gases_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_20"),
                            [
                                // [0] Corruption
                                new(
                                    associatedType: typeof(GCorruption),
                                    name: Localization_Elements.Gas_Corruption_Name,
                                    description: Localization_Elements.Gas_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_16")
                                ),

                                // [1] Steam
                                new(
                                    associatedType: typeof(Steam),
                                    name: Localization_Elements.Gas_Steam_Name,
                                    description: Localization_Elements.Gas_Steam_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_19")
                                ),

                                // [2] Smoke
                                new(
                                    associatedType: typeof(Smoke),
                                    name: Localization_Elements.Gas_Smoke_Name,
                                    description: Localization_Elements.Gas_Smoke_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_20")
                                ),
                            ]
                        ),

                        // Solids
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Solids_Name,
                            description: Localization_Catalog.Subcategory_Elements_Solids_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_15"),
                            [
                                // [0] Glass
                                new(
                                    associatedType: typeof(Glass),
                                    name: Localization_Elements.Solid_Immovable_Glass_Name,
                                    description: Localization_Elements.Solid_Immovable_Glass_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_12")
                                ),

                                // [1] Iron
                                new(
                                    associatedType: typeof(Iron),
                                    name: Localization_Elements.Solid_Immovable_Iron_Name,
                                    description: Localization_Elements.Solid_Immovable_Iron_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_13")
                                ),

                                // [2] Wall
                                new(
                                    associatedType: typeof(Wall),
                                    name: Localization_Elements.Solid_Immovable_Wall_Name,
                                    description: Localization_Elements.Solid_Immovable_Wall_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_14")
                                ),

                                // [3] Wood
                                new(
                                    associatedType: typeof(Wood),
                                    name: Localization_Elements.Solid_Immovable_Wood_Name,
                                    description: Localization_Elements.Solid_Immovable_Wood_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_15")
                                ),

                                // [4] Corruption
                                new(
                                    associatedType: typeof(IMCorruption),
                                    name: Localization_Elements.Solid_Immovable_Corruption_Name,
                                    description: Localization_Elements.Solid_Immovable_Corruption_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_18")
                                ),

                                // [5] Red Brick
                                new(
                                    associatedType: typeof(RedBrick),
                                    name: Localization_Elements.Solid_Immovable_RedBrick_Name,
                                    description: Localization_Elements.Solid_Immovable_RedBrick_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_21")
                                ),

                                // [6] Tree Leaf
                                new(
                                    associatedType: typeof(TreeLeaf),
                                    name: Localization_Elements.Solid_Immovable_TreeLeaf_Name,
                                    description: Localization_Elements.Solid_Immovable_TreeLeaf_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_22")
                                ),

                                // [7] Mounting Block
                                new(
                                    associatedType: typeof(MountingBlock),
                                    name: Localization_Elements.Solid_Immovable_MountingBlock_Name,
                                    description: Localization_Elements.Solid_Immovable_MountingBlock_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_23")
                                ),

                                // [8] Lamp
                                new(
                                    associatedType: typeof(Lamp),
                                    name: Localization_Elements.Solid_Immovable_Lamp_Name,
                                    description: Localization_Elements.Solid_Immovable_Lamp_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_25")
                                ),

                                // [9] Dry Sponge
                                new(
                                    associatedType: typeof(DrySponge),
                                    name: Localization_Elements.Solid_Immovable_DrySponge_Name,
                                    description: Localization_Elements.Solid_Immovable_DrySponge_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_34")
                                ),

                                // [10] Wet Sponge
                                new(
                                    associatedType: typeof(WetSponge),
                                    name: Localization_Elements.Solid_Immovable_WetSponge_Name,
                                    description: Localization_Elements.Solid_Immovable_WetSponge_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_35")
                                ),

                                // [11] Gold
                                new(
                                    associatedType: typeof(Gold),
                                    name: Localization_Elements.Solid_Immovable_Gold_Name,
                                    description: Localization_Elements.Solid_Immovable_Gold_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_36")
                                ),
                            ]
                        ),

                        // Energies
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Energies_Name,
                            description: Localization_Catalog.Subcategory_Elements_Energies_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_24"),
                            [
                                // [0] Fire
                                new(
                                    associatedType: typeof(Fire),
                                    name: Localization_Elements.Energy_Fire_Name,
                                    description: Localization_Elements.Energy_Fire_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_24")
                                )
                            ]
                        ),

                        // Explosives
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Explosives_Name,
                            description: Localization_Catalog.Subcategory_Elements_Explosives_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_33"),
                            [
                                // [0] Bomb
                                new(
                                    associatedType: typeof(Bomb),
                                    name: Localization_Elements.Solid_Movable_Bomb_Name,
                                    description: Localization_Elements.Solid_Movable_Bomb_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_31")
                                ),

                                // [1] Dynamite
                                new(
                                    associatedType: typeof(Dynamite),
                                    name: Localization_Elements.Solid_Movable_Dynamite_Name,
                                    description: Localization_Elements.Solid_Movable_Dynamite_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_32")
                                ),

                                // [2] TNT
                                new(
                                    associatedType: typeof(Tnt),
                                    name: Localization_Elements.Solid_Movable_TNT_Name,
                                    description: Localization_Elements.Solid_Movable_TNT_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_33")
                                ),
                            ]
                        ),

                        // Technologies
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Technologies_Name,
                            description: Localization_Catalog.Subcategory_Elements_Technologies_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_37"),
                            [
                                // [0] Heater
                                new(
                                    associatedType: typeof(Heater),
                                    name: Localization_Elements.Solid_Immovable_Heater_Name,
                                    description: Localization_Elements.Solid_Immovable_Heater_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_37")
                                ),

                                // [1] Freezer
                                new(
                                    associatedType: typeof(Freezer),
                                    name: Localization_Elements.Solid_Immovable_Freezer_Name,
                                    description: Localization_Elements.Solid_Immovable_Freezer_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_38")
                                ),
                            ]
                        ),

                        // Specials
                        new(
                            name: Localization_Catalog.Subcategory_Elements_Specials_Name,
                            description: Localization_Catalog.Subcategory_Elements_Specials_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_element_26"),
                            [
                                // [0] Void
                                new(
                                    associatedType: typeof(Elements.Solids.Immovables.Void),
                                    name: Localization_Elements.Solid_Immovable_Void_Name,
                                    description: Localization_Elements.Solid_Immovable_Void_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_26")
                                ),

                                // [1] Clone
                                new(
                                    associatedType: typeof(Clone),
                                    name: Localization_Elements.Solid_Immovable_Clone_Name,
                                    description: Localization_Elements.Solid_Immovable_Clone_Description,
                                    contentType: ItemContentType.Element,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_element_27")
                                ),
                            ]
                        ),
                    ]
                ),

                // Tools
                new(
                    Localization_Catalog.Category_Tools_Name,
                    Localization_Catalog.Category_Tools_Description,
                    AssetDatabase.GetTexture("texture_icon_gui_53"),
                    [
                        // Environment
                        new(
                            name: Localization_Catalog.Subcategory_Tools_Environment_Name,
                            description: Localization_Catalog.Subcategory_Tools_Environment_Description,
                            iconTexture: AssetDatabase.GetTexture("texture_icon_gui_54"),
                            [
                                // [0] Heat Tool
                                new(
                                    associatedType: typeof(HeatTool),
                                    name: Localization_Tools.Environment_Heat_Name,
                                    description: Localization_Tools.Environment_Heat_Description,
                                    contentType: ItemContentType.Tool,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_tool_1")
                                ),

                                // [1] Freeze Tool
                                new(
                                    associatedType: typeof(FreezeTool),
                                    name: Localization_Tools.Environment_Freeze_Name,
                                    description: Localization_Tools.Environment_Freeze_Description,
                                    contentType: ItemContentType.Tool,
                                    iconTexture: AssetDatabase.GetTexture("texture_icon_tool_2")
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
