using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_CreditsMenu
    {
        private readonly List<SCreditSection> creditSections = [];
        private readonly List<SGUIElement> creditElements = [];

        private SGUIElement lastElement;
        private int elementCount;

        private static readonly float verticalSpacing = 64f;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildContent();
            BuildElements();
            RegisterElements(layoutBuilder);

            this.elementCount = this.creditElements.Count;
            this.lastElement = this.creditElements[this.elementCount - 1];
        }

        private void BuildContent()
        {
            this.creditSections.Add(new(string.Empty, [
                new()
                {
                    Texture = this.gameTitleTexture,
                    ContentType = SCreditContentType.Image,
                    TextureScale = new(2.5f),
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Title_Creator, [
                new()
                {
                    Text = SGameConstants.AUTHOR,
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Title_Roles, [
                new() {
                    Text = SLocalization_GUIs.Menu_Credits_Title_Programming,
                    ContentType = SCreditContentType.Title,
                },

                new()
                {
                    Text = SGameConstants.AUTHOR,
                },

                new() {
                    Text = SLocalization_GUIs.Menu_Credits_Title_Artists,
                    ContentType = SCreditContentType.Title,
                },

                new()
                {
                    Text = SGameConstants.AUTHOR,
                },

                new() {
                    Text = SLocalization_GUIs.Menu_Credits_Title_Composers,
                    ContentType = SCreditContentType.Title,
                },

                new()
                {
                    Text = SGameConstants.AUTHOR,
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Title_Contributors, [
                new() {
                    Text = SLocalization_GUIs.Menu_Credits_Title_Programming,
                    ContentType = SCreditContentType.Title,
                },

                new() {
                    Text = "Igor S. Zizinio",
                },

                new() {
                    Text = SLocalization_GUIs.Menu_Credits_Title_Artists,
                    ContentType = SCreditContentType.Title,
                },

                new() {
                    Text = "Focsi",
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Title_SpecialThanks, [
                new()
                {
                    Text = SLocalization_GUIs.Menu_Credits_People_Parents,
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Title_Tools, [
                new()
                {
                    ContentType = SCreditContentType.Image,
                    Texture = this.monogameLogoTexture,
                    TextureScale = new(0.2f),
                },

                new()
                {
                    ContentType = SCreditContentType.Image,
                    Texture = this.xnaLogoTexture,
                    TextureScale = new(0.15f),
                },
            ]));

            this.creditSections.Add(new(SLocalization_GUIs.Menu_Credits_Message_Finalization, [
                new()
                {
                    Text = SLocalization_GUIs.Menu_Credits_Message_ThankPlayer,
                },

                new()
                {
                    ContentType = SCreditContentType.Image,
                    Texture = this.starciadCharacterTexture,
                    Margin = new(0f, this.starciadCharacterTexture.Height / 2f),
                },
            ]));
        }

        private void BuildElements()
        {
            Vector2 margin = new(0, verticalSpacing * 2f);

            foreach (SCreditSection creditSection in this.creditSections)
            {
                if (!string.IsNullOrWhiteSpace(creditSection.Title))
                {
                    SGUILabelElement sectionTitleElement = new(this.SGameInstance)
                    {
                        Scale = new(0.25f),
                        SpriteFont = this.digitalDiscoSpriteFont,
                        Margin = margin,
                        OriginPivot = SCardinalDirection.Center,
                        PositionAnchor = SCardinalDirection.South,
                    };

                    sectionTitleElement.SetTextualContent(creditSection.Title);
                    sectionTitleElement.PositionRelativeToScreen();

                    this.creditElements.Add(sectionTitleElement);

                    margin.Y += sectionTitleElement.GetStringSize().Height + verticalSpacing;
                }

                foreach (SCreditContent content in creditSection.Contents)
                {
                    if (content.ContentType == SCreditContentType.Title)
                    {
                        SGUILabelElement contentTitleElement = new(this.SGameInstance)
                        {
                            Scale = new(0.2f),
                            SpriteFont = this.digitalDiscoSpriteFont,
                            Margin = margin + content.Margin,
                            OriginPivot = SCardinalDirection.Center,
                            PositionAnchor = SCardinalDirection.South,
                        };

                        contentTitleElement.SetTextualContent(content.Text);
                        contentTitleElement.PositionRelativeToScreen();

                        this.creditElements.Add(contentTitleElement);

                        margin.Y += contentTitleElement.GetStringSize().Height + verticalSpacing;

                        continue;
                    }

                    if (content.ContentType == SCreditContentType.Text)
                    {
                        SGUILabelElement contentText = new(this.SGameInstance)
                        {
                            Scale = new(0.15f),
                            SpriteFont = this.digitalDiscoSpriteFont,
                            Margin = margin + content.Margin,
                            OriginPivot = SCardinalDirection.Center,
                            PositionAnchor = SCardinalDirection.South,
                        };

                        contentText.Margin = margin + content.Margin;
                        contentText.SetTextualContent(content.Text);
                        contentText.PositionRelativeToScreen();

                        this.creditElements.Add(contentText);

                        margin.Y += contentText.GetStringSize().Height + verticalSpacing;

                        continue;
                    }

                    if (content.ContentType == SCreditContentType.Image)
                    {
                        SGUIImageElement contentImage = new(this.SGameInstance)
                        {
                            Scale = content.TextureScale,
                            Size = content.Texture.GetSize(),
                            Texture = content.Texture,
                            OriginPivot = SCardinalDirection.Center,
                            PositionAnchor = SCardinalDirection.South,
                            Margin = margin + content.Margin
                        };
                        contentImage.PositionRelativeToScreen();

                        this.creditElements.Add(contentImage);

                        margin.Y += contentImage.Size.Height + verticalSpacing;

                        continue;
                    }
                }

                margin.Y += verticalSpacing;
            }
        }

        private void RegisterElements(ISGUILayoutBuilder layoutBuilder)
        {
            foreach (SGUIElement element in this.creditElements)
            {
                layoutBuilder.AddElement(element);
            }
        }
    }
}
