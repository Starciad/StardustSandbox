using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.AudioSystem;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.BackgroundSystem;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Extensions;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class CreditsUI : UIBase
    {
        private enum CreditContentType : byte
        {
            Text,
            Title,
            Image
        }

        private readonly struct CreditContent
        {
            internal readonly CreditContentType ContentType { get; init; }
            internal readonly string Text { get; init; }
            internal readonly Texture2D Texture { get; init; }
            internal readonly Vector2 TextureScale { get; init; }
            internal readonly Vector2 Margin { get; init; }

            public CreditContent()
            {
                this.ContentType = CreditContentType.Text;
                this.TextureScale = Vector2.One;
            }
        }

        private readonly struct CreditSection(string title, CreditContent[] contents)
        {
            internal readonly string Title => title;
            internal readonly CreditContent[] Contents => contents;
        }

        private UIElement lastElement;
        private int elementCount;

        private readonly CreditSection[] creditSections;
        private readonly List<UIElement> creditElements = [];

        private readonly World world;

        private readonly AmbientManager ambientManager;
        private readonly InputManager inputManager;
        private readonly UIManager uiManager;

        private const float SPEED = 0.75f;
        private const float VERTICAL_SPACING = 64.0f;

        internal CreditsUI(
            AmbientManager ambientManager,
            UIIndex index,
            InputManager inputManager,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.ambientManager = ambientManager;
            this.inputManager = inputManager;
            this.uiManager = uiManager;
            this.world = world;

            // Build Credit Sections
            this.creditSections = [
                // Game Title
                new(string.Empty,
                [
                    new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.GameTitle),
                        ContentType = CreditContentType.Image,
                        TextureScale = new(2.5f),
                    },
                ]),

                // Creator
                new(Localization_GUIs.Menu_Credits_Title_Creator,
                [
                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },
                ]),

                // Roles
                new(Localization_GUIs.Menu_Credits_Title_Roles, [
                    new() {
                        Text = Localization_GUIs.Menu_Credits_Title_Programming,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },

                    new() {
                        Text = Localization_GUIs.Menu_Credits_Title_Artists,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },

                    new() {
                        Text = Localization_GUIs.Menu_Credits_Title_Composers,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },
                ]),

                // Contributors
                new(Localization_GUIs.Menu_Credits_Title_Contributors, [
                    new() {
                        Text = Localization_GUIs.Menu_Credits_Title_Programming,
                        ContentType = CreditContentType.Title,
                    },

                    new() {
                        Text = "Igor S. Zizinio",
                    },

                    new() {
                        Text = Localization_GUIs.Menu_Credits_Title_Artists,
                        ContentType = CreditContentType.Title,
                    },

                    new() {
                        Text = "Focsi",
                    },
                ]),

                // Testers
                new(Localization_GUIs.Menu_Credits_Title_Testers, [
                    new() {
                        Text = "Igor S. Zizinio",
                    },

                    new() {
                        Text = "Focsi",
                    },

                    new() {
                        Text = "Gabriel \"Insanya\" Cordeiro",
                    },

                    new() {
                        Text = "Eduardo \"Ice227\" Wesley",
                    },
                ]),

                // Special Thanks
                new(Localization_GUIs.Menu_Credits_Title_SpecialThanks, [
                    new()
                    {
                        Text = Localization_GUIs.Menu_Credits_People_Parents,
                    },
                ]),

                // Tools
                new(Localization_GUIs.Menu_Credits_Title_Tools, [
                    new()
                    {
                        ContentType = CreditContentType.Image,
                        Texture = AssetDatabase.GetTexture(TextureIndex.ThirdPartyMonogame),
                        TextureScale = new(0.2f),
                    },

                    new()
                    {
                        ContentType = CreditContentType.Image,
                        Texture = AssetDatabase.GetTexture(TextureIndex.ThirdPartyXna),
                        TextureScale = new(0.15f),
                    },
                ]),

                // Finalization
                new(Localization_GUIs.Menu_Credits_Message_Finalization, [
                    new()
                    {
                        Text = Localization_GUIs.Menu_Credits_Message_ThankPlayer,
                    },

                    new()
                    {
                        ContentType = CreditContentType.Image,
                        Texture = AssetDatabase.GetTexture(TextureIndex.CharacterStarciad),
                        Margin = new(0.0f, AssetDatabase.GetTexture(TextureIndex.CharacterStarciad).Height / 2.0f),
                    },
                ]),
            ];
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildElements(root);
            RegisterElements(root);

            this.elementCount = this.creditElements.Count;
            this.lastElement = this.creditElements[this.elementCount - 1];
        }

        private void BuildElements(Container root)
        {
            Vector2 margin = new(0.0f, VERTICAL_SPACING * 2.0f);

            for (byte i = 0; i < this.creditSections.Length; i++)
            {
                CreditSection creditSection = this.creditSections[i];

                if (!string.IsNullOrWhiteSpace(creditSection.Title))
                {
                    Label sectionTitleElement = new()
                    {
                        Scale = new(0.25f),
                        SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                        Margin = margin,
                        Alignment = CardinalDirection.South,
                        TextContent = creditSection.Title
                    };

                    root.AddChild(sectionTitleElement);

                    this.creditElements.Add(sectionTitleElement);

                    margin.Y += sectionTitleElement.Size.Y + VERTICAL_SPACING;
                }

                for (int j = 0; j < creditSection.Contents.Length; j++)
                {
                    CreditContent content = creditSection.Contents[j];

                    if (content.ContentType == CreditContentType.Title)
                    {
                        Label contentTitleElement = new()
                        {
                            Scale = new(0.2f),
                            SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                            Margin = margin + content.Margin,
                            Alignment = CardinalDirection.South,
                            TextContent = content.Text
                        };

                        root.AddChild(contentTitleElement);

                        this.creditElements.Add(contentTitleElement);

                        margin.Y += contentTitleElement.Size.Y + VERTICAL_SPACING;

                        continue;
                    }

                    if (content.ContentType == CreditContentType.Text)
                    {
                        Label contentText = new()
                        {
                            Scale = new(0.15f),
                            SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                            Margin = margin + content.Margin,
                            Alignment = CardinalDirection.South,
                            TextContent = content.Text
                        };

                        contentText.Margin = margin + content.Margin;
                        root.AddChild(contentText);

                        this.creditElements.Add(contentText);

                        margin.Y += contentText.Size.Y + VERTICAL_SPACING;

                        continue;
                    }

                    if (content.ContentType == CreditContentType.Image)
                    {
                        Image contentImage = new()
                        {
                            Scale = content.TextureScale,
                            Size = content.Texture.GetSize().ToVector2(),
                            Texture = content.Texture,
                            Alignment = CardinalDirection.South,
                            Margin = margin + content.Margin
                        };

                        root.AddChild(contentImage);

                        this.creditElements.Add(contentImage);

                        margin.Y += contentImage.Size.Y + VERTICAL_SPACING;

                        continue;
                    }
                }

                margin.Y += VERTICAL_SPACING;
            }
        }

        private void ResetElementsPosition()
        {
            for (int i = 0, length = this.creditElements.Count; i < length; i++)
            {
                UIElement element = this.creditElements[i];

                element.Position = new(
                    element.Position.X,
                    ScreenConstants.SCREEN_HEIGHT + (element.Size.Y * element.Scale.Y) + (i * VERTICAL_SPACING)
                );
            }
        }

        private void RegisterElements(Container root)
        {
            foreach (UIElement element in this.creditElements)
            {
                root.AddChild(element);
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateUserInput();
            UpdateElementsPosition();
            CheckIfTheCreditsHaveFinished();
        }

        private void UpdateUserInput()
        {
            if (this.inputManager.MouseState.LeftButton == ButtonState.Pressed ||
                this.inputManager.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.uiManager.CloseGUI();
            }
        }

        private void UpdateElementsPosition()
        {
            foreach (UIElement creditElement in this.creditElements)
            {
                creditElement.Position = new(creditElement.Position.X, creditElement.Position.Y - SPEED);
            }
        }

        private void CheckIfTheCreditsHaveFinished()
        {
            if (((this.lastElement.Position.Y + this.lastElement.Size.Y) * this.lastElement.Scale.Y) + 16.0f < 0.0f)
            {
                this.uiManager.CloseGUI();
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Credits);
            this.ambientManager.CloudHandler.IsActive = false;
            this.ambientManager.CelestialBodyHandler.IsActive = false;
            this.ambientManager.SkyHandler.IsActive = false;

            this.world.IsActive = false;
            this.world.IsVisible = false;

            SongEngine.Play(SongIndex.V01_EndlessRebirth);

            ResetElementsPosition();
        }

        #endregion
    }
}
