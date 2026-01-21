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

using StardustSandbox.Core;
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
using StardustSandbox.Core.InputSystem.Game;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI;
using StardustSandbox.Core.UI.Common.Tools;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;
using StardustSandbox.Core.Localization;

using System;

namespace StardustSandbox.Core.UI.Common.HUD
{
    internal sealed class HudUI : UIBase
    {
        private int slotSelectedIndex = 0;
        private bool isTopToolbarExpanded = true, isLeftToolbarExpanded = true, isRightToolbarExpanded = true;

        private Container topToolbarContainer, leftToolbarContainer, rightToolbarContainer;
        private Image topToolbarBackground, leftToolbarBackground, rightToolbarBackground;
        private Image topDrawerButton, leftDrawerButton, rightDrawerButton;
        private Image toolbarSearchButton, toolbarCurrentlySelectedToolBackground, toolbarCurrentlySelectedToolIcon;
        private Image simulationPausedBackground;
        private SlotInfo[] leftPanelBottomButtonSlotInfos, leftPanelTopButtonSlotInfos, rightPanelBottomButtonSlotInfos, rightPanelTopButtonSlotInfos;

        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] toolbarSlots = new SlotInfo[UIConstants.ELEMENT_BUTTONS_LENGTH];
        private readonly ButtonInfo[] leftPanelTopButtonInfos, leftPanelBottomButtonInfos, rightPanelTopButtonInfos, rightPanelBottomButtonInfos;

        private readonly InputController inputController;
        private readonly ConfirmUI guiConfirm;
        private readonly UIManager uiManager;

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
            InputController inputController,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.inputController = inputController;
            this.guiConfirm = confirmUI;
            this.uiManager = uiManager;
            this.tooltipBox = tooltipBox;

