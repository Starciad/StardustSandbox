using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.GUI;
using PixelDust.Game.Constants.GUI;
using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Common.HUD
{
    [PGameContent]
    [PGUIRegister]
    public sealed partial class PGUI_HUD : PGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D squareShapeTexture;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = PGUIConstants.HUD_NAME;

            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");

            SelectItemSlot(0, GetGameItemByIndex(0).Identifier);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            UpdateHeader();
            UpdateHeaderElementSelectionSlots();
        }

        private void UpdateHeader()
        {
            // If the mouse is over the header, the player will not be able to interact with the environment. Otherwise, this permission is conceived.
            this.Game.GameInputManager.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.headerContainer.Position, this.headerContainer.Size);
        }

        private void UpdateHeaderElementSelectionSlots()
        {
            // Individually check all element slots present in the HEADER.
            for (int i = 0; i < PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                PGUIImageElement slot = (PGUIImageElement)this.headerElementSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(slot.Position, new Size2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectItemSlot(i, (string)slot.GetData(PHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                // Highlight coloring of currently selected slot.
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
    }
}
