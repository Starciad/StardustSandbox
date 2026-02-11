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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Enums.UI.Tools;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.UI.Common.Tools;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.UI.Common.HUD
{
    internal sealed class SaveUI : UIBase
    {
        private Texture2D worldThumbnailTexture;

        private Label menuTitle, nameSectionTitle, descriptionSectionTitle, thumbnailSectionTitle, titleTextualContent, descriptionTextualContent;
        private Image background, titleInputField, descriptionInputField, thumbnailPreviewElement;
        private SlotInfo[] menuButtonSlotInfos;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtonInfos, fieldButtonInfos, footerButtonInfos;
        private readonly SlotInfo[] fieldButtonSlotInfos, footerButtonSlotInfos;

        private readonly World world;
        private readonly TextInputUI textInputUI;

        private readonly UIManager uiManager;

        private readonly GraphicsDevice graphicsDevice;

        internal SaveUI(
            ActorManager actorManager,
            GraphicsDevice graphicsDevice,
            TextInputUI textInputUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.graphicsDevice = graphicsDevice;
            this.textInputUI = textInputUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseUI),
            ];

            this.fieldButtonInfos = [
                // Name Field
                new(TextureIndex.None, null, string.Empty, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);

                    this.textInputUI.Configure(new()
                    {
                        Synopsis = Localization_Messages.Input_World_Name,
                        InputMode = InputMode.Normal,
                        InputRestriction = InputRestriction.Alphanumeric,
                        MaxCharacters = 50,

                        Content = world.Information.Name,

                        OnValidationCallback = (result) =>
                        {
                            return string.IsNullOrWhiteSpace(result)
                                ? new(ValidationStatus.Failure, Localization_Messages.Input_World_Name_Validation_Empty)
                                : new(ValidationStatus.Success); },

                        OnSendCallback = result =>
                        {
                            world.Information.Name = result;
                        },
                    });

                    this.uiManager.OpenUI(UIIndex.TextInput);
                }),

                // Description Field
                new(TextureIndex.None, null, string.Empty, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);

                    this.textInputUI.Configure(new()
                    {
                        Synopsis = Localization_Messages.Input_World_Description,
                        InputMode = InputMode.Normal,
                        MaxCharacters = 500,
                        Content = world.Information.Description,

                        OnValidationCallback = (result) =>
                        {
                            return string.IsNullOrWhiteSpace(result)
                                ? new(ValidationStatus.Failure, Localization_Messages.Input_World_Description_Validation_Empty)
                                : new(ValidationStatus.Success);
                        },

                        OnSendCallback = (result) =>
                        {
                            world.Information.Description = result;
                        },
                    });

                    this.uiManager.OpenUI(UIIndex.TextInput);
                })
            ];

            this.footerButtonInfos = [
                // Save Button
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.Save_Save_Description, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_World_Saved);
                    SavingSerializer.Save(actorManager, world, this.graphicsDevice);

                    GameHandler.DefineLoadedSaveFile(world.Information.Name);

                    this.uiManager.CloseUI();
                }),
            ];

            this.fieldButtonSlotInfos = new SlotInfo[this.fieldButtonInfos.Length];
            this.footerButtonSlotInfos = new SlotInfo[this.footerButtonInfos.Length];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildNameSection();
            BuildDescriptionSection();
            BuildThumbnailSection();
            BuildFooterButtons();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            Image shadow = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundSave,
                Size = new(1084.0f, 540.0f),
            };

            root.AddChild(shadow);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(24.0f, 10.0f),
                TextContent = Localization_GUIs.Save_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            this.menuButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.background,
                this.menuButtonInfos,
                new(-32.0f, -72.0f),
                80.0f,
                UIDirection.Northeast
            );
        }

        private void BuildNameSection()
        {
            this.nameSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.Save_Name_Title,
            };

            this.titleInputField = new()
            {
                Alignment = UIDirection.Southwest,
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(0, 220, 163, 38),
                Scale = new(2.0f),
                Size = new(163.0f, 38.0f),
                Margin = new(0.0f, 48.0f),
            };

            this.titleTextualContent = new()
            {
                Scale = new(0.1f),
                Margin = new(16.0f, 0.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.West,
            };

            this.background.AddChild(this.nameSectionTitle);
            this.nameSectionTitle.AddChild(this.titleInputField);
            this.titleInputField.AddChild(this.titleTextualContent);

            this.fieldButtonSlotInfos[0] = new(this.titleInputField, null, this.titleTextualContent);
        }

        private void BuildDescriptionSection()
        {
            this.descriptionSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.Save_Description_Title,
            };

            this.descriptionInputField = new()
            {
                Alignment = UIDirection.Southwest,
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(0, 220, 163, 38),
                Scale = new(2.0f),
                Size = new(163.0f, 38.0f),
                Margin = new(0.0f, 48.0f),
            };

            this.descriptionTextualContent = new()
            {
                Scale = new(0.1f),
                Margin = new(16.0f, 0.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.West,
            };

            this.titleInputField.AddChild(this.descriptionSectionTitle);
            this.descriptionSectionTitle.AddChild(this.descriptionInputField);
            this.descriptionInputField.AddChild(this.descriptionTextualContent);

            this.fieldButtonSlotInfos[1] = new(this.descriptionInputField, null, this.descriptionTextualContent);
        }

        private void BuildThumbnailSection()
        {
            this.thumbnailSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(-176.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.Northeast,
                TextContent = Localization_GUIs.Save_Thumbnail_Title
            };

            this.thumbnailPreviewElement = new()
            {
                Alignment = UIDirection.Southwest,
                Scale = new(12.5f),
                Margin = new(0.0f, 8.0f),
            };

            this.background.AddChild(this.thumbnailSectionTitle);
            this.thumbnailSectionTitle.AddChild(this.thumbnailPreviewElement);
        }

        private void BuildFooterButtons()
        {
            float marginX = 32.0f;

            for (int i = 0; i < this.footerButtonInfos.Length; i++)
            {
                ButtonInfo button = this.footerButtonInfos[i];

                Image background = new()
                {
                    TextureIndex = TextureIndex.UIButtons,
                    SourceRectangle = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Size = new(320.0f, 80.0f),
                    Margin = new(marginX, -32.0f),
                    Alignment = UIDirection.Southwest,
                };

                Label label = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.background.AddChild(background);
                background.AddChild(label);

                this.footerButtonSlotInfos[i] = new(background, null, label);

                marginX += background.Size.X + 32.0f;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image background = new()
            {
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.0f),
                Size = new(32.0f),
                Margin = margin,
            };

            Image icon = new()
            {
                TextureIndex = button.TextureIndex,
                SourceRectangle = button.TextureSourceRectangle,
                Scale = new(1.5f),
                Size = new(32.0f)
            };

            return new(background, icon);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateFieldButtons()
        {
            for (int i = 0; i < this.fieldButtonInfos.Length; i++)
            {
                SlotInfo slot = this.fieldButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                    break;
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.fieldButtonInfos[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateFooterButtons()
        {
            for (int i = 0; i < this.footerButtonInfos.Length; i++)
            {
                SlotInfo slot = this.footerButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.footerButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.footerButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.footerButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateInfos()
        {
            this.worldThumbnailTexture = this.world.CreateThumbnail(this.graphicsDevice);
            this.thumbnailPreviewElement.Texture = this.worldThumbnailTexture;

            this.titleTextualContent.TextContent = this.world.Information.Name.Truncate(19);
            this.descriptionTextualContent.TextContent = this.world.Information.Description.Truncate(19);
        }

        protected override void OnOpened()
        {
            if (string.IsNullOrWhiteSpace(this.world.Information.Name))
            {
                this.world.Information.Name = Localization_Statements.Untitled;
            }

            if (string.IsNullOrWhiteSpace(this.world.Information.Description))
            {
                this.world.Information.Description = Localization_Messages.NoDescription;
            }

            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            UpdateInfos();
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
            this.worldThumbnailTexture.Dispose();
            this.worldThumbnailTexture = null;
        }
    }
}

