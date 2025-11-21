using MessagePack;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.IO.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SaveFileResource(uint index, ElementIndex value)
    {
        [Key(0)] public uint Index => index;
        [Key(1)] public ElementIndex Value => value;
    }
}
