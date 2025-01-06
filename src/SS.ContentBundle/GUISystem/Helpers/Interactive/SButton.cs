using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive
{
    internal sealed class SButton(Texture2D iconTexture, string displayName, string description, Action clickAction)
    {
        internal Texture2D IconTexture => iconTexture;
        internal string DisplayName => displayName;
        internal string Description => description;
        internal Action ClickAction => clickAction;
    }
}
