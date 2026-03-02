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
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class TutorialUI : UIBase
    {
        private int currentPageIndex = 0;

        private Image panelBackground, shadowBackground;
        private Label title;
        private Text content;

        private readonly UIManager uiManager;

        private readonly SystemInformationSettings systemInformationSettings;

        private readonly TutorialContent[] contents;

        internal TutorialUI(
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;

            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();
            this.systemInformationSettings = SettingsSerializer.Load<SystemInformationSettings>();

            this.contents =
            [
                new(
                    "Introdução",
                    "Bem-vindo(a) ao Stardust Sandbox. Aqui você pode criar experiências usando diferentes materiais e entidades. Este tutorial mostra apenas o essencial para começar. Depois disso, você poderá explorar livremente."
                ),

                new(
                    "Câmera",
                    string.Format("Use {0} {1} {2} {3} para mover a câmera pelo mapa. Use {4} para aproximar e {5} para afastar a visão. Se quiser se mover mais rápido, segure {6} enquanto utiliza as teclas de movimento.", controlSettings.MoveCameraUpKeyboardBinding, controlSettings.MoveCameraRightKeyboardBinding, controlSettings.MoveCameraDownKeyboardBinding, controlSettings.MoveCameraLeftKeyboardBinding, controlSettings.ZoomCameraInKeyboardBinding, controlSettings.ZoomCameraOutKeyboardBinding, controlSettings.MoveCameraFastKeyboardBinding)
                ),

                new(
                    "Desenhar",
                    "Escolha um item na barra superior para torná-lo ativo. Depois, segure o botão esquerdo do mouse e arraste sobre o mapa para aplicar o item. Use a roda do mouse para ajustar o tamanho do pincel conforme necessário."
                ),

                new(
                    "Apagar",
                    "Para remover conteúdo do mapa, segure o botão direito do mouse sobre a área desejada ou selecione a ferramenta Borracha. Apagar faz parte do processo de testar e ajustar suas criações."
                ),

                new(
                    "Interface",
                    "A barra superior mostra o item atual e permite acessar outros materiais pelo explorador. A barra esquerda reúne ferramentas e configurações do mundo. A barra direita contém ações importantes como salvar, carregar e limpar o mapa."
                ),

                new(
                    "Simulação",
                    "Tudo acontece dentro do mapa. Os elementos são materiais como areia e água que seguem regras físicas e interagem entre si. Atores são entidades que reagem ao ambiente e podem modificar o mundo ao seu redor."
                ),

                new(
                    "Salvar",
                    "Use a barra direita para salvar suas criações. Assim você pode continuar depois sem perder seu progresso."
                ),

                new(
                    "Explorar",
                    "Experimente combinar diferentes elementos para observar novos comportamentos. Testar, ajustar e tentar novamente é a melhor forma de aprender como a simulação funciona."
                )
            ];
        }

        private bool TryNextPage()
        {
            if (this.currentPageIndex < this.contents.Length - 1)
            {
                this.currentPageIndex++;
                RefreshContent();
                return true;
            }

            return false;
        }

        private void RefreshContent()
        {
            TutorialContent content = this.contents[this.currentPageIndex];

            this.title.TextContent = content.Title;
            this.content.TextContent = content.Description;
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
                Size = new(390.0f, 520.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.title = new()
            {
                Alignment = UIDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Margin = new(0.0f, 24.0f),
                TextContent = "Title",
                Color = AAP64ColorPalette.Umber,
            };

            this.content = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.055f),
                Margin = new(24.0f, 96.0f),
                LineHeight = 1.5f,
                TextAreaSize = new(365.0f, 472.0f),
                TextContent = "Description",
                Color = AAP64ColorPalette.Umber,
            };

            this.panelBackground.AddChild(this.title);
            this.panelBackground.AddChild(this.content);
        }

        protected override void OnScreenResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Interaction.OnMouseLeftClick(this.Root) && !TryNextPage())
            {
                this.uiManager.CloseUI();
                this.systemInformationSettings.TutorialDisplayed = true;
                SettingsSerializer.Save(this.systemInformationSettings);
            }
        }

        protected override void OnOpened()
        {
            this.currentPageIndex = 0;
            RefreshContent();
        }
    }
}
