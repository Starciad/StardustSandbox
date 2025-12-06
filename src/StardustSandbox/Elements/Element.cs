using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

using System;

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

        internal void SetContext(in ElementContext context)
        {
            this.context = context;
        }

        #region Virtual Methods

        protected virtual void OnInstantiated(in ElementContext context) { return; }
        protected virtual void OnBeforeStep(in ElementContext context) { return; }
        protected virtual void OnStep(in ElementContext context) { return; }
        protected virtual void OnAfterStep(in ElementContext context) { return; }
        protected virtual void OnDestroyed(in ElementContext context) { return; }
        protected virtual void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors) { return; }
        protected virtual void OnTemperatureChanged(in ElementContext context, float currentValue) { return; }

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

        internal void Steps(in GameTime gameTime)
        {
            bool hasTemperature = this.Characteristics.HasFlag(ElementCharacteristics.HasTemperature);
            bool affectsNeighbors = this.Characteristics.HasFlag(ElementCharacteristics.AffectsNeighbors);

            if (hasTemperature || affectsNeighbors)
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

            OnBeforeStep(this.context);
            OnStep(this.context);
            OnAfterStep(this.context);
        }

        // Updated temperature transfer using Fourier's law of thermal conduction
        // Fourier's law: Q = -k * A * (dT/dx) * dt
        private void UpdateTemperature(in GameTime gameTime, in ElementNeighbors neighbors)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            float currentTemperature = this.context.SlotLayer.Temperature;
            float totalHeatTransfer = 0.0f;
            int validNeighborCount = 0;

            for (int i = 0; i < neighbors.Length; i++)
            {
                // Foreground layer
                if (neighbors.HasNeighbor(i))
                {
                    SlotLayer neighborForeground = neighbors.GetSlotLayer(i, Layer.Foreground);

                    if (!neighborForeground.HasState(ElementStates.IsEmpty) && neighborForeground.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                    {
                        float neighborTemp = neighborForeground.Temperature;
                        float heatTransfer = TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;

                        totalHeatTransfer += heatTransfer;
                        validNeighborCount++;
                    }
                }

                // Background layer
                if (neighbors.HasNeighbor(i))
                {
                    SlotLayer neighborBackground = neighbors.GetSlotLayer(i, Layer.Background);

                    if (!neighborBackground.HasState(ElementStates.IsEmpty) && neighborBackground.Element.Characteristics.HasFlag(ElementCharacteristics.HasTemperature))
                    {
                        float neighborTemp = neighborBackground.Temperature;
                        float heatTransfer = TemperatureConstants.THERMAL_CONDUCTIVITY * TemperatureConstants.AREA * (neighborTemp - currentTemperature) / TemperatureConstants.DISTANCE * deltaTime;

                        totalHeatTransfer += heatTransfer;
                        validNeighborCount++;
                    }
                }
            }

            // Update temperature based on total heat transfer
            float newTemperature = currentTemperature + totalHeatTransfer;
            this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(newTemperature));

            // Equilibrium check (optional: can be tuned for stability)
            if (Math.Abs(totalHeatTransfer) < TemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp(currentTemperature));
            }

            OnTemperatureChanged(this.context, this.context.SlotLayer.Temperature);
        }

        #endregion

        internal void Draw(SpriteBatch spriteBatch)
        {
            ElementRenderer.Draw(this.context, this, spriteBatch, this.TextureOriginOffset);
        }
    }
}
