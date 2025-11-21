using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Elements;
using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Gases;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;

using System;

namespace StardustSandbox.Databases
{
    internal static class ElementDatabase
    {
        private static Element[] elements;

        internal static void Load()
        {
            elements = [
                new Dirt(AAP64ColorPalette.Burgundy, ElementIndex.Dirt, AssetDatabase.GetTexture("texture_element_1")),
                new Mud(new(75, 36, 38, 255), ElementIndex.Mud, AssetDatabase.GetTexture("texture_element_2")),
                new Water(new(8, 120, 284, 255), ElementIndex.Water, AssetDatabase.GetTexture("texture_element_3")),
                new Stone(new(66, 65, 65, 255), ElementIndex.Stone, AssetDatabase.GetTexture("texture_element_4")),
                new Grass(new(69, 110, 55, 255), ElementIndex.Grass, AssetDatabase.GetTexture("texture_element_5")),
                new Ice(new(117, 215, 246, 255), ElementIndex.Ice, AssetDatabase.GetTexture("texture_element_6")),
                new Sand(new(248, 246, 68, 255), ElementIndex.Sand, AssetDatabase.GetTexture("texture_element_7")),
                new Snow(new(202, 242, 239, 255), ElementIndex.Snow, AssetDatabase.GetTexture("texture_element_8")),
                new MCorruption(AAP64ColorPalette.PurpleGray, ElementIndex.MCorruption, AssetDatabase.GetTexture("texture_element_9")),
                new Lava(AAP64ColorPalette.Orange, ElementIndex.Lava, AssetDatabase.GetTexture("texture_element_10")),
                new Acid(new(059, 167, 005, 255), ElementIndex.Acid, AssetDatabase.GetTexture("texture_element_11")),
                new Glass(new(249, 253, 254, 21), ElementIndex.Glass, AssetDatabase.GetTexture("texture_element_12")),
                new Iron(new(66, 66, 66, 255), ElementIndex.Iron, AssetDatabase.GetTexture("texture_element_13")),
                new Wall(new(22, 99, 50, 255), ElementIndex.Wall, AssetDatabase.GetTexture("texture_element_14")),
                new Wood(new(67, 34, 0, 255), ElementIndex.Wood, AssetDatabase.GetTexture("texture_element_15")),
                new GCorruption(new(169, 76, 192, 181), ElementIndex.GCorruption, AssetDatabase.GetTexture("texture_element_16")),
                new LCorruption(AAP64ColorPalette.PurpleGray, ElementIndex.LCorruption, AssetDatabase.GetTexture("texture_element_17")),
                new IMCorruption( AAP64ColorPalette.PurpleGray, ElementIndex.IMCorruption, AssetDatabase.GetTexture("texture_element_18")),
                new Steam(new(171, 208, 218, 136), ElementIndex.Steam, AssetDatabase.GetTexture("texture_element_19")),
                new Smoke(new(56, 56, 56, 191), ElementIndex.Smoke, AssetDatabase.GetTexture("texture_element_20")),
                new RedBrick(AAP64ColorPalette.Crimson, ElementIndex.RedBrick, AssetDatabase.GetTexture("texture_element_21")),
                new TreeLeaf(AAP64ColorPalette.MossGreen, ElementIndex.TreeLeaf, AssetDatabase.GetTexture("texture_element_22")),
                new MountingBlock(AAP64ColorPalette.White, ElementIndex.MountingBlock, AssetDatabase.GetTexture("texture_element_23")),
                new Fire(AAP64ColorPalette.Amber, ElementIndex.Fire, AssetDatabase.GetTexture("texture_element_24")),
                new Lamp(AAP64ColorPalette.Rust, ElementIndex.Lamp, AssetDatabase.GetTexture("texture_element_25")),
                new Elements.Solids.Immovables.Void(AAP64ColorPalette.DarkGray, ElementIndex.Void, AssetDatabase.GetTexture("texture_element_26")),
                new Clone(AAP64ColorPalette.Amber, ElementIndex.Clone, AssetDatabase.GetTexture("texture_element_27")),
                new Oil(Color.Black, ElementIndex.Oil, AssetDatabase.GetTexture("texture_element_28")),
                new Salt(AAP64ColorPalette.White, ElementIndex.Salt, AssetDatabase.GetTexture("texture_element_29")),
                new Saltwater(new(62, 182, 249, 255), ElementIndex.Saltwater, AssetDatabase.GetTexture("texture_element_30")),
                new Bomb(AAP64ColorPalette.Charcoal, ElementIndex.Bomb, AssetDatabase.GetTexture("texture_element_31")),
                new Dynamite(AAP64ColorPalette.OrangeRed, ElementIndex.Dynamite, AssetDatabase.GetTexture("texture_element_32")),
                new Tnt(AAP64ColorPalette.DarkRed, ElementIndex.Tnt, AssetDatabase.GetTexture("texture_element_33")),
                new DrySponge(AAP64ColorPalette.Amber, ElementIndex.DrySponge, AssetDatabase.GetTexture("texture_element_34")),
                new WetSponge(AAP64ColorPalette.Amber.Darken(0.25f), ElementIndex.WetSponge, AssetDatabase.GetTexture("texture_element_35")),
                new Gold(AAP64ColorPalette.Gold, ElementIndex.Gold, AssetDatabase.GetTexture("texture_element_36")),
                new Heater(AAP64ColorPalette.DarkRed, ElementIndex.Heater, AssetDatabase.GetTexture("texture_element_37")),
                new Freezer(AAP64ColorPalette.NavyBlue, ElementIndex.Freezer, AssetDatabase.GetTexture("texture_element_38")),
                new Ash(AAP64ColorPalette.LightGrayBlue, ElementIndex.Ash, AssetDatabase.GetTexture("texture_element_39")),
            ];
        }

        internal static Element GetElementByIndex(ElementIndex index)
        {
            return elements[(byte)index];
        }

        internal static Element GetElementByType(Type type)
        {
            return Array.Find(elements, x => x.GetType() == type);
        }
    }
}
