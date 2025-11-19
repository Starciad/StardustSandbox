using StardustSandbox.IO.Saving.World.Content;
using StardustSandbox.IO.Saving.World.Environment;
using StardustSandbox.IO.Saving.World.Information;

namespace StardustSandbox.IO.Saving
{
    public sealed class SaveFileWorld
    {
        public SaveFileWorldInformation Information { get; set; } = new();
        public SaveFileWorldResources Resources { get; set; } = new();
        public SaveFileWorldEnvironment Environment { get; set; } = new();
        public SaveFileWorldContent Content { get; set; } = new();
    }
}
