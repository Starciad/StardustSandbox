using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.Mathematics;

using System;

namespace PixelDust.Game.GUI.Common
{
    public sealed partial class PGUI_HUD : PGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D squareShapeTexture;

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");

            base.OnAwake();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                PGUIElement slot = this.headerElementSlots[i];

                if (this.GUIEvents.OnMouseLeave(slot.Position, new Size2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    Console.WriteLine("Mouse Leave!");
                    slot.Style.Color = Color.Red;
                }
                else
                {
                    slot.Style.Color = Color.White;
                }
            }
        }
    }
}
