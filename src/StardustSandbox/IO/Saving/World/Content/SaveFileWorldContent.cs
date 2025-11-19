using System.Collections.Generic;

namespace StardustSandbox.IO.Saving.World.Content
{
    public sealed class SaveFileWorldContent
    {
        public IEnumerable<SaveFileSlot> Slots { get; set; }
    }
}
