using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISAmbientManager : ISManager
    {
        ISTimeHandler TimeHandler { get; }
        ISBackgroundHandler BackgroundHandler { get; }
        ISSkyHandler SkyHandler { get; }
        ISCelestialBodyHandler CelestialBodyHandler { get; }
        ISCloudHandler CloudHandler { get; }
    }
}
