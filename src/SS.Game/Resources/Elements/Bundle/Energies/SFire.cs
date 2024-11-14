using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Templates.Energies;
using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Elements.Utilities;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Bundle.Gases;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Energies
{
    public sealed class SFire : SEnergy
    {
        public SFire(ISGame gameInstance) : base(gameInstance)
        {
            this.Id = 023;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_24");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism());
            this.EnableNeighborsAction = true;
            this.DefaultTemperature = 500;
        }

        protected override void OnBeforeStep()
        {
            if (SRandomMath.Chance(SElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR, SElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR_TOTAL))
            {
                this.Context.DestroyElement();

                if (SRandomMath.Chance(SElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE, SElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE_TOTAL))
                {
                    this.Context.InstantiateElement<SSmoke>();
                }
            }
        }

        protected override void OnStep()
        {
            Point targetPos = new(this.Context.Position.X + SRandomMath.Range(-1, 2), this.Context.Position.Y - 1);

            if (this.Context.IsEmptyElementSlot(targetPos))
            {
                if (this.Context.TrySetPosition(targetPos))
                {
                    return;
                }
            }
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            for (int i = 0; i < length; i++)
            {
                SWorldSlot slot = neighbors[i].Item2;

                if (slot == null)
                {
                    continue;
                }

                slot.SetTemperatureValue(slot.Temperature + 1);
            }
        }
    }
}
