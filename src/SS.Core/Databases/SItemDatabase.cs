﻿using Microsoft.Xna.Framework.Graphics;

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

        public IEnumerable<SCategory> Categories { get; private set; }
        public IEnumerable<SItem> Items { get; private set; }

        private readonly Dictionary<string, SCategory> categories = [];
        private readonly Dictionary<string, SItem> items = [];

        public override void Initialize()
        {
            foreach (SCategory category in this.categories.Values)
            {
                foreach (SItem item in this.items.Values)
                {
                    if (item.Category == category)
                    {
                        category.AddItem(item);
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

        public SCategory GetCategoryById(string identifier)
        {
            return this.categories[identifier];
        }

        public SItem GetItemById(string identifier)
        {
            return this.items[identifier];
        }
    }
}
