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
            /* 001. */
            elementDatabase.RegisterElement(new SDirt(game, SElementConstants.DIRT_IDENTIFIER));
            /* 002. */
            elementDatabase.RegisterElement(new SMud(game, SElementConstants.MUD_IDENTIFIER));
            /* 003. */
            elementDatabase.RegisterElement(new SWater(game, SElementConstants.WATER_IDENTIFIER));
            /* 004. */
            elementDatabase.RegisterElement(new SStone(game, SElementConstants.STONE_IDENTIFIER));
            /* 005. */
            elementDatabase.RegisterElement(new SGrass(game, SElementConstants.GRASS_IDENTIFIER));
            /* 006. */
            elementDatabase.RegisterElement(new SIce(game, SElementConstants.ICE_IDENTIFIER));
            /* 007. */
            elementDatabase.RegisterElement(new SSand(game, SElementConstants.SAND_IDENTIFIER));
            /* 008. */
            elementDatabase.RegisterElement(new SSnow(game, SElementConstants.SNOW_IDENTIFIER));
            /* 009. */
            elementDatabase.RegisterElement(new SMCorruption(game, SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER));
            /* 010. */
            elementDatabase.RegisterElement(new SLava(game, SElementConstants.LAVA_IDENTIFIER));
            /* 011. */
            elementDatabase.RegisterElement(new SAcid(game, SElementConstants.ACID_IDENTIFIER));
            /* 012. */
            elementDatabase.RegisterElement(new SGlass(game, SElementConstants.GLASS_IDENTIFIER));
            /* 013. */
            elementDatabase.RegisterElement(new SIron(game, SElementConstants.IRON_IDENTIFIER));
            /* 014. */
            elementDatabase.RegisterElement(new SWall(game, SElementConstants.WALL_IDENTIFIER));
            /* 015. */
            elementDatabase.RegisterElement(new SWood(game, SElementConstants.WOOD_IDENTIFIER));
            /* 016. */
            elementDatabase.RegisterElement(new SGCorruption(game, SElementConstants.GAS_CORRUPTION_IDENTIFIER));
            /* 017. */
            elementDatabase.RegisterElement(new SLCorruption(game, SElementConstants.LIQUID_CORRUPTION_IDENTIFIER));
            /* 018. */
            elementDatabase.RegisterElement(new SIMCorruption(game, SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER));
            /* 019. */
            elementDatabase.RegisterElement(new SSteam(game, SElementConstants.STEAM_IDENTIFIER));
            /* 020. */
            elementDatabase.RegisterElement(new SSmoke(game, SElementConstants.SMOKE_IDENTIFIER));
            /* 021. */
            elementDatabase.RegisterElement(new SRedBrick(game, SElementConstants.RED_BRICK_IDENTIFIER));
            /* 022. */
            elementDatabase.RegisterElement(new STreeLeaf(game, SElementConstants.TREE_LEAF_IDENTIFIER));
            /* 023. */
            elementDatabase.RegisterElement(new SMountingBlock(game, SElementConstants.MOUNTING_BLOCK_IDENTIFIER));
            /* 024. */
            elementDatabase.RegisterElement(new SFire(game, SElementConstants.FIRE_IDENTIFIER));
            /* 025. */
            elementDatabase.RegisterElement(new SLamp(game, SElementConstants.LAMP_IDENTIFIER));
            /* 026. */
            elementDatabase.RegisterElement(new SVoid(game, SElementConstants.VOID_IDENTIFIER));
            /* 027. */
            elementDatabase.RegisterElement(new SClone(game, SElementConstants.CLONE_IDENTIFIER));
            /* 028. */
            elementDatabase.RegisterElement(new SOil(game, SElementConstants.OIL_IDENTIFIER));
            /* 029. */
            elementDatabase.RegisterElement(new SSalt(game, SElementConstants.SALT_IDENTIFIER));
            /* 030. */
            elementDatabase.RegisterElement(new SSaltwater(game, SElementConstants.SALTWATER_IDENTIFIER));
            /* 031. */
            elementDatabase.RegisterElement(new SBomb(game, SElementConstants.BOMB_IDENTIFIER));
            /* 032. */
            elementDatabase.RegisterElement(new SDynamite(game, SElementConstants.DYNAMITE_IDENTIFIER));
            /* 033. */
            elementDatabase.RegisterElement(new STnt(game, SElementConstants.TNT_IDENTIFIER));
            /* 034. */
            elementDatabase.RegisterElement(new SDrySponge(game, SElementConstants.DRY_SPONGE_IDENTIFIER));
            /* 035. */
            elementDatabase.RegisterElement(new SWetSponge(game, SElementConstants.WET_SPONGE_IDENTIFIER));
            /* 036. */
            elementDatabase.RegisterElement(new SGold(game, SElementConstants.GOLD_IDENTIFIER));
            /* 037. */
            elementDatabase.RegisterElement(new SHeater(game, SElementConstants.HEATER_IDENTIFIER));
            /* 038. */
            elementDatabase.RegisterElement(new SFreezer(game, SElementConstants.FREEZER_IDENTIFIER));
        }
    }
}
