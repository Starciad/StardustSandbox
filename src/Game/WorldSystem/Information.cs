using Microsoft.Xna.Framework;

using StardustSandbox.Interfaces;

namespace StardustSandbox.WorldSystem
{
    internal sealed class Information : IResettable
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal Point Size { get; set; }

        public void Reset()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
        }
    }
}
