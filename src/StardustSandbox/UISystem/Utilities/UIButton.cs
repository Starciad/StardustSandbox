using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class UIButton(Texture2D iconTexture, Rectangle? iconTextureRectangle, string name, string description, Action clickAction)
    {
        internal Texture2D IconTexture => iconTexture;
        internal Rectangle? IconTextureRectangle => iconTextureRectangle;
        internal string Name => name;
        internal string Description => description;
        internal Action ClickAction => clickAction;
    }
}
