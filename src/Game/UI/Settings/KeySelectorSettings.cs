using Microsoft.Xna.Framework.Input;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct KeySelectorSettings
    {
        internal readonly string Synopsis { get; init; }
        internal readonly Action<Keys> OnSelectedKey { get; init; }
    }
}
