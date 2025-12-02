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
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class HudUI : UIBase
    {
        private byte slotSelectedIndex = 0;

        private Container topToolbarContainer, leftToolbarContainer, rightToolbarContainer;
        private Image topToolbarBackground, leftToolbarBackground, rightToolbarBackground;
        private Image topDrawerButton, leftDrawerButton, rightDrawerButton;
        private Image toolbarSearchButton, toolbarCurrentlySelectedToolBackground, toolbarCurrentlySelectedToolIcon;
        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] toolbarSlots = new SlotInfo[UIConstants.HUD_ELEMENT_BUTTONS_LENGTH];
        private readonly SlotInfo[] leftPanelTopButtons, leftPanelBottomButtons, rightPanelTopButtons, rightPanelBottomButtons;
        private readonly ButtonInfo[] leftPanelTopButtonInfos, leftPanelBottomButtonInfos, rightPanelTopButtonInfos, rightPanelBottomButtonInfos;

        private readonly ConfirmSettings reloadSimulationConfirmSettings, eraseEverythingConfirmSettings;
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

            SelectItemSlot(0, 0, 0, 0);

            this.leftPanelTopButtonInfos = [
                new(TextureIndex.IconUI, new(64, 0, 32, 32), Localization_GUIs.HUD_Button_EnvironmentSettings_Name, Localization_GUIs.HUD_Button_EnvironmentSettings_Description, () => this.uiManager.OpenGUI(UIIndex.EnvironmentSettings)),
                new(TextureIndex.IconUI, new(32, 0, 32, 32), Localization_GUIs.HUD_Button_PenSettings_Name, Localization_GUIs.HUD_Button_PenSettings_Description, () => this.uiManager.OpenGUI(UIIndex.PenSettings)),
                new(TextureIndex.IconUI, new(160, 0, 32, 32), Localization_GUIs.HUD_Button_WorldSettings_Name, Localization_GUIs.HUD_Button_WorldSettings_Description, () => this.uiManager.OpenGUI(UIIndex.WorldSettings)),
                new(TextureIndex.IconUI, new(128, 64, 32, 32), Localization_GUIs.HUD_Button_Information_Name, Localization_GUIs.HUD_Button_Information_Description, () => this.uiManager.OpenGUI(UIIndex.Information)),
            ];

            this.leftPanelBottomButtonInfos = [
                new(TextureIndex.IconUI, this.pauseAndResumeRectangles[0], Localization_GUIs.HUD_Button_PauseSimulation_Name, Localization_GUIs.HUD_Button_PauseSimulation_Description, () => this.gameManager.ToggleState(GameStates.IsSimulationPaused)),
                new(TextureIndex.IconUI, this.speedIconRectangles[0], Localization_GUIs.HUD_Button_Speed_Name, Localization_GUIs.HUD_Button_Speed_Description, () =>
                {
                    this.gameManager.SetSimulationSpeed(
                        this.world.Simulation.CurrentSpeed == SimulationSpeed.Normal ? SimulationSpeed.Fast :
                        this.world.Simulation.CurrentSpeed == SimulationSpeed.Fast ? SimulationSpeed.VeryFast :
                        SimulationSpeed.Normal
                    );
                }),
            ];

            this.rightPanelTopButtonInfos = [
                new(TextureIndex.IconUI, new(32, 192, 32, 32), Localization_GUIs.HUD_Button_GameMenu_Name, Localization_GUIs.HUD_Button_GameMenu_Description, () => this.uiManager.OpenGUI(UIIndex.Pause)),
                new(TextureIndex.IconUI, new(64, 192, 32, 32), Localization_GUIs.HUD_Button_SaveMenu_Name, Localization_GUIs.HUD_Button_SaveMenu_Description, () => this.uiManager.OpenGUI(UIIndex.SaveSettings)),
            ];

            this.rightPanelBottomButtonInfos = [
                new(TextureIndex.IconUI, new(224, 96, 32, 32), Localization_GUIs.HUD_Button_EraseEverything_Name, Localization_GUIs.HUD_Button_EraseEverything_Description, () =>
                {
                    this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
                    this.guiConfirm.Configure(this.eraseEverythingConfirmSettings);
                    this.uiManager.OpenGUI(this.guiConfirm.Index);
                }),
                new(TextureIndex.IconUI, new(160, 192, 32, 32), Localization_GUIs.HUD_Button_ReloadSimulation_Name, Localization_GUIs.HUD_Button_ReloadSimulation_Description, () =>
                {
                    this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
                    this.guiConfirm.Configure(this.reloadSimulationConfirmSettings);
                    this.uiManager.OpenGUI(this.guiConfirm.Index);
                }),
            ];

            this.leftPanelTopButtons = new SlotInfo[this.leftPanelTopButtonInfos.Length];
            this.leftPanelBottomButtons = new SlotInfo[this.leftPanelBottomButtonInfos.Length];
            this.rightPanelTopButtons = new SlotInfo[this.rightPanelTopButtonInfos.Length];
            this.rightPanelBottomButtons = new SlotInfo[this.rightPanelBottomButtonInfos.Length];
        }

        #region BUILD

        protected override void OnBuild(Container root)
        {
            BuildToolbar(ref this.topToolbarContainer, ref this.topToolbarBackground, root, new Vector2(ScreenConstants.SCREEN_WIDTH, 96), TextureIndex.None, new(0, 0, 1280, 96), CardinalDirection.Northwest, BuildTopToolbarContent);
            BuildToolbar(ref this.leftToolbarContainer, ref this.leftToolbarBackground, root, new(96, 608), TextureIndex.UIBackgroundHudVerticalToolbar, new(0, 0, 96, 608), CardinalDirection.Southwest, c => BuildPanelToolbarContent(c, this.leftPanelTopButtonInfos, this.leftPanelTopButtons, CardinalDirection.North, true));
            BuildToolbar(ref this.rightToolbarContainer, ref this.rightToolbarBackground, root, new(96, 608), TextureIndex.UIBackgroundHudVerticalToolbar, new(96, 0, 96, 608), CardinalDirection.Southeast, c => BuildPanelToolbarContent(c, this.rightPanelTopButtonInfos, this.rightPanelTopButtons, CardinalDirection.North, true));
            BuildPanelToolbarContent(this.leftToolbarContainer, this.leftPanelBottomButtonInfos, this.leftPanelBottomButtons, CardinalDirection.South, false);
            BuildPanelToolbarContent(this.rightToolbarContainer, this.rightPanelBottomButtonInfos, this.rightPanelBottomButtons, CardinalDirection.South, false);
            BuildDrawerButton(ref this.topDrawerButton, this.topToolbarContainer, new(163, 220, 80, 24), new(80, 24), new(0, 48f), CardinalDirection.South);
            BuildDrawerButton(ref this.leftDrawerButton, this.leftToolbarContainer, new(243, 220, 24, 80), new(24, 80), new(48f, 0f), CardinalDirection.East);
            BuildDrawerButton(ref this.rightDrawerButton, this.rightToolbarContainer, new(267, 220, 24, 80), new(24, 80), new(-48f, 0f), CardinalDirection.West);
            root.AddChild(this.tooltipBox);
        }

        private static void BuildToolbar(ref Container container, ref Image background, Container root, Vector2 size, TextureIndex bgTexture, Rectangle? sourceRectangle, CardinalDirection alignment, Action<Container> buildContent)
        {
            container = new() { Size = size, Alignment = alignment };
            root.AddChild(container);

            background = new()
            {
                Texture = bgTexture == TextureIndex.None ? AssetDatabase.GetTexture(TextureIndex.UIBackgroundHudHorizontalToolbar) : AssetDatabase.GetTexture(bgTexture),
                SourceRectangle = sourceRectangle,
                Size = size,
            };

            container.AddChild(background);
            buildContent?.Invoke(container);
        }

        private void BuildTopToolbarContent(Container container)
        {
            CreateTopToolbarCurrentlySelectedToolSlot();
            CreateTopToolbarSlots();
            CreateTopToolbarSearchSlot();
        }

        private static void BuildPanelToolbarContent(Container container, ButtonInfo[] buttonInfos, SlotInfo[] slots, CardinalDirection alignment, bool isTop)
        {
            Vector2 margin = isTop ? new(0, UIConstants.HUD_SLOT_SPACING / 2f) : new(0, -(UIConstants.HUD_SLOT_SPACING / 2f));
            int direction = isTop ? 1 : -1;

            for (int i = 0; i < buttonInfos.Length; i++)
            {
                SlotInfo slot = CreateButtonSlot(margin, buttonInfos[i]);
                slot.Background.Alignment = alignment;
                container.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
                slots[i] = slot;
                margin.Y += direction * (UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2));
            }
        }

        private static void BuildDrawerButton(ref Image drawerButton, Container container, Rectangle srcRect, Vector2 size, Vector2 margin, CardinalDirection alignment)
        {
            drawerButton = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = srcRect,
                Size = size,
                Margin = margin,
                Scale = new(2f),
                Alignment = alignment,
            };

            container.AddChild(drawerButton);
        }

        private void CreateTopToolbarCurrentlySelectedToolSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.West,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE, 0),
            };
            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(64, 32, 32, 32),
                Alignment = CardinalDirection.Center,
                Scale = new(2f),
                Size = new(32f),
            };
            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);

            this.toolbarCurrentlySelectedToolBackground = slotSearchBackground;
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

                this.topToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
                this.toolbarSlots[i] = slot;
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void CreateTopToolbarSearchSlot()
        {
            Image slotSearchBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                Alignment = CardinalDirection.East,
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = new(UIConstants.HUD_GRID_SIZE * -1, 0),
            };
            Image slotIcon = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(0, 0, 32, 32),
                Alignment = CardinalDirection.Center,
                Scale = new(2f),
                Size = new(32f),
            };
            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(slotIcon);
            this.toolbarSearchButton = slotSearchBackground;
        }

        private static SlotInfo CreateButtonSlot(Vector2 margin, Texture2D iconTexture, Rectangle? iconTextureRectangle)
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

        #region UPDATE

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.tooltipBox.CanDraw = false;

            UpdatePlayerInteractionOnToolbarHover();
            UpdateSimulationControlIcons();

            UpdateTopToolbarToolPreview();
            UpdateTopToolbarItemButtons();
            UpdateTopToolbarSearchButton();

            // UpdateLeftToolbar();
            // UpdateRightToolbar();

            // UpdateToolbars();
            // UpdateDrawerButtons();

            this.tooltipBox.RefreshDisplay();
        }

        private void UpdatePlayerInteractionOnToolbarHover()
        {
            bool isMouseOverDrawerButtons =
                Interaction.OnMouseLeftOver(this.topDrawerButton.Position, this.topDrawerButton.Size) ||
                Interaction.OnMouseLeftOver(this.leftDrawerButton.Position, this.leftDrawerButton.Size) ||
                Interaction.OnMouseLeftOver(this.rightDrawerButton.Position, this.rightDrawerButton.Size);

            bool isMouseOverToolbars =
                Interaction.OnMouseLeftOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size) ||
                Interaction.OnMouseLeftOver(this.leftToolbarContainer.Position, this.leftToolbarContainer.Size) ||
                Interaction.OnMouseLeftOver(this.rightToolbarContainer.Position, this.rightToolbarContainer.Size);

            this.inputController.Player.CanModifyEnvironment = !isMouseOverDrawerButtons && !isMouseOverToolbars;
        }

        private void UpdateSimulationControlIcons()
        {
            this.leftPanelBottomButtons[0].Icon.SourceRectangle = this.gameManager.HasState(GameStates.IsSimulationPaused)
                ? this.pauseAndResumeRectangles[1] // Resume
                : this.pauseAndResumeRectangles[0]; // Pause

            this.leftPanelBottomButtons[1].Icon.SourceRectangle = this.world.Simulation.CurrentSpeed switch
            {
                SimulationSpeed.Fast => this.speedIconRectangles[1],
                SimulationSpeed.VeryFast => this.speedIconRectangles[2],
                _ => this.speedIconRectangles[0],
            };

            this.toolbarCurrentlySelectedToolIcon.SourceRectangle = this.inputController.Pen.Tool switch
            {
                PenTool.Visualization => new(96, 64, 32, 32),
                PenTool.Pencil => new(64, 32, 32, 32),
                PenTool.Eraser => new(96, 32, 32, 32),
                PenTool.Fill => new(128, 32, 32, 32),
                PenTool.Replace => new(160, 32, 32, 32),
                _ => new(192, 0, 32, 32),
            };
        }

        private void UpdateTopToolbarToolPreview()
        {
            if (Interaction.OnMouseLeftClick(
                this.toolbarCurrentlySelectedToolIcon.Position,
                this.toolbarCurrentlySelectedToolIcon.Size
            ))
            {
                this.inputController.Pen.Tool = this.inputController.Pen.Tool switch
                {
                    PenTool.Visualization => PenTool.Pencil,
                    PenTool.Pencil => PenTool.Eraser,
                    PenTool.Eraser => PenTool.Fill,
                    PenTool.Fill => PenTool.Replace,
                    PenTool.Replace => PenTool.Visualization,
                    _ => PenTool.Visualization,
                };
            }

            if (Interaction.OnMouseRightClick(
                this.toolbarCurrentlySelectedToolIcon.Position,
                this.toolbarCurrentlySelectedToolIcon.Size
            ))
            {
                this.inputController.Pen.Tool = this.inputController.Pen.Tool switch
                {
                    PenTool.Visualization => PenTool.Replace,
                    PenTool.Pencil => PenTool.Visualization,
                    PenTool.Eraser => PenTool.Pencil,
                    PenTool.Fill => PenTool.Eraser,
                    PenTool.Replace => PenTool.Fill,
                    _ => PenTool.Visualization,
                };
            }

            if (Interaction.OnMouseLeftOver(
                this.toolbarCurrentlySelectedToolIcon.Position,
                this.toolbarCurrentlySelectedToolIcon.Size
            ))
            {
                this.tooltipBox.CanDraw = true;

                switch (this.inputController.Pen.Tool)
                {
                    case PenTool.Visualization:
                        TooltipBoxContent.SetTitle(Localization_WorldGizmos.Visualization_Name);
                        TooltipBoxContent.SetDescription(Localization_WorldGizmos.Visualization_Description);
                        break;

                    case PenTool.Pencil:
                        TooltipBoxContent.SetTitle(Localization_WorldGizmos.Pencil_Name);
                        TooltipBoxContent.SetDescription(Localization_WorldGizmos.Pencil_Description);
                        break;

                    case PenTool.Eraser:
                        TooltipBoxContent.SetTitle(Localization_WorldGizmos.Eraser_Name);
                        TooltipBoxContent.SetDescription(Localization_WorldGizmos.Eraser_Description);
                        break;

                    case PenTool.Fill:
                        TooltipBoxContent.SetTitle(Localization_WorldGizmos.Fill_Name);
                        TooltipBoxContent.SetDescription(Localization_WorldGizmos.Fill_Description);
                        break;

                    case PenTool.Replace:
                        TooltipBoxContent.SetTitle(Localization_WorldGizmos.Replace_Name);
                        TooltipBoxContent.SetDescription(Localization_WorldGizmos.Replace_Description);
                        break;

                    default:
                        TooltipBoxContent.SetTitle(Localization_Statements.Unknown);
                        TooltipBoxContent.SetDescription(string.Empty);
                        break;
                }

                this.toolbarCurrentlySelectedToolBackground.Scale = Vector2.Lerp(
                    this.toolbarCurrentlySelectedToolBackground.Scale,
                    new(UIConstants.HUD_SLOT_SCALE + 0.45f + 0.2f),
                    0.2f
                );

                this.toolbarCurrentlySelectedToolBackground.Color = AAP64ColorPalette.Graphite;
            }
            else
            {
                this.toolbarCurrentlySelectedToolBackground.Scale = Vector2.Lerp(
                    this.toolbarCurrentlySelectedToolBackground.Scale,
                    new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                    0.2f
                );

                this.toolbarCurrentlySelectedToolBackground.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateTopToolbarItemButtons()
        {
            for (byte i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                SlotInfo slot = this.toolbarSlots[i];
                bool isOver = Interaction.OnMouseLeftOver(slot.Background.Position, slot.Background.Size);

                if (Interaction.OnMouseLeftClick(slot.Background.Position, slot.Background.Size))
                {
                    SelectItemSlot(i, (Item)slot.Background.GetData(UIConstants.DATA_ITEM));
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    slot.Background.Scale = Vector2.Lerp(
                        slot.Background.Scale,
                        new(UIConstants.HUD_SLOT_SCALE + 0.2f),
                        0.2f
                    );

                    Item item = (Item)slot.Background.GetData(UIConstants.DATA_ITEM);

                    TooltipBoxContent.SetTitle(item.Name);
                    TooltipBoxContent.SetDescription(item.Description);
                }
                else
                {
                    slot.Background.Scale = Vector2.Lerp(
                        slot.Background.Scale,
                        new(UIConstants.HUD_SLOT_SCALE),
                        0.2f
                    );
                }

                slot.Background.Color = this.slotSelectedIndex == i
                    ? AAP64ColorPalette.OrangeRed
                    : isOver
                        ? AAP64ColorPalette.EmeraldGreen
                        : AAP64ColorPalette.White;
            }
        }

        private void UpdateTopToolbarSearchButton()
        {
            if (Interaction.OnMouseLeftClick(this.toolbarSearchButton.Position, this.toolbarSearchButton.Size))
            {
                this.uiManager.OpenGUI(UIIndex.ItemExplorer);
            }

            if (Interaction.OnMouseLeftOver(this.toolbarSearchButton.Position, this.toolbarSearchButton.Size))
            {
                this.toolbarSearchButton.Color = AAP64ColorPalette.Graphite;
                this.tooltipBox.CanDraw = true;

                this.toolbarSearchButton.Scale = Vector2.Lerp(
                    this.toolbarSearchButton.Scale,
                    new(UIConstants.HUD_SLOT_SCALE + 0.45f + 0.2f),
                    0.2f
                );

                TooltipBoxContent.SetTitle(Localization_GUIs.HUD_Button_ItemExplorer_Name);
                TooltipBoxContent.SetDescription(Localization_GUIs.HUD_Button_ItemExplorer_Description);
            }
            else
            {
                this.toolbarSearchButton.Scale = Vector2.Lerp(
                    this.toolbarSearchButton.Scale,
                    new(UIConstants.HUD_SLOT_SCALE + 0.45f),
                    0.2f
                );

                this.toolbarSearchButton.Color = AAP64ColorPalette.White;
            }
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
