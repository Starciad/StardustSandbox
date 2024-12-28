using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase)
        {
            elementDatabase.RegisterElement(new SDirt(game, SElementConstants.IDENTIFIER_DIRT));
            elementDatabase.RegisterElement(new SMud(game, SElementConstants.IDENTIFIER_MUD));
            elementDatabase.RegisterElement(new SWater(game, SElementConstants.IDENTIFIER_WATER));
            elementDatabase.RegisterElement(new SStone(game, SElementConstants.IDENTIFIER_STONE));
            elementDatabase.RegisterElement(new SGrass(game, SElementConstants.IDENTIFIER_GRASS));
            elementDatabase.RegisterElement(new SIce(game, SElementConstants.IDENTIFIER_ICE));
            elementDatabase.RegisterElement(new SSand(game, SElementConstants.IDENTIFIER_SAND));
            elementDatabase.RegisterElement(new SSnow(game, SElementConstants.IDENTIFIER_SNOW));
            elementDatabase.RegisterElement(new SMCorruption(game, SElementConstants.IDENTIFIER_MOVABLE_CORRUPTION));
            elementDatabase.RegisterElement(new SLava(game, SElementConstants.IDENTIFIER_LAVA));
            elementDatabase.RegisterElement(new SAcid(game, SElementConstants.IDENTIFIER_ACID));
            elementDatabase.RegisterElement(new SGlass(game, SElementConstants.IDENTIFIER_GLASS));
            elementDatabase.RegisterElement(new SMetal(game, SElementConstants.IDENTIFIER_METAL));
            elementDatabase.RegisterElement(new SWall(game, SElementConstants.IDENTIFIER_WALL));
            elementDatabase.RegisterElement(new SWood(game, SElementConstants.IDENTIFIER_WOOD));
            elementDatabase.RegisterElement(new SGCorruption(game, SElementConstants.IDENTIFIER_GAS_CORRUPTION));
            elementDatabase.RegisterElement(new SLCorruption(game, SElementConstants.IDENTIFIER_LIQUID_CORRUPTION));
            elementDatabase.RegisterElement(new SIMCorruption(game, SElementConstants.IDENTIFIER_IMMOVABLE_CORRUPTION));
            elementDatabase.RegisterElement(new SSteam(game, SElementConstants.IDENTIFIER_STEAM));
            elementDatabase.RegisterElement(new SSmoke(game, SElementConstants.IDENTIFIER_SMOKE));
            elementDatabase.RegisterElement(new SRedBrick(game, SElementConstants.IDENTIFIER_RED_BRICK));
            elementDatabase.RegisterElement(new STreeLeaf(game, SElementConstants.IDENTIFIER_TREE_LEAF));
            elementDatabase.RegisterElement(new SMountingBlock(game, SElementConstants.IDENTIFIER_MOUNTING_BLOCK));
            elementDatabase.RegisterElement(new SFire(game, SElementConstants.IDENTIFIER_FIRE));
        }
    }
}
