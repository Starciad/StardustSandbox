using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class EnvironmentSettingsUI : UI
    {
        private ImageUIElement panelBackgroundElement;

        private LabelUIElement menuTitleElement;
        private LabelUIElement timeStateSectionTitleElement;
        private LabelUIElement timeSectionTitleElement;

        private readonly TooltipBoxUIElement tooltipBoxElement;

        private readonly UISlot[] menuButtonSlots;
        private readonly UISlot[] timeStateButtonSlots;
        private readonly UISlot[] timeButtonSlots;

        private readonly UIButton[] menuButtons;
        private readonly UIButton[] timeStateButtons;
        private readonly UIButton[] timeButtons;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D frozenTimeIconTexture;
        private readonly Texture2D timeProgressIconTexture;
        private readonly Texture2D midnightIconTexture;
        private readonly Texture2D dawnIconTexture;
        private readonly Texture2D morningIconTexture;
        private readonly Texture2D noonIconTexture;
        private readonly Texture2D afternoonIconTexture;
        private readonly Texture2D duskIconTexture;
        private readonly Texture2D eveningIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal EnvironmentSettingsUI(
            GameManager gameManager,
            UIIndex index,
            TooltipBoxUIElement tooltipBoxElement,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.tooltipBoxElement = tooltipBoxElement;
            this.uiManager = uiManager;
            this.world = world;

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = AssetDatabase.GetTexture("texture_gui_background_7");
            this.guiSmallButtonTexture = AssetDatabase.GetTexture("texture_gui_button_1");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

            this.exitIconTexture = AssetDatabase.GetTexture("texture_icon_gui_16");
            this.frozenTimeIconTexture = AssetDatabase.GetTexture("texture_icon_gui_29");
            this.timeProgressIconTexture = AssetDatabase.GetTexture("texture_icon_gui_30");
            this.midnightIconTexture = AssetDatabase.GetTexture("texture_icon_gui_31");
            this.dawnIconTexture = AssetDatabase.GetTexture("texture_icon_gui_32");
            this.morningIconTexture = AssetDatabase.GetTexture("texture_icon_gui_33");
            this.noonIconTexture = AssetDatabase.GetTexture("texture_icon_gui_34");
            this.afternoonIconTexture = AssetDatabase.GetTexture("texture_icon_gui_35");
            this.duskIconTexture = AssetDatabase.GetTexture("texture_icon_gui_36");
            this.eveningIconTexture = AssetDatabase.GetTexture("texture_icon_gui_37");

            this.menuButtons = [
                new(this.exitIconTexture, Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.timeStateButtons = [
                new UIButton(this.frozenTimeIconTexture, Localization_Statements.Disable, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Disable_Description, () => SetTimeFreezeState(true)),
                new UIButton(this.timeProgressIconTexture, Localization_Statements.Enable, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Button_Enable_Description, () => SetTimeFreezeState(false)),
            ];

            this.timeButtons = [
                new UIButton(this.midnightIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Midnight_Description, () => SetTimeButtonAction(new TimeSpan(0, 0, 0))),
                new UIButton(this.dawnIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dawn_Description, () => SetTimeButtonAction(new TimeSpan(6, 0, 0))),
                new UIButton(this.morningIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Morning_Description, () => SetTimeButtonAction(new TimeSpan(9, 0, 0))),
                new UIButton(this.noonIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Noon_Description, () => SetTimeButtonAction(new TimeSpan(12, 0, 0))),
                new UIButton(this.afternoonIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Afternoon_Description, () => SetTimeButtonAction(new TimeSpan(15, 0, 0))),
                new UIButton(this.duskIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Dusk_Description, () => SetTimeButtonAction(new TimeSpan(18, 0, 0))),
                new UIButton(this.eveningIconTexture, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Title, Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Button_Evening_Description, () => SetTimeButtonAction(new TimeSpan(21, 0, 0))),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.timeStateButtonSlots = new UISlot[this.timeStateButtons.Length];
            this.timeButtonSlots = new UISlot[this.timeButtons.Length];
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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildTimeStateSection(layout);
            BuildTimeSection(layout);

            layout.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = this.particleTexture,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = this.panelBackgroundTexture,
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
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_EnvironmentSettings_Title);
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
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

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

        private void BuildTimeStateSection(Layout layout)
        {
            this.timeStateSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.timeStateSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_TimeState_Title);
            this.timeStateSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.timeStateSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.timeStateButtonSlots.Length; i++)
            {
                UIButton button = this.timeStateButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.timeStateSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.timeStateButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildTimeSection(Layout layout)
        {
            this.timeSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(this.timeStateSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.timeStateButtonSlots.Length) + 64, 0f),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.timeSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_EnvironmentSettings_Section_Time_Title);
            this.timeSectionTitleElement.PositionRelativeToElement(this.timeStateSectionTitleElement);

            layout.AddElement(this.timeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.timeButtonSlots.Length; i++)
            {
                UIButton button = this.timeButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.timeSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.timeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        // =============================================================== //

        private UISlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            ImageUIElement backgroundElement = new()
            {
                Texture = this.guiSmallButtonTexture,
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            ImageUIElement iconElement = new()
            {
                Texture = iconTexture,
                OriginPivot = CardinalDirection.Center,
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

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateTimeStateButtons();
            UpdateTimeButtons();

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

        private void UpdateTimeStateButtons()
        {
            for (int i = 0; i < this.timeStateButtons.Length; i++)
            {
                UISlot slot = this.timeStateButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.timeStateButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.timeStateButtons[i].Name;
                    TooltipContent.Description = this.timeStateButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
                }
            }

            if (this.world.Time.IsFrozen)
            {
                this.timeStateButtonSlots[0].BackgroundElement.Color = AAP64ColorPalette.SelectedColor;
            }
            else
            {
                this.timeStateButtonSlots[1].BackgroundElement.Color = AAP64ColorPalette.SelectedColor;
            }
        }

        private void UpdateTimeButtons()
        {
            for (int i = 0; i < this.timeButtons.Length; i++)
            {
                UISlot slot = this.timeButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.timeButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.timeButtons[i].Name;
                    TooltipContent.Description = this.timeButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
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
