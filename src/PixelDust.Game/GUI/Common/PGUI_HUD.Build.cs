using Microsoft.Xna.Framework;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Enums.General;
using System.Collections.Generic;
using PixelDust.Game.GUI.Elements;

namespace PixelDust.Game.GUI.Common
{
    public partial class PGUI_HUD
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _root;

        private readonly PGUIElement[] headerElementSlots = new PGUIElement[PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH];

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._root = layout.RootElement;

            // Containers
            PGUIContainerElement headerContainer = layout.CreateElement<PGUIContainerElement>();
            PGUIContainerElement leftMenuContainer = layout.CreateElement<PGUIContainerElement>();
            PGUIContainerElement rightMenuContainer = layout.CreateElement<PGUIContainerElement>();

            // Append
            this._root.AppendChild(headerContainer);
            this._root.AppendChild(leftMenuContainer);
            this._root.AppendChild(rightMenuContainer);

            // Styles
            // (Header)
            headerContainer.Style.Size = new Size2(this._root.Style.Size.Width, 96f);

            // Process
            CreateHeader(headerContainer);
        }
        private void CreateHeader(PGUIContainerElement headerContainer)
        {
            PGUIImageElement backgroundImage = this._layout.CreateElement<PGUIImageElement>();
            PGUIContainerElement slotArea = this._layout.CreateElement<PGUIContainerElement>();

            // Background
            backgroundImage.SetTexture(this.particleTexture);
            backgroundImage.Style.Color = new Color(Color.White, 32);
            backgroundImage.Style.Size = headerContainer.Style.Size;

            // Slot
            slotArea.Style.Color = Color.Transparent;
            slotArea.Style.Size = headerContainer.Style.Size;

            // Append
            headerContainer.AppendChild(backgroundImage);
            headerContainer.AppendChild(slotArea);

            // ================================= //

            CreateSlots();
            CreateSearchSlot();

            // ================================= //

            void CreateSlots()
            {
                int slotSize = PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
                int slotScale = PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SCALE;
                int slotSpacing = slotSize * 2;

                Vector2 slotMargin = new(slotSpacing, 0);

                for (int i = 0; i < PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
                {
                    PGUIImageElement slotBackground = this._layout.CreateElement<PGUIImageElement>();
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(PCardinalDirection.Center);
                    slotBackground.Style.PositionAnchor = PCardinalDirection.West;
                    slotBackground.Style.Size = new Size2(slotScale);
                    slotBackground.Style.Margin = slotMargin;

                    slotArea.AppendChild(slotBackground);
                    this.headerElementSlots[i] = slotBackground;

                    slotMargin.X += slotSpacing + slotSize / 2;
                }
            }

            void CreateSearchSlot()
            {
                PGUIImageElement slotSearchBackground = this._layout.CreateElement<PGUIImageElement>();
                slotSearchBackground.SetTexture(this.squareShapeTexture);
                slotSearchBackground.SetOriginPivot(PCardinalDirection.Center);
                slotSearchBackground.Style.PositionAnchor = PCardinalDirection.East;
                slotSearchBackground.Style.Size = new Size2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SCALE + 0.45f);
                slotSearchBackground.Style.Margin = new Vector2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0);

                slotArea.AppendChild(slotSearchBackground);
            }
        }
    }
}
