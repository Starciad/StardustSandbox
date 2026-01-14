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

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Backgrounds;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Net;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class MainUI : UIBase
    {
        private Image background, gameTitle;
        private Label updateLabel;

        private float animationTime;

        private readonly float[] buttonAnimationOffsets;

        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly ActorManager actorManager;
        private readonly InputController inputController;
        private readonly AmbientManager ambientManager;
        private readonly UIManager uiManager;
        private readonly World world;

        private const float animationSpeed = 2.0f;
        private const float animationAmplitude = 10.0f;
        private const float buttonAnimationSpeed = 1.5f;
        private const float buttonAnimationAmplitude = 5.0f;

        internal MainUI(
            ActorManager actorManager,
            AmbientManager ambientManager,
            InputController inputController,
            UIIndex index,
            UIManager uiManager,
            World world
        ) : base(index)
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
                new(TextureIndex.None, null, Localization_GUIs.Main_Quit, string.Empty, Program.Quit)
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
            this.buttonAnimationOffsets = new float[this.menuButtonLabels.Length];
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildMainPanel(root);
            BuildDecorations();
            BuildGameTitle();
            BuildButtons();
            BuildInfos(root);
        }

        private void BuildMainPanel(Container root)
        {
            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(487.0f, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 180),
                Size = Vector2.One,
            };
            root.AddChild(this.background);
        }

        private void BuildDecorations()
        {
            this.background.AddChild(new Image
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.MiscellaneousTheatricalCurtains),
                Scale = new(2.0f)
            });
        }

        private void BuildInfos(Container root)
        {
            this.updateLabel = new()
            {
                Margin = new(-16.0f, 16.0f),
                Scale = new(0.075f),
                Alignment = UIDirection.Northeast,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_Messages.IsUpdateAvailable,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

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

            root.AddChild(this.updateLabel);
            root.AddChild(versionLabel);
            root.AddChild(copyrightLabel);
        }

        private void BuildGameTitle()
        {
            this.gameTitle = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GameTitle),
                Scale = new(1.5f),
                Size = new(292.0f, 112.0f),
                Margin = new(0.0f, 32.0f),
                Alignment = UIDirection.North,
            };

            this.background.AddChild(this.gameTitle);
        }

        private void BuildButtons()
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

                this.background.AddChild(label);
                this.menuButtonLabels[i] = label;
                marginY += 75.0f;
            }
        }

        #endregion

        #region UPDATING

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateAnimations(gameTime);
            UpdateMenuButtons();
            UpdateUpdateButton();
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            this.animationTime += elapsedTime * animationSpeed;
            this.gameTitle.Margin = new(
                0.0f,
                32.0f + (MathF.Sin(this.animationTime) * animationAmplitude)
            );

            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                Label button = this.menuButtonLabels[i];
                this.buttonAnimationOffsets[i] += elapsedTime * buttonAnimationSpeed;

                button.Margin = new(
                    0.0f,
                    (i * 75.0f) + (MathF.Sin(this.buttonAnimationOffsets[i]) * buttonAnimationAmplitude)
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

        private void UpdateUpdateButton()
        {
            if (UpdateChecker.IsUpdateAvailable)
            {
                this.updateLabel.CanDraw = true;
                this.updateLabel.Color = Interaction.OnMouseOver(this.updateLabel) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;

                if (Interaction.OnMouseLeftClick(this.updateLabel))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    BrowserUtility.OpenUrl(NetConstants.ITCH_URL);
                }
            }
            else
            {
                this.updateLabel.CanDraw = false;
            }
        }

        #endregion

        protected override void OnOpened()
        {
            GameHandler.StopGame(this.actorManager, this.inputController, this.world);

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.MainMenu);
            this.gameTitle.Margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                this.menuButtonLabels[i].Margin = Vector2.Zero;
                this.buttonAnimationOffsets[i] = Core.Random.GetFloat() * MathF.PI * 2.0f;
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

