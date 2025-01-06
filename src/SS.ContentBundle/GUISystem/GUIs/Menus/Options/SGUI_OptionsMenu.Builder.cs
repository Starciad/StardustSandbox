using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Selectors;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Localization;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_OptionsMenu
    {
        private SGUILabelElement titleLabelElement;

        private SGUIImageElement panelBackgroundElement;

        private readonly SGUIContainerElement[] sectionContainers;
        private readonly SGUILabelElement[] sectionButtonElements;
        private readonly SGUILabelElement[] systemButtonElements;

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

        private static readonly float leftPanelMarginVerticalSpacing = 48f;
        private static readonly float rightPanelMarginVerticalSpacing = 48f;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            // Decorations
            BuildPanelBackground(layoutBuilder);
            BuildTitle(layoutBuilder);

            // Buttons
            BuildSectionButtons(layoutBuilder);
            BuildSystemButtons(layoutBuilder);

            // Sections
            BuildSections(layoutBuilder);
        }

        private void BuildPanelBackground(ISGUILayoutBuilder layoutBuilder)
        {
            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.panelBackgroundTexture,
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.titleLabelElement = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                Margin = new(0f, 52.5f),
                Color = SColorPalette.White,
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.titleLabelElement.SetTextualContent(this.titleName);
            this.titleLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new(4.4f));
            this.titleLabelElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.titleLabelElement);
        }

        private void BuildSectionButtons(ISGUILayoutBuilder layoutBuilder)
        {
            // BUTTONS
            Vector2 margin = new(-335f, 64f);

            // Labels
            for (int i = 0; i < this.sectionNames.Length; i++)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.North;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(this.sectionNames[i]);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.sectionButtonElements[i] = labelElement;
                margin.Y += leftPanelMarginVerticalSpacing;

                layoutBuilder.AddElement(labelElement);
            }
        }

        private void BuildSystemButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(-335f, -64f);

            for (int i = 0; i < this.systemButtons.Length; i++)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.South;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(this.systemButtons[i].DisplayName);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

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
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization_GUIs.Menu_Options_Section_Video_Resolution, 03, Array.ConvertAll(SScreenConstants.RESOLUTIONS, x => x.ToString())));

            // 1. Fullscreen
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization_GUIs.Menu_Options_Section_Video_Fullscreen, 00, [SLocalization_Statements.False, SLocalization_Statements.True]));

            // 2. VSync
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization_GUIs.Menu_Options_Section_Video_VSync, 00, [SLocalization_Statements.False, SLocalization_Statements.True]));

            // 3. Borderless
            this.videoSectionOptionSelectors.Add(new SOptionSelector(SLocalization_GUIs.Menu_Options_Section_Video_Borderless, 00, [SLocalization_Statements.False, SLocalization_Statements.True]));

            // [ LABELS ]
            Vector2 margin = new(200f, 64f);

            foreach (SOptionSelector optionSelector in this.videoSectionOptionSelectors)
            {
                SGUILabelElement labelElement = CreateOptionButtonLabelElement();

                labelElement.Margin = margin;
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);
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

            Vector2 margin = new(200f, 64f);

            foreach (SGameCulture gameCulture in SLocalizationConstants.AVAILABLE_GAME_CULTURES)
            {
                SGUILabelElement labelElement = CreateOptionButtonLabelElement();

                labelElement.Margin = margin;
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);
                labelElement.SetTextualContent(gameCulture.CultureInfo.NativeName.FirstCharToUpper());
                labelElement.AddData(SGUIConstants.DATA_LANGUAGE_CODE, gameCulture.Language);

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
