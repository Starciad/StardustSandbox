using PixelDust.Game.Items;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Databases
{
    public sealed class PItemDatabase : PGameObject
    {
        private readonly List<PItem> items = [];

        public void Build(PAssetDatabase assetDatabase)
        {
            this.items.ForEach(x => x.Build(assetDatabase));
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
