using StardustSandbox.Core.IO.Files.Saving.World.Content;
using StardustSandbox.Core.IO.Files.Saving.World.Environment;
using StardustSandbox.Core.IO.Files.Saving.World.Information;

namespace StardustSandbox.Core.IO.Files.Saving
{
    public sealed class SSaveFileWorld
    {
        public SSaveFileWorldInformation Information { get; set; } = new();
        public SSaveFileWorldResources Resources { get; set; } = new();
        public SSaveFileWorldEnvironment Environment { get; set; } = new();
        public SSaveFileWorldContent Content { get; set; } = new();
    }
}
