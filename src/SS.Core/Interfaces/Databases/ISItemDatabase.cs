using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Items;

using System;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISItemDatabase
    {
        int TotalCategoryCount { get; }
        int TotalItemCount { get; }

        SItemCategory[] Categories { get; }
        SItem[] Items { get; }

        void RegisterCategory(string identifier, string displayName, string description, Texture2D iconTexture);
        void RegisterItem(string identifier, string displayName, string description, SItemContentType contentType, string categoryIdentifier, Texture2D iconTexture, Type referencedType);

        SItemCategory GetCategoryById(string identifier);
        SItem GetItemById(string identifier);
    }
}
