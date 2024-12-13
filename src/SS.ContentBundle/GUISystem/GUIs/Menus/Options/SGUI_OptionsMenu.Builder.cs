using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Selectors;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_OptionsMenu
    {
        private SGUILabelElement titleLabel;

        private SGUISliceImageElement panelBackground;
        private SGUISliceImageElement leftPanelBackground;
        private SGUISliceImageElement rightPanelBackground;

        private readonly SGUIContainerElement[] sectionContainers = new SGUIContainerElement[2];
        private readonly SGUILabelElement[] sectionButtonElements = new SGUILabelElement[2];
        private readonly SGUILabelElement[] systemButtonElements = new SGUILabelElement[2];

        private readonly List<SOptionSelector> generalSectionOptionSelectors = [];
        private readonly List<SOptionSelector> videoSectionOptionSelectors = [];
        private readonly List<SOptionSelector> volumeSectionOptionSelectors = [];
        private readonly List<SOptionSelector> cursorSectionOptionSelectors = [];

        private readonly List<SGUILabelElement> generalSectionButtons = [];
        private readonly List<SGUILabelElement> videoSectionButtons = [];
        private readonly List<SGUILabelElement> volumeSectionButtons = [];
        private readonly List<SGUILabelElement> cursorSectionButtons = [];
        private readonly List<SGUILabelElement> languageSectionButtons = [];

        private static readonly Vector2 defaultButtonScale = new(0.11f);
        private static readonly Vector2 defaultButtonBorderOffset = new(2f);
        private static readonly Vector2 baseVerticalMargin = new(0, 4f);

        private static readonly float leftPanelMarginVerticalSpacing = 58f;
        private static readonly float rightPanelMarginVerticalSpacing = 55f;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            // Decorations
            BuildPanels(layoutBuilder);
            BuildTitle(layoutBuilder);

            // Buttons
            BuildSectionButtons(layoutBuilder);
            BuildSystemButtons(layoutBuilder);

            // Sections
            BuildSections(layoutBuilder);
        }

        private void BuildPanels(ISGUILayoutBuilder layoutBuilder)
        {
            BuildPanelBackground(layoutBuilder);
            BuildLeftPanel(layoutBuilder);
            BuildRightPanel(layoutBuilder);
        }

        private void BuildPanelBackground(ISGUILayoutBuilder layoutBuilder)
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

            layoutBuilder.AddElement(this.panelBackground);
        }

        private void BuildLeftPanel(ISGUILayoutBuilder layoutBuilder)
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
            layoutBuilder.AddElement(this.leftPanelBackground);
        }

        private void BuildRightPanel(ISGUILayoutBuilder layoutBuilder)
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
            layoutBuilder.AddElement(this.rightPanelBackground);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.titleLabel = new(this.SGameInstance)
            {
                Scale = new Vector2(0.15f),
                Margin = new Vector2(0f, 52.5f),
                Color = SColorPalette.White,
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.titleLabel.SetTextualContent(this.titleName);
            this.titleLabel.SetAllBorders(true, SColorPalette.DarkGray, new Vector2(4.4f));
            this.titleLabel.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.titleLabel);
        }

        private void BuildSectionButtons(ISGUILayoutBuilder layoutBuilder)
        {
            // BUTTONS
            Vector2 margin = baseVerticalMargin;

            // Labels
            for (int i = 0; i < this.sectionNames.Length; i++)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.North;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(this.sectionNames[i]);
                labelElement.PositionRelativeToElement(this.leftPanelBackground);

                this.sectionButtonElements[i] = labelElement;
                margin.Y += leftPanelMarginVerticalSpacing;

                layoutBuilder.AddElement(labelElement);
            }
        }

        private void BuildSystemButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = baseVerticalMargin;

            for (int i = 0; i < this.systemButtonNames.Length; i++)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.South;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(this.systemButtonNames[i]);
                labelElement.PositionRelativeToElement(this.leftPanelBackground);

                this.systemButtonElements[i] = labelElement;
                margin.Y -= leftPanelMarginVerticalSpacing;

                layoutBuilder.AddElement(labelElement);
            }
        }

        // ============================================================================ //

        private void BuildSections(ISGUILayoutBuilder layoutBuilder)
        {
            BuildVideoSection(layoutBuilder);
            BuildLanguageSection(layoutBuilder);
        }

        private void BuildVideoSection(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIContainerElement container = new(this.SGameInstance);

            // [ FIELDS ]
            // 0. Resolution
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization.GUI_Menu_Options_Section_Video_Resolution, 03, Array.ConvertAll(SScreenConstants.RESOLUTIONS, x => x.ToString())));

            // 1. Fullscreen
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization.GUI_Menu_Options_Section_Video_Fullscreen, 00, [SLocalization.Statements_False, SLocalization.Statements_True]));

            // 2. VSync
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization.GUI_Menu_Options_Section_Video_VSync, 00, [SLocalization.Statements_False, SLocalization.Statements_True]));

            // 3. Borderless
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization.GUI_Menu_Options_Section_Video_Borderless, 00, [SLocalization.Statements_False, SLocalization.Statements_True]));

            // [ LABELS ]
            Vector2 margin = new(0f, 4f);

            foreach (SOptionSelector optionSelector in this.videoSectionOptionSelectors)
            {
                SGUILabelElement labelElement = CreateOptionButtonLabelElement();

                labelElement.Margin = margin;
                labelElement.PositionRelativeToElement(this.rightPanelBackground);
                labelElement.SetTextualContent(optionSelector.ToString());

                this.videoSectionButtons.Add(labelElement);
                container.AddElement(labelElement);

                margin.Y += rightPanelMarginVerticalSpacing;
            }

            this.sectionContainers[(byte)SMenuSection.Video] = container;
            layoutBuilder.AddElement(container);
        }

        private void BuildLanguageSection(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIContainerElement container = new(this.SGameInstance);

            Vector2 margin = new(0f, 4f);

            foreach (SGameCulture gameCulture in SLocalizationConstants.AVAILABLE_GAME_CULTURES)
            {
                SGUILabelElement labelElement = CreateOptionButtonLabelElement();

                labelElement.Margin = margin;
                labelElement.PositionRelativeToElement(this.rightPanelBackground);
                labelElement.SetTextualContent(gameCulture.CultureInfo.NativeName);
                labelElement.AddData(SOptionsMenuConstants.DATA_FILED_LANGUAGE_CODE, gameCulture.Language);

                this.languageSectionButtons.Add(labelElement);
                container.AddElement(labelElement);

                margin.Y += rightPanelMarginVerticalSpacing;
            }

            this.sectionContainers[(byte)SMenuSection.Language] = container;
            layoutBuilder.AddElement(container);
        }

        // ============================================================================ //

        private SGUILabelElement CreateButtonLabelElement()
        {
            SGUILabelElement labelElement = new(this.SGameInstance)
            {
                Scale = defaultButtonScale,
                Color = SColorPalette.White,
                OriginPivot = SCardinalDirection.Center,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            labelElement.SetAllBorders(true, SColorPalette.DarkGray, defaultButtonBorderOffset);

            return labelElement;
        }

        private SGUILabelElement CreateOptionButtonLabelElement()
        {
            SGUILabelElement labelElement = new(this.SGameInstance)
            {
                Scale = new(0.12f),
                Color = SColorPalette.White,
                SpriteFont = this.digitalDiscoSpriteFont,
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
            };

            labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));

            return labelElement;
        }
    }
}
