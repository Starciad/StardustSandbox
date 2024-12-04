using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        private ISGUILayoutBuilder layout;

        private SGUILabelElement titleLabel;

        private SGUISliceImageElement panelBackground;
        private SGUISliceImageElement leftPanelBackground;
        private SGUISliceImageElement rightPanelBackground;

        private readonly SGUIContainerElement[] sectionContainers = new SGUIContainerElement[4];
        private readonly SGUILabelElement[] sectionButtonElements = new SGUILabelElement[4];
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
                labelElement.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
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
        }

        private void BuildGeneralSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // ============================================================================ //

            #region FIELDS

            // 1. Language Option
            SGUILabelElement laguageOptionField = new(this.SGameInstance)
            {
                Scale = new Vector2(0.08f),
                Margin = new Vector2(0f, 4f),
                Color = SColorPalette.SteelBlue,
                BorderOffset = new Vector2(1.5f),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center
            };

            laguageOptionField.SetTextContent($"Language: {CultureInfo.GetCultureInfo("en-US").NativeName}");
            laguageOptionField.SetBorders(true);
            laguageOptionField.SetBordersColor(SColorPalette.DarkGray);
            laguageOptionField.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            laguageOptionField.PositionRelativeToElement(this.rightPanelBackground);

            #endregion

            // ============================================================================ //

            container.AddElement(laguageOptionField);

            this.sectionContainers[(byte)SMenuOption.General] = container;
            this.layout.AddElement(container);
        }

        private void BuildVideoSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. Resolution
            // 2. Fullscreen
            // 3. VSync
            // 4. MaxFrameRate
            // 5. Borderless

            this.sectionContainers[(byte)SMenuOption.Video] = container;
            this.layout.AddElement(container);
        }

        private void BuildVolumeSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. MasterVolume
            // 2. MusicVolume
            // 3. SFXVolume

            this.sectionContainers[(byte)SMenuOption.Volume] = container;
            this.layout.AddElement(container);
        }

        private void BuildCursorSection()
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 1. CursorColor
            // 2. CursorBackgroundColor
            // 3. CursorScale

            this.sectionContainers[(byte)SMenuOption.Cursor] = container;
            this.layout.AddElement(container);
        }
    }
}