            SelectItemSlot(0, 0, 0, 0);

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
                    this.guiConfirm.Configure(new()
                    {
                        Caption = Localization_Messages.Confirm_Simulation_EraseEverything_Title,
                        Message = Localization_Messages.Confirm_Simulation_EraseEverything_Description,
                        OnConfirmCallback = status =>
                        {
                            if (status == ConfirmStatus.Confirmed)
                            {
                                GameHandler.Reset(actorManager, world);
                            }

                            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
                        },
                    });
                    this.uiManager.OpenUI(UIIndex.Confirm);
                }),
                new(TextureIndex.IconUI, new(160, 192, 32, 32), Localization_GUIs.HUD_ReloadSimulation_Name, Localization_GUIs.HUD_ReloadSimulation_Description, () =>
                {
                    GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                    this.guiConfirm.Configure(new()
                    {
                        Caption = Localization_Messages.Confirm_Simulation_Reload_Title,
                        Message = Localization_Messages.Confirm_Simulation_Reload_Description,
                        OnConfirmCallback = status =>
                        {
                            if (status == ConfirmStatus.Confirmed)
                            {
                                GameHandler.ReloadSaveFile(actorManager, world);
                            }

                            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
                        },
                    });
                    this.uiManager.OpenUI(UIIndex.Confirm);
                }),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildToolbar(ref this.topToolbarContainer, ref this.topToolbarBackground, root, new(ScreenConstants.SCREEN_WIDTH, 96), TextureIndex.UIBackgroundHudHorizontalToolbar, new(0, 0, 1280, 96), UIDirection.Northwest, BuildTopToolbarContent);
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
            float marginX = UIConstants.ELEMENT_BUTTONS_LENGTH / 2.0f * 73.85f * -1.0f;

            Item[] items = CatalogDatabase.GetItems(UIConstants.ELEMENT_BUTTONS_LENGTH);

            for (int i = 0, length = items.Length; i < length; i++)
            {
                Item currentItem = items[i];
                SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(marginX, 0.0f), currentItem.TextureIndex, currentItem.SourceRectangle);

                slot.Background.Alignment = UIDirection.Center;

                if (!slot.Background.ContainsData(UIConstants.DATA_ITEM))
                {
                    slot.Background.SetData(UIConstants.DATA_ITEM, currentItem);
                }

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
            Color backgroundColor = new(AAP64ColorPalette.DarkGray, 120);

            this.simulationPausedBackground = new()
            {
                CanDraw = false,
                TextureIndex = TextureIndex.Pixel,
                Size = Vector2.One,
                Color = backgroundColor,
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

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            // Toggle HUD visibility with D1 key
            if (GameParameters.CanHideHud && Input.KeyboardState.IsKeyDown(Keys.D1) && !Input.PreviousKeyboardState.IsKeyDown(Keys.D1))
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

            this.inputController.Player.CanModifyEnvironment = !isMouseOverDrawerButtons && !isMouseOverToolbars;
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
                this.inputController.Pen.Tool = this.inputController.Pen.Tool.Next();
            }

            if (Interaction.OnMouseRightClick(this.toolbarCurrentlySelectedToolIcon))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.inputController.Pen.Tool = this.inputController.Pen.Tool.Previous();
            }
        }

        private void UpdateTopToolbarItemButtons()
        {
            for (int i = 0; i < UIConstants.ELEMENT_BUTTONS_LENGTH; i++)
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
                    SelectItemSlot(i, (Item)slot.Background.GetData(UIConstants.DATA_ITEM));
                    break;
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    slot.Background.Scale = Vector2.Lerp(slot.Background.Scale, new(2.2f), 0.2f);

                    Item item = (Item)slot.Background.GetData(UIConstants.DATA_ITEM);

                    TooltipBoxContent.SetTitle(item.Name);
                    TooltipBoxContent.SetDescription(item.Description);
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

                TooltipBoxContent.SetTitle(Localization_GUIs.HUD_ItemExplorer_Name);
                TooltipBoxContent.SetDescription(Localization_GUIs.HUD_ItemExplorer_Description);
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

                    TooltipBoxContent.SetTitle(button.Name);
                    TooltipBoxContent.SetDescription(button.Description);

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
            for (int i = 0; i < UIConstants.ELEMENT_BUTTONS_LENGTH; i++)
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
            for (int i = 0; i < UIConstants.ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                SlotInfo currentSlot = this.toolbarSlots[i];
                SlotInfo nextSlot = this.toolbarSlots[i + 1];

                if (currentSlot.Background.ContainsData(UIConstants.DATA_ITEM) &&
                    nextSlot.Background.ContainsData(UIConstants.DATA_ITEM))
                {
                    currentSlot.Background.SetData(
                        UIConstants.DATA_ITEM,
                        nextSlot.Background.GetData(UIConstants.DATA_ITEM)
                    );

                    currentSlot.Icon.TextureIndex = nextSlot.Icon.TextureIndex;
                    currentSlot.Icon.SourceRectangle = nextSlot.Icon.SourceRectangle;
                }
            }

            UpdateLastSlot(item);
        }

        private void UpdateLastSlot(Item item)
        {
            SlotInfo lastSlot = this.toolbarSlots[^1];

            lastSlot.Background.SetData(UIConstants.DATA_ITEM, item);
            lastSlot.Icon.TextureIndex = item.TextureIndex;
            lastSlot.Icon.SourceRectangle = item.SourceRectangle;

            SelectItemSlot(Convert.ToByte(this.toolbarSlots.Length - 1), item);
        }

        internal bool ItemIsEquipped(Item item)
        {
            for (int i = 0; i < UIConstants.ELEMENT_BUTTONS_LENGTH; i++)
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

        private void SelectItemSlot(int slotIndex, Item item)
        {
            this.slotSelectedIndex = slotIndex;
            this.inputController.Player.SelectItem(item);
        }

        private void SelectItemSlot(int slotIndex, byte categoryIndex, byte subcategoryIndex, byte itemIndex)
        {
            SelectItemSlot(slotIndex, CatalogDatabase.GetItem(categoryIndex, subcategoryIndex, itemIndex));
        }
    }
}

