using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.World.General
{
    public sealed class SWorldInfo
    {
        public string Identifier { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SSize2 Size { get; set; }
    }
}
