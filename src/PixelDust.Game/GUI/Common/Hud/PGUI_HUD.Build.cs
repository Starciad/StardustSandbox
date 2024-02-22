using Microsoft.Xna.Framework;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Mathematics;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Constants.GUI;

namespace PixelDust.Game.GUI.Common
{
    public partial class PGUI_HUD
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _root;

        private PGUIElement headerContainer;
        private PGUIElement leftMenuContainer;
        private PGUIElement rightMenuContainer;

        private readonly PGUIElement[] headerElementSlots = new PGUIElement[PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH];

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            this.Name = PGUIConstants.HUD_NAME;
            this._layout = layout;
            this._root = layout.RootElement;

            // Containers
            this.headerContainer = layout.CreateElement<PGUIContainerElement>();
            this.leftMenuContainer = layout.CreateElement<PGUIContainerElement>();
            this.rightMenuContainer = layout.CreateElement<PGUIContainerElement>();

            this.headerContainer.PositionRelativeToElement(this._root);
            this.leftMenuContainer.PositionRelativeToElement(this._root);
            this.rightMenuContainer.PositionRelativeToElement(this._root);

            // Styles
            // (Header)
            this.headerContainer.SetSize(new Size2(this._root.Size.Width, 96f));

            // Process
            CreateHeader(this.headerContainer);
        }

        private void CreateHeader(PGUIElement header)
        {
            PGUIImageElement slotAreaBackground = this._layout.CreateElement<PGUIImageElement>();

            // Background
            slotAreaBackground.SetTexture(this.particleTexture);
            slotAreaBackground.SetScale(header.Size.ToVector2());
            slotAreaBackground.SetColor(new Color(Color.White, 32));
            slotAreaBackground.SetSize(header.Size);

            // Append
            slotAreaBackground.PositionRelativeToElement(header);

            // ================================= //

            CreateSlots();
            CreateSearchSlot();

            // ================================= //

            void CreateSlots()
            {
                int slotSize = PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
                int slotScale = PHUDConstants.SLOT_SCALE;
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
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(PCardinalDirection.West);
                    slotBackground.SetSize(new Size2(slotSize));
                    slotBackground.SetMargin(slotMargin);
                    slotBackground.AddData(PHUDConstants.DATA_FILED_ELEMENT_ID, i);

                    // Icon
                    slotIcon.SetTexture(GetGameElement(i).IconTexture);
                    slotIcon.SetOriginPivot(PCardinalDirection.Center);
                    slotIcon.SetScale(new Vector2(1.5f));
                    slotIcon.SetSize(new Size2(slotSize));

                    // Update
                    slotBackground.PositionRelativeToElement(slotAreaBackground);
                    slotIcon.PositionRelativeToElement(slotBackground);

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
                slotSearchBackground.SetScale(new Vector2(PHUDConstants.SLOT_SCALE + 0.45f));
                slotSearchBackground.SetPositionAnchor(PCardinalDirection.East);
                slotSearchBackground.SetSize(new Size2(PHUDConstants.SLOT_SCALE + 0.45f));
                slotSearchBackground.SetMargin(new Vector2(PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0));

                slotSearchBackground.PositionRelativeToElement(slotAreaBackground);
            }
        }
    }
}
