using StardustSandbox.Core.Items;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISItemDatabase
    {
        int TotalCategoryCount { get; }
        int TotalItemCount { get; }

        IEnumerable<SCategory> Categories { get; }
        IEnumerable<SItem> Items { get; }

        void RegisterCategory(SCategory category);
        void RegisterItem(SItem item);

        SCategory GetCategoryById(string identifier);
        SItem GetItemById(string identifier);
    }
}
