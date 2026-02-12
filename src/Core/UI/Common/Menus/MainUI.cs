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
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Backgrounds;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem.Game;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.UI.Common.Menus
{
    internal sealed class MainUI : UIBase
    {
        private Image shadowBackground, gameTitle;
        private SlotInfo[] topButtonSlotInfos;

        private float animationTime;

        private readonly float[] buttonAnimationOffsets;

        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos, topButtonInfos;

        private readonly ActorManager actorManager;
        private readonly InputController inputController;
        private readonly AmbientManager ambientManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal MainUI(
            ActorManager actorManager,
            AmbientManager ambientManager,
            InputController inputController,
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

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_GUIs.Main_Create, string.Empty, () => GameHandler.StartGame(actorManager, ambientManager, inputController, uiManager, world)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Play, string.Empty, () => this.uiManager.OpenUI(UIIndex.PlayMenu)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Options, string.Empty, () => this.uiManager.OpenUI(UIIndex.OptionsMenu)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Credits, string.Empty, () => this.uiManager.OpenUI(UIIndex.CreditsMenu)),
                new(TextureIndex.None, null, Localization_GUIs.Main_Quit, string.Empty, stardustSandboxGame.Quit)
            ];

            this.topButtonInfos = [
                new(TextureIndex.IconUI, new(320, 160, 32, 32), "Achievements", string.Empty, () => this.uiManager.OpenUI(UIIndex.AchievementsMenu))
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
            this.buttonAnimationOffsets = new float[this.menuButtonLabels.Length];
        }

        protected override void OnBuild(Container root)
        {
            BuildMainPanel(root);
            BuildDecorations();
            BuildGameTitle();
            BuildMenuButtons();
            BuildTopButtons(root);
            BuildInfos(root);
        }

        private void BuildMainPanel(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = new(487.0f, GameScreen.GetViewport().Y),
                Color = new(AAP64ColorPalette.DarkGray, 180),
                Size = Vector2.One,
            };

            root.AddChild(this.shadowBackground);
        }

        private void BuildDecorations()
        {
            this.shadowBackground.AddChild(new Image
            {
                TextureIndex = TextureIndex.MiscellaneousTheatricalCurtains,
                Scale = new(2.0f)
            });
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

        private void BuildGameTitle()
        {
            this.gameTitle = new()
            {
                TextureIndex = TextureIndex.GameTitle,
                Scale = new(1.5f),
                Size = new(292.0f, 112.0f),
                Margin = new(0.0f, 32.0f),
                Alignment = UIDirection.North,
            };

            this.shadowBackground.AddChild(this.gameTitle);
        }

        private void BuildMenuButtons()
        {
            float marginY = 0f;

            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                ButtonInfo info = this.menuButtonInfos[i];

                Label label = new()
                {
                    Scale = new(0.15f),
                    Color = AAP64ColorPalette.White,
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.Center,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    TextContent = info.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 4.0f,
                    BorderThickness = 4.0f,
                };

                this.shadowBackground.AddChild(label);
                this.menuButtonLabels[i] = label;
                marginY += 75.0f;
            }
        }

        private void BuildTopButtons(Container root)
        {
            this.topButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                root,
                this.topButtonInfos,
                new(-16.0f, 16.0f),
                -80.0f,
                UIDirection.Northeast
            );
        }

        protected override void OnResize(Vector2 size)
        {
            this.shadowBackground.Scale = new(487.0f, size.Y);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateAnimations(gameTime);
            UpdateMenuButtons();
            UpdateTopButtons();
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            this.animationTime += elapsedTime * UIConstants.MAIN_ANIMATION_SPEED;
            this.gameTitle.Margin = new(
                0.0f,
                32.0f + (MathF.Sin(this.animationTime) * UIConstants.MAIN_ANIMATION_AMPLITUDE)
            );

            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                Label button = this.menuButtonLabels[i];
                this.buttonAnimationOffsets[i] += elapsedTime * UIConstants.MAIN_BUTTON_ANIMATION_SPEED;

                button.Margin = new(
                    0.0f,
                    (i * 75.0f) + (MathF.Sin(this.buttonAnimationOffsets[i]) * UIConstants.MAIN_BUTTON_ANIMATION_AMPLITUDE)
                );
            }
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseEnter(label))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(label))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        private void UpdateTopButtons()
        {
            for (int i = 0; i < this.topButtonInfos.Length; i++)
            {
                SlotInfo slotInfo = this.topButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.topButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slotInfo.Background.Color = Interaction.OnMouseOver(slotInfo.Background) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            GameHandler.StopGame(this.actorManager, this.inputController, this.world);

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.MainMenu);
            this.gameTitle.Margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                this.menuButtonLabels[i].Margin = Vector2.Zero;
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

