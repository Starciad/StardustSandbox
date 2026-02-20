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
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Backgrounds;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class MainUI : UIBase
    {
        private Image shadowBackground, theatricalCurtains, gameTitle;
        private SlotInfo[] headerButtonSlotInfos;

        private float animationTime;

        private readonly float[] buttonAnimationOffsets;

        private readonly Label[] generalButtonLabels;
        private readonly ButtonInfo[] generalButtonInfos, headerButtonInfos;

        private readonly ActorManager actorManager;
        private readonly PlayerInputController inputController;
        private readonly AmbientManager ambientManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal MainUI(
            ActorManager actorManager,
            AmbientManager ambientManager,
            PlayerInputController inputController,
            StardustSandboxGame stardustSandboxGame,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.actorManager = actorManager;
            this.ambientManager = ambientManager;
            this.inputController = inputController;
            this.uiManager = uiManager;
            this.world = world;

            this.generalButtonInfos = [
                new(TextureIndex.None, null, Localization_GUIs.Main_Create, string.Empty, () => GameHandler.StartGame(actorManager, ambientManager, inputController, uiManager, world)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Play, string.Empty, () => this.uiManager.OpenUI(UIIndex.Play)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Options, string.Empty, () =>
                {
                    ((OptionsUI)UIDatabase.GetUI(UIIndex.Options)).Setup();
                    this.uiManager.OpenUI(UIIndex.Options);
                }),
                new(TextureIndex.None, null, Localization_GUIs.Main_Credits, string.Empty, () => this.uiManager.OpenUI(UIIndex.Credits)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Quit, string.Empty, stardustSandboxGame.Exit)
            ];

            this.headerButtonInfos = [
                new(TextureIndex.IconUI, new(320, 160, 32, 32), string.Empty, string.Empty, () => this.uiManager.OpenUI(UIIndex.Achievements))
            ];

            this.generalButtonLabels = new Label[this.generalButtonInfos.Length];
            this.buttonAnimationOffsets = new float[this.generalButtonLabels.Length];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildGeneralButtons();
            BuildHeaderButtons(root);
            BuildInfos(root);
        }

        private void BuildBackground(Container root)
        {
            Vector2 viewport = GameScreen.GetViewport();

            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = new(487.0f, viewport.Y),
                Color = new(AAP64ColorPalette.DarkGray, 80),
                Size = Vector2.One,
            };

            this.theatricalCurtains = new()
            {
                TextureIndex = TextureIndex.MiscellaneousTheatricalCurtains,
                Size = new(640.0f, 360.0f),
                Scale = new(
                    viewport.X / 640.0f,
                    viewport.Y / 360.0f
                ),
            };

            this.gameTitle = new()
            {
                TextureIndex = TextureIndex.GameTitle,
                Scale = new(1.5f),
                Size = new(292.0f, 112.0f),
                Margin = new(0.0f, 32.0f),
                Alignment = UIDirection.North,
            };

            root.AddChild(this.theatricalCurtains);
            root.AddChild(this.shadowBackground);

            this.shadowBackground.AddChild(this.gameTitle);
        }

        private static void BuildInfos(Container root)
        {
            Label versionLabel = new()
            {
                Margin = new(-32.0f, -32.0f),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.Southeast,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = $"Ver. {GameConstants.VERSION}",
            };

            Label copyrightLabel = new()
            {
                Margin = new(0.0f, -32.0f),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.South,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = $"(c) {GameConstants.YEAR} {GameConstants.AUTHOR}",
            };

            root.AddChild(versionLabel);
            root.AddChild(copyrightLabel);
        }

        private void BuildGeneralButtons()
        {
            float marginY = 0f;

            for (int i = 0; i < this.generalButtonLabels.Length; i++)
            {
                ButtonInfo info = this.generalButtonInfos[i];

                Label label = new()
                {
                    Scale = new(0.15f),
                    Color = AAP64ColorPalette.White,
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.Center,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    TextContent = info.Name,
                    IsFocusable = true,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 4.0f,
                    BorderThickness = 4.0f,

                    OnSelected = () =>
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Click);
                        info.ClickAction?.Invoke();
                    },

                    OnFocusGained = (element) =>
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                        ((Label)element).Color = AAP64ColorPalette.LemonYellow;
                    },

                    OnFocusLost = (element) =>
                    {
                        ((Label)element).Color = AAP64ColorPalette.White;
                    }
                };

                this.shadowBackground.AddChild(label);
                this.generalButtonLabels[i] = label;
                marginY += 75.0f;
            }
        }

        private void BuildHeaderButtons(Container root)
        {
            this.headerButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                root,
                this.headerButtonInfos,
                new(-16.0f, 16.0f),
                -80.0f,
                UIDirection.Northeast
            );
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = new(487.0f, newSize.Y);
            this.theatricalCurtains.Scale = new(
                newSize.X / 640.0f,
                newSize.Y / 360.0f
            );
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateAnimations(gameTime);
            // UpdateHeaderButtons();
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            this.animationTime += elapsedTime * UIConstants.MAIN_ANIMATION_SPEED;
            this.gameTitle.Margin = new(
                0.0f,
                32.0f + (MathF.Sin(this.animationTime) * UIConstants.MAIN_ANIMATION_AMPLITUDE)
            );

            for (int i = 0; i < this.generalButtonLabels.Length; i++)
            {
                Label button = this.generalButtonLabels[i];
                this.buttonAnimationOffsets[i] += elapsedTime * UIConstants.MAIN_BUTTON_ANIMATION_SPEED;

                button.Margin = new(
                    0.0f,
                    (i * 75.0f) + (MathF.Sin(this.buttonAnimationOffsets[i]) * UIConstants.MAIN_BUTTON_ANIMATION_AMPLITUDE)
                );
            }
        }

        // private void UpdateHeaderButtons()
        // {
        //     for (int i = 0; i < this.headerButtonInfos.Length; i++)
        //     {
        //         SlotInfo slotInfo = this.headerButtonSlotInfos[i];
        // 
        //         // if (Interaction.OnMouseEnter(slotInfo.Background))
        //         // {
        //         //     SoundEngine.Play(SoundEffectIndex.GUI_Hover);
        //         // }
        // 
        //         if (slotInfo.Background.IsSelected)
        //         {
        //             SoundEngine.Play(SoundEffectIndex.GUI_Click);
        //             this.headerButtonInfos[i].ClickAction?.Invoke();
        //             break;
        //         }
        // 
        //         slotInfo.Background.Color = slotInfo.Background.IsFocused ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
        //     }
        // }

        protected override void OnOpened()
        {
            GameHandler.StopGame(this.actorManager, this.inputController, this.world);

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.MainMenu);
            this.gameTitle.Margin = Vector2.Zero;

            for (int i = 0; i < this.generalButtonLabels.Length; i++)
            {
                this.generalButtonLabels[i].Margin = Vector2.Zero;
                this.buttonAnimationOffsets[i] = Randomness.Random.GetFloat() * MathF.PI * 2.0f;
            }

            if (MediaPlayer.State != MediaState.Playing || SongEngine.CurrentSongIndex != SongIndex.Volume_01_Track_01)
            {
                SongEngine.Play(SongIndex.Volume_01_Track_01);
            }
        }

        protected override void OnClosed()
        {
            this.world.Clear();
            this.world.CanDraw = false;
        }
    }
}

