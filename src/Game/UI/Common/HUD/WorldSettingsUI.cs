using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class WorldSettingsUI : UIBase
    {
        private Point worldTargetSize;

        private Image background;
        private Label menuTitle, sizeSectionTitle;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtonInfos, sizeButtonInfos;
        private readonly SlotInfo[] menuButtonSlotInfos, sizeButtonSlotInfos;

        private readonly ActorManager actorManager;
        private readonly ConfirmUI confirmUI;
        private readonly MessageUI messageUI;
        private readonly UIManager uiManager;
        private readonly World world;

        internal WorldSettingsUI(
            ActorManager actorManager,
            ConfirmUI confirmUI,
            UIIndex index,
            MessageUI messageUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.actorManager = actorManager;
            this.confirmUI = confirmUI;
            this.messageUI = messageUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseUI),
            ];

            this.sizeButtonInfos = [
                new(TextureIndex.IconUI, new(0, 128, 32, 32), Localization_GUIs.WorldSettings_Size_Small_Name, Localization_GUIs.WorldSettings_Size_Small_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[0]); }),
                new(TextureIndex.IconUI, new(32, 128, 32, 32), Localization_GUIs.WorldSettings_Size_MediumSmall_Name, Localization_GUIs.WorldSettings_Size_MediumSmall_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[1]); }),
                new(TextureIndex.IconUI, new(64, 128, 32, 32), Localization_GUIs.WorldSettings_Size_Medium_Name, Localization_GUIs.WorldSettings_Size_Medium_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[2]); }),
                new(TextureIndex.IconUI, new(96, 128, 32, 32), Localization_GUIs.WorldSettings_Size_MediumLarge_Name, Localization_GUIs.WorldSettings_Size_MediumLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[3]); }),
                new(TextureIndex.IconUI, new(128, 128, 32, 32), Localization_GUIs.WorldSettings_Size_Large_Name, Localization_GUIs.WorldSettings_Size_Large_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[4]); }),
                new(TextureIndex.IconUI, new(160, 128, 32, 32), Localization_GUIs.WorldSettings_Size_VeryLarge_Name, Localization_GUIs.WorldSettings_Size_VeryLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[5]); }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.sizeButtonSlotInfos = new SlotInfo[this.sizeButtonInfos.Length];
        }

        private void SetWorldSizeButtonAction(Point size)
        {
            this.uiManager.CloseUI();
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            this.worldTargetSize = size;

            this.confirmUI.Configure(new()
            {
                Caption = Localization_Messages.Confirm_World_Resize_Title,
                Message = Localization_Messages.Confirm_World_Resize_Description,
                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        GameHandler.Reset(this.actorManager, this.world);
                        this.world.StartNew(this.worldTargetSize);

                        StatusSettings statusSettings = SettingsSerializer.Load<StatusSettings>();

                        if (!statusSettings.TheMovementTutorialWasDisplayed)
                        {
                            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();

                            SoundEngine.Play(SoundEffectIndex.GUI_Message);

                            this.messageUI.SetContent(string.Format(Localization_Messages.Tutorial_Move, controlSettings.MoveCameraUp, controlSettings.MoveCameraLeft, controlSettings.MoveCameraDown, controlSettings.MoveCameraRight));
                            this.uiManager.OpenUI(UIIndex.Message);

                            SettingsSerializer.Save<StatusSettings>(new(statusSettings)
                            {
                                TheMovementTutorialWasDisplayed = true,
                            });
                        }
                    }

                    GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
                },
            });

            this.uiManager.OpenUI(UIIndex.Confirm);
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildSizeSection();

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
                Texture = AssetDatabase.GetTexture(TextureIndex.GUIBackgroundWorldSettings),
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
                TextContent = Localization_GUIs.WorldSettings_Title,

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

        private void BuildSizeSection()
        {
            this.sizeSectionTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                TextContent = Localization_GUIs.WorldSettings_Size_Title
            };

            this.background.AddChild(this.sizeSectionTitle);

            // Buttons
            float marginX = 0.0f;

            for (int i = 0; i < this.sizeButtonInfos.Length; i++)
            {
                ButtonInfo button = this.sizeButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, 52.0f), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.sizeSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.sizeButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GUIButtons),
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

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateSizeButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
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

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }
            }
        }

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.sizeButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.sizeButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.sizeButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.sizeButtonInfos[i].Description);

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
