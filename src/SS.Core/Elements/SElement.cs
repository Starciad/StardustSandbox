using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Objects;

using System;

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
            this.Rendering.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.Rendering.SetContext(this.Context);
            this.Rendering.Draw(gameTime, spriteBatch);
        }

        public void InstantiateStep(ISWorldSlot worldSlot)
        {
            OnInstantiateStep(worldSlot);
        }

        public void Steps()
        {
            if (this.Context.TryGetElementNeighbors(this.Context.Position, out ISWorldSlot[] neighbors))
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
            float totalTemperatureChange = 0;

            foreach (ISWorldSlot neighbor in neighbors)
            {
                if (!neighbor.Element.EnableTemperature)
                {
                    continue;
                }

                totalTemperatureChange += this.Context.Slot.Temperature - neighbor.Temperature;
            }

            int averageTemperatureChange = (int)Math.Round(totalTemperatureChange / neighbors.Length);

            this.Context.SetElementTemperature(STemperatureMath.Clamp(this.Context.Slot.Temperature - averageTemperatureChange));
            if (MathF.Abs(averageTemperatureChange) < STemperatureMath.EquilibriumThreshold)
            {
                this.Context.SetElementTemperature(STemperatureMath.Clamp(this.Context.Slot.Temperature + averageTemperatureChange));
            }

            OnTemperatureChanged(this.Context.Slot.Temperature);
        }

        protected virtual void OnInstantiateStep(ISWorldSlot worldSlot) { return; }
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnNeighbors(ReadOnlySpan<ISWorldSlot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
    }
}
