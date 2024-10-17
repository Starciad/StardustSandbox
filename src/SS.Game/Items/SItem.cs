using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Enums.Items;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Items
{
    public sealed class SItem
    {
        public string Identifier { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public SItemContentType ContentType { get; private set; }
        public SItemCategory Category { get; private set; }
        public Texture2D IconTexture { get; private set; }
        public Type ReferencedType { get; private set; }

        public SItem(string identifier, string displayName, string description, SItemContentType contentType, SItemCategory category, Texture2D iconTexture, Type referencedType)
        {
            this.Identifier = identifier;
            this.DisplayName = displayName;
            this.Description = description;
            this.ContentType = contentType;
            this.Category = category;
            this.IconTexture = iconTexture;
            this.ReferencedType = referencedType;
        }
    }
}
