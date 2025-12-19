using Microsoft.Xna.Framework;

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

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class TemperatureSettingsUI : UIBase
    {
        private enum TemperatureIndex : byte
        {
            None = 0,
            VeryCold = 1,
            Cold = 2,
            Normal = 3,
            Hot = 4,
            VeryHot = 5,
        }

        private sealed class Section
        {
            internal string Title => title;
            internal TimeSpan StartTime => startTime;
            internal TimeSpan EndTime => endTime;
            internal TemperatureIndex Index { get => index; set => index = value; }

            internal SlotInfo[] ButtonSlotInfos => buttonSlotInfos;
            internal ButtonInfo[] ButtonInfos => buttonInfos;

            private TemperatureIndex index;

            private readonly SlotInfo[] buttonSlotInfos;
            private readonly ButtonInfo[] buttonInfos;

            private readonly string title;
            private readonly TimeSpan startTime;
            private readonly TimeSpan endTime;

            internal Section(string title, TimeSpan startTime, TimeSpan endTime)
            {
                this.title = title;
                this.startTime = startTime;
                this.endTime = endTime;

                this.buttonInfos =
                [
                    new(TextureIndex.IconUI, new(224, 192, 32, 32), "None", "No temperature effect.", () => this.Index = TemperatureIndex.None),
                    new(TextureIndex.IconUI, new(0, 224, 32, 32), "Very Cold", "Extremely cold temperature effect.", () => this.Index = TemperatureIndex.VeryCold),
                    new(TextureIndex.IconUI, new(32, 224, 32, 32), "Cold", "Mildly cold temperature effect.", () => this.Index = TemperatureIndex.Cold),
                    new(TextureIndex.IconUI, new(64, 224, 32, 32), "Normal", "Normal temperature effect.", () => this.Index = TemperatureIndex.Normal),
                    new(TextureIndex.IconUI, new(96, 224, 32, 32), "Hot", "Mildly hot temperature effect.", () => this.Index = TemperatureIndex.Hot),
                    new(TextureIndex.IconUI, new(128, 224, 32, 32), "Very Hot", "Extremely hot temperature effect.", () => this.Index = TemperatureIndex.VeryHot),
                ];

                this.buttonSlotInfos = new SlotInfo[this.buttonInfos.Length];

                for (int i = 0; i < this.buttonInfos.Length; i++)
                {
                    SlotInfo buttonSlotInfo = CreateButtonSlot(new(0, 0), this.buttonInfos[i]);

                    buttonSlotInfo.Background.Alignment = UIDirection.Southwest;
                    buttonSlotInfo.Icon.Alignment = UIDirection.Center;

                    buttonSlotInfo.Icon.Texture = this.buttonInfos[i].Texture;
                    buttonSlotInfo.Icon.SourceRectangle = this.buttonInfos[i].TextureSourceRectangle;

                    buttonSlotInfo.Background.AddChild(buttonSlotInfo.Icon);

                    this.buttonSlotInfos[i] = buttonSlotInfo;
                }
            }
        }

        private Image background;
        private Label menuTitle;

        private SlotInfo exitButtonSlotInfo;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo exitButtonInfo;
        private readonly Section[] sections;

        private readonly GameManager gameManager;

        internal TemperatureSettingsUI(
            GameManager gameManager,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.tooltipBox = tooltipBox;

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseGUI);

            this.sections =
            [
                new("Late Night", new(0, 0, 0), new(3, 0, 0)),
                new("Early Morning", new(3, 0, 0), new(6, 0, 0)),
                new("Dawn", new(6, 0, 0), new(8, 0, 0)),
                new("Morning", new(8, 0, 0), new(12, 0, 0)),
                new("Early Afternoon", new(12, 0, 0), new(15, 0, 0)),
                new("Afternoon", new(15, 0, 0), new(18, 0, 0)),
                new("Evening", new(18, 0, 0), new(20, 0, 0)),
                new("Night", new(20, 0, 0), new(24, 0, 0)),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildSections();

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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundTemperatureSettings),
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
                TextContent = "Temperature Settings",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildExitButton()
        {
            float marginX = -32.0f;

            ButtonInfo button = this.exitButtonInfo;
            SlotInfo slot = CreateButtonSlot(new(marginX, -72.0f), button);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.background.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildSections()
        {
            for (int i = 0; i < this.sections.Length; i++)
            {
                Section section = this.sections[i];
                
                Label sectionTitle = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.08f),
                    Margin = new(32.0f, 80.0f + (i * 112.0f)),
                    TextContent = string.Format("{0} ({1:00}:{2:00} - {3:00}:{4:00})",
                        section.Title,
                        section.StartTime.Hours, section.StartTime.Minutes,
                        section.EndTime.Hours, section.EndTime.Minutes
                    ),

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                if (i > (this.sections.Length / 2) - 1)
                {
                    // sectionTitle.Margin.X += 540.0f;
                    sectionTitle.Margin = new(540.0f + 32.0f, 80.0f + ((i - (this.sections.Length / 2)) * 112.0f));
                }

                for (int j = 0; j < section.ButtonSlotInfos.Length; j++)
                {
                    SlotInfo buttonSlot = section.ButtonSlotInfos[j];

                    buttonSlot.Background.Margin = new(j * 80.0f, 48.0f);
                    buttonSlot.Icon.Alignment = UIDirection.Center;

                    sectionTitle.AddChild(buttonSlot.Background);
                }

                this.background.AddChild(sectionTitle);
            }
        }

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

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateSections();

            base.Update(gameTime);
        }

        private void UpdateExitButton()
        {
            if (Interaction.OnMouseLeftClick(this.exitButtonSlotInfo.Background))
            {
                this.exitButtonInfo.ClickAction?.Invoke();
            }
            
            if (Interaction.OnMouseOver(this.exitButtonSlotInfo.Background))
            {
                this.tooltipBox.CanDraw = true;

                TooltipBoxContent.SetTitle(this.exitButtonInfo.Name);
                TooltipBoxContent.SetDescription(this.exitButtonInfo.Description);

                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateSections()
        {
            for (int i = 0; i < this.sections.Length; i++)
            {
                Section section = this.sections[i];

                for (int j = 0; j < section.ButtonSlotInfos.Length; j++)
                {
                    SlotInfo buttonSlot = section.ButtonSlotInfos[j];
                    ButtonInfo buttonInfo = section.ButtonInfos[j];

                    if (Interaction.OnMouseLeftClick(buttonSlot.Background))
                    {
                        buttonInfo.ClickAction?.Invoke();
                    }

                    if (Interaction.OnMouseOver(buttonSlot.Background))
                    {
                        this.tooltipBox.CanDraw = true;

                        TooltipBoxContent.SetTitle(buttonInfo.Name);
                        TooltipBoxContent.SetDescription(buttonInfo.Description);

                        buttonSlot.Background.Color = AAP64ColorPalette.HoverColor;
                    }
                    else if ((int)section.Index == j)
                    {
                        buttonSlot.Background.Color = AAP64ColorPalette.Graphite;
                    }
                    else
                    {
                        buttonSlot.Background.Color = AAP64ColorPalette.White;
                    }
                }
            }
        }

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
