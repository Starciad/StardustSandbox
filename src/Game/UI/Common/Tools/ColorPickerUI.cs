using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class ColorPickerUI : UIBase
    {
        private ColorPickerSettings colorPickerSettings;
        private Text captionElement;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo[] menuButtonInfos;
        private readonly ColorButtonInfo[] colorButtonInfos;

        private readonly Label[] menuButtonLabels;
        private readonly ColorSlotInfo[] colorButtonSlotInfos;

        private readonly GameManager gameManager;
        private readonly InputController inputController;
        private readonly UIManager uiManager;

        internal ColorPickerUI(
            GameManager gameManager,
            UIIndex index,
            InputController inputController,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.inputController = inputController;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, this.uiManager.CloseGUI),
            ];

            this.colorButtonInfos = [
                new(Localization_Colors.DarkGray, AAP64ColorPalette.DarkGray),
                new(Localization_Colors.Charcoal, AAP64ColorPalette.Charcoal),
                new(Localization_Colors.Maroon, AAP64ColorPalette.Maroon),
                new(Localization_Colors.DarkRed, AAP64ColorPalette.DarkRed),
                new(Localization_Colors.Crimson, AAP64ColorPalette.Crimson),
                new(Localization_Colors.OrangeRed, AAP64ColorPalette.OrangeRed),
                new(Localization_Colors.Orange, AAP64ColorPalette.Orange),
                new(Localization_Colors.Amber, AAP64ColorPalette.Amber),
                new(Localization_Colors.Gold, AAP64ColorPalette.Gold),
                new(Localization_Colors.LemonYellow, AAP64ColorPalette.LemonYellow),
                new(Localization_Colors.LimeGreen, AAP64ColorPalette.LimeGreen),
                new(Localization_Colors.GrassGreen, AAP64ColorPalette.GrassGreen),
                new(Localization_Colors.ForestGreen, AAP64ColorPalette.ForestGreen),
                new(Localization_Colors.EmeraldGreen, AAP64ColorPalette.EmeraldGreen),
                new(Localization_Colors.DarkGreen, AAP64ColorPalette.DarkGreen),
                new(Localization_Colors.MossGreen, AAP64ColorPalette.MossGreen),
                new(Localization_Colors.DarkTeal, AAP64ColorPalette.DarkTeal),
                new(Localization_Colors.NavyBlue, AAP64ColorPalette.NavyBlue),
                new(Localization_Colors.RoyalBlue, AAP64ColorPalette.RoyalBlue),
                new(Localization_Colors.SkyBlue, AAP64ColorPalette.SkyBlue),
                new(Localization_Colors.Cyan, AAP64ColorPalette.Cyan),
                new(Localization_Colors.Mint, AAP64ColorPalette.Mint),
                new(Localization_Colors.White, AAP64ColorPalette.White),
                new(Localization_Colors.PaleYellow, AAP64ColorPalette.PaleYellow),
                new(Localization_Colors.Peach, AAP64ColorPalette.Peach),
                new(Localization_Colors.Salmon, AAP64ColorPalette.Salmon),
                new(Localization_Colors.Rose, AAP64ColorPalette.Rose),
                new(Localization_Colors.Magenta, AAP64ColorPalette.Magenta),
                new(Localization_Colors.Violet, AAP64ColorPalette.Violet),
                new(Localization_Colors.PurpleGray, AAP64ColorPalette.PurpleGray),
                new(Localization_Colors.DarkPurple, AAP64ColorPalette.DarkPurple),
                new(Localization_Colors.Cocoa, AAP64ColorPalette.Cocoa),
                new(Localization_Colors.Umber, AAP64ColorPalette.Umber),
                new(Localization_Colors.Brown, AAP64ColorPalette.Brown),
                new(Localization_Colors.Rust, AAP64ColorPalette.Rust),
                new(Localization_Colors.Sand, AAP64ColorPalette.Sand),
                new(Localization_Colors.Tan, AAP64ColorPalette.Tan),
                new(Localization_Colors.LightGrayBlue, AAP64ColorPalette.LightGrayBlue),
                new(Localization_Colors.SteelBlue, AAP64ColorPalette.SteelBlue),
                new(Localization_Colors.Slate, AAP64ColorPalette.Slate),
                new(Localization_Colors.Graphite, AAP64ColorPalette.Graphite),
                new(Localization_Colors.Gunmetal, AAP64ColorPalette.Gunmetal),
                new(Localization_Colors.Coal, AAP64ColorPalette.Coal),
                new(Localization_Colors.DarkBrown, AAP64ColorPalette.DarkBrown),
                new(Localization_Colors.Burgundy, AAP64ColorPalette.Burgundy),
                new(Localization_Colors.Clay, AAP64ColorPalette.Clay),
                new(Localization_Colors.Terracotta, AAP64ColorPalette.Terracotta),
                new(Localization_Colors.Blush, AAP64ColorPalette.Blush),
                new(Localization_Colors.PaleBlue, AAP64ColorPalette.PaleBlue),
                new(Localization_Colors.LavenderBlue, AAP64ColorPalette.LavenderBlue),
                new(Localization_Colors.Periwinkle, AAP64ColorPalette.Periwinkle),
                new(Localization_Colors.Cerulean, AAP64ColorPalette.Cerulean),
                new(Localization_Colors.TealGray, AAP64ColorPalette.TealGray),
                new(Localization_Colors.HunterGreen, AAP64ColorPalette.HunterGreen),
                new(Localization_Colors.PineGreen, AAP64ColorPalette.PineGreen),
                new(Localization_Colors.SeafoamGreen, AAP64ColorPalette.SeafoamGreen),
                new(Localization_Colors.MintGreen, AAP64ColorPalette.MintGreen),
                new(Localization_Colors.Aquamarine, AAP64ColorPalette.Aquamarine),
                new(Localization_Colors.Khaki, AAP64ColorPalette.Khaki),
                new(Localization_Colors.Beige, AAP64ColorPalette.Beige),
                new(Localization_Colors.Sepia, AAP64ColorPalette.Sepia),
                new(Localization_Colors.Coffee, AAP64ColorPalette.Coffee),
                new(Localization_Colors.DarkBeige, AAP64ColorPalette.DarkBeige),
                new(Localization_Colors.DarkTaupe, AAP64ColorPalette.DarkTaupe),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
            this.colorButtonSlotInfos = new ColorSlotInfo[this.colorButtonInfos.Length];
        }

        internal void Configure(ColorPickerSettings settings)
        {
            this.colorPickerSettings = settings;
        }

        private void SelectColorButtonAction(Color color)
        {
            this.uiManager.CloseGUI();
            this.colorPickerSettings?.OnSelectCallback?.Invoke(new(color));
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildCaption(root);
            BuildColorButtons(root);
            BuildMenuButtons(root);

            root.AddChild(this.tooltipBox);
        }

        private static void BuildBackground(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            root.AddChild(background);
        }

        private void BuildCaption(Container root)
        {
            this.captionElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.North,
                TextContent = Localization_GUIs.Tools_ColorPicker_Title,
            };

            root.AddChild(this.captionElement);
        }

        private void BuildColorButtons(Container root)
        {
            Vector2 baseMargin = new(74.0f, 192.0f);
            Vector2 margin = baseMargin;

            Vector2 textureSize = new(40.0f, 22.0f);

            int buttonsPerRow = 12;

            int totalButtons = this.colorButtonInfos.Length;
            int totalRows = (totalButtons + buttonsPerRow - 1) / buttonsPerRow;

            int index = 0;

            for (byte row = 0; row < totalRows; row++)
            {
                for (byte col = 0; col < buttonsPerRow; col++)
                {
                    if (index >= totalButtons)
                    {
                        break;
                    }

                    ColorButtonInfo colorButton = this.colorButtonInfos[index];

                    Image background = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(386, 0, 40, 22),
                        Scale = new(2.0f),
                        Size = textureSize,
                        Color = colorButton.Color,
                        Margin = margin,
                    };

                    Image borderElement = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(386, 22, 40, 22),
                        Scale = new(2.0f),
                        Size = textureSize,
                    };

                    background.AddChild(borderElement);

                    root.AddChild(background);

                    this.colorButtonSlotInfos[index] = new(background, borderElement);
                    index++;

                    margin.X += background.Size.X + 16.0f;
                }

                margin.X = baseMargin.X;
                margin.Y += (textureSize.Y * 2.0f) + 16.0f;
            }
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -48.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.South,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                marginY -= 72;

                root.AddChild(label);

                this.menuButtonLabels[i] = label;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateColorButtons();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateColorButtons()
        {
            for (int i = 0; i < this.colorButtonInfos.Length; i++)
            {
                ColorSlotInfo colorSlot = this.colorButtonSlotInfos[i];
                ColorButtonInfo colorButton = this.colorButtonInfos[i];

                if (Interaction.OnMouseLeftClick(colorSlot.Border))
                {
                    SelectColorButtonAction(colorButton.Color);
                }

                if (Interaction.OnMouseOver(colorSlot.Border))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(colorButton.Name);
                    TooltipBoxContent.SetDescription(string.Empty);
                }
            }
        }

        #endregion

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.inputController.Disable();
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
            this.inputController.Activate();
        }
    }
}
