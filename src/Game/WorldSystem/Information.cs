using Microsoft.Xna.Framework;

using StardustSandbox.Interfaces;

using System;

namespace StardustSandbox.WorldSystem
{
    internal sealed class Information : IResettable
    {
        internal string Identifier { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal Point Size { get; set; }

        public void Reset()
        {
            this.Identifier = Guid.NewGuid().ToString();

            this.Name = string.Empty;
            this.Description = string.Empty;
        }
    }
}
