using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.Hud
{
    public partial class SGUI_HUD
    {
        private ISGUILayoutBuilder layout;

        private SGUIImageElement topToolbarContainer;
        private SGUIImageElement leftToolbarContainer;
        private SGUIImageElement rightToolbarContainer;

        private SGUIImageElement toolbarElementSearchButton;
        private readonly (SGUIImageElement background, SGUIImageElement icon)[] toolbarElementSlots = new (SGUIImageElement, SGUIImageElement)[SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH];

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildTopToolbar();
            BuildLeftToolbar();
            BuildRightToolbar();
        }

        private void BuildTopToolbar()
        {
            this.topToolbarContainer = new(this.SGameInstance);
            this.topToolbarContainer.SetTexture(this.particleTexture);
            this.topToolbarContainer.SetScale(new Vector2(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96));
            this.topToolbarContainer.SetColor(new Color(Color.White, 32));
            this.topToolbarContainer.SetSize(SSize2F.One);
            this.topToolbarContainer.PositionRelativeToScreen();

            this.layout.AddElement(this.topToolbarContainer);

            // ================================= //

            CreateSlots();
            CreateSearchSlot();

            // ================================= //

            void CreateSlots()
            {
                int slotSize = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
                int slotScale = SHUDConstants.SLOT_SCALE;
                int slotSpacing = slotSize * 2;

                Vector2 slotMargin = new(slotSpacing, 0);

                for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
                {
                    // Creation
                    SGUIImageElement slotBackground = new(this.SGameInstance);
                    SGUIImageElement slotIcon = new(this.SGameInstance);

                    // Background
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(SCardinalDirection.Center);
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(SCardinalDirection.West);
                    slotBackground.SetSize(new SSize2(slotSize));
                    slotBackground.SetMargin(slotMargin);

                    SItem selectedItem = this.SGameInstance.ItemDatabase.Items[i];
                    if (!slotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                    {
                        slotBackground.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, selectedItem.Identifier);
                    }

                    // Icon
                    slotIcon.SetTexture(selectedItem.IconTexture);
                    slotIcon.SetOriginPivot(SCardinalDirection.Center);
                    slotIcon.SetScale(new Vector2(1.5f));
                    slotIcon.SetSize(new SSize2(slotSize));

                    // Update
                    slotBackground.PositionRelativeToElement(this.topToolbarContainer);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Save
                    this.toolbarElementSlots[i] = (slotBackground, slotIcon);

                    // Spacing
                    slotMargin.X += slotSpacing + (slotSize / 2);

                    this.layout.AddElement(slotBackground);
                    this.layout.AddElement(slotIcon);
                }
            }

            void CreateSearchSlot()
            {
                SGUIImageElement slotSearchBackground = new(this.SGameInstance);
                SGUIImageElement slotIcon = new(this.SGameInstance);

                // Background
                slotSearchBackground.SetTexture(this.squareShapeTexture);
                slotSearchBackground.SetOriginPivot(SCardinalDirection.Center);
                slotSearchBackground.SetScale(new Vector2(SHUDConstants.SLOT_SCALE + 0.45f));
                slotSearchBackground.SetPositionAnchor(SCardinalDirection.East);
                slotSearchBackground.SetSize(new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE));
                slotSearchBackground.SetMargin(new Vector2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0));
                slotSearchBackground.PositionRelativeToElement(this.topToolbarContainer);

                // Icon
                slotIcon.SetTexture(this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1"));
                slotIcon.SetOriginPivot(SCardinalDirection.Center);
                slotIcon.SetScale(new Vector2(2f));
                slotIcon.SetSize(new SSize2(1));
                slotIcon.PositionRelativeToElement(slotSearchBackground);

                this.layout.AddElement(slotSearchBackground);
                this.layout.AddElement(slotIcon);

                this.toolbarElementSearchButton = slotSearchBackground;
            }
        }
        private void BuildLeftToolbar()
        {
            this.leftToolbarContainer = new(this.SGameInstance);
            this.leftToolbarContainer.SetTexture(this.particleTexture);
            this.leftToolbarContainer.SetScale(new Vector2(96, 608));
            this.leftToolbarContainer.SetColor(new Color(Color.White, 32));
            this.leftToolbarContainer.SetSize(SSize2F.One);
            this.leftToolbarContainer.SetOriginPivot(SCardinalDirection.Northeast);
            this.leftToolbarContainer.SetPositionAnchor(SCardinalDirection.Southwest);
            this.leftToolbarContainer.PositionRelativeToScreen();

            this.layout.AddElement(this.leftToolbarContainer);
        }
        private void BuildRightToolbar()
        {
            this.rightToolbarContainer = new(this.SGameInstance);
            this.rightToolbarContainer.SetTexture(this.particleTexture);
            this.rightToolbarContainer.SetScale(new Vector2(96, 608));
            this.rightToolbarContainer.SetColor(new Color(Color.White, 32));
            this.rightToolbarContainer.SetSize(SSize2F.One);
            this.rightToolbarContainer.SetOriginPivot(SCardinalDirection.Northwest);
            this.rightToolbarContainer.SetPositionAnchor(SCardinalDirection.Southeast);
            this.rightToolbarContainer.PositionRelativeToScreen();

            this.layout.AddElement(this.rightToolbarContainer);
        }
    }
}
