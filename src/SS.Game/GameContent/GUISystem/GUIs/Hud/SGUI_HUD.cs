using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.GameContent.GUISystem.Elements.Graphics;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.GameContent.GUISystem.GUIs.Hud
{
    public sealed partial class SGUI_HUD : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D squareShapeTexture;

        private int slotSelectedIndex = 0;

        public SGUI_HUD(SGame gameInstance, SGUIEvents guiEvents) : base(gameInstance, guiEvents)
        {
            this.Name = SGUIConstants.HUD_NAME;

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");

            SelectItemSlot(0, GetGameItemByIndex(0).Identifier);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            UpdateHeader();
            UpdateHeaderElements();
        }

        private void UpdateHeader()
        {
            // If the mouse is over the header, the player will not be able to interact with the environment. Otherwise, this permission is conceived.
            this.SGameInstance.GameInputManager.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.headerContainer.Position, this.headerContainer.Size);
        }

        private void UpdateHeaderElements()
        {
            #region ELEMENT SLOTS
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
                // Highlight when mouse is over slot.
                else if (this.GUIEvents.OnMouseOver(slot.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    slot.SetColor(Color.DarkGray);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    slot.SetColor(Color.White);
                }
            }
            #endregion

            #region SEARCH BUTTON
            // Check if the mouse clicked on the search button.
            if (this.GUIEvents.OnMouseDown(this.headerSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
            {
                this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_NAME);
                this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.ELEMENT_EXPLORER_NAME);
            }

            if (this.GUIEvents.OnMouseOver(this.headerSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
            {
                this.headerSearchButton.SetColor(Color.DarkGray);
            }
            else
            {
                this.headerSearchButton.SetColor(Color.White);
            }
            #endregion
        }
    }
}
