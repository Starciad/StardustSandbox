using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core
{
    public sealed partial class SGame
    {
        // Event occurs when the game window returns to focus.
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            this.isFocused = true;
        }

        // Event occurs when the game window stops having focus.
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            this.isFocused = false;
        }

        // Event occurs when the game process is finished.
        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            base.OnExiting(sender, args);
        }
    }
}
