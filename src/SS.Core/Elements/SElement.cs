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

        public void InstantiateStep(ISWorldSlot worldSlot)
        {
            OnInstantiateStep(worldSlot);
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
            float totalTemperatureChangeForeground = 0;
            float totalTemperatureChangeBackground = 0;

            int foregroundNeighborCount = 0;
            int backgroundNeighborCount = 0;

            foreach (ISWorldSlot neighbor in neighbors)
            {
                // Update Foreground Layer Temperatures
                if (!neighbor.ForegroundLayer.IsEmpty && neighbor.ForegroundLayer.Element.EnableTemperature)
                {
                    totalTemperatureChangeForeground += this.context.Slot.ForegroundLayer.Temperature - neighbor.ForegroundLayer.Temperature;
                    foregroundNeighborCount++;
                }

                // Update Background Layer Temperatures
                if (!neighbor.BackgroundLayer.IsEmpty && neighbor.BackgroundLayer.Element.EnableTemperature)
                {
                    totalTemperatureChangeBackground += this.context.Slot.BackgroundLayer.Temperature - neighbor.BackgroundLayer.Temperature;
                    backgroundNeighborCount++;
                }

                // Cross-interaction between Foreground and Background
                if (!neighbor.ForegroundLayer.IsEmpty && neighbor.ForegroundLayer.Element.EnableTemperature)
                {
                    totalTemperatureChangeBackground += this.context.Slot.BackgroundLayer.Temperature - neighbor.ForegroundLayer.Temperature;
                    backgroundNeighborCount++;
                }

                if (!neighbor.BackgroundLayer.IsEmpty && neighbor.BackgroundLayer.Element.EnableTemperature)
                {
                    totalTemperatureChangeForeground += this.context.Slot.ForegroundLayer.Temperature - neighbor.BackgroundLayer.Temperature;
                    foregroundNeighborCount++;
                }
            }

            // Calculate average changes for each layer
            int averageTemperatureChangeForeground = foregroundNeighborCount > 0
                ? (int)Math.Round(totalTemperatureChangeForeground / foregroundNeighborCount)
                : 0;

            int averageTemperatureChangeBackground = backgroundNeighborCount > 0
                ? (int)Math.Round(totalTemperatureChangeBackground / backgroundNeighborCount)
                : 0;

            // Apply changes to Foreground
            short newTemperatureForeground = STemperatureMath.Clamp((short)(this.context.Slot.ForegroundLayer.Temperature - averageTemperatureChangeForeground));
            this.context.SetElementTemperature(SWorldLayer.Foreground, newTemperatureForeground);

            // Check if it is close to balance for the Foreground
            if (MathF.Abs(averageTemperatureChangeForeground) < STemperatureMath.EquilibriumThreshold)
            {
                newTemperatureForeground = STemperatureMath.Clamp((short)(this.context.Slot.ForegroundLayer.Temperature + averageTemperatureChangeForeground));

                this.context.SetElementTemperature(SWorldLayer.Foreground, newTemperatureForeground);
            }

            // Apply Changes to Background
            short newTemperatureBackground = STemperatureMath.Clamp((short)(this.context.Slot.BackgroundLayer.Temperature - averageTemperatureChangeBackground));
            this.context.SetElementTemperature(SWorldLayer.Background, newTemperatureBackground);

            // Check if it is close to balance for the Background
            if (MathF.Abs(averageTemperatureChangeBackground) < STemperatureMath.EquilibriumThreshold)
            {
                newTemperatureBackground = STemperatureMath.Clamp((short)(this.context.Slot.BackgroundLayer.Temperature + averageTemperatureChangeBackground));
                this.context.SetElementTemperature(SWorldLayer.Background, newTemperatureBackground);
            }

            // Notify changes
            OnTemperatureChanged(newTemperatureForeground, newTemperatureBackground);
        }

        protected virtual void OnInstantiateStep(ISWorldSlot worldSlot) { return; }
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        protected virtual void OnBehaviourStep() { return; }

        protected virtual void OnNeighbors(ReadOnlySpan<ISWorldSlot> neighbors) { return; }
        protected virtual void OnTemperatureChanged(short foregroundValue, short backgroundValue) { return; }
    }
}
