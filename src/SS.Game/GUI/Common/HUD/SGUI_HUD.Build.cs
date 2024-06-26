﻿using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.GUI.Elements.Common;
using StardustSandbox.Game.GUI.Elements.Common.Graphics;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.GUI.Common.HUD
{
    public partial class SGUI_HUD
    {
        private ISGUILayoutBuilder _layout;
        private SGUIRootElement _root;

        private SGUIElement headerContainer;
        private SGUIElement leftMenuContainer;
        private SGUIElement rightMenuContainer;

        private readonly SGUIElement[] headerElementSlots = new SGUIElement[SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH];

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._root = layout.RootElement;

            // Containers
            this.headerContainer = layout.CreateElement<SGUIContainerElement>();
            this.leftMenuContainer = layout.CreateElement<SGUIContainerElement>();
            this.rightMenuContainer = layout.CreateElement<SGUIContainerElement>();

            this.headerContainer.PositionRelativeToElement(this._root);
            this.leftMenuContainer.PositionRelativeToElement(this._root);
            this.rightMenuContainer.PositionRelativeToElement(this._root);

            // Styles
            // (Header)
            this.headerContainer.SetSize(new SSize2(this._root.Size.Width, 96f));

            // Process
            CreateHeader(this.headerContainer);
        }

        private void CreateHeader(SGUIElement header)
        {
            SGUIImageElement slotAreaBackground = this._layout.CreateElement<SGUIImageElement>();

            // Background
            slotAreaBackground.SetTexture(this.particleTexture);
            slotAreaBackground.SetScale(header.Size.ToVector2());
            slotAreaBackground.SetColor(new Color(Color.White, 32));
            slotAreaBackground.SetSize(header.Size);

            // Append
            slotAreaBackground.PositionRelativeToElement(header);

            // ================================= //

            CreateSlots();
            // CreateSearchSlot();

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
                    SGUIImageElement slotBackground = this._layout.CreateElement<SGUIImageElement>();
                    SGUIImageElement slotIcon = this._layout.CreateElement<SGUIImageElement>();

                    // Background
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(SCardinalDirection.Center);
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(SCardinalDirection.West);
                    slotBackground.SetSize(new SSize2(slotSize));
                    slotBackground.SetMargin(slotMargin);

                    SItem selectedItem = GetGameItemByIndex(i);

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
                    slotBackground.PositionRelativeToElement(slotAreaBackground);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Save
                    this.headerElementSlots[i] = slotBackground;

                    // Spacing
                    slotMargin.X += slotSpacing + (slotSize / 2);
                }
            }

            //void CreateSearchSlot()
            //{
            //    SGUIImageElement slotSearchBackground = this._layout.CreateElement<SGUIImageElement>();
            //
            //    slotSearchBackground.SetTexture(this.squareShapeTexture);
            //    slotSearchBackground.SetOriginPivot(SCardinalDirection.Center);
            //    slotSearchBackground.SetScale(new Vector2(SHUDConstants.SLOT_SCALE + 0.45f));
            //    slotSearchBackground.SetPositionAnchor(SCardinalDirection.East);
            //    slotSearchBackground.SetSize(new SSize2(SHUDConstants.SLOT_SCALE + 0.45f));
            //    slotSearchBackground.SetMargin(new Vector2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0));
            //
            //    slotSearchBackground.PositionRelativeToElement(slotAreaBackground);
            //}
        }
    }
}
