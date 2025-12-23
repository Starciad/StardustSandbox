using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class EnvironmentSettingsUI : UIBase
    {
        private Image background;
        private Label menuTitle, timeStateSectionTitle, timeSectionTitle;

        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] menuButtonSlotInfos, timeStateButtonSlotInfos, timeButtonSlotInfos;
        private readonly ButtonInfo[] menuButtonInfos, timeStateButtonInfos, timeButtonInfos;

        private readonly UIManager uiManager;
        private readonly World world;

        internal EnvironmentSettingsUI(
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseGUI),
                new(TextureIndex.IconTools, new(32, 0, 32, 32), Localization_GUIs.EnvironmentSettings_TemperatureEditor_Name, Localization_GUIs.EnvironmentSettings_TemperatureEditor_Description, () => this.uiManager.OpenGUI(UIIndex.TemperatureSettings)),
            ];

            this.timeStateButtonInfos = [
                new ButtonInfo(TextureIndex.IconUI, new(160, 64, 32, 32), Localization_Statements.Disable, Localization_GUIs.EnvironmentSettings_TimeState_Disable_Description, () => { this.world.Time.IsFrozen = true; }),
                new ButtonInfo(TextureIndex.IconUI, new(192, 64, 32, 32), Localization_Statements.Enable, Localization_GUIs.EnvironmentSettings_TimeState_Enable_Description, () => { this.world.Time.IsFrozen = false; }),
            ];

            this.timeButtonInfos = [
                new ButtonInfo(TextureIndex.IconUI, new(0, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_LateNight_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_LateNight_Description, () => this.world.Time.SetTime(new(0, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(192, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyMorning_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyMorning_Description, () => this.world.Time.SetTime(new(3, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(32, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Dawn_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Dawn_Description, () => this.world.Time.SetTime(new(6, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(64, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Morning_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Morning_Description, () => this.world.Time.SetTime(new(8, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(96, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyAfternoon_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyAfternoon_Description, () => this.world.Time.SetTime(new(12, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(128, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Afternoon_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Afternoon_Description, () => this.world.Time.SetTime(new(15, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(160, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Evening_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Evening_Description, () => this.world.Time.SetTime(new(18, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(192, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Night_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Night_Description, () => this.world.Time.SetTime(new(20, 0, 0))),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.timeStateButtonSlotInfos = new SlotInfo[this.timeStateButtonInfos.Length];
            this.timeButtonSlotInfos = new SlotInfo[this.timeButtonInfos.Length];
        }

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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundEnvironmentSettings),
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
                TextContent = Localization_GUIs.EnvironmentSettings_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
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

        private void BuildTimeStateSection()
        {
            this.timeStateSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.EnvironmentSettings_TimeState_Title
            };

            this.background.AddChild(this.timeStateSectionTitle);

            // Buttons
            float marginX = 0.0f;

            for (int i = 0; i < this.timeStateButtonSlotInfos.Length; i++)
            {
                ButtonInfo button = this.timeStateButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, 52.0f), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.timeStateSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.timeStateButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        private void BuildTimeSection()
        {
            this.timeSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(this.timeStateButtonSlotInfos[^1].Background.Position.X + 96.0f, 0.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.EnvironmentSettings_TimeOfDay_Title
            };

            this.timeStateSectionTitle.AddChild(this.timeSectionTitle);

            // Buttons
            float marginX = 0.0f;

            for (int i = 0; i < this.timeButtonSlotInfos.Length; i++)
            {
                ButtonInfo button = this.timeButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, 52.0f), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.timeSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.timeButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            return new(
                background: new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = margin,
                },

                icon: new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f)
                }
            );
        }

        #endregion

        #region UPDATE

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateTimeStateButtons();
            UpdateTimeButtons();

            base.Update(gameTime);
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

        private void UpdateTimeStateButtons()
        {
            for (int i = 0; i < this.timeStateButtonInfos.Length; i++)
            {
                SlotInfo slot = this.timeStateButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.timeStateButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeStateButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.timeStateButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }

            if (this.world.Time.IsFrozen)
            {
                this.timeStateButtonSlotInfos[0].Background.Color = AAP64ColorPalette.SelectedColor;
            }
            else
            {
                this.timeStateButtonSlotInfos[1].Background.Color = AAP64ColorPalette.SelectedColor;
            }
        }

        private void UpdateTimeButtons()
        {
            for (int i = 0; i < this.timeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.timeButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.timeButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.timeButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        #endregion

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
