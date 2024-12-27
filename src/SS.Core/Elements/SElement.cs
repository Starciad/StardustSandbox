using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World.Data;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Elements
{
    public abstract class SElement : SGameObject, ISElement
    {
        public uint Identifier => this.identifier;
        public Texture2D Texture => this.texture;

        public Color ReferenceColor => this.referenceColor;

        public int DefaultDispersionRate => this.defaultDispersionRate;
        public short DefaultTemperature => this.defaultTemperature;
        public short DefaultFlammabilityResistance => this.defaultFlammabilityResistance;
        public short DefaultDensity => this.defaultDensity;

        public bool EnableDefaultBehaviour => this.enableDefaultBehaviour;
        public bool EnableNeighborsAction => this.enableNeighborsAction;
        public bool EnableTemperature => this.enableTemperature;
        public bool EnableFlammability => this.enableFlammability;

        public SElementRendering Rendering => this.rendering;
        public ISElementContext Context { get => this.context; set => this.context = value; }

        // =========================== //

        protected uint identifier;
        protected Texture2D texture;

        protected Color referenceColor = Color.White;

        protected int defaultDispersionRate = 1;
        protected short defaultTemperature;
        protected short defaultFlammabilityResistance;
        protected short defaultDensity;

        protected bool enableDefaultBehaviour = true;
        protected bool enableNeighborsAction;
        protected bool enableTemperature = true;
        protected bool enableFlammability;

        private ISElementContext context;

        private readonly SElementRendering rendering;

        public SElement(ISGame gameInstance) : base(gameInstance)
        {
            this.rendering = new(gameInstance, this);
        }

        public override void Update(GameTime gameTime)
        {
            this.rendering.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.rendering.SetContext(this.context);
            this.rendering.Draw(gameTime, spriteBatch);
        }

        public void InstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer)
        {
            OnInstantiateStep(worldSlot, worldLayer);
        }

        public void Steps()
        {
            if (this.EnableTemperature || this.EnableNeighborsAction)
            {
                IEnumerable<SWorldSlot> neighbors = this.context.GetNeighboringSlots();

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

        private void UpdateTemperature(IEnumerable<SWorldSlot> neighbors)
        {
            float totalTemperatureChange = 0;

            short neighborsForegroundLayerLength = 0;
            short neighborsBackgroundLayerLength = 0;

            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer neighborForeground = neighbor.GetLayer(SWorldLayer.Foreground);
                SWorldSlotLayer neighborBackground = neighbor.GetLayer(SWorldLayer.Background);

                if (!neighborForeground.IsEmpty)
                {
                    if (neighborForeground.Element.EnableTemperature)
                    {
                        totalTemperatureChange += this.context.SlotLayer.Temperature - neighborForeground.Temperature;
                    }

                    neighborsForegroundLayerLength++;
                }

                if (!neighborBackground.IsEmpty)
                {
                    if (neighborBackground.Element.EnableTemperature)
                    {
                        totalTemperatureChange += this.context.SlotLayer.Temperature - neighborBackground.Temperature;
                    }

                    neighborsBackgroundLayerLength++;
                }
            }

            short averageTemperatureChange = (short)Math.Round(totalTemperatureChange / (short)(neighborsForegroundLayerLength + neighborsBackgroundLayerLength));
            this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(this.context.SlotLayer.Temperature - averageTemperatureChange)));

            if (MathF.Abs(averageTemperatureChange) < STemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(this.context.SlotLayer.Temperature + averageTemperatureChange)));
            }

            OnTemperatureChanged(this.context.SlotLayer.Temperature);
        }

        protected virtual void OnInstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer) { return; }
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnNeighbors(IEnumerable<SWorldSlot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
    }
}
