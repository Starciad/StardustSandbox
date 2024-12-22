using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Data;

using System;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal sealed class SWater : SLiquid
    {
        internal SWater(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Water;
            this.referenceColor = new(8, 120, 284, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_3");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultDispersionRate = 3;
            this.defaultTemperature = 25;
            this.enableNeighborsAction = true;
        }

        protected override void OnNeighbors(SWorldSlot[] neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                SWorldSlot slot = neighbors[i];

                switch (slot.GetLayer(this.Context.Layer).Element)
                {
                    case SDirt:
                        this.Context.DestroyElement();
                        this.Context.ReplaceElement<SMud>(slot.Position, this.Context.Layer);
                        return;

                    case SStone:
                        if (SRandomMath.Range(0, 150) == 0)
                        {
                            this.Context.DestroyElement();
                            this.Context.ReplaceElement<SSand>(slot.Position, this.Context.Layer);
                        }
                        return;

                    case SFire:
                        this.Context.DestroyElement(slot.Position, this.Context.Layer);
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