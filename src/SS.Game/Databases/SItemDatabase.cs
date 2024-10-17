using StardustSandbox.Game.Enums.Items;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Databases
{
    public sealed class SItemDatabase : SGameObject
    {
        public SItemCategory[] Categories => this.categories;
        public SItem[] Items => this.items;

        private SItem[] items;
        private SItemCategory[] categories;

        private readonly SAssetDatabase _assetDatabase;

        public SItemDatabase(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance)
        {
            this._assetDatabase = assetDatabase;
        }

        protected override void OnInitialize()
        {
            BuildCategories();
            BuildItems();

            // After the categories and items are constructed individually, all items that are part of the category are allocated to the categories.
            for (int i = 0; i < this.categories.Length; i++)
            {
                SItemCategory category = this.categories[i];

                for (int j = 0; j < this.items.Length; j++)
                {
                    SItem item = this.items[j];

                    if (item.Category == category)
                    {
                        category.AddItem(item);
                    }
                }
            }
        }

        private void BuildCategories()
        {
            this.categories = [
                new SItemCategory("powders", "Powders", "", null),
                new SItemCategory("liquids", "Liquids", "", null),
                new SItemCategory("gases", "Gases", "", null),
                new SItemCategory("solids", "Solids", "", null),
                new SItemCategory("walls", "Walls", "", null),
                // new SItemCategory("electronics", "", "", null),
                // new SItemCategory("explosives", "", "", null),
                // new SItemCategory("entities", "", "", null),
                // new SItemCategory("bgos", "", "", null),
                // new SItemCategory("tools", "", "", null),
                // new SItemCategory("specials", "", "", null),
            ];
        }

        private void BuildItems()
        {
            this.items = [
                #region ELEMENTS
                new(
                    identifier: "element_dirt",
                    displayName: "Dirt",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_1"),
                    referencedType: typeof(SDirt)
                ),

                new(
                    identifier: "element_mud",
                    displayName: "Mud",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_2"),
                    referencedType: typeof(SMud)
                ),

                new(
                    identifier: "element_water",
                    displayName: "Water",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Liquids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_3"),
                    referencedType: typeof(SWater)
                ),

                new(
                    identifier: "element_stone",
                    displayName: "Stone",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_4"),
                    referencedType: typeof(SStone)
                ),

                new(
                    identifier: "element_grass",
                    displayName: "Grass",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_5"),
                    referencedType: typeof(SGrass)
                ),

                new(
                    identifier: "element_ice",
                    displayName: "Ice",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_6"),
                    referencedType: typeof(SIce)
                ),

                new(
                    identifier: "element_sand",
                    displayName: "Sand",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_7"),
                    referencedType: typeof(SSand)
                ),

                new(
                    identifier: "element_snow",
                    displayName: "Snow",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_8"),
                    referencedType: typeof(SSnow)
                ),

                new(
                    identifier: "element_movable_corruption",
                    displayName: "Corruption (Movable)",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Powders],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_9"),
                    referencedType: typeof(SMCorruption)
                ),

                new(
                    identifier: "element_lava",
                    displayName: "Lava",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Liquids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_10"),
                    referencedType: typeof(SLava)
                ),

                new(
                    identifier: "element_acid",
                    displayName: "Acid",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Liquids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_11"),
                    referencedType: typeof(SAcid)
                ),

                new(
                    identifier: "element_glass",
                    displayName: "Glass",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Solids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_12"),
                    referencedType: typeof(SGlass)
                ),

                new(
                    identifier: "element_metal",
                    displayName: "Metal",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Solids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_13"),
                    referencedType: typeof(SMetal)
                ),

                new(
                    identifier: "element_wall",
                    displayName: "Wall",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Walls],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_14"),
                    referencedType: typeof(SWall)
                ),

                new(
                    identifier: "element_wood",
                    displayName: "Wood",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Solids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_15"),
                    referencedType: typeof(SWood)
                ),

                new(
                    identifier: "element_gas_corruption",
                    displayName: "Corruption (Gas)",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Gases],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_16"),
                    referencedType: typeof(SGCorruption)
                ),

                new(
                    identifier: "element_liquid_corruption",
                    displayName: "Corruption (Liquid)",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Liquids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_17"),
                    referencedType: typeof(SLCorruption)
                ),

                new(
                    identifier: "element_immovable_corruption",
                    displayName: "Corruption (Immovable)",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Solids],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_18"),
                    referencedType: typeof(SIMCorruption)
                ),

                new(
                    identifier: "element_steam",
                    displayName: "Steam",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Gases],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_19"),
                    referencedType: typeof(SSteam)
                ),

                new(
                    identifier: "element_smoke",
                    displayName: "Smoke",
                    description: string.Empty,
                    contentType: SItemContentType.Element,
                    category: this.categories[(byte)SItemCategoryId.Gases],
                    iconTexture: this._assetDatabase.GetTexture("icon_element_20"),
                    referencedType: typeof(SSmoke)
                ),
                #endregion
            ];
        }

        public SItem GetItemById(string id)
        {
            return Array.Find(this.items, x => x.Identifier == id);
        }
    }
}
