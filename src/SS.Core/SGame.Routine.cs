using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Plugins;

namespace StardustSandbox.Core
{
    public sealed partial class SGame
    {
        protected override void Initialize()
        {
            foreach (SPluginBuilder pluginBuilder in this.pluginBuilders)
            {
                pluginBuilder.Initialize(this, this.Content);
            }

            // Databases
            this._assetDatabase.Initialize();
            this._elementDatabase.Initialize();
            this._itemDatabase.Initialize();
            this._guiDatabase.Initialize();
            this._backgroundDatabase.Initialize();
            this._entityDatabase.Initialize();

            // Managers
            this._graphicsManager.Initialize();
            this._gameInputManager.Initialize();
            this._shaderManager.Initialize();
            this._inputManager.Initialize();
            this._guiManager.Initialize();
            this._cursorManager.Initialize();
            this._backgroundManager.Initialize();
            this._entityManager.Initialize();

            // Core
            this._world.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._sb = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this._guiManager.ShowGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.isFocused)
            {
                return;
            }

            // Managers
            this._graphicsManager.Update(gameTime);
            this._gameInputManager.Update(gameTime);
            this._shaderManager.Update(gameTime);
            this._inputManager.Update(gameTime);
            this._guiManager.Update(gameTime);
            this._cursorManager.Update(gameTime);
            this._backgroundManager.Update(gameTime);
            this._entityManager.Update(gameTime);

            // Core
            this._world.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
