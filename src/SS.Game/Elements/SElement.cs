﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Elements.Rendering;
using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements
{
    public abstract class SElement : SGameObject
    {
        // VARIABLES

        #region Settings (Header)
        public uint Id { get; protected set; }
        public Texture2D Texture { get; protected set; }
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
        public SElementRendering Rendering { get; private set; }
        public ISElementContext Context { get; internal set; }
        #endregion

        public SElement(SGame gameInstance) : base(gameInstance)
        {
            this.Rendering = new(gameInstance, this);
        }

        // METHODS

        #region Engine
        public override void Update(GameTime gameTime)
        {
            this.Rendering.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.Rendering.SetContext(this.Context);
            this.Rendering.Draw(gameTime, spriteBatch);
        }

        public void AwakeStep(SWorldSlot worldSlot)
        {
            OnAwakeStep(worldSlot);
        }

        public void Steps()
        {
            if (this.Context.TryGetElementNeighbors(this.Context.Position, out ReadOnlySpan<(Point, SWorldSlot)> neighbors))
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
        protected virtual void OnAwakeStep(SWorldSlot worldSlot) { return; }

        // Steps
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        public virtual void OnBehaviourStep() { return; }
        #endregion

        #region System
        private void UpdateTemperature(ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            float totalTemperatureChange = 0;

            foreach ((Point, SWorldSlot) neighbor in neighbors)
            {
                if (!neighbor.Item2.Element.EnableTemperature)
                {
                    continue;
                }

                totalTemperatureChange += this.Context.Slot.Temperature - neighbor.Item2.Temperature;
            }

            int averageTemperatureChange = (int)Math.Round(totalTemperatureChange / neighbors.Length);

            this.Context.SetElementTemperature(STemperatureMath.Clamp(this.Context.Slot.Temperature - averageTemperatureChange));
            if (MathF.Abs(averageTemperatureChange) < STemperatureMath.EquilibriumThreshold)
            {
                this.Context.SetElementTemperature(STemperatureMath.Clamp(this.Context.Slot.Temperature + averageTemperatureChange));
            }

            OnTemperatureChanged(this.Context.Slot.Temperature);
        }
        #endregion

        #region Events
        protected virtual void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length) { return; }
        protected virtual void OnTemperatureChanged(short currentValue) { return; }
        #endregion
    }
}
