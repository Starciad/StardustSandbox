using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase)
        {
            elementDatabase.RegisterElement(new SDirt(game));
            elementDatabase.RegisterElement(new SMud(game));
            elementDatabase.RegisterElement(new SWater(game));
            elementDatabase.RegisterElement(new SStone(game));
            elementDatabase.RegisterElement(new SGrass(game));
            elementDatabase.RegisterElement(new SIce(game));
            elementDatabase.RegisterElement(new SSand(game));
            elementDatabase.RegisterElement(new SSnow(game));
            elementDatabase.RegisterElement(new SMCorruption(game));
            elementDatabase.RegisterElement(new SLava(game));
            elementDatabase.RegisterElement(new SAcid(game));
            elementDatabase.RegisterElement(new SGlass(game));
            elementDatabase.RegisterElement(new SMetal(game));
            elementDatabase.RegisterElement(new SWall(game));
            elementDatabase.RegisterElement(new SWood(game));
            elementDatabase.RegisterElement(new SGCorruption(game));
            elementDatabase.RegisterElement(new SLCorruption(game));
            elementDatabase.RegisterElement(new SIMCorruption(game));
            elementDatabase.RegisterElement(new SSteam(game));
            elementDatabase.RegisterElement(new SSmoke(game));
            elementDatabase.RegisterElement(new SRedBrick(game));
            elementDatabase.RegisterElement(new STreeLeaf(game));
            elementDatabase.RegisterElement(new SMountingBlock(game));
            elementDatabase.RegisterElement(new SFire(game));
        }
    }
}
