using StardustSandbox.Core.Enums.Simulation;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISGameManager : ISManager
    {
        SGameState GameState { get; }

        void StartGame();
        void SetSimulationSpeed(SSimulationSpeed speed);
    }
}
