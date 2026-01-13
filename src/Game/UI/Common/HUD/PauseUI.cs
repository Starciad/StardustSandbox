using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class PauseUI : UIBase
    {
        private Image background;
        private Label menuTitle;

        private readonly ButtonInfo[] menuButtonInfos;
        private readonly SlotInfo[] menuButtonSlotInfos;

        private readonly ConfirmUI confirmUI;

        private readonly UIManager uiManager;

        internal PauseUI(
            ConfirmUI confirmUI,
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.confirmUI = confirmUI;
            this.uiManager = uiManager;

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Resume, string.Empty, this.uiManager.CloseUI),
                new(TextureIndex.None, null, Localization_Statements.Options, string.Empty, () =>
                {
                    this.uiManager.OpenUI(UIIndex.OptionsMenu);
                    GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                }),
                new(TextureIndex.None, null, Localization_Statements.Exit, string.Empty, () =>
                {
                    this.confirmUI.Configure(new()
                    {
                        Caption = Localization_Messages.Confirm_Simulation_Exit_Title,
                        Message = Localization_Messages.Confirm_Simulation_Exit_Description,
                        OnConfirmCallback = status =>
                        {
                            if (status == ConfirmStatus.Confirmed)
                            {
                                uiManager.Reset();
                                uiManager.OpenUI(UIIndex.MainMenu);
                            }
                        }
                    });
                    this.uiManager.OpenUI(UIIndex.Confirm);
                    GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
        }

        #region BUILDERS

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
        }

        private void BuildBackground(Container root)
        {
            Image shadow = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GUIBackgroundPause),
                Size = new(542.0f, 540.0f),
                Alignment = UIDirection.Center,
            };

            root.AddChild(shadow);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = UIDirection.North,
                Margin = new(0.0f, 10.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.Pause_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginY = 118.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Image background = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GUIButtons),
                    SourceRectangle = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Size = new(320.0f, 80.0f),
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.North,
                };

                Label label = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.background.AddChild(background);
                background.AddChild(label);

                this.menuButtonSlotInfos[i] = new(background, null, label);

                marginY += background.Size.Y + 32.0f;
            }
        }

        #endregion

        internal override void Update(GameTime gameTime)
        {
            UpdateMenuButtons();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
