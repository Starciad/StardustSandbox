using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.Mathematics;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.Elements;

namespace PixelDust.Game.GUI.Common
{
    public sealed partial class PGUI_HUD : PGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D squareShapeTexture;

        private int slotSelectedIndex = 0;

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");

            SelectElementSlot(0, 0);

            base.OnAwake();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                PGUIImageElement slot = (PGUIImageElement)this.headerElementSlots[i];

                // Check Click
                if (this.GUIEvents.OnMouseClick(slot.Position, new Size2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectElementSlot(i, (int)slot.GetData(PHUDConstants.SLOT_ELEMENT_INDEX_NAME));
                }

                // Set Color
                if (i == this.slotSelectedIndex)
                {
                    slot.SetColor(Color.Red);
                }
                else
                {
                    slot.SetColor(Color.White);
                }
            }
        }

        private void SelectElementSlot(int slotIndex, int elementId)
        {
            this.slotSelectedIndex = slotIndex;
            this.Game.GameInputManager.SetSelectedElement(GetGameElement(elementId));
        }

        private PElement GetGameElement(int id)
        {
            return this.Game.ElementDatabase.GetElementById((uint)id);
        }
    }
}
