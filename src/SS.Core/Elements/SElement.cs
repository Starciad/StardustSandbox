using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
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

        public void InstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer)
        {
            OnInstantiateStep(worldSlot, worldLayer);
        }

        public void Steps()
        {
            if (this.context.TryGetNeighboringSlots(this.context.Slot.Position, out SWorldSlot[] neighbors))
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

        private void UpdateTemperature(SWorldSlot[] neighbors)
        {
            SWorldSlot slot = this.context.GetWorldSlot();
            SWorldSlotLayer currentLayerData = slot.GetLayer(this.context.Layer);

            SWorldLayer currentLayer = this.context.Layer;
            SWorldLayer oppositeLayer = this.context.Layer == SWorldLayer.Foreground ? SWorldLayer.Background : SWorldLayer.Foreground;

            float totalTemperatureChange = 0f;
            totalTemperatureChange += GetTemperatureChange(currentLayerData.Temperature, neighbors, currentLayer);
            totalTemperatureChange += GetTemperatureChange(currentLayerData.Temperature, neighbors, oppositeLayer);

            short neighborsCurrentLayerLength = 0;
            short neighborsOppositeLayerLength = 0;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors[i].GetLayer(currentLayer).IsEmpty)
                {
                    neighborsCurrentLayerLength++;
                }

                if (!neighbors[i].GetLayer(oppositeLayer).IsEmpty)
                {
                    neighborsOppositeLayerLength++;
                }
            }

            short averageTemperatureChange = (short)Math.Round(totalTemperatureChange / (short)(neighborsCurrentLayerLength + neighborsOppositeLayerLength));
            this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(currentLayerData.Temperature - averageTemperatureChange)));

            if (MathF.Abs(averageTemperatureChange) < STemperatureConstants.EQUILIBRIUM_THRESHOLD)
            {
                this.context.SetElementTemperature(this.context.Layer, STemperatureMath.Clamp((short)(currentLayerData.Temperature + averageTemperatureChange)));
            }

            OnTemperatureChanged(currentLayerData.Temperature);
        }
        private static float GetTemperatureChange(short currentTemperature, SWorldSlot[] neighbors, SWorldLayer layer)
        {
            float totalChange = 0f;

            foreach (SWorldSlot neighborSlot in neighbors)
            {
                SWorldSlotLayer neighborLayerData = neighborSlot.GetLayer(layer);

                if (!neighborLayerData.IsEmpty && neighborLayerData.Element.EnableTemperature)
                {
                    short neighborTemp = neighborLayerData.Temperature;
                    totalChange += currentTemperature - neighborTemp;
                }
            }

            return totalChange;
        }
        
        protected virtual void OnInstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer) { return; }
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnNeighbors(SWorldSlot[] neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
    }
}
