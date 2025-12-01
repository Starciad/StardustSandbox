using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.Extensions;
using StardustSandbox.IO.Handlers;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class SaveSettingsUI : UI
    {
        private Texture2D worldThumbnailTexture;

        private Image panelBackgroundElement;

        private Label menuTitleElement;
        private Label nameSectionTitleElement;
        private Label descriptionSectionTitleElement;
        private Label thumbnailSectionTitleElement;

        private Image titleInputFieldElement;
        private Image descriptionInputFieldElement;

        private Label titleTextualContentElement;
        private Label descriptionTextualContentElement;

        private Image thumbnailPreviewElement;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtons;
        private readonly ButtonInfo[] fieldButtons;
        private readonly ButtonInfo[] footerButtons;

        private readonly SlotInfo[] menuButtonSlots;
        private readonly SlotInfo[] fieldButtonSlots;
        private readonly SlotInfo[] footerButtonSlots;

        private readonly World world;
        private readonly TextInputUI textInputUI;

        private readonly TextInputSettings nameInputBuilder;
        private readonly TextInputSettings descriptionInputBuilder;

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

            this.menuButtons = [
                new(TextureIndex.UIButtons, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.fieldButtons = [
                new(TextureIndex.None, null, "Name Field", string.Empty, NameFieldButtonAction),
                new(TextureIndex.None, null, "Description Field", string.Empty, DescriptionFieldButtonAction)
            ];

            this.footerButtons = [
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.HUD_Complements_SaveSettings_Button_Save_Description, SaveButtonAction),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.fieldButtonSlots = new SlotInfo[this.fieldButtons.Length];
            this.footerButtonSlots = new SlotInfo[this.footerButtons.Length];

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

        #region ACTIONS

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        // Fields
        private void NameFieldButtonAction()
        {
            this.nameInputBuilder.Content = this.world.Information.Name;

            this.textInputUI.Configure(this.nameInputBuilder);
            this.uiManager.OpenGUI(UIIndex.TextInput);
        }

        private void DescriptionFieldButtonAction()
        {
            this.descriptionInputBuilder.Content = this.world.Information.Description;

            this.textInputUI.Configure(this.descriptionInputBuilder);
            this.uiManager.OpenGUI(UIIndex.TextInput);
        }

        // Footer
        private void SaveButtonAction()
        {
            WorldSavingHandler.Serialize(this.world, this.graphicsDevice);
            this.uiManager.CloseGUI();
        }

        #endregion

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
            Image backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundSaveSettings),
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            root.AddChild(backgroundShadowElement);
            root.AddChild(this.panelBackgroundElement);
        }

        private void BuildTitle()
        {
            this.menuTitleElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.Northwest,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3f,
                BorderThickness = 3f,
            };

            this.panelBackgroundElement.AddChild(this.menuTitleElement);
        }

        private void BuildMenuButtons()
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.Northeast;

                // Update
                this.panelBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildNameSection()
        {
            this.nameSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
            };

            this.titleInputFieldElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(0, 220, 163, 38),
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.titleTextualContentElement = new()
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.West,
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Name_Title
            };

            this.panelBackgroundElement.AddChild(this.nameSectionTitleElement);
            this.nameSectionTitleElement.AddChild(this.titleInputFieldElement);
            this.titleInputFieldElement.AddChild(this.titleTextualContentElement);

            this.fieldButtonSlots[0] = new(this.titleInputFieldElement, null, this.titleTextualContentElement);
        }

        private void BuildDescriptionSection()
        {
            this.descriptionSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 96f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
            };

            this.descriptionInputFieldElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(0, 220, 163, 38),
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.descriptionTextualContentElement = new()
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.West,
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Description_Title
            };

            this.titleInputFieldElement.AddChild(this.descriptionSectionTitleElement);
            this.descriptionSectionTitleElement.AddChild(this.descriptionInputFieldElement);
            this.descriptionInputFieldElement.AddChild(this.descriptionTextualContentElement);

            this.fieldButtonSlots[1] = new(this.descriptionInputFieldElement, null, this.descriptionTextualContentElement);
        }

        private void BuildThumbnailSection()
        {
            this.thumbnailSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(-32f, 112f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Northeast,
                TextContent = Localization_GUIs.HUD_Complements_SaveSettings_Section_Thumbnail_Title
            };

            this.thumbnailPreviewElement = new()
            {
                Scale = new(12.5f),
                Margin = new(0f, 48f),
            };

            this.panelBackgroundElement.AddChild(this.thumbnailSectionTitleElement);
            this.thumbnailSectionTitleElement.AddChild(this.thumbnailPreviewElement);
        }

        private void BuildFooterButtons()
        {
            Vector2 margin = new(32f, -96f);

            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                ButtonInfo button = this.footerButtons[i];

                Image backgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Scale = new(1f),
                    Size = new(320, 80),
                    Margin = margin,
                    Alignment = CardinalDirection.Southwest,
                };

                Label label = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2f,
                    BorderThickness = 2f,
                };

                this.panelBackgroundElement.AddChild(backgroundElement);
                backgroundElement.AddChild(label);

                this.footerButtonSlots[i] = new(backgroundElement, null, label);

                margin.X += backgroundElement.Size.X + 32;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            Image iconElement = new()
            {
                Texture = button.IconTexture,
                SourceRectangle = button.IconTextureRectangle,
                Scale = new(1.5f),
                Size = new(UIConstants.HUD_GRID_SIZE)
            };

            return new(backgroundElement, iconElement);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();

            this.tooltipBox.RefreshDisplay(TooltipBoxContent.Title, TooltipBoxContent.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = this.menuButtons[i].Name;
                    TooltipBoxContent.Description = this.menuButtons[i].Description;

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
            for (int i = 0; i < this.fieldButtons.Length; i++)
            {
                SlotInfo slot = this.fieldButtonSlots[i];

                Vector2 size = slot.Background.Size / 2;
                Vector2 position = slot.Background.Position + size;

                if (Interaction.OnMouseClick(position, size))
                {
                    this.fieldButtons[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateFooterButtons()
        {
            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                SlotInfo slot = this.footerButtonSlots[i];

                Vector2 size = slot.Background.Size / 2;
                Vector2 position = slot.Background.Position + size;

                if (Interaction.OnMouseClick(position, size))
                {
                    this.footerButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = this.footerButtons[i].Name;
                    TooltipBoxContent.Description = this.footerButtons[i].Description;

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

            this.titleTextualContentElement.TextContent = this.world.Information.Name.Truncate(19);
            this.descriptionTextualContentElement.TextContent = this.world.Information.Description.Truncate(19);
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
