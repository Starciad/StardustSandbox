using Microsoft.Xna.Framework;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.Enums.General;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;

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
            headerContainer.Style.SetSize(new Size2(this._root.Style.Size.Width, 96f));

            // Process
            CreateHeader(headerContainer);
        }
        private void CreateHeader(PGUIContainerElement headerContainer)
        {
            PGUIImageElement backgroundImage = this._layout.CreateElement<PGUIImageElement>();
            PGUIContainerElement slotArea = this._layout.CreateElement<PGUIContainerElement>();

            // Background
            backgroundImage.SetTexture(this.particleTexture);
            backgroundImage.SetScale(headerContainer.Style.Size.ToVector2());
            backgroundImage.SetColor(new Color(Color.White, 32));
            backgroundImage.Style.SetSize(headerContainer.Style.Size);

            // Slot
            slotArea.Style.SetSize(headerContainer.Style.Size);

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
                    // Creation
                    PGUIImageElement slotBackground = this._layout.CreateElement<PGUIImageElement>();
                    PGUIImageElement slotIcon = this._layout.CreateElement<PGUIImageElement>();

                    // Background
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(PCardinalDirection.Center);
                    slotBackground.SetScale(new(slotScale));
                    slotBackground.AddData(PHUDConstants.SLOT_ELEMENT_INDEX_NAME, i);

                    slotBackground.Style.SetPositionAnchor(PCardinalDirection.West);
                    slotBackground.Style.SetSize(new Size2(slotSize));
                    slotBackground.Style.SetMargin(slotMargin);

                    // Icon
                    slotIcon.SetTexture(GetGameElement(i).IconTexture);
                    slotIcon.SetOriginPivot(PCardinalDirection.Center);
                    slotIcon.SetScale(new(1.5f));

                    slotIcon.Style.SetSize(new Size2(slotSize));

                    // Append
                    slotArea.AppendChild(slotBackground);
                    slotBackground.AppendChild(slotIcon);

                    // Save
                    this.headerElementSlots[i] = slotBackground;

                    // Spacing
                    slotMargin.X += slotSpacing + (slotSize / 2);
                }
            }

            void CreateSearchSlot()
            {
                PGUIImageElement slotSearchBackground = this._layout.CreateElement<PGUIImageElement>();
                slotSearchBackground.SetTexture(this.squareShapeTexture);
                slotSearchBackground.SetOriginPivot(PCardinalDirection.Center);
                slotSearchBackground.SetScale(new Vector2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SCALE + 0.45f));

                slotSearchBackground.Style.SetPositionAnchor(PCardinalDirection.East);
                slotSearchBackground.Style.SetSize(new Size2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SCALE + 0.45f));
                slotSearchBackground.Style.SetMargin(new Vector2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0));

                slotArea.AppendChild(slotSearchBackground);
            }
        }
    }
}
