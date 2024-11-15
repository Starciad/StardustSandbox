using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Templates.Energies;
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
                (Point position, SWorldSlot slot) = neighbors[i];
                SElement element = slot.Element;

                if (slot == null || element == null)
                {
                    continue;
                }

                // Increase neighboring temperature by fire's heat value
                _ = this.Context.TrySetElementTemperature((short)(slot.Temperature + SElementConstants.FIRE_HEAT_VALUE));

                // Check if the element is flammable
                if (element.EnableFlammability)
                {
                    // Adjust combustion chance based on the element's flammability resistance
                    int combustionChance = SElementConstants.CHANCE_OF_COMBUSTION;
                    bool isAbove = position.Y < this.Context.Position.Y;

                    // Increase chance of combustion if the element is directly above
                    if (isAbove)
                    {
                        combustionChance += 10;
                    }

                    // Attempt combustion based on flammabilityResistance
                    if (SRandomMath.Chance(combustionChance, 100 + element.DefaultFlammabilityResistance))
                    {
                        this.Context.ReplaceElement<SFire>(position);
                    }
                }
            }
        }
    }
}
