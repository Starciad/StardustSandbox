using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_CreditsMenu
    {
        private ISGUILayoutBuilder layout;

        private readonly List<SCreditSection> creditSections = [];
        private readonly List<SGUIElement> creditElements = [];

        private static readonly float verticalSpacing = 64f;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildContent();
            BuildElements();
            RegisterElements();
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

            this.creditSections.Add(new("Criador e Idealizador", [
                new()
                {
                    Text = "Starciad",
                },
            ]));

            this.creditSections.Add(new("Contribuidores", [

                new() {
                    Text = "Programming and Scripting",
                    ContentType = SCreditContentType.Title,
                },

                new() {
                    Text = "Igor S. Zizinio",
                },

                new() {
                    Text = "Visual Design",
                    ContentType = SCreditContentType.Title,
                },

                new() {
                    Text = "Focsi",
                }
            ]));

            this.creditSections.Add(new("Agradecimentos Especiais", [
                new()
                {
                    Text = "Aos meus pais.",
                },
            ]));

            this.creditSections.Add(new("Recursos", [
                new()
                {
                    Text = "MonoGame Framework",
                },

                new()
                {
                    Text = "XNA",
                },
            ]));

            this.creditSections.Add(new("E esse é o fim!", [
                new()
                {
                    Text = "Obrigado por Jogar!",
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
                        Scale = new(0.2f),
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
                            Scale = new(0.15f),
                            SpriteFont = this.digitalDiscoSpriteFont,
                            Margin = margin,
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
                            Scale = new(0.1f),
                            SpriteFont = this.digitalDiscoSpriteFont,
                            Margin = margin,
                            OriginPivot = SCardinalDirection.Center,
                            PositionAnchor = SCardinalDirection.South,
                        };

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
                            Margin = margin,
                            OriginPivot = SCardinalDirection.Center,
                            PositionAnchor = SCardinalDirection.South
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

        private void RegisterElements()
        {
            foreach (SGUIElement element in this.creditElements)
            {
                this.layout.AddElement(element);
            }
        }
    }
}
