using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.GUI;
using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.GUI.Elements.Common.Graphics;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.GUI.Common.HUD
{
    [SGameContent]
    [SGUIRegister]
    public sealed partial class SGUI_HUD : SGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D squareShapeTexture;

        private int slotSelectedIndex = 0;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = SGUIConstants.HUD_NAME;

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");

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
            this.SGameInstance.GameInputManager.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.headerContainer.Position, this.headerContainer.Size);
        }

        private void UpdateHeaderElementSelectionSlots()
        {
            // Individually check all element slots present in the HEADER.
            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                SGUIImageElement slot = (SGUIImageElement)this.headerElementSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(slot.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectItemSlot(i, (string)slot.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
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
