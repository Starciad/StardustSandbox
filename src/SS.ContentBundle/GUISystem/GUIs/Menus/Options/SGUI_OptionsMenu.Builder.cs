using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu
    {
        private SGUILabelElement titleLabelElement;
        private SGUIImageElement panelBackgroundElement;

        private readonly SGUILabelElement[] systemButtonElements;
        private readonly List<SGUIContainerElement> sectionContainerElements = [];
        private readonly List<SGUILabelElement> sectionButtonElements = [];

        private static readonly Vector2 defaultRightPanelMargin = new(200f, 64f);

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
            foreach (SSection section in this.root.Sections)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.North;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(section.Name);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.sectionButtonElements.Add(labelElement);
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
                labelElement.SetTextualContent(this.systemButtons[i].Name);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.systemButtonElements[i] = labelElement;
                margin.Y -= leftPanelMarginVerticalSpacing;

                layoutBuilder.AddElement(labelElement);
            }
        }

        // ============================================================================ //

        private void BuildSections(ISGUILayoutBuilder layoutBuilder)
        {
            foreach (SSection section in this.root.Sections)
            {
                SGUIContainerElement containerElement = new(this.SGameInstance);

                Vector2 margin = defaultRightPanelMargin;

                foreach (SOption option in section.Options)
                {
                    SGUIElement element = CreateOptionElement(option);

                    element.Margin = margin;
                    element.PositionRelativeToElement(this.panelBackgroundElement);

                    containerElement.AddElement(element);

                    margin.Y += rightPanelMarginVerticalSpacing;
                }

                this.sectionContainerElements.Add(containerElement);
                layoutBuilder.AddElement(containerElement);
            }
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

        // ============================================================================ //

        private SGUIElement CreateOptionElement(SOption option)
        {
            return option.OptionType switch
            {
                SOptionType.Selector => CreateSelectorOptionElement(option),
                SOptionType.Slider => CreateSliderOptionElement(option),
                SOptionType.Color => CreateColorOptionElement(option),
                SOptionType.Toggle => CreateToogleOptionElement(option),
                _ => null,
            };
        }

        private SGUIElement CreateSelectorOptionElement(SOption option)
        {
            SGUILabelElement element = CreateOptionButtonLabelElement();

            element.SetTextualContent(option.Name);
            element.AddData("option", option);

            return element;
        }

        private SGUIElement CreateSliderOptionElement(SOption option)
        {
            SGUILabelElement element = CreateOptionButtonLabelElement();

            element.SetTextualContent(option.Name);
            element.AddData("option", option);

            return element;
        }

        private SGUIElement CreateColorOptionElement(SOption option)
        {
            SGUILabelElement element = CreateOptionButtonLabelElement();

            element.SetTextualContent(option.Name);
            element.AddData("option", option);

            return element;
        }

        private SGUIElement CreateToogleOptionElement(SOption option)
        {
            SGUILabelElement element = CreateOptionButtonLabelElement();

            element.SetTextualContent(option.Name);
            element.AddData("option", option);

            return element;
        }
    }
}
