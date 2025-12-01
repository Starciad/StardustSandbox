using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

using System;

namespace StardustSandbox.UISystem.Information
{
    internal sealed class ButtonInfo(TextureIndex iconTextureIndex, Rectangle? iconTextureRectangle, string name, string description, Action clickAction)
    {
        internal Texture2D IconTexture => AssetDatabase.GetTexture(iconTextureIndex);
        internal Rectangle? IconTextureRectangle => iconTextureRectangle;
        internal string Name => name;
        internal string Description => description;
        internal Action ClickAction => clickAction;
    }
}
