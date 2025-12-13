using Microsoft.Xna.Framework;

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
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;
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

        private readonly ConfirmSettings changeWorldSizeConfirmSettings;

        private readonly ConfirmUI confirmUI;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal WorldSettingsUI(
            ConfirmUI confirmUI,
            GameManager gameManager,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.confirmUI = confirmUI;
            this.gameManager = gameManager;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;

            this.changeWorldSizeConfirmSettings = new()
            {
                Caption = Localization_Messages.Confirm_World_Resize_Title,
                Message = Localization_Messages.Confirm_World_Resize_Description,
                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        world.StartNew(this.worldTargetSize);
                    }

                    gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
                },
            };

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseGUI),
            ];

            this.sizeButtonInfos = [
                new(TextureIndex.IconUI, new(0, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[0]); }),
                new(TextureIndex.IconUI, new(32, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[1]); }),
                new(TextureIndex.IconUI, new(64, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[2]); }),
                new(TextureIndex.IconUI, new(96, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[3]); }),
                new(TextureIndex.IconUI, new(128, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[4]); }),
                new(TextureIndex.IconUI, new(160, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[5]); }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.sizeButtonSlotInfos = new SlotInfo[this.sizeButtonInfos.Length];
        }

        private void SetWorldSizeButtonAction(Point size)
        {
            this.uiManager.CloseGUI();
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.worldTargetSize = size;
            this.confirmUI.Configure(this.changeWorldSizeConfirmSettings);
            this.uiManager.OpenGUI(this.confirmUI.Index);
        }

        #region BUILDER

        protected override void OnBuild(in Container root)
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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundWorldSettings),
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
                TextContent = Localization_GUIs.HUD_Complements_WorldSettings_Title,

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
                TextContent = Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Title
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

        internal override void Update(in GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateSizeButtons();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.sizeButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.sizeButtonInfos[i].ClickAction?.Invoke();
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
