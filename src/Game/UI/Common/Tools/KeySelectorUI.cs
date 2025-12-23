using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class KeySelectorUI : UIBase
    {
        private KeySelectorSettings settings;

        private Text message;

        private readonly GameWindow gameWindow;
        private readonly InputController inputController;
        private readonly UIManager uiManager;

        internal KeySelectorUI(
            GameWindow gameWindow,
            UIIndex index,
            InputController inputController,
            UIManager uiManager
        ) : base(index)
        {
            this.gameWindow = gameWindow;
            this.inputController = inputController;
            this.uiManager = uiManager;
        }

        internal void Configure(in KeySelectorSettings settings)
        {
            this.settings = settings;
            this.message.TextContent = settings.Synopsis;
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMessage(root);
        }

        private static void BuildBackground(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(background);
        }

        private void BuildMessage(Container root)
        {
            this.message = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.North,
            };

            root.AddChild(this.message);
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            this.inputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
            this.inputController.Activate();

            this.gameWindow.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            SoundEngine.Play(SoundEffectIndex.GUI_Accepted);

            this.uiManager.CloseGUI();
            this.settings.OnSelectedKey?.Invoke(inputKeyEventArgs.Key);
        }
    }
}
