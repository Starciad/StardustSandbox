using StardustSandbox.Enums.States;
using StardustSandbox.Managers;

namespace StardustSandbox.InputSystem.Game.Handlers
{
    internal sealed class SimulationHandler(GameManager gameManager)
    {
        private readonly GameManager gameManager = gameManager;

        internal void TogglePause()
        {
            this.gameManager.ToggleState(GameStates.IsSimulationPaused);
        }
    }
}
