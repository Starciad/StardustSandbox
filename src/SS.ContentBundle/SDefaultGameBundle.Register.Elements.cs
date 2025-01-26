using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables.Explosives;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle
    {
        protected override void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase)
        {
            elementDatabase.RegisterElement(new SDirt(game, SElementConstants.DIRT_IDENTIFIER));
            elementDatabase.RegisterElement(new SMud(game, SElementConstants.MUD_IDENTIFIER));
            elementDatabase.RegisterElement(new SWater(game, SElementConstants.WATER_IDENTIFIER));
            elementDatabase.RegisterElement(new SStone(game, SElementConstants.STONE_IDENTIFIER));
            elementDatabase.RegisterElement(new SGrass(game, SElementConstants.GRASS_IDENTIFIER));
            elementDatabase.RegisterElement(new SIce(game, SElementConstants.ICE_IDENTIFIER));
            elementDatabase.RegisterElement(new SSand(game, SElementConstants.SAND_IDENTIFIER));
            elementDatabase.RegisterElement(new SSnow(game, SElementConstants.SNOW_IDENTIFIER));
            elementDatabase.RegisterElement(new SMCorruption(game, SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SLava(game, SElementConstants.LAVA_IDENTIFIER));
            elementDatabase.RegisterElement(new SAcid(game, SElementConstants.ACID_IDENTIFIER));
            elementDatabase.RegisterElement(new SGlass(game, SElementConstants.GLASS_IDENTIFIER));
            elementDatabase.RegisterElement(new SIron(game, SElementConstants.IRON_IDENTIFIER));
            elementDatabase.RegisterElement(new SWall(game, SElementConstants.WALL_IDENTIFIER));
            elementDatabase.RegisterElement(new SWood(game, SElementConstants.WOOD_IDENTIFIER));
            elementDatabase.RegisterElement(new SGCorruption(game, SElementConstants.GAS_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SLCorruption(game, SElementConstants.LIQUID_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SIMCorruption(game, SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SSteam(game, SElementConstants.STEAM_IDENTIFIER));
            elementDatabase.RegisterElement(new SSmoke(game, SElementConstants.SMOKE_IDENTIFIER));
            elementDatabase.RegisterElement(new SRedBrick(game, SElementConstants.RED_BRICK_IDENTIFIER));
            elementDatabase.RegisterElement(new STreeLeaf(game, SElementConstants.TREE_LEAF_IDENTIFIER));
            elementDatabase.RegisterElement(new SMountingBlock(game, SElementConstants.MOUNTING_BLOCK_IDENTIFIER));
            elementDatabase.RegisterElement(new SFire(game, SElementConstants.FIRE_IDENTIFIER));
            elementDatabase.RegisterElement(new SLamp(game, SElementConstants.LAMP_IDENTIFIER));
            elementDatabase.RegisterElement(new SVoid(game, SElementConstants.VOID_IDENTIFIER));
            elementDatabase.RegisterElement(new SClone(game, SElementConstants.CLONE_IDENTIFIER));
            elementDatabase.RegisterElement(new SOil(game, SElementConstants.OIL_IDENTIFIER));
            elementDatabase.RegisterElement(new SSalt(game, SElementConstants.SALT_IDENTIFIER));
            elementDatabase.RegisterElement(new SSaltwater(game, SElementConstants.SALTWATER_IDENTIFIER));
            elementDatabase.RegisterElement(new SBomb(game, SElementConstants.BOMB_IDENTIFIER));
            elementDatabase.RegisterElement(new SDynamite(game, SElementConstants.DYNAMITE_IDENTIFIER));
            elementDatabase.RegisterElement(new STnt(game, SElementConstants.TNT_IDENTIFIER));
        }
    }
}
