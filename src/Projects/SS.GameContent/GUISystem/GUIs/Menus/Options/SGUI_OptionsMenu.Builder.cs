using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu
    {
        private SGUILabelElement titleLabelElement;
        private SGUIImageElement panelBackgroundElement;

        private readonly SGUILabelElement[] systemButtonElements;
        private readonly Dictionary<string, IEnumerable<SGUILabelElement>> sectionContents = [];
        private readonly Dictionary<string, SGUIContainerElement> sectionContainerElements = [];
        private readonly Dictionary<string, SGUILabelElement> sectionButtonElements = [];

        private readonly List<(SGUIElement, SGUIElement)> plusAndMinusButtons = [];

        private static readonly Vector2 defaultRightPanelMargin = new(-112f, 64f);

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

            // Final
            layoutBuilder.AddElement(this.tooltipBoxElement);
            SelectSection("general");
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
            foreach (KeyValuePair<string, SSection> item in this.root.Sections)
            {
                SGUILabelElement labelElement = CreateButtonLabelElement();

                labelElement.PositionAnchor = SCardinalDirection.North;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(item.Value.Name);
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.sectionButtonElements.Add(item.Key, labelElement);
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
            foreach (KeyValuePair<string, SSection> item in this.root.Sections)
            {
                List<SGUILabelElement> contentBuffer = [];
                SGUIContainerElement containerElement = new(this.SGameInstance);

                Vector2 margin = defaultRightPanelMargin;

                foreach (SOption option in item.Value.Options.Values)
                {
                    SGUILabelElement labelElement = CreateOptionElement(option);

                    labelElement.Margin = margin;
                    labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                    switch (option)
                    {
                        case SColorOption:
                            BuildColorPreview(containerElement, labelElement);
                            break;

                        case SValueOption:
                            BuildValueControls(option, containerElement, labelElement);
                            break;

                        case SToggleOption:
                            BuildTogglePreview(containerElement, labelElement);
                            break;

                        default:
                            break;
                    }

                    containerElement.AddElement(labelElement);
                    margin.Y += rightPanelMarginVerticalSpacing;

                    contentBuffer.Add(labelElement);
                }

                this.sectionContainerElements.Add(item.Key, containerElement);
                layoutBuilder.AddElement(containerElement);

                this.sectionContents.Add(item.Key, contentBuffer);
            }
        }

        private void BuildColorPreview(SGUIContainerElement containerElement, SGUILabelElement labelElement)
        {
            SSize2 textureSize = new(40, 22);
            SSize2 labelElementSize = labelElement.GetStringSize();

            SColorSlot colorSlot = new(
                new(this.SGameInstance)
                {
                    Texture = this.colorButtonTexture,
                    TextureClipArea = new(new(00, 00), textureSize.ToPoint()),
                    Scale = new(1.5f),
                    Size = textureSize,
                    Margin = new(labelElementSize.Width + 6f, labelElementSize.Height / 2 * -1),
                },

                new(this.SGameInstance)
                {
                    Texture = this.colorButtonTexture,
                    TextureClipArea = new(new(00, 22), textureSize.ToPoint()),
                    Scale = new(1.5f),
                    Size = textureSize,
                }
            );

            colorSlot.BackgroundElement.PositionRelativeToElement(labelElement);
            colorSlot.BorderElement.PositionRelativeToElement(colorSlot.BackgroundElement);

            containerElement.AddElement(colorSlot.BackgroundElement);
            containerElement.AddElement(colorSlot.BorderElement);

            labelElement.AddData("color_slot", colorSlot);
        }

        private void BuildValueControls(SOption option, SGUIContainerElement containerElement, SGUILabelElement labelElement)
        {
            SSize2 labelElementSize = labelElement.GetStringSize();

            SGUIImageElement minusElement = new(this.SGameInstance)
            {
                Texture = this.minusIconTexture,
                Size = new(32),
                Margin = new(0, labelElementSize.Height / 2 * -1)
            };

            SGUIImageElement plusElement = new(this.SGameInstance)
            {
                Texture = this.plusIconTexture,
                Size = new(32),
                Margin = new(this.minusIconTexture.Width + (this.minusIconTexture.Width / 2), 0f),
            };

            plusElement.AddData("option", option);
            minusElement.AddData("option", option);

            labelElement.AddData("plus_element", plusElement);
            labelElement.AddData("minus_element", minusElement);

            minusElement.PositionRelativeToElement(labelElement);
            plusElement.PositionRelativeToElement(minusElement);

            containerElement.AddElement(plusElement);
            containerElement.AddElement(minusElement);

            this.plusAndMinusButtons.Add((plusElement, minusElement));
        }

        private void BuildTogglePreview(SGUIContainerElement containerElement, SGUILabelElement labelElement)
        {
            SSize2 textureSize = new(32);
            SSize2 labelElementSize = labelElement.GetStringSize();

            SGUIImageElement togglePreviewImageElement = new(this.SGameInstance)
            {
                Texture = this.toggleButtonTexture,
                TextureClipArea = new(new(00, 00), textureSize.ToPoint()),
                Scale = new(1.25f),
                Size = textureSize,
                Margin = new(labelElementSize.Width + 6f, labelElementSize.Height / 2 * -1),
            };

            togglePreviewImageElement.PositionRelativeToElement(labelElement);

            containerElement.AddElement(togglePreviewImageElement);

            labelElement.AddData("toogle_preview", togglePreviewImageElement);
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
                OriginPivot = SCardinalDirection.East,
            };

            labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));

            return labelElement;
        }

        // ============================================================================ //

        private SGUILabelElement CreateOptionElement(SOption option)
        {
            SGUILabelElement labelElement = option switch
            {
                SButtonOption => CreateButtonOptionElement(option),
                SSelectorOption => CreateSelectorOptionElement(option),
                SValueOption => CreateValueOptionElement(option),
                SColorOption => CreateColorOptionElement(option),
                SToggleOption => CreateToggleOptionElement(option),
                _ => null,
            };

            labelElement.AddData("option", option);

            return labelElement;
        }

        private SGUILabelElement CreateButtonOptionElement(SOption option)
        {
            SGUILabelElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(option.Name);
            return labelElement;
        }

        private SGUILabelElement CreateSelectorOptionElement(SOption option)
        {
            SGUILabelElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": ", option.GetValue()));
            return labelElement;
        }

        private SGUILabelElement CreateValueOptionElement(SOption option)
        {
            SGUILabelElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": ", option.GetValue()));
            return labelElement;
        }

        private SGUILabelElement CreateColorOptionElement(SOption option)
        {
            SGUILabelElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": "));
            return labelElement;
        }

        private SGUILabelElement CreateToggleOptionElement(SOption option)
        {
            SGUILabelElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": "));
            return labelElement;
        }
    }
}
