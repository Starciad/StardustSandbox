﻿using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Energies;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.ContentBundle.Elements.Energies
{
    public sealed class SFire : SEnergy
    {
        public SFire(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 023;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_24");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultTemperature = 500;
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

        protected override void OnNeighbors(ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            for (int i = 0; i < length; i++)
            {
                (Point position, ISWorldSlot slot) = neighbors[i];
                ISElement element = slot.Element;

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
