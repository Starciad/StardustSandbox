using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu : SGUISystem
    {
        private enum SSystemButton : byte
        {
            Return = 0,
            Save = 1
        }

        private byte selectedSectionIndex;
        private bool restartMessageAppeared;

        private SVideoSettings videoSettings;
        private SLanguageSettings languageSettings;

        private readonly Texture2D panelBackgroundTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont digitalDiscoSpriteFont;

        private readonly string titleName = SLocalization_GUIs.Menu_Options_Title;

        private readonly SButton[] systemButtons;
        
        private readonly SGUI_Message guiMessage;
        private readonly SRoot root;

        internal SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Message guiMessage) : base(gameInstance, identifier, guiEvents)
        {
            this.guiMessage = guiMessage;

            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_13");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8");

            this.systemButtons = [
                new(null, SLocalization_Statements.Return, string.Empty, ReturnButtonAction),
                new(null, SLocalization_Statements.Save, string.Empty, SaveButtonAction),
            ];

            this.root = new()
            {
                Sections = [
                    new("general", "General", string.Empty, [
                        new("language", "Language", string.Empty, SOptionType.Selector)
                        {
                            Values = SLocalizationConstants.AVAILABLE_GAME_CULTURES,
                        },
                    ]),
                    
                    new("volume", "Volume", string.Empty, [
                        new("master_volume", "Master Volume", string.Empty, SOptionType.Slider)
                        {
                            Range = new(000, 100),
                        },
                        new("music_volume", "Music Volume", string.Empty, SOptionType.Slider)
                        {
                            Range = new(000, 100),
                        },
                        new("sfx_volume", "SFX Volume", string.Empty, SOptionType.Slider)
                        {
                            Range = new(000, 100),
                        },
                    ]),

                    new("video", "Video", string.Empty, [
                        new("resolution", "Resolution", string.Empty, SOptionType.Selector)
                        {
                            Values = Array.ConvertAll<SSize2, object>(SScreenConstants.RESOLUTIONS, x => x),
                        },
                        new("fullscreen", "Fullscreen", string.Empty, SOptionType.Toggle),
                        new("vsync", "VSync", string.Empty, SOptionType.Toggle),
                        new("borderless", "Borderless", string.Empty, SOptionType.Toggle),
                    ]),

                    new("graphics", "Graphics", string.Empty, [

                    ]),

                    new("cursor", "Cursor", string.Empty, [
                        new("border_color", "Border Color", string.Empty, SOptionType.Color),
                        new("background_color", "Background Color", string.Empty, SOptionType.Color),
                        new("scale", "Scale", string.Empty, SOptionType.Selector)
                        {
                            Values = ["Very Small", "Small", "Medium", "Large", "Very Large"],
                        },
                    ]),
                ],
            };

            this.systemButtonElements = new SGUILabelElement[this.systemButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateSectionButtons();
            UpdateSystemButtons();
        }

        private void UpdateSectionButtons()
        {
            for (byte i = 0; i < this.sectionButtonElements.Count; i++)
            {
                SGUILabelElement labelElement = this.sectionButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    SelectSection(i);
                }

                if (this.selectedSectionIndex.Equals(i))
                {
                    labelElement.Color = SColorPalette.LemonYellow;
                    continue;
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    this.systemButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void SelectSection(byte index)
        {
            this.selectedSectionIndex = byte.Clamp(index, 0, (byte)(this.sectionButtonElements.Count - 1));

            for (byte i = 0; i < this.sectionContainerElements.Count; i++)
            {
                if (this.selectedSectionIndex.Equals(i))
                {
                    this.sectionContainerElements[i].Active();
                    continue;
                }

                this.sectionContainerElements[i].Disable();
            }
        }
    }
}
