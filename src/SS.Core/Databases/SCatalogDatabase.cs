using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed partial class SCatalogDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISCatalogDatabase
    {
        public int TotalCategoryCount => this.categories.Count;
        public int TotalItemCount => this.items.Count;

        public IEnumerable<SCategory> Categories { get; private set; }
        public IEnumerable<SItem> Items { get; private set; }

        private readonly Dictionary<string, SCategory> categories = [];
        private readonly Dictionary<string, SItem> items = [];

        public override void Initialize()
        {
            foreach (SCategory category in this.categories.Values)
            {
                foreach (SSubcategory subcategory in category.Subcategories)
                {
                    foreach (SItem item in this.items.Values)
                    {
                        if (item.Subcategory == subcategory)
                        {
                            subcategory.AddItem(item);
                        }
                    }
                }
            }

            this.Categories = this.categories.Values;
            this.Items = this.items.Values;
        }

        public void RegisterCategory(SCategory value)
        {
            this.categories.Add(value.Identifier, value);
        }

        public void RegisterItem(SItem item)
        {
            this.items.Add(item.Identifier, item);
        }

        public SCategory GetCategory(string identifier)
        {
            return this.categories[identifier];
        }

        public SItem GetItem(string identifier)
        {
            return this.items[identifier];
        }
    }
}
