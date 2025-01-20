using System.Collections.Generic;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content
{
    public sealed class SSaveFileWorldContent
    {
        public IEnumerable<SSaveFileWorldSlot> Slots { get; set; }
    }
}
