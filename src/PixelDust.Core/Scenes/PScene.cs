namespace PixelDust.Core.Scenes
{
    /// <summary>
    /// Base abstract class for defining scenes in the project.
    /// </summary>
    public abstract class PScene
    {
        /// <summary>
        /// Loads the scene.
        /// </summary>
        internal void Load()
        {
            OnLoad();
        }

        /// <summary>
        /// Unloads the scene.
        /// </summary>
        internal void Unload()
        {
            OnUnload();
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        internal void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        internal void Draw()
        {
            OnDraw();
        }

        // =========================== //

        /// <summary>
        /// Called when the scene is being loaded.
        /// </summary>
        protected abstract void OnLoad();

        /// <summary>
        /// Called when the scene is being unloaded.
        /// </summary>
        protected abstract void OnUnload();

        /// <summary>
        /// Called during each update cycle of the scene.
        /// </summary>
        protected virtual void OnUpdate() { return; }

        /// <summary>
        /// Called during each draw cycle of the scene.
        /// </summary>
        protected virtual void OnDraw() { return; }
    }
}