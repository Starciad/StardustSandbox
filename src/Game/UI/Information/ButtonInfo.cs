using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

using System;

namespace StardustSandbox.UI.Information
{
    internal sealed class ButtonInfo(TextureIndex textureIndex, Rectangle? textureSourceRectangle, string name, string description, Action clickAction)
    {
        internal Texture2D Texture => AssetDatabase.GetTexture(textureIndex);
        internal Rectangle? TextureSourceRectangle => textureSourceRectangle;
        internal string Name => name;
        internal string Description => description;
        internal Action ClickAction => clickAction;
    }
}
