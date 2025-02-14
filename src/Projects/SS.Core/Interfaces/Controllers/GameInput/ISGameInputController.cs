using StardustSandbox.Core.Controllers.GameInput.Simulation;

namespace StardustSandbox.Core.Interfaces.Controllers.GameInput
{
    public interface ISGameInputController
    {
        SSimulationPen Pen { get; }
        SSimulationPlayer Player { get; }

        void Activate();
        void Disable();
    }
}
