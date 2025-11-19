using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class UIButton(Texture2D iconTexture, string name, string description, Action clickAction)
    {
        internal Texture2D IconTexture => iconTexture;
        internal string Name => name;
        internal string Description => description;
        internal Action ClickAction => clickAction;
    }
}
