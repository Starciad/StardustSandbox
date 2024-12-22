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
            this.assetDatabase.Initialize();
            this.elementDatabase.Initialize();
            this.itemDatabase.Initialize();
            this.guiDatabase.Initialize();
            this.backgroundDatabase.Initialize();
            this.entityDatabase.Initialize();

            // Managers
            this.gameManager.Initialize();
            this.graphicsManager.Initialize();
            this.shaderManager.Initialize();
            this.inputManager.Initialize();
            this.guiManager.Initialize();
            this.cursorManager.Initialize();
            this.backgroundManager.Initialize();
            this.entityManager.Initialize();

            // Controllers
            this.gameInputController.Initialize();

            // Core
            this.world.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new(this.GraphicsDevice);
        }

        protected override void BeginRun()
        {
            this.guiManager.OpenGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
            this.gameManager.GameState.IsPaused = false;
            this.gameManager.GameState.IsSimulationPaused = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameManager.GameState.IsFocused || this.gameManager.GameState.IsPaused)
            {
                return;
            }

            // Controllers
            this.gameInputController.Update(gameTime);

            // Managers
            this.gameManager.Update(gameTime);
            this.graphicsManager.Update(gameTime);
            this.shaderManager.Update(gameTime);
            this.inputManager.Update(gameTime);
            this.guiManager.Update(gameTime);
            this.cursorManager.Update(gameTime);
            this.backgroundManager.Update(gameTime);

            if (!this.gameManager.GameState.IsSimulationPaused && !this.gameManager.GameState.IsCriticalMenuOpen)
            {
                // Managers
                this.entityManager.Update(gameTime);

                // Core
                this.world.Update(gameTime);
            }

            base.Update(gameTime);
        }
    }
}
