using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers
{
    internal sealed class SSimulationHandler(ISGame game)
    {
        private readonly ISGame game = game;

        public void TogglePause()
        {
            this.game.GameManager.GameState.IsSimulationPaused = !this.game.GameManager.GameState.IsSimulationPaused;
        }
    }
}
