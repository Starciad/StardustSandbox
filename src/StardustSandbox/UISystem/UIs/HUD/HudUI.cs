using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Catalog;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class HudUI : UI
    {
        internal bool IsTopToolbarVisible
        {
            get => this.topToolbarContainer.CanDraw;
            set => this.topToolbarContainer.CanDraw = value;
        }

        internal bool IsLeftToolbarVisible
        {
            get => this.leftToolbarContainer.CanDraw;
            set => this.leftToolbarContainer.CanDraw = value;
        }

        internal bool IsRightToolbarVisible
        {
            get => this.rightToolbarContainer.CanDraw;
            set => this.rightToolbarContainer.CanDraw = value;
        }

        private byte slotSelectedIndex = 0;

        private Container topToolbarContainer;
        private Container leftToolbarContainer;
        private Container rightToolbarContainer;

        private Image topToolbarBackground;
        private Image leftToolbarBackground;
        private Image rightToolbarBackground;

        private Image topDrawerButton;
        private Image leftDrawerButton;
        private Image rightDrawerButton;

        private Image toolbarSearchButton;
        private Image toolbarCurrentlySelectedToolIcon;

        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] toolbarSlots = new SlotInfo[UIConstants.HUD_ELEMENT_BUTTONS_LENGTH];
        private readonly SlotInfo[] leftPanelTopButtons;
        private readonly SlotInfo[] leftPanelBottomButtons;
        private readonly SlotInfo[] rightPanelTopButtons;
        private readonly SlotInfo[] rightPanelBottomButtons;

        private readonly ButtonInfo[] leftPanelTopButtonInfos;
        private readonly ButtonInfo[] leftPanelBottomButtonInfos;
        private readonly ButtonInfo[] rightPanelTopButtonInfos;
        private readonly ButtonInfo[] rightPanelBottomButtonInfos;

        private readonly ConfirmSettings reloadSimulationConfirmSettings;
        private readonly ConfirmSettings eraseEverythingConfirmSettings;

        private readonly GameManager gameManager;
        private readonly InputController inputController;
        private readonly ConfirmUI guiConfirm;
        private readonly UIManager uiManager;
        private readonly World world;

        private readonly Rectangle[] speedIconRectangles = [
            new(192, 128, 32, 32),
            new(224, 128, 32, 32),
            new(0, 160, 32, 32),
        ];

        private readonly Rectangle[] pauseAndResumeRectangles = [
            new(96, 192, 32, 32),
            new(128, 192, 32, 32),
        ];

        internal HudUI(
            GameManager gameManager,
            InputController inputController,
            ConfirmUI confirmUI,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.inputController = inputController;
            this.guiConfirm = confirmUI;
            this.uiManager = uiManager;
            this.tooltipBox = tooltipBox;
            this.world = world;

            this.reloadSimulationConfirmSettings = new()
            {
                Caption = Localization_Messages.Confirm_Simulation_Reload_Title,
                Message = Localization_Messages.Confirm_Simulation_Reload_Description,

                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        this.world.Reload();
                    }

                    gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
                },
            };

            this.eraseEverythingConfirmSettings = new()
            {
                Caption = Localization_Messages.Confirm_Simulation_EraseEverything_Title,
                Message = Localization_Messages.Confirm_Simulation_EraseEverything_Description,

                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        this.world.Reset();
                    }

                    gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
                },
            };

            // Default Selection
            SelectItemSlot(0, 0, 0, 0);

            this.leftPanelTopButtonInfos = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(64, 0, 32, 32), Localization_GUIs.HUD_Button_EnvironmentSettings_Name, Localization_GUIs.HUD_Button_EnvironmentSettings_Description, EnvironmentSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(32, 0, 32, 32), Localization_GUIs.HUD_Button_PenSettings_Name, Localization_GUIs.HUD_Button_PenSettings_Description, PenSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(160, 0, 32, 32), Localization_GUIs.HUD_Button_WorldSettings_Name, Localization_GUIs.HUD_Button_WorldSettings_Description, WorldSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(128, 64, 32, 32), Localization_GUIs.HUD_Button_Information_Name, Localization_GUIs.HUD_Button_Information_Description, InfoButtonAction),
            ];

            this.leftPanelBottomButtonInfos = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), this.pauseAndResumeRectangles[0], Localization_GUIs.HUD_Button_PauseSimulation_Name, Localization_GUIs.HUD_Button_PauseSimulation_Description, PauseSimulationButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), this.speedIconRectangles[0], Localization_GUIs.HUD_Button_Speed_Name, Localization_GUIs.HUD_Button_Speed_Description, ChangeSimulationSpeedButtonAction),
            ];

            this.rightPanelTopButtonInfos = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(32, 192, 32, 32), Localization_GUIs.HUD_Button_GameMenu_Name, Localization_GUIs.HUD_Button_GameMenu_Description, GameMenuButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(64, 192, 32, 32), Localization_GUIs.HUD_Button_SaveMenu_Name, Localization_GUIs.HUD_Button_SaveMenu_Description, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtonInfos = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(224, 96, 32, 32), Localization_GUIs.HUD_Button_EraseEverything_Name, Localization_GUIs.HUD_Button_EraseEverything_Description, EraseEverythingButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(160, 192, 32, 32), Localization_GUIs.HUD_Button_ReloadSimulation_Name, Localization_GUIs.HUD_Button_ReloadSimulation_Description, ReloadSimulationButtonAction),
            ];

            this.leftPanelTopButtons = new SlotInfo[this.leftPanelTopButtonInfos.Length];
            this.leftPanelBottomButtons = new SlotInfo[this.leftPanelBottomButtonInfos.Length];
            this.rightPanelTopButtons = new SlotInfo[this.rightPanelTopButtonInfos.Length];
            this.rightPanelBottomButtons = new SlotInfo[this.rightPanelBottomButtonInfos.Length];
        }

        #region ACTIONS

        #region LEFT PANEL

        #region Top Buttons

        private void EnvironmentSettingsButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.EnvironmentSettings);
        }

        private void PenSettingsButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.PenSettings);
        }

        private void WorldSettingsButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.WorldSettings);
        }

        private void InfoButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.Information);
        }

        #endregion

        #region Bottom Buttons

        private void PauseSimulationButtonAction()
        {
            this.gameManager.ToggleState(GameStates.IsSimulationPaused);
        }

        private void ChangeSimulationSpeedButtonAction()
        {
            switch (this.world.Simulation.CurrentSpeed)
            {
                case SimulationSpeed.Normal:
                    this.gameManager.SetSimulationSpeed(SimulationSpeed.Fast);
                    break;

                case SimulationSpeed.Fast:
                    this.gameManager.SetSimulationSpeed(SimulationSpeed.VeryFast);
                    break;

                case SimulationSpeed.VeryFast:
                    this.gameManager.SetSimulationSpeed(SimulationSpeed.Normal);
                    break;

                default:
                    this.gameManager.SetSimulationSpeed(SimulationSpeed.Normal);
                    break;
            }
        }

        #endregion

        #endregion

        // ==================================================== //

        #region RIGHT PANEL

        #region Top Buttons

        private void GameMenuButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.Pause);
        }

        private void SaveMenuButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.SaveSettings);
        }

        #endregion

        #region Bottom Buttons

        private void ReloadSimulationButtonAction()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.guiConfirm.Configure(this.reloadSimulationConfirmSettings);
            this.uiManager.OpenGUI(this.guiConfirm.Index);
        }

        private void EraseEverythingButtonAction()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.guiConfirm.Configure(this.eraseEverythingConfirmSettings);
            this.uiManager.OpenGUI(this.guiConfirm.Index);
        }

        #endregion

        #endregion

        #endregion

        #region BUILD

        protected override void OnBuild(Container root)
        {
            BuildToolbars(root);
            BuildDrawerButtons();

            root.AddChild(this.tooltipBox);
        }

        private void BuildToolbars(Container root)
        {
            this.topToolbarContainer = new()
            {
                Size = new(ScreenConstants.SCREEN_WIDTH, 96),
            };

            this.leftToolbarContainer = new()
            {
                Size = new(96, 608),
                Alignment = CardinalDirection.Southwest,
            };

            this.rightToolbarContainer = new()
            {
                Size = new(96, 608),
                Alignment = CardinalDirection.Southeast,
            };

            root.AddChild(this.topToolbarContainer);
            root.AddChild(this.leftToolbarContainer);
            root.AddChild(this.rightToolbarContainer);

            BuildTopToolbar(this.topToolbarContainer);
            BuildLeftToolbar(this.leftToolbarContainer);
            BuildRightToolbar(this.rightToolbarContainer);
        }

        #region Top Toolbar

        private void BuildTopToolbar(Container container)
        {
            this.topToolbarBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudHorizontalToolbar),
                SourceRectangle = new(new(0, 0), new(ScreenConstants.SCREEN_WIDTH, 96)),
                Size = container.Size,
            };

            container.AddChild(this.topToolbarBackground);

            CreateTopToolbarCurrentlySelectedToolSlot();
            CreateTopToolbarSlots();
            CreateTopToolbarSearchSlot();
        }

        private void CreateTopToolbarCurrentlySelectedToolSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.West,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE, 0),
            };

            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                SourceRectangle = new(64, 32, 32, 32),
                Alignment = CardinalDirection.Center,
                Scale = new(2f),
                Size = new(32f),
            };

            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);

            this.toolbarCurrentlySelectedToolIcon = slotIcon;
        }

        private void CreateTopToolbarSlots()
        {
            Vector2 margin = new(UIConstants.HUD_SLOT_SPACING * 2f, 0);

            Item[] items = CatalogDatabase.GetItems(UIConstants.HUD_ELEMENT_BUTTONS_LENGTH);

            for (int i = 0, length = items.Length; i < length; i++)
            {
                Item curentItem = items[i];

                SlotInfo slot = CreateButtonSlot(margin, curentItem);

                slot.Background.Alignment = CardinalDirection.West;

                if (!slot.Background.ContainsData(UIConstants.DATA_ITEM))
                {
                    slot.Background.AddData(UIConstants.DATA_ITEM, curentItem);
                }

                // Update
                this.topToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.toolbarSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void CreateTopToolbarSearchSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.East,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE * -1, 0),
            };

            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                SourceRectangle = new(0, 0, 32, 32),
                Alignment = CardinalDirection.Center,
                Scale = new(2f),
                Size = new(32f),
            };

            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);

            this.toolbarSearchButton = slotSearchBackground;
        }

        #endregion

        private void BuildLeftToolbar(Container container)
        {
            this.leftToolbarBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudVerticalToolbar),
                SourceRectangle = new(new(0, 0), new(96, 608)),
                Size = container.Size,
            };

            container.AddChild(this.leftToolbarBackground);

            #region BUTTONS

            Vector2 margin = new(0, UIConstants.HUD_SLOT_SPACING / 2f);

            // Top
            for (int i = 0; i < this.leftPanelTopButtonInfos.Length; i++)
            {
                ButtonInfo button = this.leftPanelTopButtonInfos[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.North;

                this.leftToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.leftPanelTopButtons[i] = slot;

                margin.Y += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
            }

            margin = new(0, -(UIConstants.HUD_SLOT_SPACING / 2f));

            // Bottom
            for (int i = 0; i < this.leftPanelBottomButtonInfos.Length; i++)
            {
                ButtonInfo button = this.leftPanelBottomButtonInfos[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                this.leftToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.leftPanelBottomButtons[i] = slot;

                margin.Y -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
            }

            #endregion
        }

        private void BuildRightToolbar(Container container)
        {
            this.rightToolbarBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudVerticalToolbar),
                SourceRectangle = new(new(96, 0), new(96, 608)),
                Size = container.Size,
            };

            container.AddChild(this.rightToolbarBackground);

            #region BUTTONS

            Vector2 margin = new(0f, UIConstants.HUD_SLOT_SPACING / 2f);

            // Top
            for (int i = 0; i < this.rightPanelTopButtonInfos.Length; i++)
            {
                ButtonInfo button = this.rightPanelTopButtonInfos[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.North;

                this.rightToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.rightPanelTopButtons[i] = slot;

                margin.Y += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
            }

            margin = new(margin.X, -(UIConstants.HUD_SLOT_SPACING / 2f));

            // Bottom
            for (int i = 0; i < this.rightPanelBottomButtonInfos.Length; i++)
            {
                ButtonInfo button = this.rightPanelBottomButtonInfos[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                this.rightToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.rightPanelBottomButtons[i] = slot;

                margin.Y -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
            }

            #endregion
        }

        // ============================================================= //

        private void BuildDrawerButtons()
        {
            BuildTopDrawerButton();
            BuildLeftDrawerButton();
            BuildRightDrawerButton();
        }

        private void BuildTopDrawerButton()
        {
            this.topDrawerButton = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(163, 220, 80, 24),
                Size = new(80, 24),
                Margin = new(0, 48f),
                Scale = new(2f),
                Alignment = CardinalDirection.South,
            };

            this.topToolbarContainer.AddChild(this.topDrawerButton);
        }

        private void BuildLeftDrawerButton()
        {
            this.leftDrawerButton = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(243, 220, 24, 80),
                Size = new(24, 80),
                Scale = new(2f),
                Margin = new(48f, 0f),
                Alignment = CardinalDirection.East,
            };

            this.leftToolbarContainer.AddChild(this.leftDrawerButton);
        }

        private void BuildRightDrawerButton()
        {
            this.rightDrawerButton = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(267, 220, 24, 80),
                Size = new(24, 80),
                Scale = new(2f),
                Margin = new(-48f, 0f),
                Alignment = CardinalDirection.West,
            };

            this.rightToolbarContainer.AddChild(this.rightDrawerButton);
        }

        // ============================================================= //

        private static SlotInfo CreateButtonSlot(Vector2 margin, Texture2D iconTexture, Rectangle? iconTextureRectangle)
        {
            Image backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            Image iconElement = new()
            {
                Texture = iconTexture,
                SourceRectangle = iconTextureRectangle,
                Scale = new(1.5f),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Alignment = CardinalDirection.Center,
            };

            return new(backgroundElement, iconElement);
        }

        private static SlotInfo CreateButtonSlot(Vector2 margin, Item item)
        {
            return CreateButtonSlot(margin, item.IconTexture, item.IconTextureRectangle);
        }

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            return CreateButtonSlot(margin, button.IconTexture, button.IconTextureRectangle);
        }

        #endregion

        #region UPDATE ROUTINE

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            this.tooltipBox.RefreshDisplay(TooltipBoxContent.Title, TooltipBoxContent.Description);
        }

        #endregion

        #region UTILITIES

        internal void AddItemToToolbar(Item item)
        {
            if (TryHighlightExistingItem(item))
            {
                return;
            }

            ShiftItemsAndAddToLastSlot(item);
        }

        private bool TryHighlightExistingItem(Item item)
        {
            for (byte i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                SlotInfo slot = this.toolbarSlots[i];

                if (slot.Background.ContainsData(UIConstants.DATA_ITEM) &&
                    slot.Background.GetData(UIConstants.DATA_ITEM) is Item otherItem &&
                    item == otherItem)
                {
                    SelectItemSlot(i, otherItem);
                    return true;
                }
            }

            return false;
        }

        private void ShiftItemsAndAddToLastSlot(Item item)
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                SlotInfo currentSlot = this.toolbarSlots[i];
                SlotInfo nextSlot = this.toolbarSlots[i + 1];

                if (currentSlot.Background.ContainsData(UIConstants.DATA_ITEM) &&
                    nextSlot.Background.ContainsData(UIConstants.DATA_ITEM))
                {
                    currentSlot.Background.UpdateData(
                        UIConstants.DATA_ITEM,
                        nextSlot.Background.GetData(UIConstants.DATA_ITEM)
                    );

                    currentSlot.Icon.Texture = nextSlot.Icon.Texture;
                }
            }

            UpdateLastSlot(item);
        }

        private void UpdateLastSlot(Item item)
        {
            SlotInfo lastSlot = this.toolbarSlots[^1];

            lastSlot.Background.UpdateData(UIConstants.DATA_ITEM, item);
            lastSlot.Icon.Texture = item.IconTexture;

            SelectItemSlot(Convert.ToByte(this.toolbarSlots.Length - 1), item);
        }

        internal bool ItemIsEquipped(Item item)
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                Item hudItem = (Item)this.toolbarSlots[i].Background.GetData(UIConstants.DATA_ITEM);

                if (item == hudItem)
                {
                    return true;
                }
            }

            return false;
        }

        internal void SetToolIcon(Rectangle iconRectangle)
        {
            this.toolbarCurrentlySelectedToolIcon.SourceRectangle = iconRectangle;
        }

        private void SelectItemSlot(byte slotIndex, Item item)
        {
            this.slotSelectedIndex = slotIndex;
            this.inputController.Player.SelectItem(item);
        }

        private void SelectItemSlot(byte slotIndex, byte categoryIndex, byte subcategoryIndex, byte itemIndex)
        {
            SelectItemSlot(slotIndex, CatalogDatabase.GetItem(categoryIndex, subcategoryIndex, itemIndex));
        }

        #endregion
    }
}
