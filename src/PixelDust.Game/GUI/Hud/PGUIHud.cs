using MLEM.Ui.Elements;
using MLEM.Ui;

using PixelDust.Core.GUI;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.GUI
{
    internal sealed class PGUIHud : PGUI
    {
        protected override void OnBuild(IPGUIBuilder builder)
        {
            // Toolbar (HEADER)
            using (builder.Create(new Panel(Anchor.TopCenter, new(1f, 0.5f), Vector2.Zero)))
            {
                // Pen
                builder.CreateClosed(new Button(Anchor.CenterLeft, new(32, 32)));

                // Slots
                for (int i = 0; i < 5; i++)
                {
                    builder.CreateClosed(new Button(Anchor.Center, new(32, 32)));
                }

                // Search
                builder.CreateClosed(new Button(Anchor.CenterRight, new(32, 32)));
            }

            // Toolbar (LEFT)

        }
    }
}
