using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private enum SMenuOption : byte
        {
            General = 0,
            UI = 1,
            Video = 2,
            Volume = 3,
            Cursor = 4,
            Controls = 5,
            Language = 6
        }

        private ISGUILayoutBuilder layout;

        private SGUILabelElement titleLabel;

        private SGUISliceImageElement panelBackground;
        private SGUISliceImageElement leftPanelBackground;
        private SGUISliceImageElement rightPanelBackground;

        private readonly SGUIContainerElement[] sectionContainers = new SGUIContainerElement[7];
        private readonly SGUILabelElement[] sectionButtonElements = new SGUILabelElement[7];
        private SGUILabelElement returnButtonElement;

        private static readonly Vector2 defaultButtonScale = new(0.11f);
        private static readonly Vector2 defaultButtonBorderOffset = new(2f);

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            // Decorations
            BuildPanels();
            BuildTitle();

            // Buttons
            BuildSectionButtons();
            BuildReturnButton();

            // Sections
            BuildSections();
        }

        private void BuildPanels()
        {
            BuildPanelBackground();
            BuildLeftPanel();
            BuildRightPanel();
        }

        private void BuildPanelBackground()
        {
            this.panelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32f, 15f),
                Size = new SSize2(32),
                Margin = new Vector2(128f),
                Color = SColorPalette.NavyBlue
            };

            this.panelBackground.PositionRelativeToScreen();

            this.layout.AddElement(this.panelBackground);
        }

        private void BuildLeftPanel()
        {
            this.leftPanelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(9f, 13f),
                Margin = new Vector2(32f),
                Size = new SSize2(32),
                Color = SColorPalette.RoyalBlue
            };

            this.leftPanelBackground.PositionRelativeToElement(this.panelBackground);
            this.layout.AddElement(this.leftPanelBackground);
        }

        private void BuildRightPanel()
        {
            this.rightPanelBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(18.2f, 13f),
                Margin = new Vector2(90f, 0f),
                Size = new SSize2(32),
                PositionAnchor = SCardinalDirection.Northeast,
                Color = SColorPalette.RoyalBlue
            };

            this.rightPanelBackground.PositionRelativeToElement(this.leftPanelBackground);
            this.layout.AddElement(this.rightPanelBackground);
        }

        private void BuildTitle()
        {
            this.titleLabel = new(this.SGameInstance)
            {
                Scale = new Vector2(0.15f),
                Margin = new Vector2(0f, 52.5f),
                Color = SColorPalette.White,
                BorderOffset = new Vector2(4.4f),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center
            };

            this.titleLabel.SetTextContent(this.titleName);
            this.titleLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            this.titleLabel.SetBorders(true);
            this.titleLabel.SetBordersColor(SColorPalette.DarkGray);
            this.titleLabel.PositionRelativeToScreen();

            this.layout.AddElement(this.titleLabel);
        }

        private void BuildSectionButtons()
        {
            // BUTTONS
            Vector2 baseMargin = new(0f, 4f);

            // Labels
            for (int i = 0; i < this.sectionNames.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = defaultButtonScale,
                    Margin = baseMargin,
                    Color = SColorPalette.White,
                    BorderOffset = defaultButtonBorderOffset,
                    PositionAnchor = SCardinalDirection.North,
                    OriginPivot = SCardinalDirection.Center
                };

                labelElement.SetTextContent(this.sectionNames[i]);
                labelElement.SetBorders(true);
                labelElement.SetBordersColor(SColorPalette.DarkGray);
                labelElement.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
                labelElement.PositionRelativeToElement(this.leftPanelBackground);

                this.sectionButtonElements[i] = labelElement;
                baseMargin.Y += 58;
            }

            for (int i = 0; i < this.sectionButtonElements.Length; i++)
            {
                this.layout.AddElement(this.sectionButtonElements[i]);
            }
        }

        private void BuildReturnButton()
        {
            SGUILabelElement labelElement = new(this.SGameInstance)
            {
                Scale = defaultButtonScale,
                Margin = new Vector2(0f, -4f),
                Color = SColorPalette.White,
                BorderOffset = defaultButtonBorderOffset,
                PositionAnchor = SCardinalDirection.South,
                OriginPivot = SCardinalDirection.Center
            };

            labelElement.SetTextContent(this.returnButtonName);
            labelElement.SetBorders(true);
            labelElement.SetBordersColor(SColorPalette.DarkGray);
            labelElement.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            labelElement.PositionRelativeToElement(this.leftPanelBackground);

            this.returnButtonElement = labelElement;

            this.layout.AddElement(this.returnButtonElement);
        }

        private void BuildSections()
        {
            BuildGeneralSection();
            BuildUISection();
            BuildVideoSection();
            BuildVolumeSection();
            BuildCursorSection();
            BuildCuntrolsSection();
            BuildLanguageSection();
        }

        private void BuildGeneralSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            SGUILabelElement labelElement = new(this.SGameInstance)
            {
                Scale = defaultButtonScale,
                Margin = new Vector2(0f, -4f),
                Color = SColorPalette.White,
                BorderOffset = defaultButtonBorderOffset,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center
            };

            labelElement.SetTextContent("Test");
            labelElement.SetBorders(true);
            labelElement.SetBordersColor(SColorPalette.DarkGray);
            labelElement.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            // labelElement.PositionRelativeToElement(this.rightPanelBackground);
            labelElement.PositionRelativeToScreen();

            container.AddElement(labelElement);

            this.sectionContainers[(byte)SMenuOption.General] = container;
            this.layout.AddElement(container);
        }

        private void BuildUISection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.UI] = container;
            this.layout.AddElement(container);
        }

        private void BuildVideoSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.Video] = container;
            this.layout.AddElement(container);
        }

        private void BuildVolumeSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.Volume] = container;
            this.layout.AddElement(container);
        }

        private void BuildCursorSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.Cursor] = container;
            this.layout.AddElement(container);
        }

        private void BuildCuntrolsSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.Controls] = container;
            this.layout.AddElement(container);
        }

        private void BuildLanguageSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            this.sectionContainers[(byte)SMenuOption.Language] = container;
            this.layout.AddElement(container);
        }
    }
}
