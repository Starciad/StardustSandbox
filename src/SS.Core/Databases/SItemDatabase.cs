using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed partial class SItemDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISItemDatabase
    {
        public int TotalCategoryCount => this.categories.Count;
        public int TotalItemCount => this.items.Count;

        public SItemCategory[] Categories { get; private set; }
        public SItem[] Items { get; private set; }

        private readonly Dictionary<string, SItemCategory> categories = [];
        private readonly Dictionary<string, SItem> items = [];

        public override void Initialize()
        {
            foreach (SItemCategory category in this.categories.Values)
            {
                foreach (SItem item in this.items.Values)
                {
                    if (item.Category == category)
                    {
                        category.AddItem(item);
                    }
                }
            }

            this.Categories = [.. this.categories.Values];
            this.Items = [.. this.items.Values];
        }

        public void RegisterCategory(string identifier, string displayName, string description, Texture2D iconTexture)
        {
            this.categories.Add(identifier, new(identifier, displayName, description, iconTexture));
        }

        public void RegisterItem(string identifier, string displayName, string description, SItemContentType contentType, string categoryIdentifier, Texture2D iconTexture, Type referencedType)
        {
            this.items.Add(identifier, new(identifier, displayName, description, contentType, this.categories[categoryIdentifier], iconTexture, referencedType));
        }

        public SItemCategory GetCategoryById(string identifier)
        {
            return this.categories[identifier];
        }

        public SItem GetItemById(string identifier)
        {
            return this.items[identifier];
        }
    }
}
