using StardustSandbox.Game.Items;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Game.Databases
{
    public sealed class SItemDatabase : SGameObject
    {
        public string[] Categories => [.. this.categories];
        public SItem[] Items => [.. this.items];

        private readonly List<SItem> items = [];
        private readonly List<string> categories = [];

        private readonly Dictionary<string, List<SItem>> catalog = [];

        public void Build(SAssetDatabase assetDatabase)
        {
            BuildItems(assetDatabase);
            BuildCategories();
        }
        private void BuildItems(SAssetDatabase assetDatabase)
        {
            this.items.ForEach(x => x.Build(assetDatabase));
        }
        private void BuildCategories()
        {
            IEnumerable<IGrouping<string, SItem>> groupedItems = this.items.GroupBy(item => item.Category);

            foreach (IGrouping<string, SItem> group in groupedItems)
            {
                string category = group.Key;

                if (!this.categories.Contains(category))
                {
                    this.categories.Add(category);
                }

                if (this.catalog.TryGetValue(category, out List<SItem> value))
                {
                    value.AddRange([.. group]);
                }
                else
                {
                    this.catalog.Add(category, [.. group]);
                }
            }
        }

        internal void RegisterItem(SItem item)
        {
            this.items.Add(item);
        }

        public SItem[] GetCatalogItems(int categoryIndex)
        {
            return [.. this.catalog[this.categories[categoryIndex]]];
        }

        public SItem[] GetCatalogItems(string categoryId)
        {
            return [.. this.catalog[categoryId]];
        }

        public SItem GetItemById(string id)
        {
            return this.items.Find(x => x.Identifier == id);
        }
    }
}
