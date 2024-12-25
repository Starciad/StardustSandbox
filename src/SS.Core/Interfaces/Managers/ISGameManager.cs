namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISGameManager : ISManager
    {
        SGameState GameState { get; }

        void StartGame();
    }
}
