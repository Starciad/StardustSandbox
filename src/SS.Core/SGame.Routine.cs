using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Plugins;

namespace StardustSandbox.Game
{
    public sealed partial class SGame
    {
        protected override void Initialize()
        {
            foreach (SPluginBuilder pluginBuilder in this.pluginBuilders)
            {
                pluginBuilder.Initialize(this, this.Content);
            }

            #region Databases
            this._assetDatabase.Initialize();
            this._elementDatabase.Initialize();
            this._itemDatabase.Initialize();
            this._guiDatabase.Initialize();
            this._backgroundDatabase.Initialize();
            #endregion

            #region Managers
            this._graphicsManager.Initialize();
            this._gameInputManager.Initialize();
            this._shaderManager.Initialize();
            this._inputManager.Initialize();
            this._guiManager.Initialize();
            this._cursorManager.Initialize();
            this._backgroundManager.Initialize();
            #endregion

            #region Game
            this._world.Initialize();
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._sb = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this._guiManager.ShowGUI(SGUIConstants.HUD_NAME);
            // this._guiManager.ShowGUI(SGUIConstants.ELEMENT_EXPLORER_NAME);
        }

        protected override void Update(GameTime gameTime)
        {
            // Managers
            this._graphicsManager.Update(gameTime);
            this._gameInputManager.Update(gameTime);
            this._shaderManager.Update(gameTime);
            this._inputManager.Update(gameTime);
            this._guiManager.Update(gameTime);
            this._cursorManager.Update(gameTime);
            this._backgroundManager.Update(gameTime);

            // Core
            this._world.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
