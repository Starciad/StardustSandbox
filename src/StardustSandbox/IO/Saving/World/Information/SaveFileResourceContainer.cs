using MessagePack;

using StardustSandbox.Enums.Indexers;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.IO.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SaveFileResourceContainer
    {
        [Key(0)]
        public IEnumerable<SaveFileResource> Resources
        {
            get => this.resources;
            set => this.resources = [.. value];
        }

        private List<SaveFileResource> resources = [];

        public SaveFileResourceContainer()
        {

        }

        public void Add(ElementIndex value)
        {
            this.resources.Add(new((uint)this.resources.Count, value));
        }

        public ElementIndex FindValueByIndex(uint index)
        {
            return this.resources.FirstOrDefault(x => x.Index == index).Value;
        }

        public uint FindIndexByValue(ElementIndex value)
        {
            return this.resources.FirstOrDefault(x => x.Value == value).Index;
        }

        public bool ContainsIndex(uint index)
        {
            return this.resources.Find(x => x.Index == index) != null;
        }

        public bool ContainsValue(ElementIndex value)
        {
            return this.resources.Find(x => x.Value == value) != null;
        }
    }
}
