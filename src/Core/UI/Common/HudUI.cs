/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Enums.UI.Tools;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class HudUI : UIBase
    {
        private int slotSelectedIndex = 0;
        private bool isTopToolbarExpanded = true, isLeftToolbarExpanded = true, isRightToolbarExpanded = true;

        private Item[] toolbarItems = new Item[UIConstants.HUD_ELEMENT_BUTTONS_LENGTH];

        private Container topToolbarContainer, leftToolbarContainer, rightToolbarContainer;
        private Image topToolbarBackground, leftToolbarBackground, rightToolbarBackground;
        private Image topDrawerButton, leftDrawerButton, rightDrawerButton;
        private Image toolbarSearchButton, toolbarCurrentlySelectedToolBackground, toolbarCurrentlySelectedToolIcon;
        private Image simulationPausedBackground;
        private SlotInfo[] leftPanelBottomButtonSlotInfos, leftPanelTopButtonSlotInfos, rightPanelBottomButtonSlotInfos, rightPanelTopButtonSlotInfos;

        private readonly NotificationBox notificationBox;
        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] toolbarSlots = new SlotInfo[UIConstants.HUD_ELEMENT_BUTTONS_LENGTH];
        private readonly ButtonInfo[] leftPanelTopButtonInfos, leftPanelBottomButtonInfos, rightPanelTopButtonInfos, rightPanelBottomButtonInfos;

        private readonly PlayerInputController playerInputController;
        private readonly UIManager uiManager;

        private readonly SystemInformationSettings systemInformationSettings;

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
            ActorManager actorManager,
            ConfirmUI confirmUI,
            NotificationBox notificationBox,
            PlayerInputController playerInputController,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.playerInputController = playerInputController;
            this.uiManager = uiManager;
            this.notificationBox = notificationBox;
            this.tooltipBox = tooltipBox;

            this.systemInformationSettings = SettingsSerializer.Load<SystemInformationSettings>();

            this.leftPanelTopButtonInfos = [
                new(TextureIndex.IconUI, new(64, 0, 32, 32), Localization_GUIs.HUD_EnvironmentSettings_Name, Localization_GUIs.HUD_EnvironmentSettings_Description, () => this.uiManager.OpenUI(UIIndex.EnvironmentSettings)),
                new(TextureIndex.IconUI, new(32, 0, 32, 32), Localization_GUIs.HUD_PenSettings_Name, Localization_GUIs.HUD_PenSettings_Description, () => this.uiManager.OpenUI(UIIndex.PenSettings)),
                new(TextureIndex.IconUI, new(160, 0, 32, 32), Localization_GUIs.HUD_WorldSettings_Name, Localization_GUIs.HUD_WorldSettings_Description, () => this.uiManager.OpenUI(UIIndex.WorldSettings)),
                new(TextureIndex.IconUI, new(128, 64, 32, 32), Localization_GUIs.HUD_Information_Name, Localization_GUIs.HUD_Information_Description, () => this.uiManager.OpenUI(UIIndex.Information)),
            ];

            this.leftPanelBottomButtonInfos = [
                new(TextureIndex.IconUI, this.pauseAndResumeRectangles[0], Localization_GUIs.HUD_PauseSimulation_Name, Localization_GUIs.HUD_PauseSimulation_Description, () => GameHandler.ToggleState(GameStates.IsSimulationPaused)),
                new(TextureIndex.IconUI, this.speedIconRectangles[0], Localization_GUIs.HUD_Speed_Name, Localization_GUIs.HUD_Speed_Description, () =>
                {
                    SimulationSpeed speed =
                        GameHandler.SimulationSpeed is SimulationSpeed.Normal ? SimulationSpeed.Fast :
                        GameHandler.SimulationSpeed is SimulationSpeed.Fast ? SimulationSpeed.VeryFast :
                        SimulationSpeed.Normal;

                    GameHandler.SetSpeed(speed, actorManager, world);
                }),
            ];

            this.rightPanelTopButtonInfos = [
                new(TextureIndex.IconUI, new(32, 192, 32, 32), Localization_GUIs.HUD_GameMenu_Name, Localization_GUIs.HUD_GameMenu_Description, () => this.uiManager.OpenUI(UIIndex.Pause)),
                new(TextureIndex.IconUI, new(64, 192, 32, 32), Localization_GUIs.HUD_SaveMenu_Name, Localization_GUIs.HUD_SaveMenu_Description, () => this.uiManager.OpenUI(UIIndex.Save)),
            ];

            this.rightPanelBottomButtonInfos = [
                new(TextureIndex.IconUI, new(224, 96, 32, 32), Localization_GUIs.HUD_EraseEverything_Name, Localization_GUIs.HUD_EraseEverything_Description, () =>
                {
                    GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                    confirmUI.Setup(
                        Localization_Messages.Confirm_Simulation_EraseEverything_Title,
                        Localization_Messages.Confirm_Simulation_EraseEverything_Description,
                        status =>
                        {
                            if (status is ConfirmStatus.Confirmed)
                            {
                                GameHandler.Reset(actorManager, world);
                            }

                            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
                        }
                    );
                    this.uiManager.OpenUI(UIIndex.Confirm);
                }),
                new(TextureIndex.IconUI, new(160, 192, 32, 32), Localization_GUIs.HUD_ReloadSimulation_Name, Localization_GUIs.HUD_ReloadSimulation_Description, () =>
                {
                    GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                    confirmUI.Setup(
                        Localization_Messages.Confirm_Simulation_Reload_Title,
                        Localization_Messages.Confirm_Simulation_Reload_Description,
                        status =>
                        {
                            if (status is ConfirmStatus.Confirmed)
                            {
                                GameHandler.ReloadSaveFile(actorManager, world);
                            }

                            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
                        }
                    );
                    this.uiManager.OpenUI(UIIndex.Confirm);
                }),
            ];
        }

        private void SelectItemSlot(int slotIndex, Item item)
        {
            this.slotSelectedIndex = slotIndex;
            this.playerInputController.Player.SelectItem(item);
        }

        private void SelectItemSlot(int slotIndex, byte categoryIndex, byte subcategoryIndex, byte itemIndex)
        {
            SelectItemSlot(slotIndex, CatalogDatabase.GetItem(categoryIndex, subcategoryIndex, itemIndex));
        }

        private void RefreshToolbarItems()
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                Item item = this.toolbarItems[i];
                SlotInfo slot = this.toolbarSlots[i];

                slot.Icon.TextureIndex = item.TextureIndex;
                slot.Icon.SourceRectangle = item.SourceRectangle;
            }
        }

        internal void Setup()
        {
            // Ensure the HUD is visible when set up
            this.topToolbarContainer.CanDraw = true;
            this.leftToolbarContainer.CanDraw = true;
            this.rightToolbarContainer.CanDraw = true;

            this.isTopToolbarExpanded = true;
            this.isLeftToolbarExpanded = true;
            this.isRightToolbarExpanded = true;

            SelectItemSlot(0, 0, 0, 0);

            this.toolbarItems = CatalogDatabase.GetItems(UIConstants.HUD_ELEMENT_BUTTONS_LENGTH);
            RefreshToolbarItems();
        }

        private void ShiftSlotsAndAddNewItem(Item item)
        {
            // Shift all items to the left, effectively removing the first one and making space for the new item at the end
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                this.toolbarItems[i] = this.toolbarItems[i + 1];
            }

            // Set the last position with the new item
            this.toolbarItems[^1] = item;

            // Refresh UI to reflect the array changes
            RefreshToolbarItems();

            // Select the newly added item
            SelectItemSlot(this.toolbarItems.Length - 1, item);
        }

        private bool TrySelectExistingItem(Item item)
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                if (this.toolbarItems[i] == item)
                {
                    SelectItemSlot(i, item);
                    return true;
                }
            }

            return false;
        }

        internal void AddItemToToolbar(Item item)
        {
            if (TrySelectExistingItem(item))
            {
                return;
            }

            ShiftSlotsAndAddNewItem(item);
        }

        internal bool ItemIsEquipped(Item item)
        {
            return Array.Exists(this.toolbarItems, x => x == item);
        }

        internal void SetToolIcon(Rectangle iconRectangle)
        {
            this.toolbarCurrentlySelectedToolIcon.SourceRectangle = iconRectangle;
        }

        protected override void OnBuild(Container root)
        {
            BuildToolbar(ref this.topToolbarContainer, ref this.topToolbarBackground, root, new(1280, 96), TextureIndex.UIBackgroundHudHorizontalToolbar, new(0, 0, 1280, 96), UIDirection.North, BuildTopToolbarContent);
            BuildToolbar(ref this.leftToolbarContainer, ref this.leftToolbarBackground, root, new(96, 608), TextureIndex.UIBackgroundHudVerticalToolbar, new(0, 0, 96, 608), UIDirection.Southwest,
                c =>
                {
                    this.leftPanelTopButtonSlotInfos = BuildPanelToolbarContent(c, this.leftPanelTopButtonInfos, UIDirection.North, true);
                }
            );
            BuildToolbar(ref this.rightToolbarContainer, ref this.rightToolbarBackground, root, new(96, 608), TextureIndex.UIBackgroundHudVerticalToolbar, new(96, 0, 96, 608), UIDirection.Southeast,
                c =>
                {
                    this.rightPanelTopButtonSlotInfos = BuildPanelToolbarContent(c, this.rightPanelTopButtonInfos, UIDirection.North, true);
                }
            );
            this.leftPanelBottomButtonSlotInfos = BuildPanelToolbarContent(this.leftToolbarContainer, this.leftPanelBottomButtonInfos, UIDirection.South, false);
            this.rightPanelBottomButtonSlotInfos = BuildPanelToolbarContent(this.rightToolbarContainer, this.rightPanelBottomButtonInfos, UIDirection.South, false);
            BuildDrawerButton(ref this.topDrawerButton, this.topToolbarContainer, new(163, 220, 80, 24), new(80, 24), new(0, 48f), UIDirection.South);
            BuildDrawerButton(ref this.leftDrawerButton, this.leftToolbarContainer, new(243, 220, 24, 80), new(24, 80), new(48f, 0f), UIDirection.East);
            BuildDrawerButton(ref this.rightDrawerButton, this.rightToolbarContainer, new(267, 220, 24, 80), new(24, 80), new(-48f, 0f), UIDirection.West);

            root.AddChild(this.tooltipBox);
            root.AddChild(this.notificationBox);

            BuildSimulationPausedOverlay(root);
        }

        private static void BuildToolbar(ref Container container, ref Image background, Container root, Vector2 size, TextureIndex textureIndex, Rectangle? sourceRectangle, UIDirection alignment, Action<Container> buildContent)
        {
            container = new() { Size = size, Alignment = alignment };
            root.AddChild(container);

            background = new()
            {
                TextureIndex = textureIndex,
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

        private static SlotInfo[] BuildPanelToolbarContent(Container container, ButtonInfo[] buttonInfos, UIDirection alignment, bool isTop)
        {
            return isTop ?
                UIBuilderUtility.BuildVerticalButtonLine(container, buttonInfos, new(0.0f, 32.0f), 80.0f, alignment) :
                UIBuilderUtility.BuildVerticalButtonLine(container, buttonInfos, new(0.0f, -32.0f), -80.0f, alignment);
        }

        private static void BuildDrawerButton(ref Image drawerButton, Container container, Rectangle srcRect, Vector2 size, Vector2 margin, UIDirection alignment)
        {
            drawerButton = new()
            {
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = srcRect,
                Size = size,
                Margin = margin,
                Scale = new(2.0f),
                Alignment = alignment,
            };

            container.AddChild(drawerButton);
        }

        private void CreateTopToolbarCurrentlySelectedToolSlot()
        {
            Image slotSearchBackground = new()
            {
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.45f),
                Alignment = UIDirection.West,
                Size = new(32.0f),
                Margin = new(32.0f, 0.0f),
            };

            Image icon = new()
            {
                TextureIndex = TextureIndex.IconUI,
                SourceRectangle = new(64, 32, 32, 32),
                Alignment = UIDirection.Center,
                Scale = new(2.0f),
                Size = new(32.0f),
            };

            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(icon);

            this.toolbarCurrentlySelectedToolBackground = slotSearchBackground;
            this.toolbarCurrentlySelectedToolIcon = icon;
        }

        private void CreateTopToolbarSlots()
        {
            float marginX = UIConstants.HUD_ELEMENT_BUTTONS_LENGTH / 2.0f * 73.85f * -1.0f;

            for (int i = 0, length = UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i < length; i++)
            {
                SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(marginX, 0.0f), TextureIndex.IconElements, new(0, 0, 32, 32));

                slot.Background.Alignment = UIDirection.Center;

                this.topToolbarBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.toolbarSlots[i] = slot;
                marginX += 80.0f;
            }
        }

        private void CreateTopToolbarSearchSlot()
        {
            Image slotSearchBackground = new()
            {
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.45f),
                Alignment = UIDirection.East,
                Size = new(32.0f),
                Margin = new(-32.0f, 0.0f),
            };

            Image icon = new()
            {
                TextureIndex = TextureIndex.IconUI,
                SourceRectangle = new(0, 0, 32, 32),
                Alignment = UIDirection.Center,
                Scale = new(2.0f),
                Size = new(32.0f),
            };

            this.topToolbarBackground.AddChild(slotSearchBackground);
            slotSearchBackground.AddChild(icon);
            this.toolbarSearchButton = slotSearchBackground;
        }

        private void BuildSimulationPausedOverlay(Container root)
        {
            this.simulationPausedBackground = new()
            {
                CanDraw = false,
                TextureIndex = TextureIndex.Pixel,
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 120),
                Alignment = UIDirection.Center,
            };

            Label pauseLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.VcrOsdMono1001,
                Color = AAP64ColorPalette.White,
                Scale = new(0.08f),
                Alignment = UIDirection.Center,
                TextContent = Localization_GUIs.HUD_SimulationPaused,
            };

            this.simulationPausedBackground.Scale = new(
                (pauseLabel.Size.X / 2.0f) + 225.0f,
                (pauseLabel.Size.Y / 2.0f) + 70.0f
            );

            this.simulationPausedBackground.AddChild(pauseLabel);
            root.AddChild(this.simulationPausedBackground);
        }

        protected override void OnScreenResize(Vector2 newSize)
        {
            this.notificationBox.OnScreenResize(newSize);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            // Toggle HUD visibility with D1 key
            if (GameParameters.CanHideHud && InputEngine.CurrentKeyboardState.IsKeyDown(Keys.D1) && !InputEngine.PreviousKeyboardState.IsKeyDown(Keys.D1))
            {
                this.topToolbarContainer.CanDraw = !this.topToolbarContainer.CanDraw;
                this.leftToolbarContainer.CanDraw = !this.leftToolbarContainer.CanDraw;
                this.rightToolbarContainer.CanDraw = !this.rightToolbarContainer.CanDraw;
            }

            UpdatePlayerInteractionOnToolbarHover();
            UpdateSimulationControlIcons();

            AnimateToolbarPosition(this.topToolbarContainer, this.isTopToolbarExpanded, new(0.0f, (this.topToolbarContainer.Size.Y + (this.topDrawerButton.Size.Y / 2.0f) + 8.0f) * -1.0f));
            AnimateToolbarPosition(this.leftToolbarContainer, this.isLeftToolbarExpanded, new((this.leftToolbarContainer.Size.X + (this.leftDrawerButton.Size.X / 2.0f) + 8.0f) * -1.0f, 0.0f));
            AnimateToolbarPosition(this.rightToolbarContainer, this.isRightToolbarExpanded, new(this.rightToolbarContainer.Size.X + (this.rightDrawerButton.Size.X / 2.0f) + 8.0f, 0.0f));

            AnimateDrawerButton(this.topDrawerButton);
            AnimateDrawerButton(this.leftDrawerButton);
            AnimateDrawerButton(this.rightDrawerButton);

            if (this.isTopToolbarExpanded)
            {
                UpdateTopToolbarToolPreview();
                UpdateTopToolbarItemButtons();
                UpdateTopToolbarSearchButton();
            }

            if (this.isLeftToolbarExpanded)
            {
                UpdatePanelButtons(this.leftPanelTopButtonSlotInfos, this.leftPanelTopButtonInfos);
                UpdatePanelButtons(this.leftPanelBottomButtonSlotInfos, this.leftPanelBottomButtonInfos);
            }

            if (this.isRightToolbarExpanded)
            {
                UpdatePanelButtons(this.rightPanelTopButtonSlotInfos, this.rightPanelTopButtonInfos);
                UpdatePanelButtons(this.rightPanelBottomButtonSlotInfos, this.rightPanelBottomButtonInfos);
            }

            UpdateDrawerButtons();

            this.simulationPausedBackground.CanDraw = GameHandler.HasState(GameStates.IsSimulationPaused);
        }

        private void UpdatePlayerInteractionOnToolbarHover()
        {
            bool isMouseOverDrawerButtons =
                Interaction.OnMouseOver(this.topDrawerButton) ||
                Interaction.OnMouseOver(this.leftDrawerButton) ||
                Interaction.OnMouseOver(this.rightDrawerButton);

            bool isMouseOverToolbars =
                Interaction.OnMouseOver(this.topToolbarContainer) ||
                Interaction.OnMouseOver(this.leftToolbarContainer) ||
                Interaction.OnMouseOver(this.rightToolbarContainer);

            this.playerInputController.Player.CanModifyEnvironment = !isMouseOverDrawerButtons && !isMouseOverToolbars;
        }

        private void UpdateSimulationControlIcons()
        {
            this.leftPanelBottomButtonSlotInfos[0].Icon.SourceRectangle = GameHandler.HasState(GameStates.IsSimulationPaused)
                ? this.pauseAndResumeRectangles[1] // Resume
                : this.pauseAndResumeRectangles[0]; // Pause

            this.leftPanelBottomButtonSlotInfos[1].Icon.SourceRectangle = GameHandler.SimulationSpeed switch
            {
                SimulationSpeed.Fast => this.speedIconRectangles[1],
                SimulationSpeed.VeryFast => this.speedIconRectangles[2],
                _ => this.speedIconRectangles[0],
            };

            this.toolbarCurrentlySelectedToolIcon.SourceRectangle = this.playerInputController.Pen.Tool switch
            {
                PenTool.Visualization => new(96, 64, 32, 32),
                PenTool.Pencil => new(64, 32, 32, 32),
                PenTool.Eraser => new(96, 32, 32, 32),
                PenTool.Fill => new(128, 32, 32, 32),
                PenTool.Replace => new(160, 32, 32, 32),
                _ => new(192, 0, 32, 32),
            };
        }

        private static void AnimateToolbarPosition(Container toolbarContainer, bool shouldExpand, Vector2 collapsedOffset)
        {
            toolbarContainer.Margin = Vector2.Lerp(
                toolbarContainer.Margin,
                shouldExpand ? Vector2.Zero : collapsedOffset,
                0.2f
            );
        }

        private static void AnimateDrawerButton(Image drawerButton)
        {
            drawerButton.Color = Interaction.OnMouseOver(drawerButton) ? AAP64ColorPalette.Graphite : AAP64ColorPalette.White;
        }

        private void UpdateTopToolbarToolPreview()
        {
            if (Interaction.OnMouseEnter(this.toolbarCurrentlySelectedToolIcon))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseOver(this.toolbarCurrentlySelectedToolIcon))
            {
                this.tooltipBox.CanDraw = true;

                switch (this.playerInputController.Pen.Tool)
                {
                    case PenTool.Visualization:
                        this.tooltipBox.SetTitle(Localization_WorldGizmos.Visualization_Name);
                        this.tooltipBox.SetDescription(Localization_WorldGizmos.Visualization_Description);
                        break;

                    case PenTool.Pencil:
                        this.tooltipBox.SetTitle(Localization_WorldGizmos.Pencil_Name);
                        this.tooltipBox.SetDescription(Localization_WorldGizmos.Pencil_Description);
                        break;

                    case PenTool.Eraser:
                        this.tooltipBox.SetTitle(Localization_WorldGizmos.Eraser_Name);
                        this.tooltipBox.SetDescription(Localization_WorldGizmos.Eraser_Description);
                        break;

                    case PenTool.Fill:
                        this.tooltipBox.SetTitle(Localization_WorldGizmos.Fill_Name);
                        this.tooltipBox.SetDescription(Localization_WorldGizmos.Fill_Description);
                        break;

                    case PenTool.Replace:
                        this.tooltipBox.SetTitle(Localization_WorldGizmos.Replace_Name);
                        this.tooltipBox.SetDescription(Localization_WorldGizmos.Replace_Description);
                        break;

                    default:
                        this.tooltipBox.SetTitle(Localization_Statements.Unknown);
                        this.tooltipBox.SetDescription(string.Empty);
                        break;
                }

                this.toolbarCurrentlySelectedToolBackground.Scale = Vector2.Lerp(this.toolbarCurrentlySelectedToolBackground.Scale, new(2.65f), 0.2f);
                this.toolbarCurrentlySelectedToolBackground.Color = AAP64ColorPalette.Graphite;
            }
            else
            {
                this.toolbarCurrentlySelectedToolBackground.Scale = Vector2.Lerp(this.toolbarCurrentlySelectedToolBackground.Scale, new(2.45f), 0.2f);
                this.toolbarCurrentlySelectedToolBackground.Color = AAP64ColorPalette.White;
            }

            if (Interaction.OnMouseLeftClick(this.toolbarCurrentlySelectedToolIcon))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.playerInputController.Pen.Tool = this.playerInputController.Pen.Tool.Next();
            }

            if (Interaction.OnMouseRightClick(this.toolbarCurrentlySelectedToolIcon))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.playerInputController.Pen.Tool = this.playerInputController.Pen.Tool.Previous();
            }
        }

        private void UpdateTopToolbarItemButtons()
        {
            for (int i = 0; i < UIConstants.HUD_ELEMENT_BUTTONS_LENGTH; i++)
            {
                SlotInfo slot = this.toolbarSlots[i];
                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SelectItemSlot(i, this.toolbarItems[i]);
                    break;
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    slot.Background.Scale = Vector2.Lerp(slot.Background.Scale, new(2.2f), 0.2f);

                    this.tooltipBox.SetTitle(this.toolbarItems[i].Name);
                    this.tooltipBox.SetDescription(this.toolbarItems[i].Description);
                }
                else
                {
                    slot.Background.Scale = Vector2.Lerp(slot.Background.Scale, new(2.0f), 0.2f);
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
            if (Interaction.OnMouseEnter(this.toolbarSearchButton))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseOver(this.toolbarSearchButton))
            {
                this.toolbarSearchButton.Color = AAP64ColorPalette.Graphite;
                this.tooltipBox.CanDraw = true;
                this.toolbarSearchButton.Scale = Vector2.Lerp(this.toolbarSearchButton.Scale, new(2.65f), 0.2f);

                this.tooltipBox.SetTitle(Localization_GUIs.HUD_ItemExplorer_Name);
                this.tooltipBox.SetDescription(Localization_GUIs.HUD_ItemExplorer_Description);
            }
            else
            {
                this.toolbarSearchButton.Scale = Vector2.Lerp(this.toolbarSearchButton.Scale, new(2.45f), 0.2f);
                this.toolbarSearchButton.Color = AAP64ColorPalette.White;
            }

            if (Interaction.OnMouseLeftClick(this.toolbarSearchButton))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.uiManager.OpenUI(UIIndex.ItemExplorer);
            }
        }

        private void UpdatePanelButtons(SlotInfo[] slots, ButtonInfo[] buttons)
        {
            if (slots.Length != buttons.Length)
            {
                throw new ArgumentException("Slots and buttons arrays must have the same length.");
            }

            for (int i = 0, total = slots.Length; i < total; i++)
            {
                SlotInfo slot = slots[i];
                ButtonInfo button = buttons[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    slot.Background.Color = AAP64ColorPalette.Graphite;

                    this.tooltipBox.CanDraw = true;

                    this.tooltipBox.SetTitle(button.Name);
                    this.tooltipBox.SetDescription(button.Description);

                    slot.Background.Scale = Vector2.Lerp(slot.Background.Scale, new(2.2f), 0.2f);
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                    slot.Background.Scale = Vector2.Lerp(slot.Background.Scale, new(2.0f), 0.2f);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    button.ClickAction?.Invoke();
                    break;
                }
            }
        }

        private static void UpdateDrawerButton(Image image, ref bool expanded)
        {
            if (Interaction.OnMouseEnter(image))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseLeftClick(image))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                expanded = !expanded;
            }
        }

        private void UpdateDrawerButtons()
        {
            UpdateDrawerButton(this.topDrawerButton, ref this.isTopToolbarExpanded);
            UpdateDrawerButton(this.leftDrawerButton, ref this.isLeftToolbarExpanded);
            UpdateDrawerButton(this.rightDrawerButton, ref this.isRightToolbarExpanded);
        }

        protected override void OnOpened()
        {
            AchievementEngine.AchievementUnlocked += OnAchievementUnlocked;
        }

        protected override void OnClosed()
        {
            AchievementEngine.AchievementUnlocked -= OnAchievementUnlocked;
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            this.notificationBox.EnqueueNotification(TextureIndex.Achievements, achievement.AchievedIconSourceRectangle, achievement.Title);
        }
    }
}

