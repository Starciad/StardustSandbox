using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.Interactive
{
    internal sealed class SButton(Texture2D iconTexture, string displayName, Action clickAction)
    {
        internal Texture2D IconTexture => iconTexture;
        internal string DisplayName => displayName;
        internal Action ClickAction => clickAction;
    }
}
