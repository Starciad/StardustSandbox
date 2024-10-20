using StardustSandbox.Game.Enums.Items;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Databases
{
    public sealed partial class SItemDatabase
    {
        private void BuildCategories()
        {
            AddCategory("powders", "Powders", "", this._assetDatabase.GetTexture("icon_element_1"));
            AddCategory("liquids", "Liquids", "", this._assetDatabase.GetTexture("icon_element_3"));
            AddCategory("gases", "Gases", "", this._assetDatabase.GetTexture("icon_element_20"));
            AddCategory("solids", "Solids", "", this._assetDatabase.GetTexture("icon_element_13"));
            AddCategory("walls", "Walls", "", this._assetDatabase.GetTexture("icon_element_14"));

            // "electronics"
            // "explosives"
            // "entities"
            // "bgos"
            // "tools"
            // "specials"
        }

        private void BuildItems()
        {
            AddItem("element_dirt", "Dirt", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_1"), typeof(SDirt));
            AddItem("element_mud", "Mud", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_2"), typeof(SMud));
            AddItem("element_water", "Water", string.Empty, SItemContentType.Element, this.categories["liquids"], this._assetDatabase.GetTexture("icon_element_3"), typeof(SWater));
            AddItem("element_stone", "Stone", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_4"), typeof(SStone));
            AddItem("element_grass", "Grass", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_5"), typeof(SGrass));
            AddItem("element_ice", "Ice", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_6"), typeof(SIce));
            AddItem("element_sand", "Sand", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_7"), typeof(SSand));
            AddItem("element_snow", "Snow", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_8"), typeof(SSnow));
            AddItem("element_movable_corruption", "Corruption (Movable)", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_9"), typeof(SMCorruption));
            AddItem("element_lava", "Lava", string.Empty, SItemContentType.Element, this.categories["liquids"], this._assetDatabase.GetTexture("icon_element_10"), typeof(SLava));
            AddItem("element_acid", "Acid", string.Empty, SItemContentType.Element, this.categories["liquids"], this._assetDatabase.GetTexture("icon_element_11"), typeof(SAcid));
            AddItem("element_glass", "Glass", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_12"), typeof(SGlass));
            AddItem("element_metal", "Metal", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_13"), typeof(SMetal));
            AddItem("element_wall", "Wall", string.Empty, SItemContentType.Element, this.categories["walls"], this._assetDatabase.GetTexture("icon_element_14"), typeof(SWall));
            AddItem("element_wood", "Wood", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_15"), typeof(SWood));
            AddItem("element_gas_corruption", "Corruption (Gas)", string.Empty, SItemContentType.Element, this.categories["gases"], this._assetDatabase.GetTexture("icon_element_16"), typeof(SGCorruption));
            AddItem("element_liquid_corruption", "Corruption (Liquid)", string.Empty, SItemContentType.Element, this.categories["liquids"], this._assetDatabase.GetTexture("icon_element_17"), typeof(SLCorruption));
            AddItem("element_immovable_corruption", "Corruption (Immovable)", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_18"), typeof(SIMCorruption));
            AddItem("element_steam", "Steam", string.Empty, SItemContentType.Element, this.categories["gases"], this._assetDatabase.GetTexture("icon_element_19"), typeof(SSteam));
            AddItem("element_smoke", "Smoke", string.Empty, SItemContentType.Element, this.categories["gases"], this._assetDatabase.GetTexture("icon_element_20"), typeof(SSmoke));
            AddItem("element_red_brick", "Red Brick", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_21"), typeof(SRedBrick));
            AddItem("element_tree_leaf", "Tree Leaf", string.Empty, SItemContentType.Element, this.categories["solids"], this._assetDatabase.GetTexture("icon_element_22"), typeof(STreeLeaf));
            AddItem("element_mounting_block", "Mounting Block", string.Empty, SItemContentType.Element, this.categories["powders"], this._assetDatabase.GetTexture("icon_element_23"), typeof(SMountingBlock));
        }
    }
}
