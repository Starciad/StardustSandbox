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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class SaveSettingsUI : UI
    {
        private Texture2D worldThumbnailTexture;

        private ImageUIElement panelBackgroundElement;

        private LabelUIElement menuTitleElement;
        private LabelUIElement nameSectionTitleElement;
        private LabelUIElement descriptionSectionTitleElement;
        private LabelUIElement thumbnailSectionTitleElement;

        private ImageUIElement titleInputFieldElement;
        private ImageUIElement descriptionInputFieldElement;

        private LabelUIElement titleTextualContentElement;
        private LabelUIElement descriptionTextualContentElement;

        private ImageUIElement thumbnailPreviewElement;

        private readonly TooltipBoxUIElement tooltipBoxElement;

        private readonly UIButton[] menuButtons;
        private readonly UIButton[] fieldButtons;
        private readonly UIButton[] footerButtons;

        private readonly UISlot[] menuButtonSlots;
        private readonly UISlot[] fieldButtonSlots;
        private readonly UISlot[] footerButtonSlots;

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
            TooltipBoxUIElement tooltipBoxElement,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.graphicsDevice = graphicsDevice;
            this.textInputUI = textInputUI;
            this.tooltipBoxElement = tooltipBoxElement;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.fieldButtons = [
                new(null, null, "Name Field", string.Empty, NameFieldButtonAction),
                new(null, null, "Description Field", string.Empty, DescriptionFieldButtonAction)
            ];

            this.footerButtons = [
                new(null, null, Localization_Statements.Save, Localization_GUIs.HUD_Complements_SaveSettings_Button_Save_Description, SaveButtonAction),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.fieldButtonSlots = new UISlot[this.fieldButtons.Length];
            this.footerButtonSlots = new UISlot[this.footerButtons.Length];

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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildNameSection(layout);
            BuildDescriptionSection(layout);
            BuildThumbnailSection(layout);
            BuildFooterButtons(layout);

            layout.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundSaveSettings),
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layout.AddElement(backgroundShadowElement);
            layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(Layout layout)
        {
            this.menuTitleElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_SaveSettings_Title);
            this.menuTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];
                UISlot slot = CreateButtonSlot(margin, button);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildNameSection(Layout layout)
        {
            this.nameSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            this.titleInputFieldElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                TextureClipArea = new(0, 220, 163, 38),
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.titleTextualContentElement = new()
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                OriginPivot = CardinalDirection.East,
                PositionAnchor = CardinalDirection.West
            };

            this.nameSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_SaveSettings_Section_Name_Title);

            this.nameSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.titleInputFieldElement.PositionRelativeToElement(this.nameSectionTitleElement);
            this.titleTextualContentElement.PositionRelativeToElement(this.titleInputFieldElement);

            this.fieldButtonSlots[0] = new(this.titleInputFieldElement, null, this.titleTextualContentElement);

            layout.AddElement(this.nameSectionTitleElement);
            layout.AddElement(this.titleInputFieldElement);
            layout.AddElement(this.titleTextualContentElement);
        }

        private void BuildDescriptionSection(Layout layout)
        {
            this.descriptionSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 96f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            this.descriptionInputFieldElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                TextureClipArea = new(0, 220, 163, 38),
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.descriptionTextualContentElement = new()
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                OriginPivot = CardinalDirection.East,
                PositionAnchor = CardinalDirection.West
            };

            this.descriptionSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_SaveSettings_Section_Description_Title);

            this.descriptionSectionTitleElement.PositionRelativeToElement(this.titleInputFieldElement);
            this.descriptionInputFieldElement.PositionRelativeToElement(this.descriptionSectionTitleElement);
            this.descriptionTextualContentElement.PositionRelativeToElement(this.descriptionInputFieldElement);

            this.fieldButtonSlots[1] = new(this.descriptionInputFieldElement, null, this.descriptionTextualContentElement);

            layout.AddElement(this.descriptionSectionTitleElement);
            layout.AddElement(this.descriptionInputFieldElement);
            layout.AddElement(this.descriptionTextualContentElement);
        }

        private void BuildThumbnailSection(Layout layout)
        {
            this.thumbnailSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(-32f, 112f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                PositionAnchor = CardinalDirection.Northeast,
                OriginPivot = CardinalDirection.Southwest
            };

            this.thumbnailPreviewElement = new()
            {
                Scale = new(12.5f),
                Margin = new(0f, 48f),
                OriginPivot = CardinalDirection.Southwest,
            };

            this.thumbnailSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_SaveSettings_Section_Thumbnail_Title);
            this.thumbnailSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.thumbnailPreviewElement.PositionRelativeToElement(this.thumbnailSectionTitleElement);

            layout.AddElement(this.thumbnailSectionTitleElement);
            layout.AddElement(this.thumbnailPreviewElement);
        }

        private void BuildFooterButtons(Layout layout)
        {
            Vector2 margin = new(32f, -96f);

            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                UIButton button = this.footerButtons[i];

                ImageUIElement backgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                    TextureClipArea = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Scale = new(1f),
                    Size = new(320, 80),
                    Margin = margin,
                    PositionAnchor = CardinalDirection.Southwest,
                };

                LabelUIElement labelElement = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                    PositionAnchor = CardinalDirection.Center,
                    OriginPivot = CardinalDirection.Center
                };

                labelElement.SetTextualContent(button.Name);
                labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2));

                backgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                labelElement.PositionRelativeToElement(backgroundElement);

                layout.AddElement(backgroundElement);
                layout.AddElement(labelElement);

                this.footerButtonSlots[i] = new(backgroundElement, null, labelElement);

                margin.X += backgroundElement.Size.X + 32;
            }
        }

        // =============================================================== //

        private UISlot CreateButtonSlot(Vector2 margin, UIButton button)
        {
            ImageUIElement backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                TextureClipArea = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            ImageUIElement iconElement = new()
            {
                Texture = button.IconTexture,
                TextureClipArea = button.IconTextureRectangle,
                OriginPivot = CardinalDirection.Center,
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

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();

            this.tooltipBoxElement.RefreshDisplay(TooltipContent.Title, TooltipContent.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UISlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.menuButtons[i].Name;
                    TooltipContent.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateFieldButtons()
        {
            for (int i = 0; i < this.fieldButtons.Length; i++)
            {
                UISlot slot = this.fieldButtonSlots[i];

                Vector2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size;

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.fieldButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateFooterButtons()
        {
            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                UISlot slot = this.footerButtonSlots[i];

                Vector2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size;

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.footerButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.footerButtons[i].Name;
                    TooltipContent.Description = this.footerButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateInfos()
        {
            this.worldThumbnailTexture = this.world.CreateThumbnail(this.graphicsDevice);
            this.thumbnailPreviewElement.Texture = this.worldThumbnailTexture;

            this.titleTextualContentElement.SetTextualContent(this.world.Information.Name.Truncate(19));
            this.descriptionTextualContentElement.SetTextualContent(this.world.Information.Description.Truncate(19));
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
