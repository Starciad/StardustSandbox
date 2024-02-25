using PixelDust.Game.Items;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.Databases
{
    public sealed class PItemDatabase : PGameObject
    {
        private readonly List<PItem> items = [];

        public void RegisterItem(PItem item)
        {
            items.Add(item);
        }
    }
}
