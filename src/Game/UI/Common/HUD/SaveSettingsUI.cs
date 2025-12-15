using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.Extensions;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class SaveSettingsUI : UIBase
    {
        private Texture2D worldThumbnailTexture;

        private Label menuTitle, nameSectionTitle, descriptionSectionTitle, thumbnailSectionTitle, titleTextualContent, descriptionTextualContent;
        private Image background, titleInputField, descriptionInputField, thumbnailPreviewElement;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtonInfos, fieldButtonInfos, footerButtonInfos;
        private readonly SlotInfo[] menuButtonSlotInfos, fieldButtonSlotInfos, footerButtonSlotInfos;

        private readonly World world;
        private readonly TextInputUI textInputUI;

        private readonly TextInputSettings nameInputBuilder, descriptionInputBuilder;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        private readonly GraphicsDevice graphicsDevice;

        internal SaveSettingsUI(
            GameManager gameManager,
            GraphicsDevice graphicsDevice,
            UIIndex index,
            TextInputUI textInputUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.graphicsDevice = graphicsDevice;
            this.textInputUI = textInputUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseGUI),
            ];

            this.fieldButtonInfos = [
                new(TextureIndex.None, null, "Name Field", string.Empty, () =>
                {
                    this.nameInputBuilder.Content = this.world.Information.Name;

                    this.textInputUI.Configure(this.nameInputBuilder);
                    this.uiManager.OpenGUI(UIIndex.TextInput);
                }),
                new(TextureIndex.None, null, "Description Field", string.Empty, () =>
                {
                    this.descriptionInputBuilder.Content = this.world.Information.Description;

                    this.textInputUI.Configure(this.descriptionInputBuilder);
                    this.uiManager.OpenGUI(UIIndex.TextInput);
                })
            ];

            this.footerButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.HUD_Complements_SaveSettings_Button_Save_Description, () =>
                {
                    SaveFile saveFile = SavingSerializer.Serialize(this.world, this.graphicsDevice);

                    this.world.SetSaveFile(saveFile);
                    this.uiManager.CloseGUI();
                }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.fieldButtonSlotInfos = new SlotInfo[this.fieldButtonInfos.Length];
            this.footerButtonSlotInfos = new SlotInfo[this.footerButtonInfos.Length];

            this.nameInputBuilder = new()
            {
                Synopsis = Localization_Messages.Input_World_Name,
                InputMode = InputMode.Normal,
                InputRestriction = InputRestriction.Alphanumeric,
                MaxCharacters = 50,

                OnValidationCallback = (validationState, result) =>
                {
                    if (string.IsNullOrWhiteSpace(result.Content))
                    {
                        validationState.Status = ValidationStatus.Failure;
                        validationState.Message = Localization_Messages.Input_World_Name_Validation_Empty;
                    }
                },

                OnSendCallback = result =>
                {
                    world.Information.Name = result.Content;
                },
            };

            this.descriptionInputBuilder = new()
            {
                Synopsis = Localization_Messages.Input_World_Description,
                InputMode = InputMode.Normal,
                MaxCharacters = 500,

                OnValidationCallback = (validationState, result) =>
                {
                    if (string.IsNullOrWhiteSpace(result.Content))
                    {
                        validationState.Status = ValidationStatus.Failure;
                        validationState.Message = Localization_Messages.Input_World_Description_Validation_Empty;
                    }
                },

                OnSendCallback = (result) =>
                {
                    world.Information.Description = result.Content;
                },
            };
        }

        #region BUILDER

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
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundSaveSettings),
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
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginX = -32.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, -72.0f), button);

                slot.Background.Alignment = UIDirection.Northeast;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.background.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlotInfos[i] = slot;

                // Spacing
                marginX -= 80.0f;
            }
        }

        private void BuildNameSection()
        {
            this.nameSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Name_Title,
            };

            this.titleInputField = new()
            {
                Alignment = UIDirection.Southwest,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
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
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Description_Title,
            };

            this.descriptionInputField = new()
            {
                Alignment = UIDirection.Southwest,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
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
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Thumbnail_Title
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
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.0f),
                Size = new(32.0f),
                Margin = margin,
            };

            Image icon = new()
            {
                Texture = button.Texture,
                SourceRectangle = button.TextureSourceRectangle,
                Scale = new(1.5f),
                Size = new(32.0f)
            };

            return new(background, icon);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
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

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.footerButtonInfos[i].ClickAction?.Invoke();
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

        #endregion

        #region EVENTS

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

            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            UpdateInfos();
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
            this.worldThumbnailTexture.Dispose();
            this.worldThumbnailTexture = null;
        }

        #endregion
    }
}
