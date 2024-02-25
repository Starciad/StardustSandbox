using PixelDust.Game.Items;
using PixelDust.Game.Objects;

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
    }
}
