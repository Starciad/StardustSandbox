using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Tools.Options;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private ISGUILayoutBuilder layout;

        private SGUILabelElement titleLabel;

        private SGUISliceImageElement panelBackground;
        private SGUISliceImageElement leftPanelBackground;
        private SGUISliceImageElement rightPanelBackground;

        private readonly SGUIContainerElement[] sectionContainers = new SGUIContainerElement[5];
        private readonly SGUILabelElement[] sectionButtonElements = new SGUILabelElement[5];
        private readonly SGUILabelElement[] systemButtonElements = new SGUILabelElement[2];

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
            BuildSystemButtons();

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
            this.titleLabel.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
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
                labelElement.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
                labelElement.PositionRelativeToElement(this.leftPanelBackground);

                this.sectionButtonElements[i] = labelElement;
                baseMargin.Y += 58;

                this.layout.AddElement(labelElement);
            }
        }

        private void BuildSystemButtons()
        {
            Vector2 baseMargin = new(0f, -4f);

            for (int i = 0; i < this.systemButtonNames.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = defaultButtonScale,
                    Margin = baseMargin,
                    Color = SColorPalette.White,
                    BorderOffset = defaultButtonBorderOffset,
                    PositionAnchor = SCardinalDirection.South,
                    OriginPivot = SCardinalDirection.Center
                };

                labelElement.SetTextContent(this.systemButtonNames[i]);
                labelElement.SetBorders(true);
                labelElement.SetBordersColor(SColorPalette.DarkGray);
                labelElement.SetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
                labelElement.PositionRelativeToElement(this.leftPanelBackground);

                this.systemButtonElements[i] = labelElement;
                baseMargin.Y -= 58;

                this.layout.AddElement(labelElement);
            }
        }

        private void BuildSections()
        {
            BuildGeneralSection();
            BuildVideoSection();
            BuildVolumeSection();
            BuildCursorSection();
            BuildLanguageSection();
        }

        private void BuildGeneralSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // ============================================================================ //

            // ============================================================================ //

            this.sectionContainers[(byte)SMenuSection.General] = container;
            this.layout.AddElement(container);
        }

        private void BuildVideoSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. Resolution
            _ = new
            // [ FIELDS ]
            // 1. Resolution
            SOptionSelector("Resolution", 03, SScreenConstants.RESOLUTIONS);

            // 2. Fullscreen
            _ = new
            // 2. Fullscreen
            SOptionSelector("Fullscreen", 00, [false, true]);

            // 3. VSync
            _ = new
            // 3. VSync
            SOptionSelector("VSync", 00, [false, true]);

            // 4. MaxFrameRate
            _ = new
            // 4. MaxFrameRate
            SOptionSelector("FrameRate", 01, SScreenConstants.FRAME_RATES);

            // 5. Borderless
            _ = new
            // 5. Borderless
            SOptionSelector("Borderless", 00, [false, true]);

            this.sectionContainers[(byte)SMenuSection.Video] = container;
            this.layout.AddElement(container);
        }

        private void BuildVolumeSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. MasterVolume
            // 2. MusicVolume
            // 3. SFXVolume

            this.sectionContainers[(byte)SMenuSection.Volume] = container;
            this.layout.AddElement(container);
        }

        private void BuildCursorSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. CursorColor
            // 2. CursorBackgroundColor
            // 3. CursorScale

            this.sectionContainers[(byte)SMenuSection.Cursor] = container;
            this.layout.AddElement(container);
        }

        private void BuildLanguageSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // ============================================================================ //

            // {LANGUAGES LIST}

            // ============================================================================ //

            this.sectionContainers[(byte)SMenuSection.Language] = container;
            this.layout.AddElement(container);
        }
    }
}
