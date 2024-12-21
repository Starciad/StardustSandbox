using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Elements
{
    public abstract class SElement : SGameObject, ISElement
    {
        public uint Identifier => this.identifier;
        public Texture2D Texture => this.texture;

        public Color ReferenceColor => this.referenceColor;
        public SWorldLayer AllowedLayers => this.allowedLayers;

        public int DefaultDispersionRate => this.defaultDispersionRate;
        public short DefaultTemperature => this.defaultTemperature;
        public short DefaultFlammabilityResistance => this.defaultFlammabilityResistance;

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

        protected SWorldLayer allowedLayers = SWorldLayer.Foreground;

        protected int defaultDispersionRate = 1;
        protected short defaultTemperature;
        protected short defaultFlammabilityResistance;

        protected bool enableDefaultBehaviour = true;
        protected bool enableNeighborsAction;
        protected bool enableTemperature = true;
        protected bool enableFlammability;

        private readonly SElementRendering rendering;
        private ISElementContext context;

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

        public void InstantiateStep(ISWorldSlot worldSlot, SWorldLayer worldLayer)
        {
            OnInstantiateStep(worldSlot, worldLayer);
        }

        public void Steps()
        {
            if (this.context.TryGetNeighboringSlots(this.context.Slot.Position, out ISWorldSlot[] neighbors))
            {
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

        private void UpdateTemperature(ReadOnlySpan<ISWorldSlot> neighbors)
        {
            ISWorldSlot slot = this.context.GetWorldSlot();
            ISWorldSlotLayer currentLayerData = slot.GetLayer(this.context.Layer);

            short[] neighborsCurrentLayer = GetNeighborTemperatures(neighbors, this.context.Layer);
            short[] neighborsOppositeLayer = GetNeighborTemperatures(neighbors, this.context.Layer == SWorldLayer.Foreground ? SWorldLayer.Background : SWorldLayer.Foreground);

            float totalTemperatureChange = 0f;
            totalTemperatureChange += CalculateTemperatureChange(currentLayerData.Temperature, neighborsCurrentLayer);
            totalTemperatureChange += CalculateTemperatureChange(currentLayerData.Temperature, neighborsOppositeLayer);

            short averageTemperatureChange = (short)Math.Round(totalTemperatureChange / (short)(neighborsCurrentLayer.Length + neighborsOppositeLayer.Length));
            this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(currentLayerData.Temperature - averageTemperatureChange)));

            if (MathF.Abs(averageTemperatureChange) < STemperatureMath.EquilibriumThreshold)
            {
                this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(currentLayerData.Temperature + averageTemperatureChange)));
            }

            OnTemperatureChanged(currentLayerData.Temperature);
        }
        private static short[] GetNeighborTemperatures(ReadOnlySpan<ISWorldSlot> neighbors, SWorldLayer layer)
        {
            List<short> temperatures = [];

            foreach (ISWorldSlot neighborSlot in neighbors)
            {
                ISWorldSlotLayer layerData = neighborSlot.GetLayer(layer);

                if (!layerData.IsEmpty && layerData.Element.EnableTemperature)
                {
                    temperatures.Add(layerData.Temperature);
                }
            }

            return [.. temperatures];
        }
        private static float CalculateTemperatureChange(short currentTemperature, short[] neighborTemperatures)
        {
            float totalChange = 0f;

            for (int i = 0; i < neighborTemperatures.Length; i++)
            {
                short neighborTemp = neighborTemperatures[i];
                totalChange += currentTemperature - neighborTemp;
            }

            return totalChange;
        }

        protected virtual void OnInstantiateStep(ISWorldSlot worldSlot, SWorldLayer worldLayer) { return; }
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnNeighbors(ReadOnlySpan<ISWorldSlot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
    }
}
