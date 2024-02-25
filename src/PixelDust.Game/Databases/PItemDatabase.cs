using PixelDust.Game.Items;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Game.Databases
{
    public sealed class PItemDatabase : PGameObject
    {
        public string[] Categories => [.. this.categories];
        public PItem[] Items => [.. this.items];

        private readonly List<PItem> items = [];
        private readonly List<string> categories = [];

        private readonly Dictionary<string, List<PItem>> catalog = [];

        public void Build(PAssetDatabase assetDatabase)
        {
            BuildItems(assetDatabase);
            BuildCategories();
        }
        private void BuildItems(PAssetDatabase assetDatabase)
        {
            this.items.ForEach(x => x.Build(assetDatabase));
        }
        private void BuildCategories()
        {
            IEnumerable<IGrouping<string, PItem>> groupedItems = this.items.GroupBy(item => item.Category);

            foreach (IGrouping<string, PItem> group in groupedItems)
            {
                string category = group.Key;

                if (!this.categories.Contains(category))
                {
                    this.categories.Add(category);
                }

                if (this.catalog.TryGetValue(category, out List<PItem> value))
                {
                    value.AddRange([.. group]);
                }
                else
                {
                    this.catalog.Add(category, [.. group]);
                }
            }
        }

        internal void RegisterItem(PItem item)
        {
            this.items.Add(item);
        }

        public PItem[] GetCatalogItems(int categoryIndex)
        {
            return [.. this.catalog[this.categories[categoryIndex]]];
        }

        public PItem[] GetCatalogItems(string categoryId)
        {
            return [.. this.catalog[categoryId]];
        }

        public PItem GetItemById(string id)
        {
            return this.items.Find(x => x.Identifier == id);
        }
    }
}
