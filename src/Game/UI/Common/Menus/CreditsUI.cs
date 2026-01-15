/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Audio;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Backgrounds;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Extensions;
using StardustSandbox.InputSystem;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.WorldSystem;

using System;

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

        private Container rootContainer;
        private UIElement lastElement;

        private readonly CreditSection[] creditSections;

        private readonly World world;

        private readonly AmbientManager ambientManager;
        private readonly UIManager uiManager;

        private const float SPEED = 0.05f;
        private const float VERTICAL_SPACING = 64.0f;

        internal CreditsUI(
            AmbientManager ambientManager,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.ambientManager = ambientManager;
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
                new(Localization_GUIs.Credits_Title_Creator,
                [
                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },
                ]),

                // Roles
                new(Localization_GUIs.Credits_Title_Roles, [
                    new() {
                        Text = Localization_GUIs.Credits_Title_Programming,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },

                    new() {
                        Text = Localization_GUIs.Credits_Title_Artists,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },

                    new() {
                        Text = Localization_GUIs.Credits_Title_Composers,
                        ContentType = CreditContentType.Title,
                    },

                    new()
                    {
                        Text = GameConstants.AUTHOR,
                    },
                ]),

                // Contributors
                new(Localization_GUIs.Credits_Title_Contributors, [
                    new() {
                        Text = Localization_GUIs.Credits_Title_Programming,
                        ContentType = CreditContentType.Title,
                    },

                    new() {
                        Text = "Igor S. Zizinio",
                    },

                    new() {
                        Text = Localization_GUIs.Credits_Title_Artists,
                        ContentType = CreditContentType.Title,
                    },

                    new() {
                        Text = "Focsi",
                    },
                ]),

                // Testers
                new(Localization_GUIs.Credits_Title_Testers, [
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
                new(Localization_GUIs.Credits_Title_SpecialThanks, [
                    new()
                    {
                        Text = Localization_GUIs.Credits_People_Parents,
                    },
                ]),

                // Tools
                new(Localization_GUIs.Credits_Title_Tools, [
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
                new(Localization_GUIs.Credits_Message_Finalization, [
                    new()
                    {
                        Text = Localization_GUIs.Credits_Message_ThankPlayer,
                    },

                    new()
                    {
                        ContentType = CreditContentType.Image,
                        Texture = AssetDatabase.GetTexture(TextureIndex.CharacterStarciad),
                        Margin = new(0.0f, AssetDatabase.GetTexture(TextureIndex.CharacterStarciad).Height),
                    },
                ]),
            ];
        }

        protected override void OnBuild(Container root)
        {
            this.rootContainer = root;

            BuildElements(root);
        }

        private void BuildElements(Container root)
        {
            Vector2 margin = new(0.0f, VERTICAL_SPACING * 2.0f);

            for (int i = 0; i < this.creditSections.Length; i++)
            {
                CreditSection creditSection = this.creditSections[i];

                if (!string.IsNullOrWhiteSpace(creditSection.Title))
                {
                    Label sectionTitleElement = new()
                    {
                        Scale = new(0.25f),
                        SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                        Margin = margin,
                        Alignment = UIDirection.South,
                        TextContent = creditSection.Title
                    };

                    root.AddChild(sectionTitleElement);

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
                            Alignment = UIDirection.South,
                            TextContent = content.Text
                        };

                        root.AddChild(contentTitleElement);

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
                            Alignment = UIDirection.South,
                            TextContent = content.Text
                        };

                        contentText.Margin = margin + content.Margin;
                        root.AddChild(contentText);

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
                            Alignment = UIDirection.South,
                            Margin = margin + content.Margin
                        };

                        root.AddChild(contentImage);

                        margin.Y += contentImage.Size.Y + VERTICAL_SPACING;

                        continue;
                    }
                }

                margin.Y += VERTICAL_SPACING;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateUserInput();
            CheckIfTheCreditsHaveFinished();

            this.rootContainer.Margin = new(
                this.rootContainer.Margin.X,
                this.rootContainer.Margin.Y - (SPEED * Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds))
            );
        }

        private void UpdateUserInput()
        {
            if (Input.MouseState.LeftButton == ButtonState.Pressed ||
                Input.KeyboardState.GetPressedKeyCount() > 0)
            {
                this.uiManager.CloseUI();
            }
        }

        private void CheckIfTheCreditsHaveFinished()
        {
            if (((this.lastElement.Position.Y + this.lastElement.Size.Y) * this.lastElement.Scale.Y) + 16.0f < 0.0f)
            {
                this.uiManager.CloseUI();
            }
        }

        protected override void OnOpened()
        {
            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Credits);

            this.world.CanUpdate = false;
            this.world.CanDraw = false;

            SongEngine.Play(SongIndex.Volume_01_Track_02);

            this.rootContainer.Margin = new(0.0f, ScreenConstants.SCREEN_HEIGHT / 2.0f);

            this.lastElement ??= this.rootContainer.LastChild;
        }
    }
}

