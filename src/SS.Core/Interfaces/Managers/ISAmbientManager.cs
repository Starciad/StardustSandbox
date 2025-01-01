using StardustSandbox.Core.Interfaces.Ambient.Handlers;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISAmbientManager : ISManager
    {
        ISBackgroundHandler BackgroundHandler { get; }
        ISSkyHandler SkyHandler { get; }
        ISCelestialBodyHandler CelestialBodyHandler { get; }
        ISCloudHandler CloudHandler { get; }
    }
}
