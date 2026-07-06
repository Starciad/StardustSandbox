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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem.Slots;

using System;

namespace StardustSandbox.Core.Elements
{
    internal abstract class Element
    {
        internal ElementIndex Index { get; }
        internal ElementCategory Category { get; }
        internal ElementRenderingType RenderingType { get; }
        internal Point TextureOriginOffset { get; }
        internal Color ReferenceColor { get; }

        protected GameEvents GameEvents { get; }

        public float BaseDensity { get; protected init; }
        public float BaseExplosionResistance { get; protected init; }
        public float BaseFlammabilityResistance { get; protected init; }
        public float InitialTemperature { get; protected init; }
        public int BaseDispersionRate { get; protected init; }

        public bool HasNeighborInteractions { get; protected init; }
        public bool HasTemperature { get; protected init; }
        public bool IsConductive { get; protected init; }
        public bool IsCorruptible { get; protected init; }
        public bool IsCorruption { get; protected init; }
        public bool IsElectrified { get; protected init; }
        public bool IsExplosive { get; protected init; }
        public bool IsExplosionImmune { get; protected init; }
        public bool IsFlammable { get; protected init; }
        public bool IsPushable { get; protected init; }

        private ElementContext context;

        internal Element(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents)
        {
            this.Index = index;
            this.Category = category;
            this.RenderingType = renderingType;
            this.TextureOriginOffset = textureOriginOffset;
            this.ReferenceColor = referenceColor;
            this.GameEvents = gameEvents;
        }

        internal void SetContext(ElementContext context)
        {
            this.context = context;
        }

        #region Virtual Methods

        protected virtual void OnInstantiated(ElementContext context) { return; }
        protected virtual void OnStep(ElementContext context) { return; }
        protected virtual void OnDestroyed(ElementContext context) { return; }
        protected virtual void OnNeighbors(ElementContext context, ElementNeighbors neighbors) { return; }
        protected virtual void OnTemperatureChanged(ElementContext context, float currentValue) { return; }

        #endregion

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
            if (this.IsPushable && this.context.GetPushedState())
            {
                this.context.SetPushedState(false);
            }

            bool anyCharacteristic = this.HasTemperature || this.HasNeighborInteractions || this.IsPushable;

            if (anyCharacteristic)
            {
                ElementNeighbors neighbors = this.context.GetNeighboringSlots();

                if (this.HasTemperature)
                {
                    UpdateTemperature(gameTime, neighbors);
                }

                if (this.HasNeighborInteractions)
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

            float currentTemperature = this.context.GetTemperature();
            float totalHeatTransfer = 0.0f;
            int validNeighborCount = 0;

            float CalculateHeatTransfer(Slot slot, Layer layer)
            {
                if (slot.HasElement(layer) && slot.GetElement(layer).HasTemperature)
                {
                    float neighborTemp = slot.GetTemperature(layer);
                    return TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;
                }

                return 0.0f;
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors.HasNeighbor(i))
                {
                    float fgHeat = CalculateHeatTransfer(neighbors.GetSlot(i), Layer.Foreground);

                    if (fgHeat != 0.0f)
                    {
                        totalHeatTransfer += fgHeat;
                        validNeighborCount++;
                    }

                    float bgHeat = CalculateHeatTransfer(neighbors.GetSlot(i), Layer.Background);

                    if (bgHeat != 0.0f)
                    {
                        totalHeatTransfer += bgHeat;
                        validNeighborCount++;
                    }
                }
            }

            if (this.context.GetWorldTemperature().CanApplyTemperature)
            {
                float worldTemp = this.context.GetWorldTemperature().CurrentTemperature;
                float worldHeatTransfer = TemperatureConstants.WORLD_THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (worldTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;

                totalHeatTransfer += worldHeatTransfer;
            }

            float newTemperature = currentTemperature + totalHeatTransfer;
            this.context.SetTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(newTemperature));

            if (Math.Abs(totalHeatTransfer) < TemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(currentTemperature));
            }

            switch (this.context.GetTemperature())
            {
                case TemperatureConstants.MAX_CELSIUS_VALUE:
                    this.GameEvents.Publish(new ElementReachedMaxTemperatureEvent());
                    break;

                case TemperatureConstants.MIN_CELSIUS_VALUE:
                    this.GameEvents.Publish(new ElementReachedMinTemperatureEvent());
                    break;

                default:
                    break;
            }

            OnTemperatureChanged(this.context, this.context.GetTemperature());
        }
    }
}
