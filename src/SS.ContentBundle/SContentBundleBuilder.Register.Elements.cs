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
            elementDatabase.RegisterElement(new SDirt(game, SElementIdentifierConstants.DIRT));
            elementDatabase.RegisterElement(new SMud(game, SElementIdentifierConstants.MUD));
            elementDatabase.RegisterElement(new SWater(game, SElementIdentifierConstants.WATER));
            elementDatabase.RegisterElement(new SStone(game, SElementIdentifierConstants.STONE));
            elementDatabase.RegisterElement(new SGrass(game, SElementIdentifierConstants.GRASS));
            elementDatabase.RegisterElement(new SIce(game, SElementIdentifierConstants.ICE));
            elementDatabase.RegisterElement(new SSand(game, SElementIdentifierConstants.SAND));
            elementDatabase.RegisterElement(new SSnow(game, SElementIdentifierConstants.SNOW));
            elementDatabase.RegisterElement(new SMCorruption(game, SElementIdentifierConstants.MOVABLE_CORRUPTION));
            elementDatabase.RegisterElement(new SLava(game, SElementIdentifierConstants.LAVA));
            elementDatabase.RegisterElement(new SAcid(game, SElementIdentifierConstants.ACID));
            elementDatabase.RegisterElement(new SGlass(game, SElementIdentifierConstants.GLASS));
            elementDatabase.RegisterElement(new SMetal(game, SElementIdentifierConstants.METAL));
            elementDatabase.RegisterElement(new SWall(game, SElementIdentifierConstants.WALL));
            elementDatabase.RegisterElement(new SWood(game, SElementIdentifierConstants.WOOD));
            elementDatabase.RegisterElement(new SGCorruption(game, SElementIdentifierConstants.GAS_CORRUPTION));
            elementDatabase.RegisterElement(new SLCorruption(game, SElementIdentifierConstants.LIQUID_CORRUPTION));
            elementDatabase.RegisterElement(new SIMCorruption(game, SElementIdentifierConstants.IMMOVABLE_CORRUPTION));
            elementDatabase.RegisterElement(new SSteam(game, SElementIdentifierConstants.STEAM));
            elementDatabase.RegisterElement(new SSmoke(game, SElementIdentifierConstants.SMOKE));
            elementDatabase.RegisterElement(new SRedBrick(game, SElementIdentifierConstants.RED_BRICK));
            elementDatabase.RegisterElement(new STreeLeaf(game, SElementIdentifierConstants.TREE_LEAF));
            elementDatabase.RegisterElement(new SMountingBlock(game, SElementIdentifierConstants.MOUNTING_BLOCK));
            elementDatabase.RegisterElement(new SFire(game, SElementIdentifierConstants.FIRE));
        }
    }
}
