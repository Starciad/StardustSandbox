using MessagePack;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Core.IO.Files.Saving.World.Information
{
    [MessagePackObject]
    public sealed class SSaveFileResourceContainer
    {
        [Key(0)]
        public IEnumerable<SSaveFileResource> Resources
        {
            get => this.resources;

            set => this.resources = [.. value];
        }

        private List<SSaveFileResource> resources = [];

        public void Add(string value)
        {
            this.resources.Add(new((uint)this.resources.Count, value));
        }

        public string FindValueByIndex(uint index)
        {
            return this.resources.FirstOrDefault(x => x.Index == index).Value;
        }

        public uint FindIndexByValue(string value)
        {
            return this.resources.FirstOrDefault(x => x.Value == value).Index;
        }

        public bool ContainsIndex(uint index)
        {
            return this.resources.Find(x => x.Index == index) != null;
        }

        public bool ContainsValue(string value)
        {
            return this.resources.Find(x => x.Value == value) != null;
        }
    }
}
