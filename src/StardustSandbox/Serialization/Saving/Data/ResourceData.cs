using MessagePack;

using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Serialization.Saving.Data
{
    [MessagePackObject]
    public sealed class ResourceData
    {
        [Key("Elements")]
        public ElementIndex[] Elements
        {
            get => this.elements;
            init => this.elements = value;
        }

        private readonly ElementIndex[] elements;
    }
}
