using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class EnvironmentSettingsUI : UIBase
    {
        private Image panelBackgroundElement;

        private Label menuTitleElement;
        private Label timeStateSectionTitleElement;
        private Label timeSectionTitleElement;

        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] menuButtonSlots;
        private readonly SlotInfo[] timeStateButtonSlots;
        private readonly SlotInfo[] timeButtonSlots;

        private readonly ButtonInfo[] menuButtons;
        private readonly ButtonInfo[] timeStateButtons;
        private readonly ButtonInfo[] timeButtons;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal EnvironmentSettingsUI(
            GameManager gameManager,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtons = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.timeStateButtons = [
                new ButtonInfo(TextureIndex.IconUI, new(160, 64, 32, 32), Localization_Statements.Disable, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Disable_Description, () => SetTimeFreezeState(true)),
                new ButtonInfo(TextureIndex.IconUI, new(192, 64, 32, 32), Localization_Statements.Enable, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Enable_Description, () => SetTimeFreezeState(false)),
            ];

            this.timeButtons = [
                new ButtonInfo(TextureIndex.IconUI, new(0, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Description, () => SetTimeButtonAction(new TimeSpan(0, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(32, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Description, () => SetTimeButtonAction(new TimeSpan(6, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(64, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Description, () => SetTimeButtonAction(new TimeSpan(9, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(96, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Description, () => SetTimeButtonAction(new TimeSpan(12, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(128, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Description, () => SetTimeButtonAction(new TimeSpan(15, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(160, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Description, () => SetTimeButtonAction(new TimeSpan(18, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(192, 96, 32, 32), Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Description, () => SetTimeButtonAction(new TimeSpan(21, 0, 0))),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.timeStateButtonSlots = new SlotInfo[this.timeStateButtons.Length];
            this.timeButtonSlots = new SlotInfo[this.timeButtons.Length];
        }

        #region ACTIONS

        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        private void SetTimeFreezeState(bool value)
        {
            this.world.Time.IsFrozen = value;
        }

        private void SetTimeButtonAction(TimeSpan value)
        {
            this.world.Time.SetTime(value);
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildTimeStateSection();
            BuildTimeSection();

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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundEnvironmentSettings),
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
                TextContent = Localization_GUIs.HUD_Complements_EnvironmentSettings_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
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

        private void BuildTimeStateSection()
        {
            this.timeStateSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Title
            };

            this.panelBackgroundElement.AddChild(this.timeStateSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.timeStateButtonSlots.Length; i++)
            {
                ButtonInfo button = this.timeStateButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.timeStateSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.timeStateButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildTimeSection()
        {
            this.timeSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(this.timeStateSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.timeStateButtonSlots.Length) + 64, 0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Title
            };

            this.timeStateSectionTitleElement.AddChild(this.timeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.timeButtonSlots.Length; i++)
            {
                ButtonInfo button = this.timeButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.timeSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.timeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
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

        #region UPDATE

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateTimeStateButtons();
            UpdateTimeButtons();

            this.tooltipBox.RefreshDisplay();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseLeftClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseLeftOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtons[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateTimeStateButtons()
        {
            for (int i = 0; i < this.timeStateButtons.Length; i++)
            {
                SlotInfo slot = this.timeStateButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseLeftClick(position, size))
                {
                    this.timeStateButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseLeftOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeStateButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.timeStateButtons[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }

            if (this.world.Time.IsFrozen)
            {
                this.timeStateButtonSlots[0].Background.Color = AAP64ColorPalette.SelectedColor;
            }
            else
            {
                this.timeStateButtonSlots[1].Background.Color = AAP64ColorPalette.SelectedColor;
            }
        }

        private void UpdateTimeButtons()
        {
            for (int i = 0; i < this.timeButtons.Length; i++)
            {
                SlotInfo slot = this.timeButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseLeftClick(position, size))
                {
                    this.timeButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseLeftOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.timeButtons[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
