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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class TutorialUI : UIBase
    {
        private int currentPageIndex = 0, currentSpriteOffset = 0;
        private float currentAnimationDelay = 0.0f;

        private const int SPACING_X = 360;
        private const int TOTAL_SPRITES = 3;
        private const float ANIMATION_DELAY = 0.5f;

        private Image panelBackground, shadowBackground, contentImage;
        private Label title, clickToContinue;
        private Text contentText;

        private readonly UIManager uiManager;

        private readonly SystemInformationSettings systemInformationSettings;

        private readonly TutorialContent[] contents;

        internal TutorialUI(
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;

            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();
            this.systemInformationSettings = SettingsSerializer.Load<SystemInformationSettings>();

            this.contents =
            [
                new(
                    Localization_GUIs.Tutorial_Introduction_Title,
                    Localization_GUIs.Tutorial_Introduction_Description,
                    new(0, 0)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Camera_Title,
                    string.Format(
                        Localization_GUIs.Tutorial_Camera_Description,
                        controlSettings.MoveCameraUpKeyboardBinding,
                        controlSettings.MoveCameraRightKeyboardBinding,
                        controlSettings.MoveCameraDownKeyboardBinding,
                        controlSettings.MoveCameraLeftKeyboardBinding,
                        controlSettings.ZoomCameraInKeyboardBinding,
                        controlSettings.ZoomCameraOutKeyboardBinding,
                        controlSettings.MoveCameraFastKeyboardBinding
                    ),
                    new(0, 120)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Draw_Title,
                    Localization_GUIs.Tutorial_Draw_Description,
                    new(0, 240)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Erase_Title,
                    Localization_GUIs.Tutorial_Erase_Description,
                    new(0, 360)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Interface_Title,
                    Localization_GUIs.Tutorial_Interface_Description,
                    new(0, 480)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Simulation_Title,
                    Localization_GUIs.Tutorial_Simulation_Description,
                    new(0, 600)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Save_Title,
                    Localization_GUIs.Tutorial_Save_Description,
                    new(0, 720)
                ),
            
                new(
                    Localization_GUIs.Tutorial_Explore_Title,
                    Localization_GUIs.Tutorial_Explore_Description,
                    new(0, 840)
                )
            ];
        }

        private bool TryNextPage()
        {
            if (this.currentPageIndex < this.contents.Length - 1)
            {
                this.currentPageIndex++;
                RefreshContent();
                return true;
            }

            return false;
        }

        private void RefreshContent()
        {
            TutorialContent content = this.contents[this.currentPageIndex];

            this.title.TextContent = content.Title;
            this.contentText.TextContent = content.Description;

            this.currentSpriteOffset = 0;
            this.currentAnimationDelay = 0.0f;

            this.contentImage.SourceRectangle = new(
                content.TextureOffset.X,
                content.TextureOffset.Y,
                360,
                120
            );
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildContent();
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundTutorial,
                Size = new(406.0f, 520.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildContent()
        {
            this.title = new()
            {
                Alignment = UIDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Margin = new(0.0f, 24.0f),
                TextContent = "Title",
                Color = AAP64ColorPalette.Umber,
            };

            this.contentText = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.055f),
                Margin = new(24.0f, 74.0f),
                LineHeight = 1.5f,
                TextAreaSize = new(365.0f, 472.0f),
                TextContent = "Description",
                Color = AAP64ColorPalette.Umber,
            };

            this.contentImage = new()
            {
                Alignment = UIDirection.South,
                TextureIndex = TextureIndex.UITutorial,
                SourceRectangle = new(0, 0, 360, 120),
                Size = new(360.0f, 120.0f),
                Margin = new(0.0f, -64.0f),
            };

            this.clickToContinue = new()
            {
                Alignment = UIDirection.South,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.05f),
                Margin = new(0.0f, -32.0f),
                TextContent = Localization_GUIs.Tutorial_ClickToContinue,
                Color = AAP64ColorPalette.Umber,
            };

            this.panelBackground.AddChild(this.title);
            this.panelBackground.AddChild(this.contentText);
            this.panelBackground.AddChild(this.contentImage);
            this.panelBackground.AddChild(this.clickToContinue);
        }

        protected override void OnScreenResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            UpdateContentAnimation(deltaTime);

            if (Interaction.OnMouseLeftClick(this.Root) && !TryNextPage())
            {
                this.uiManager.CloseUI();
                this.systemInformationSettings.TutorialDisplayed = true;
                SettingsSerializer.Save(this.systemInformationSettings);
            }
        }

        private void UpdateContentAnimation(float deltaTime)
        {
            this.currentAnimationDelay += deltaTime;

            if (this.currentAnimationDelay >= ANIMATION_DELAY)
            {
                this.currentAnimationDelay = 0.0f;
                this.currentSpriteOffset = (this.currentSpriteOffset + 1) % TOTAL_SPRITES;

                this.contentImage.SourceRectangle = new(
                    this.contents[this.currentPageIndex].TextureOffset.X + (SPACING_X * this.currentSpriteOffset),
                    this.contents[this.currentPageIndex].TextureOffset.Y,
                    360,
                    120
                );
            }
        }

        protected override void OnOpened()
        {
            this.currentPageIndex = 0;
            RefreshContent();
        }
    }
}
