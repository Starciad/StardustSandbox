using StardustSandbox.Core.Controllers.GameInput.Simulation;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SPencilTool
    {
        private readonly SSimulationPen simulationPen;

        internal SPencilTool(SSimulationPen simulationPen)
        {
            this.simulationPen = simulationPen;
        }
    }
}
