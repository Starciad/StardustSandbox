namespace StardustSandbox.Core.Interfaces.Ambient.Handlers
{
    public interface ISTimeHandler
    {
        float GlobalIllumination { get; }
        double CurrentSeconds { get; }
        double IntervalDuration { get; }
        double IntervalProgress { get; }
    }
}
