using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Elements
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

        internal int DefaultDispersionRate { get; init; } = 1;
        internal double DefaultTemperature { get; init; } = 25.0;
        internal double DefaultFlammabilityResistance { get; init; } = 25.0;
        internal double DefaultDensity { get; init; } = 0;
        internal double DefaultExplosionResistance { get; init; } = 0.5;

        internal ElementContext Context { get => this.context; set => this.context = value; }

        #endregion

        private ElementContext context;

        #region Virtual Methods

        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }

        protected virtual void OnInstantiated() { return; }
        protected virtual void OnDestroyed() { return; }
        protected virtual void OnNeighbors(IEnumerable<Slot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(double currentValue) { return; }

        #endregion

        #region Routines

        internal void Instantiate()
        {
            OnInstantiated();
        }

        internal void Destroy()
        {
            OnDestroyed();
        }

        internal void Steps(GameTime gameTime)
        {
            bool hasTemperature = this.Characteristics.HasFlag(ElementCharacteristics.HasTemperature);
            bool affectsNeighbors = this.Characteristics.HasFlag(ElementCharacteristics.AffectsNeighbors);

            if (hasTemperature || affectsNeighbors)
            {
                IEnumerable<Slot> neighbors = this.context.GetNeighboringSlots();

                if (hasTemperature)
                {
                    UpdateTemperature(gameTime, neighbors);
                }

                if (affectsNeighbors)
                {
                    OnNeighbors(neighbors);
                }
            }

            OnBeforeStep();
            OnStep();
            OnAfterStep();
        }

        // Updated temperature transfer using Fourier's law of thermal conduction
        private void UpdateTemperature(GameTime gameTime, IEnumerable<Slot> neighbors)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            double currentTemperature = this.context.SlotLayer.Temperature;
            double totalHeatTransfer = 0.0;
            int validNeighborCount = 0;

            foreach (Slot neighbor in neighbors)
            {
                SlotLayer neighborForeground = neighbor.GetLayer(LayerType.Foreground);
                SlotLayer neighborBackground = neighbor.GetLayer(LayerType.Background);

                // Foreground layer
                if (!neighborForeground.HasState(ElementStates.IsEmpty) &&
                    neighborForeground.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                {
                    double neighborTemp = neighborForeground.Temperature;
                    // Fourier's law: Q = -k * A * (dT/dx) * dt
                    double heatTransfer = TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;
                    totalHeatTransfer += heatTransfer;
                    validNeighborCount++;
                }

                // Background layer
                if (!neighborBackground.HasState(ElementStates.IsEmpty) &&
                    neighborBackground.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                {
                    double neighborTemp = neighborBackground.Temperature;
                    double heatTransfer = TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;
                    totalHeatTransfer += heatTransfer;
                    validNeighborCount++;
                }
            }

            // Update temperature based on total heat transfer
            double newTemperature = currentTemperature + totalHeatTransfer;
            this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(newTemperature));

            // Equilibrium check (optional: can be tuned for stability)
            if (Math.Abs(totalHeatTransfer) < TemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(currentTemperature));
            }

            OnTemperatureChanged(this.context.SlotLayer.Temperature);
        }

        #endregion

        internal void Draw(SpriteBatch spriteBatch)
        {
            ElementRenderer.Draw(this.context, this, spriteBatch, this.TextureOriginOffset);
        }
    }
}
