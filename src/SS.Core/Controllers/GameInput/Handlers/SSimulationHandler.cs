using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SSimulationHandler(ISGameManager gameManager)
    {
        private readonly ISGameManager gameManager = gameManager;

        public void TogglePause()
        {
            this.gameManager.GameState.IsSimulationPaused = !this.gameManager.GameState.IsSimulationPaused;
        }
    }
}
