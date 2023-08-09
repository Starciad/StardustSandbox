using Microsoft.Xna.Framework;

using PixelDust.Core.Worlding;

namespace PixelDust.Core.Elements
{
    public abstract class PElement
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public byte Id { get; internal set; }
        public Color Color { get; protected set; }

        public short DefaultTemperature { get; protected set; } = 20;
        public short DefaultDensity { get; protected set; } = 1;
        public bool HasColorVariation { get; protected set; } = true;
        public int DefaultDispersionRate { get; protected set; } = 1;
        public bool EnableDefaultBehaviour { get; protected set; } = true;

        protected PElementContext Context { get; private set; }

        internal void Build()
        {
            OnSettings();
        }

        internal void Update(PElementContext ctx)
        {
            Context = ctx;
            UpdateSteps();
        }

        private void UpdateSteps()
        {
            OnBeforeStep();
            OnStep();

            if (EnableDefaultBehaviour)
            {
                OnBehaviourStep();
            }

            if (Context.TryGetNeighbors(Context.Position, out (Vector2, PWorldSlot)[] neighbors))
            {
                OnNeighbors(neighbors, neighbors.Length);
            }

            OnAfterStep();
        }

        // Settings
        protected virtual void OnSettings() { return; }

        // Steps
        protected virtual void OnBeforeStep() { return; }
        protected virtual void OnStep() { return; }
        protected virtual void OnAfterStep() { return; }
        internal virtual void OnBehaviourStep() { return; }

        // Ambient
        protected virtual void OnNeighbors((Vector2, PWorldSlot)[] neighbors, int length) { return; }
    }
}
