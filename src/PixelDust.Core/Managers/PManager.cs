namespace PixelDust.Core.Managers
{
    /// <summary>
    /// Base class for building and configuring shape managers.
    /// </summary>
    public abstract class PManager
    {
        /// <summary>
        /// Invoked when the manager is being built.
        /// </summary>
        internal void Awake()
        {
            OnAwake();
        }

        /// <summary>
        /// Invoked when all project managers are finished building.
        /// </summary>
        internal void Start()
        {
            OnStart();
        }

        /// <summary>
        /// Game update call to manager.
        /// </summary>
        internal void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// Game draw call to manager.
        /// </summary>
        internal void Draw()
        {
            OnDraw();
        }

        // ========================= //

        /// <summary>
        /// Invoked when the manager is being built.
        /// </summary>
        protected virtual void OnAwake() { return; }

        /// <summary>
        /// Invoked when all project managers are finished building.
        /// </summary>
        protected virtual void OnStart() { return; }

        /// <summary>
        /// Game update call to manager.
        /// </summary>
        protected virtual void OnUpdate() { return; }

        /// <summary>
        /// Game draw call to manager.
        /// </summary>
        protected virtual void OnDraw() { return; }
    }
}
