using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    public class SWater : SLiquid
    {
        public SWater(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 002;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_3");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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