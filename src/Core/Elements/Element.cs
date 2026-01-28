/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Elements
{
    internal abstract class Element
    {
        #region Properties

        internal required ElementIndex Index { get; init; }
        internal required ElementCategory Category { get; init; }
        internal required ElementCharacteristics Characteristics { get; init; }
        internal required ElementRenderingType RenderingType { get; init; }
        internal required Point TextureOriginOffset { get; init; }
        internal required Color ReferenceColor { get; init; }

        internal int DefaultDispersionRate { get; init; }
        internal float DefaultTemperature { get; init; }
        internal float DefaultFlammabilityResistance { get; init; }
        internal float DefaultDensity { get; init; }
        internal float DefaultExplosionResistance { get; init; }

        #endregion

        private ElementContext context;

        internal Element()
        {
            this.DefaultDispersionRate = 1;
            this.DefaultTemperature = 25.0f;
            this.DefaultFlammabilityResistance = 25.0f;
            this.DefaultDensity = 0.0f;
            this.DefaultExplosionResistance = 0.5f;
        }

        internal void SetContext(ElementContext context)
        {
            this.context = context;
        }

        internal bool HasCharacteristic(ElementCharacteristics characteristic)
        {
            return this.Characteristics.HasFlag(characteristic);
        }

        #region Virtual Methods

        protected virtual void OnInstantiated(ElementContext context) { return; }
        protected virtual void OnStep(ElementContext context) { return; }
        protected virtual void OnDestroyed(ElementContext context) { return; }
        protected virtual void OnNeighbors(ElementContext context, ElementNeighbors neighbors) { return; }
        protected virtual void OnTemperatureChanged(ElementContext context, in float currentValue) { return; }

        #endregion

        #region Routines

        internal void Instantiate()
        {
            OnInstantiated(this.context);
        }

        internal void Destroy()
        {
            OnDestroyed(this.context);
        }

        internal void Steps(GameTime gameTime)
        {
            bool hasTemperature = HasCharacteristic(ElementCharacteristics.HasTemperature);
            bool affectsNeighbors = HasCharacteristic(ElementCharacteristics.AffectsNeighbors);
            bool isPushable = HasCharacteristic(ElementCharacteristics.IsPushable);

            bool anyCharacteristic = hasTemperature || affectsNeighbors || isPushable;

            if (isPushable && this.context.HasElementState(ElementStates.WasPushed))
            {
                this.context.RemoveElementState(ElementStates.WasPushed);
            }

            if (anyCharacteristic)
            {
                ElementNeighbors neighbors = this.context.GetNeighboringSlots();

                if (hasTemperature)
                {
                    UpdateTemperature(gameTime, neighbors);
                }

                if (affectsNeighbors)
                {
                    OnNeighbors(this.context, neighbors);
                }
            }

            OnStep(this.context);
        }

        // Updated temperature transfer using Fourier's law of thermal conduction
        // Fourier's law: Q = -k * A * (dT/dx) * dt
        private void UpdateTemperature(GameTime gameTime, ElementNeighbors neighbors)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            float currentTemperature = this.context.SlotLayer.Temperature;
            float totalHeatTransfer = 0.0f;
            int validNeighborCount = 0;

            float CalculateHeatTransfer(SlotLayer slotLayer)
            {
                if (!slotLayer.IsEmpty && slotLayer.Element.HasCharacteristic(ElementCharacteristics.HasTemperature))
                {
                    float neighborTemp = slotLayer.Temperature;
                    return TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;
                }

                return 0.0f;
            }

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (neighbors.HasNeighbor(i))
                {
                    float fgHeat = CalculateHeatTransfer(neighbors.GetSlotLayer(i, Layer.Foreground));

                    if (fgHeat != 0.0f)
                    {
                        totalHeatTransfer += fgHeat;
                        validNeighborCount++;
                    }

                    float bgHeat = CalculateHeatTransfer(neighbors.GetSlotLayer(i, Layer.Background));

                    if (bgHeat != 0.0f)
                    {
                        totalHeatTransfer += bgHeat;
                        validNeighborCount++;
                    }
                }
            }

            if (this.context.World.Temperature.CanApplyTemperature)
            {
                float worldTemp = this.context.World.Temperature.CurrentTemperature;
                float worldHeatTransfer = TemperatureConstants.WORLD_THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (worldTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;

                totalHeatTransfer += worldHeatTransfer;
            }

            float newTemperature = currentTemperature + totalHeatTransfer;
            this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(newTemperature));

            if (Math.Abs(totalHeatTransfer) < TemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(currentTemperature));
            }

            if (this.context.SlotLayer.Temperature == TemperatureConstants.MAX_CELSIUS_VALUE)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_009);
            }
            else if (this.context.SlotLayer.Temperature == TemperatureConstants.MIN_CELSIUS_VALUE)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_010);
            }

            OnTemperatureChanged(this.context, this.context.SlotLayer.Temperature);
        }

        #endregion
    }
}

