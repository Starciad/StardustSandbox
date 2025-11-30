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
            get => this.topToolbarContainerElement.CanDraw;
            set => this.topToolbarContainerElement.CanDraw = value;
        }

        internal bool IsLeftToolbarVisible
        {
            get => this.leftToolbarContainerElement.CanDraw;
            set => this.leftToolbarContainerElement.CanDraw = value;
        }

        internal bool IsRightToolbarVisible
        {
            get => this.rightToolbarContainerElement.CanDraw;
            set => this.rightToolbarContainerElement.CanDraw = value;
        }

        private byte slotSelectedIndex = 0;

        private Container topToolbarContainerElement;
        private Container leftToolbarContainerElement;
        private Container rightToolbarContainerElement;

        private Image topToolbarBackgroundElement;
        private Image leftToolbarBackgroundElement;
        private Image rightToolbarBackgroundElement;

        private Image topDrawerButtonElement;
        private Image leftDrawerButtonElement;
        private Image rightDrawerButtonElement;

        private Image toolbarElementSearchButtonElement;
        private Image toolbarCurrentlySelectedToolIconElement;

        private readonly TooltipBox tooltipBoxElement;

        private readonly SlotInfo[] toolbarElementSlots = new SlotInfo[UIConstants.HUD_ELEMENT_BUTTONS_LENGTH];
        private readonly SlotInfo[] leftPanelTopButtonElements;
        private readonly SlotInfo[] leftPanelBottomButtonElements;
        private readonly SlotInfo[] rightPanelTopButtonElements;
        private readonly SlotInfo[] rightPanelBottomButtonElements;

        private readonly ButtonInfo[] leftPanelTopButtons;
        private readonly ButtonInfo[] leftPanelBottomButtons;
        private readonly ButtonInfo[] rightPanelTopButtons;
        private readonly ButtonInfo[] rightPanelBottomButtons;

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
            TooltipBox tooltipBoxElement,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.inputController = inputController;
            this.guiConfirm = confirmUI;
            this.uiManager = uiManager;
            this.tooltipBoxElement = tooltipBoxElement;
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

            this.leftPanelTopButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(64, 0, 32, 32), Localization_GUIs.HUD_Button_EnvironmentSettings_Name, Localization_GUIs.HUD_Button_EnvironmentSettings_Description, EnvironmentSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(32, 0, 32, 32), Localization_GUIs.HUD_Button_PenSettings_Name, Localization_GUIs.HUD_Button_PenSettings_Description, PenSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(160, 0, 32, 32), Localization_GUIs.HUD_Button_WorldSettings_Name, Localization_GUIs.HUD_Button_WorldSettings_Description, WorldSettingsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(128, 64, 32, 32), Localization_GUIs.HUD_Button_Information_Name, Localization_GUIs.HUD_Button_Information_Description, InfoButtonAction),
            ];

            this.leftPanelBottomButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), this.pauseAndResumeRectangles[0], Localization_GUIs.HUD_Button_PauseSimulation_Name, Localization_GUIs.HUD_Button_PauseSimulation_Description, PauseSimulationButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), this.speedIconRectangles[0], Localization_GUIs.HUD_Button_Speed_Name, Localization_GUIs.HUD_Button_Speed_Description, ChangeSimulationSpeedButtonAction),
            ];

            this.rightPanelTopButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(32, 192, 32, 32), Localization_GUIs.HUD_Button_GameMenu_Name, Localization_GUIs.HUD_Button_GameMenu_Description, GameMenuButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(64, 192, 32, 32), Localization_GUIs.HUD_Button_SaveMenu_Name, Localization_GUIs.HUD_Button_SaveMenu_Description, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(224, 96, 32, 32), Localization_GUIs.HUD_Button_EraseEverything_Name, Localization_GUIs.HUD_Button_EraseEverything_Description, EraseEverythingButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(160, 192, 32, 32), Localization_GUIs.HUD_Button_ReloadSimulation_Name, Localization_GUIs.HUD_Button_ReloadSimulation_Description, ReloadSimulationButtonAction),
            ];

            this.leftPanelTopButtonElements = new SlotInfo[this.leftPanelTopButtons.Length];
            this.leftPanelBottomButtonElements = new SlotInfo[this.leftPanelBottomButtons.Length];
            this.rightPanelTopButtonElements = new SlotInfo[this.rightPanelTopButtons.Length];
            this.rightPanelBottomButtonElements = new SlotInfo[this.rightPanelBottomButtons.Length];
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
            BuildDrawerButtons(root);
            BuildToolbars(root);

            root.AddChild(this.tooltipBoxElement);
        }

        private void BuildToolbars(Container root)
        {
            this.topToolbarContainerElement = new()
            {
                Size = new(ScreenConstants.SCREEN_WIDTH, 96),
            };

            this.leftToolbarContainerElement = new()
            {
                Size = new(96, 608),
                Alignment = CardinalDirection.Northwest,
                Margin = new(0, 112),
            };

            this.rightToolbarContainerElement = new()
            {
                Size = new(96, 608),
                Margin = new(-16, 112),
                Alignment = CardinalDirection.Northeast,
            };

            root.AddChild(this.topToolbarContainerElement);
            root.AddChild(this.leftToolbarContainerElement);
            root.AddChild(this.rightToolbarContainerElement);

            BuildTopToolbar(this.topToolbarContainerElement);
            BuildLeftToolbar(this.leftToolbarContainerElement);
            BuildRightToolbar(this.rightToolbarContainerElement);
        }

        #region Top Toolbar

        private void BuildTopToolbar(Container container)
        {
            this.topToolbarBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudHorizontalToolbar),
                SourceRectangle = new(new(0, 0), new(ScreenConstants.SCREEN_WIDTH, 96)),
                Size = new(ScreenConstants.SCREEN_WIDTH, 96),
            };

            container.AddChild(this.topToolbarBackgroundElement);

            CreateTopToolbatCurrentlySelectedToolSlot();
            CreateTopToolbarSlots();
            CreateTopToolbarSearchSlot();
        }

        private void CreateTopToolbatCurrentlySelectedToolSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(0, 0, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.West,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE * 2, 0),
            };

            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                SourceRectangle = new(64, 32, 32, 32),
                Scale = new(2f),
                Size = new(1),
            };

            this.topToolbarBackgroundElement.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);

            this.toolbarCurrentlySelectedToolIconElement = slotIcon;
        }

        private void CreateTopToolbarSlots()
        {
            Vector2 margin = new(UIConstants.HUD_SLOT_SPACING * 2.5f, 0);

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
                this.topToolbarBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.toolbarElementSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void CreateTopToolbarSearchSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(0, 0, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.East,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE * 2 * -1, 0),
            };

            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                SourceRectangle = new(0, 0, 32, 32),
                Scale = new(2f),
                Size = new(1),
            };

            this.topToolbarBackgroundElement.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);

            this.toolbarElementSearchButtonElement = slotSearchBackground;
        }

        #endregion

        private void BuildLeftToolbar(Container container)
        {
            this.leftToolbarBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudVerticalToolbar),
                SourceRectangle = new(new(0, 0), new(96, 608)),
                Size = new(96, 608),
                Alignment = CardinalDirection.Northwest,
                Margin = new(0, 112),
            };

            container.AddChild(this.leftToolbarBackgroundElement);

            #region BUTTONS

            Vector2 margin = new(0, UIConstants.HUD_SLOT_SPACING);

            // Top
            for (int i = 0; i < this.leftPanelTopButtons.Length; i++)
            {
                ButtonInfo button = this.leftPanelTopButtons[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.North;

                this.leftToolbarBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.leftPanelTopButtonElements[i] = slot;

                margin.Y += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
                container.AddChild(slot.Icon);
            }

            margin = new(0, -UIConstants.HUD_SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.leftPanelBottomButtons.Length; i++)
            {
                ButtonInfo button = this.leftPanelBottomButtons[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                this.leftToolbarBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.leftPanelBottomButtonElements[i] = slot;

                margin.Y -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
                container.AddChild(slot.Icon);
            }

            #endregion
        }

        private void BuildRightToolbar(Container container)
        {
            this.rightToolbarBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundHudVerticalToolbar),
                SourceRectangle = new(new(96, 0), new(96, 608)),
                Size = new(96, 608),
                Margin = new(96, 112),
                Alignment = CardinalDirection.Northeast,
            };

            container.AddChild(this.rightToolbarBackgroundElement);

            #region BUTTONS

            Vector2 margin = new(-192, UIConstants.HUD_SLOT_SPACING);

            // Top
            for (int i = 0; i < this.rightPanelTopButtons.Length; i++)
            {
                ButtonInfo button = this.rightPanelTopButtons[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.North;

                this.rightToolbarBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.rightPanelTopButtonElements[i] = slot;

                margin.Y += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
                container.AddChild(slot.Icon);
            }

            margin = new(margin.X, -UIConstants.HUD_SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.rightPanelBottomButtons.Length; i++)
            {
                ButtonInfo button = this.rightPanelBottomButtons[i];

                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                this.rightToolbarBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.rightPanelBottomButtonElements[i] = slot;

                margin.Y -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                container.AddChild(slot.Background);
                container.AddChild(slot.Icon);
            }

            #endregion
        }

        // ============================================================= //

        private void BuildDrawerButtons(Container root)
        {
            BuildTopDrawerButton(root);
            BuildLeftDrawerButton(root);
            BuildRightDrawerButton(root);
        }

        private void BuildTopDrawerButton(Container root)
        {
            this.topDrawerButtonElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(163, 220, 80, 24),
                Size = new(80, 24),
                Scale = new(2f),
                Alignment = CardinalDirection.North,
            };

            root.AddChild(this.topDrawerButtonElement);
        }

        private void BuildLeftDrawerButton(Container root)
        {
            this.leftDrawerButtonElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(243, 220, 24, 80),
                Size = new(24, 80),
                Scale = new(2f),
                Alignment = CardinalDirection.West,
            };

            root.AddChild(this.leftDrawerButtonElement);
        }

        private void BuildRightDrawerButton(Container root)
        {
            this.rightDrawerButtonElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                SourceRectangle = new(267, 220, 24, 80),
                Size = new(24, 80),
                Scale = new(2f),
                Alignment = CardinalDirection.East,
            };

            root.AddChild(this.rightDrawerButtonElement);
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
                Size = new(UIConstants.HUD_GRID_SIZE)
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

            this.tooltipBoxElement.CanDraw = false;

            SetPlayerInteractionWhenToolbarHovered();
            UpdateSlotIcons();
            UpdateToolbars();
            UpdateDrawerButtons();

            this.tooltipBoxElement.RefreshDisplay(TooltipBoxContent.Title, TooltipBoxContent.Description);
        }

        private void UpdateSlotIcons()
        {
            // Pause
            this.leftPanelBottomButtonElements[0].Icon.SourceRectangle = this.gameManager.HasState(GameStates.IsSimulationPaused) ? this.pauseAndResumeRectangles[1] : this.pauseAndResumeRectangles[0];

            // Speed
            this.leftPanelBottomButtonElements[1].Icon.SourceRectangle = this.world.Simulation.CurrentSpeed switch
            {
                SimulationSpeed.Normal => this.speedIconRectangles[(byte)SimulationSpeed.Normal],
                SimulationSpeed.Fast => this.speedIconRectangles[(byte)SimulationSpeed.Fast],
                SimulationSpeed.VeryFast => this.speedIconRectangles[(byte)SimulationSpeed.VeryFast],
                _ => this.speedIconRectangles[(byte)SimulationSpeed.Normal],
            };
        }

        private void UpdateToolbars()
        {
            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();
        }

        private void UpdateTopToolbar()
        {
            if (!this.IsTopToolbarVisible)
            {
                return;
            }

            UpdateTopToolbarToolPreview();
            UpdateTopToolbarItemButtons();
            UpdateTopToolbarSearchButton();
        }

        private void UpdateTopToolbarToolPreview()
        {
            if (Interaction.OnMouseOver(this.toolbarCurrentlySelectedToolIconElement.Position, new(UIConstants.HUD_GRID_SIZE)))
            {
                this.tooltipBoxElement.CanDraw = true;

                switch (this.inputController.Pen.Tool)
                {
                    case PenTool.Visualization:
                        TooltipBoxContent.Title = Localization_WorldGizmos.Visualization_Name;
                        TooltipBoxContent.Description = Localization_WorldGizmos.Visualization_Description;
                        break;

                    case PenTool.Pencil:
                        TooltipBoxContent.Title = Localization_WorldGizmos.Pencil_Name;
                        TooltipBoxContent.Description = Localization_WorldGizmos.Pencil_Description;
                        break;

                    case PenTool.Eraser:
                        TooltipBoxContent.Title = Localization_WorldGizmos.Eraser_Name;
                        TooltipBoxContent.Description = Localization_WorldGizmos.Eraser_Description;
                        break;

                    case PenTool.Fill:
                        TooltipBoxContent.Title = Localization_WorldGizmos.Fill_Name;
                        TooltipBoxContent.Description = Localization_WorldGizmos.Fill_Description;
                        break;

                    case PenTool.Replace:
                        TooltipBoxContent.Title = Localization_WorldGizmos.Replace_Name;
                        TooltipBoxContent.Description = Localization_WorldGizmos.Replace_Description;
                        break;

                    default:
                        TooltipBoxContent.Title = Localization_Statements.Unknown;
                        TooltipBoxContent.Description = string.Empty;
                        break;
                }
            }
        }

        private void UpdateTopToolbarItemButtons()
        {
            for (byte i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                SlotInfo slot = this.toolbarElementSlots[i];
                bool isOver = Interaction.OnMouseOver(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE));

                if (Interaction.OnMouseClick(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    SelectItemSlot(i, (Item)slot.Background.GetData(UIConstants.DATA_ITEM));
                }

                if (isOver)
                {
                    this.tooltipBoxElement.CanDraw = true;

                    Item item = (Item)slot.Background.GetData(UIConstants.DATA_ITEM);

                    TooltipBoxContent.Title = item.Name;
                    TooltipBoxContent.Description = item.Description;
                }

                slot.Background.Color = this.slotSelectedIndex == i ?
                                        AAP64ColorPalette.OrangeRed :
                                        isOver ? AAP64ColorPalette.EmeraldGreen : AAP64ColorPalette.White;
            }
        }

        private void UpdateTopToolbarSearchButton()
        {
            if (Interaction.OnMouseClick(this.toolbarElementSearchButtonElement.Position, new(UIConstants.HUD_GRID_SIZE)))
            {
                this.uiManager.OpenGUI(UIIndex.ItemExplorer);
            }

            if (Interaction.OnMouseOver(this.toolbarElementSearchButtonElement.Position, new(UIConstants.HUD_GRID_SIZE)))
            {
                this.toolbarElementSearchButtonElement.Color = AAP64ColorPalette.Graphite;
                this.tooltipBoxElement.CanDraw = true;

                TooltipBoxContent.Title = Localization_GUIs.HUD_Button_ItemExplorer_Name;
                TooltipBoxContent.Description = Localization_GUIs.HUD_Button_ItemExplorer_Description;
            }
            else
            {
                this.toolbarElementSearchButtonElement.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateLeftToolbar()
        {
            if (!this.IsLeftToolbarVisible)
            {
                return;
            }

            CycleThroughArrayOfButtons(this.leftPanelTopButtonElements, this.leftPanelTopButtons, this.leftPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.leftPanelBottomButtonElements, this.leftPanelBottomButtons, this.leftPanelBottomButtonElements.Length);
        }

        private void UpdateRightToolbar()
        {
            if (!this.IsRightToolbarVisible)
            {
                return;
            }

            CycleThroughArrayOfButtons(this.rightPanelTopButtonElements, this.rightPanelTopButtons, this.rightPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.rightPanelBottomButtonElements, this.rightPanelBottomButtons, this.rightPanelBottomButtonElements.Length);
        }

        private void CycleThroughArrayOfButtons(SlotInfo[] toolbarSlots, ButtonInfo[] buttons, int length)
        {
            for (int i = 0; i < length; i++)
            {
                SlotInfo toolbarSlot = toolbarSlots[i];
                ButtonInfo button = buttons[i];

                bool isOver = Interaction.OnMouseOver(toolbarSlot.Background.Position, new(UIConstants.HUD_GRID_SIZE));

                if (Interaction.OnMouseClick(toolbarSlot.Background.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    button.ClickAction?.Invoke();
                }

                if (isOver)
                {
                    toolbarSlot.Background.Color = AAP64ColorPalette.EmeraldGreen;
                    this.tooltipBoxElement.CanDraw = true;

                    TooltipBoxContent.Title = button.Name;
                    TooltipBoxContent.Description = button.Description;
                }
                else
                {
                    toolbarSlot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void SetPlayerInteractionWhenToolbarHovered()
        {
            Vector2 topDrawerButtonSize = this.topDrawerButtonElement.Size / 2;
            Vector2 leftDrawerButtonSize = this.leftDrawerButtonElement.Size / 2;
            Vector2 rightDrawerButtonSize = this.rightDrawerButtonElement.Size / 2;

            Vector2 topDrawerButtonPosition = this.topDrawerButtonElement.Position + new Vector2(0, -24f);
            Vector2 leftDrawerButtonPosition = this.leftDrawerButtonElement.Position + new Vector2(-24f, 0);
            Vector2 rightDrawerButtonPosition = this.rightDrawerButtonElement.Position + new Vector2(-32f, 0);

            this.inputController.Player.CanModifyEnvironment = (!this.IsTopToolbarVisible || !Interaction.OnMouseOver(this.topToolbarContainerElement.Position, this.topToolbarContainerElement.Size)) &&
                (!this.IsLeftToolbarVisible || !Interaction.OnMouseOver(this.leftToolbarContainerElement.Position, this.leftToolbarContainerElement.Size)) &&
                (!this.IsRightToolbarVisible || !Interaction.OnMouseOver(this.rightToolbarContainerElement.Position, this.rightToolbarContainerElement.Size)) &&
                !Interaction.OnMouseOver(topDrawerButtonPosition, topDrawerButtonSize) &&
                !Interaction.OnMouseOver(leftDrawerButtonPosition, leftDrawerButtonSize) &&
                !Interaction.OnMouseOver(rightDrawerButtonPosition, rightDrawerButtonSize);
        }

        private void UpdateDrawerButtons()
        {
            UpdateDrawerButton(this.topDrawerButtonElement, this.topToolbarContainerElement, new(0, -24f), isVisible => new(0, isVisible ? 128 : 16));
            UpdateDrawerButton(this.leftDrawerButtonElement, this.leftToolbarContainerElement, new(-24f, 0), isVisible => new(isVisible ? 128 : 16, 0));
            UpdateDrawerButton(this.rightDrawerButtonElement, this.rightToolbarContainerElement, new(-32f, 0), isVisible => new(isVisible ? -80 : 32, 0));
        }

        private static void UpdateDrawerButton(Image drawerButtonElement, UIElement toolbarContainerElement, Vector2 positionOffset, Func<bool, Vector2> marginCalculator)
        {
            Vector2 size = drawerButtonElement.Size / 2;
            Vector2 position = drawerButtonElement.Position + positionOffset;

            if (Interaction.OnMouseClick(position, size))
            {
                toolbarContainerElement.CanDraw = !toolbarContainerElement.CanDraw;
            }

            drawerButtonElement.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            drawerButtonElement.Margin = marginCalculator(toolbarContainerElement.CanDraw);
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
                SlotInfo slot = this.toolbarElementSlots[i];

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
                SlotInfo currentSlot = this.toolbarElementSlots[i];
                SlotInfo nextSlot = this.toolbarElementSlots[i + 1];

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
            SlotInfo lastSlot = this.toolbarElementSlots[^1];

            lastSlot.Background.UpdateData(UIConstants.DATA_ITEM, item);
            lastSlot.Icon.Texture = item.IconTexture;

            SelectItemSlot(Convert.ToByte(this.toolbarElementSlots.Length - 1), item);
        }

        internal bool ItemIsEquipped(Item item)
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                Item hudItem = (Item)this.toolbarElementSlots[i].Background.GetData(UIConstants.DATA_ITEM);

                if (item == hudItem)
                {
                    return true;
                }
            }

            return false;
        }

        internal void SetToolIcon(Rectangle iconRectangle)
        {
            this.toolbarCurrentlySelectedToolIconElement.SourceRectangle = iconRectangle;
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
