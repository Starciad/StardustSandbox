using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Interfaces.World;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Bundle.Energies;
using StardustSandbox.Game.Resources.Elements.Bundle.Gases;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables;
using StardustSandbox.Game.Resources.Elements.Rendering;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Liquids
{
    public class SWater : SLiquid
    {
        public SWater(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 002;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_3");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.enableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            for (int i = 0; i < length; i++)
            {
                (Point position, ISWorldSlot slot) = neighbors[i];

                if (slot.Element is SDirt)
                {
                    this.Context.DestroyElement();
                    this.Context.ReplaceElement<SMud>(position);
                    return;
                }

                if (slot.Element is SStone)
                {
                    if (SRandomMath.Range(0, 150) == 0)
                    {
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement<SSand>(position);
                        return;
                    }
                }

                if (slot.Element is SFire)
                {
                    this.Context.DestroyElement(position);
                    return;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<SSteam>();
            }

            if (currentValue <= 0)
            {
                this.Context.ReplaceElement<SIce>();
            }
        }
    }
}