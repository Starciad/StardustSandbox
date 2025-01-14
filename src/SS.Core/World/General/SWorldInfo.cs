using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldInfo : ISResettable
    {
        public string Identifier { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SSize2 Size { get; set; }

        public void Reset()
        {
            this.Identifier = Guid.NewGuid().ToString();

            this.Name = string.Empty;
            this.Description = string.Empty;
        }
    }
}
