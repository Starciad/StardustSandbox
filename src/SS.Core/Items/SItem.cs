using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Items;

namespace StardustSandbox.Core.Items
{
    public sealed class SItem(string identifier, string displayName, string description, SItemContentType contentType, SCategory category, Texture2D iconTexture)
    {
        public string Identifier => identifier;
        public string DisplayName => displayName;
        public string Description => description;
        public SItemContentType ContentType => contentType;
        public SCategory Category => category;
        public Texture2D IconTexture => iconTexture;
    }
}
