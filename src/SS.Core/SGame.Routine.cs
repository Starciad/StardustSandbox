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
            this.graphicsManager.Initialize();
            this.gameInputManager.Initialize();
            this.shaderManager.Initialize();
            this.inputManager.Initialize();
            this.guiManager.Initialize();
            this.cursorManager.Initialize();
            this.backgroundManager.Initialize();
            this.entityManager.Initialize();

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
            this.guiManager.ShowGUI(SGUIConstants.MAIN_MENU_IDENTIFIER);
            this.gameState.IsPaused = false;
            this.gameState.IsSimulationPaused = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.gameState.IsFocused || this.gameState.IsPaused)
            {
                return;
            }

            // Managers
            this.graphicsManager.Update(gameTime);
            this.gameInputManager.Update(gameTime);
            this.shaderManager.Update(gameTime);
            this.inputManager.Update(gameTime);
            this.guiManager.Update(gameTime);
            this.cursorManager.Update(gameTime);
            this.backgroundManager.Update(gameTime);

            if (!this.gameState.IsSimulationPaused)
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
