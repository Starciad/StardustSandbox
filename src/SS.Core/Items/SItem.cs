using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Enums.Items;

using System;

namespace StardustSandbox.Game.Items
{
    public sealed class SItem(string identifier, string displayName, string description, SItemContentType contentType, SItemCategory category, Texture2D iconTexture, Type referencedType)
    {
        public string Identifier { get; private set; } = identifier;
        public string DisplayName { get; private set; } = displayName;
        public string Description { get; private set; } = description;
        public SItemContentType ContentType { get; private set; } = contentType;
        public SItemCategory Category { get; private set; } = category;
        public Texture2D IconTexture { get; private set; } = iconTexture;
        public Type ReferencedType { get; private set; } = referencedType;
    }
}
