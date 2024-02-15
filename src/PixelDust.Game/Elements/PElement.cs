using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Elements.Renders;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Objects;
using PixelDust.Game.Utilities;
using PixelDust.Game.World.Slots;

using System;

namespace PixelDust.Game.Elements
{
    public abstract class PElement : PGameObject
    {
        #region Settings (Header)
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public uint Id { get; internal set; }
        public Texture2D Texture { get; protected set; }
        public Texture2D IconTexture { get; protected set; }
        #endregion

        #region Settings (Defaults)
        public int DefaultDispersionRate { get; protected set; } = 1;
        public short DefaultTemperature { get; protected set; }
        #endregion

        #region Settings (Enables)
        public bool EnableDefaultBehaviour { get; protected set; } = true;
        public bool EnableNeighborsAction { get; protected set; }
        public bool EnableTemperature { get; protected set; } = true;
        #endregion

        #region Helpers
        public PElementRender Render { get; private set; }
        public PElementContext Context { get; internal set; }
        #endregion

        #region Engine
        protected override void OnAwake()
        {
            this.Render = new();
            OnSettings();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            this.Render.Update(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.Render.Draw(gameTime, spriteBatch);
        }

        public void Steps()
        {
            if (this.Context.TryGetElementNeighbors(this.Context.Position, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors))
            {
                if (this.EnableTemperature)
                {
                    UpdateTemperature(neighbors);
                }

                if (this.EnableNeighborsAction)
                { OnNeighbors(neighbors, neighbors.Length); }
            }

            OnBeforeStep();
            OnStep();
            if (this.EnableDefaultBehaviour)
            { OnBehaviourStep(); }

            OnAfterStep();
        }
        #endregion

        #region Configurations
        protected virtual void OnSettings() { return; }

        // Steps
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        public virtual void OnBehaviourStep() { return; }
        #endregion

        #region System
        private void UpdateTemperature(ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            float totalTemperatureChange = 0;

            foreach ((Vector2Int, PWorldElementSlot) neighbor in neighbors)
            {
                if (!this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id).EnableTemperature)
                {
                    continue;
                }

                totalTemperatureChange += this.Context.Slot.Temperature - neighbor.Item2.Temperature;
            }

            int averageTemperatureChange = (int)Math.Round(totalTemperatureChange / neighbors.Length);

            this.Context.SetElementTemperature(PTemperature.Clamp(this.Context.Slot.Temperature - averageTemperatureChange));
            if (MathF.Abs(averageTemperatureChange) < PTemperature.EquilibriumThreshold)
            {
                this.Context.SetElementTemperature(PTemperature.Clamp(this.Context.Slot.Temperature + averageTemperatureChange));
            }

            OnTemperatureChanged(this.Context.Slot.Temperature);
        }
        #endregion

        #region Events
        protected virtual void OnNeighbors(ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors, int length) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
        #endregion
    }
}
