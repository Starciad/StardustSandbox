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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.InputSystem.Actions;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class TutorialUI : UIBase
    {
        private Image panelBackground, shadowBackground;
        private Label title, subtitle;
        private Text content;

        private readonly TutorialSection[] sections;

        internal TutorialUI(
            PlayerInputController playerInputController
        ) : base()
        {
            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();

            this.sections =
            [
                new TutorialSection(
                    "Introdução",
                    new TutorialContent()
                    {
                        Title = "Bem-vindo",
                        Description = "Bem-vindo ao Stardust Sandbox! Este rápido tutorial mostra o básico para você começar a criar e se divertir."
                    }
                ),
            
                new TutorialSection(
                    "Câmera",
                    new TutorialContent()
                    {
                        Title = "Mover a câmera",
                        Description = string.Format("Use {0} {1} {2} {3} para mover a câmera: cima, baixo, direita e esquerda.", controlSettings.MoveCameraUpKeyboardBinding, controlSettings.MoveCameraDownKeyboardBinding, controlSettings.MoveCameraRightKeyboardBinding, controlSettings.MoveCameraLeftKeyboardBinding)
                    },
                    new TutorialContent()
                    {
                        Title = "Zoom",
                        Description = string.Format("Use {0} para aumentar e {1} para diminuir o zoom da câmera.", controlSettings.ZoomCameraInKeyboardBinding, controlSettings.ZoomCameraOutKeyboardBinding)
                    },
                    new TutorialContent()
                    {
                        Title = "Mover rápido",
                        Description = string.Format("Segure {0} enquanto se move para ir mais rápido pela tela.", controlSettings.MoveCameraFastKeyboardBinding)
                    }
                ),
            
                new TutorialSection(
                    "Selecionar e Aplicar",
                    new TutorialContent()
                    {
                        Title = "Escolher um item",
                        Description = "Selecione um item na barra superior para torná-lo o item atual."
                    },
                    new TutorialContent()
                    {
                        Title = "Aplicar no mapa",
                        Description = "Segure o botão direito do mouse e arraste sobre o mapa para adicionar o item selecionado."
                    },
                    new TutorialContent()
                    {
                        Title = "Apagar",
                        Description = "Segure o botão direito do mouse sobre a área desejada para remover itens ou use a ferramenta Borracha."
                    }
                ),
            
                new TutorialSection(
                    "Ferramentas e Interface",
                    new TutorialContent()
                    {
                        Title = "Barra superior (rápida)",
                        Description = "Aqui ficam: a ferramenta atual, os itens mais usados e o explorador de itens para ver tudo."
                    },
                    new TutorialContent()
                    {
                        Title = "Barra esquerda",
                        Description = "Ferramentas de interação (p. ex. configurações do mundo). Explore para ver opções."
                    },
                    new TutorialContent()
                    {
                        Title = "Barra direita",
                        Description = "Operações técnicas: salvar, carregar e limpar o mapa. Use com cuidado."
                    }
                ),
            
                new TutorialSection(
                    "Mundo do Jogo",
                    new TutorialContent()
                    {
                        Title = "Mapa",
                        Description = "O mapa é onde a simulação acontece. Tudo dentro do mapa faz parte da simulação."
                    },
                    new TutorialContent()
                    {
                        Title = "Elementos",
                        Description = "Elementos são materiais (ex.: areia, água). Eles interagem entre si formando comportamentos divertidos."
                    },
                    new TutorialContent()
                    {
                        Title = "Atores",
                        Description = "Atores são seres que reagem ao mundo — mexem, empurram e mudam a simulação."
                    }
                ),
            
                new TutorialSection(
                    "Dicas Rápidas",
                    new TutorialContent()
                    {
                        Title = "Experimente",
                        Description = "Tente combinar elementos diferentes e veja o que acontece — é a melhor forma de aprender!"
                    }
                ),
            
                new TutorialSection(
                    "Conclusão",
                    new TutorialContent()
                    {
                        Title = "Pronto para criar",
                        Description = "Agora é com você: explore, experimente e divirta-se. Compartilhe suas criações com a comunidade!"
                    }
                ),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundTutorial,
                Size = new(1006.0f, 715.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.title = new()
            {
                Alignment  = UIDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(16.0f, 10.0f),
                TextContent = "Tutorial",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.subtitle = new()
            {
                Alignment  = UIDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.08f),
                Margin = new(16.0f, 60.0f),
                TextContent = "Aprenda o básico para começar a criar e se divertir!",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.content = new()
            {
                Alignment  = UIDirection.Center,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.06f),
                Margin = new(16.0f, 120.0f),
                TextContent = "Use as setas para navegar pelas seções do tutorial.",
            };

            this.panelBackground.AddChild(this.title);
            this.panelBackground.AddChild(this.subtitle);
            this.panelBackground.AddChild(this.content);
        }
    }
}
