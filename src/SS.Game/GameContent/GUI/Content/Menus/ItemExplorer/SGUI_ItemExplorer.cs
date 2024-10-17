using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.GameContent.GUI.Elements.Graphics;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D guiBackgroundTexture;
        private Texture2D squareShapeTexture;

        private string selectedCategoryName;
        private int selectedPageIndex;
        private SItem[] selectedItems;

        public SGUI_ItemExplorer(SGame gameInstance) : base(gameInstance)
        {
            this.Name = SGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");
        }

        protected override void OnLoad()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = false;
        }

        protected override void OnUnload()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            #region ITEM CATALOG
            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.itemSlots.Length; i++)
            {
                (SGUIImageElement itemSlotbackground, SGUIImageElement itemSlotIcon) = this.itemSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(itemSlotbackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    // SelectItemSlot(i, (string)slot.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                // Highlight coloring of currently selected slot.
                //if (i == this.slotSelectedIndex)
                //{
                //    itemSlotbackground.SetColor(Color.Red);
                //}
                // Highlight when mouse is over slot.
                if (this.GUIEvents.OnMouseOver(itemSlotbackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    itemSlotbackground.SetColor(Color.DarkGray);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    itemSlotbackground.SetColor(Color.White);
                }
            }
            #endregion
        }
    }
}
