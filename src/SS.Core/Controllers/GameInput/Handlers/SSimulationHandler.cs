using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SSimulationHandler(SGameManager gameManager)
    {
        private readonly SGameManager gameManager = gameManager;

        public void TogglePause()
        {
            this.gameManager.GameState.IsSimulationPaused = !this.gameManager.GameState.IsSimulationPaused;
        }
    }
}
