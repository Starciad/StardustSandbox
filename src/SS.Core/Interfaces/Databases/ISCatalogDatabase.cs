using StardustSandbox.Core.Catalog;

using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISCatalogDatabase
    {
        int TotalCategoryCount { get; }
        int TotalItemCount { get; }

        IEnumerable<SCategory> Categories { get; }
        IEnumerable<SItem> Items { get; }

        void RegisterCategory(SCategory category);
        void RegisterItem(SItem item);

        SCategory GetCategoryByIdentifier(string identifier);
        SItem GetItemByIdentifier(string identifier);
    }
}
