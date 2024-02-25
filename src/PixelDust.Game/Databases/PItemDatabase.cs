using PixelDust.Game.Items;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Databases
{
    public sealed class PItemDatabase : PGameObject
    {
        private readonly List<PItem> items = [];
        private readonly Dictionary<string, List<PItem>> categories = [];

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
            foreach (PItem item in this.items)
            {
                if (this.categories.TryGetValue(item.Category, out List<PItem> value))
                {
                    value.Add(item);
                }
                else
                {
                    this.categories.Add(item.Category, [item]);
                }
            }
        }

        public void RegisterItem(PItem item)
        {
            this.items.Add(item);
        }

        public PItem GetItemById(string id)
        {
            return this.items.Find(x => x.Identifier == id);
        }

        public PItem GetItemByIndex(int index)
        {
            return this.items[Math.Clamp(index, 0, this.items.Count - 1)];
        }
    }
}
