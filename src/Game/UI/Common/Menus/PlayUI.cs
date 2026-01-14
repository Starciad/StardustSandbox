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

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

namespace StardustSandbox.UI.Common.Menus
{
    internal class PlayUI : UIBase
    {
        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly UIManager uiManager;

        internal PlayUI(
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(0, 32, 32, 32), Localization_Statements.Worlds, string.Empty, () => this.uiManager.OpenUI(UIIndex.WorldExplorerMenu)),
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Return, string.Empty, this.uiManager.CloseUI),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

        protected override void OnBuild(Container root)
        {
            BuildTitle(root);
            BuildMenuButtons(root);
        }

        private static void BuildTitle(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Scale = new(ScreenConstants.SCREEN_WIDTH, 128.0f),
                Size = Vector2.One,
            };

            Label title = new()
            {
                Scale = new(0.2f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.Center,
                TextContent = Localization_GUIs.Play_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            background.AddChild(title);
            root.AddChild(background);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = 0.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label label = new()
                {
                    Scale = new(0.15f),
                    Margin = new(0.0f, marginY),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                Image icon = new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Margin = new(-96.0f, 0.0f),
                    Scale = new(2),
                };

                label.AddChild(icon);
                root.AddChild(label);

                marginY += label.Size.Y + 64.0f;

                this.menuButtonLabels[i] = label;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
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
    }
}

