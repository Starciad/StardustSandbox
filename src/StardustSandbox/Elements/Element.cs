using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Elements.Rendering;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.World;
using StardustSandbox.Mathematics;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Elements
{
    internal abstract class Element
    {
        internal ElementIndex Index => this.index;
        internal Texture2D Texture => this.texture;

        internal Color ReferenceColor => this.referenceColor;

        internal bool EnableDefaultBehaviour => this.enableDefaultBehaviour;
        internal bool EnableNeighborsAction => this.enableNeighborsAction;
        internal bool EnableTemperature => this.enableTemperature;
        internal bool EnableFlammability => this.enableFlammability;

        internal bool IsExplosionImmune => this.isExplosionImmune;

        internal int DefaultDispersionRate => this.defaultDispersionRate;
        internal short DefaultTemperature => this.defaultTemperature;
        internal short DefaultFlammabilityResistance => this.defaultFlammabilityResistance;
        internal short DefaultDensity => this.defaultDensity;
        internal float DefaultExplosionResistance => this.defaultExplosionResistance;

        internal ElementRendering Rendering => this.rendering;
        internal ElementContext Context { get => this.context; set => this.context = value; }

        // =========================== //

        protected bool enableDefaultBehaviour = true;
        protected bool enableNeighborsAction = false;
        protected bool enableTemperature = true;
        protected bool enableFlammability = false;

        protected bool isExplosionImmune = false;

        protected int defaultDispersionRate = 1;
        protected short defaultTemperature = 25;
        protected short defaultFlammabilityResistance = 25;
        protected short defaultDensity = 0;
        protected float defaultExplosionResistance = 0.5f;

        private ElementContext context;

        protected readonly ElementIndex index = ElementIndex.None;
        protected readonly Texture2D texture = null;
        protected readonly Color referenceColor = AAP64ColorPalette.White;
        private readonly ElementRendering rendering;

        internal Element(Color referenceColor, ElementIndex index, Texture2D texture)
        {
            this.referenceColor = referenceColor;
            this.index = index;
            this.texture = texture;
            this.rendering = new(this);
        }

        internal void Update(GameTime gameTime)
        {
            this.rendering.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.rendering.SetContext(this.context);
            this.rendering.Draw(spriteBatch);
        }

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
            if (this.EnableTemperature || this.EnableNeighborsAction)
            {
                IEnumerable<Slot> neighbors = this.context.GetNeighboringSlots();

                if (this.EnableTemperature)
                {
                    UpdateTemperature(neighbors);
                }

                if (this.EnableNeighborsAction)
                {
                    OnNeighbors(neighbors);
                }
            }

            OnBeforeStep();
            OnStep();
            if (this.EnableDefaultBehaviour)
            {
                OnBehaviourStep();
            }

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
                    if (neighborForeground.Element.EnableTemperature)
                    {
                        totalTemperatureChange += this.context.SlotLayer.Temperature - neighborForeground.Temperature;
                    }

                    neighborsForegroundLayerLength++;
                }

                if (!neighborBackground.HasState(ElementStates.IsEmpty))
                {
                    if (neighborBackground.Element.EnableTemperature)
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

        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnInstantiated() { return; }
        protected virtual void OnDestroyed() { return; }
        protected virtual void OnNeighbors(IEnumerable<Slot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
    }
}
