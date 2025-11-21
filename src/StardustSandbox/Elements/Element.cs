using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
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

        internal ElementIndex Index => this.index;
        internal Texture2D Texture => this.texture;
        internal ElementCategory Category => this.category;
        internal ElementRenderingType RenderingType => this.renderingType;

        internal Color ReferenceColor => this.referenceColor;

        internal int DefaultDispersionRate => this.defaultDispersionRate;
        internal short DefaultTemperature => this.defaultTemperature;
        internal short DefaultFlammabilityResistance => this.defaultFlammabilityResistance;
        internal short DefaultDensity => this.defaultDensity;
        internal float DefaultExplosionResistance => this.defaultExplosionResistance;

        internal ElementContext Context { get => this.context; set => this.context = value; }

        #endregion

        #region Fields

        protected ElementCategory category = ElementCategory.None;
        protected ElementCharacteristics characteristics = ElementCharacteristics.None;
        protected ElementRenderingType renderingType = ElementRenderingType.Single;

        protected int defaultDispersionRate = 1;
        protected short defaultTemperature = 25;
        protected short defaultFlammabilityResistance = 25;
        protected short defaultDensity = 0;
        protected float defaultExplosionResistance = 0.5f;

        protected readonly ElementIndex index = ElementIndex.None;
        protected readonly Texture2D texture = null;
        protected readonly Color referenceColor = AAP64ColorPalette.White;

        #endregion

        private ElementContext context;

        internal Element(Color referenceColor, ElementIndex index, Texture2D texture)
        {
            this.referenceColor = referenceColor;
            this.index = index;
            this.texture = texture;
        }

        #region Virtual Methods

        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }

        protected virtual void OnInstantiated() { return; }
        protected virtual void OnDestroyed() { return; }
        protected virtual void OnNeighbors(IEnumerable<Slot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }

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

        internal void Steps()
        {
            bool hasTemperature = this.characteristics.HasFlag(ElementCharacteristics.HasTemperature);
            bool affectsNeighbors = this.characteristics.HasFlag(ElementCharacteristics.AffectsNeighbors);

            if (hasTemperature || affectsNeighbors)
            {
                IEnumerable<Slot> neighbors = this.context.GetNeighboringSlots();

                if (hasTemperature)
                {
                    UpdateTemperature(neighbors);
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

        private void UpdateTemperature(IEnumerable<Slot> neighbors)
        {
            float totalTemperatureChange = 0;

            short neighborsForegroundLayerLength = 0;
            short neighborsBackgroundLayerLength = 0;

            foreach (Slot neighbor in neighbors)
            {
                SlotLayer neighborForeground = neighbor.GetLayer(LayerType.Foreground);
                SlotLayer neighborBackground = neighbor.GetLayer(LayerType.Background);

                if (!neighborForeground.HasState(ElementStates.IsEmpty))
                {
                    if (neighborForeground.Element.HasCharacteristic(ElementCharacteristics.HasTemperature))
                    {
                        totalTemperatureChange += this.context.SlotLayer.Temperature - neighborForeground.Temperature;
                    }

                    neighborsForegroundLayerLength++;
                }

                if (!neighborBackground.HasState(ElementStates.IsEmpty))
                {
                    if (neighborBackground.Element.HasCharacteristic(ElementCharacteristics.HasTemperature))
                    {
                        totalTemperatureChange += this.context.SlotLayer.Temperature - neighborBackground.Temperature;
                    }

                    neighborsBackgroundLayerLength++;
                }
            }

            short averageTemperatureChange = (short)Math.Round(totalTemperatureChange / (short)(neighborsForegroundLayerLength + neighborsBackgroundLayerLength));
            this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp((short)(this.context.SlotLayer.Temperature - averageTemperatureChange)));

            if (MathF.Abs(averageTemperatureChange) < TemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Position, this.context.Layer, TemperatureMath.Clamp((short)(this.context.SlotLayer.Temperature + averageTemperatureChange)));
            }

            OnTemperatureChanged(this.context.SlotLayer.Temperature);
        }

        #endregion

        internal void Draw(SpriteBatch spriteBatch)
        {
            ElementRenderer.Draw(this.context, this, spriteBatch);
        }

        internal bool HasCharacteristic(ElementCharacteristics characteristic)
        {
            return this.characteristics.HasFlag(characteristic);
        }
    }
}
