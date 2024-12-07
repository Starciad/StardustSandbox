using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.Managers.IO;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu : SGUISystem
    {
        private enum SMenuSection : byte
        {
            Video = 0,
            Language = 1
        }

        private enum SSystemButton : byte
        {
            Return = 0,
            Save = 1
        }

        private enum SVideoSetting : byte
        {
            Resolution = 0,
            Fullscreen = 1,
            VSync = 2,
            Borderless = 3
        }
        
        private byte selectedSectionIndex = 0;
        private byte selectedLanguageIndex = 0;

        private SVideoSettings videoSettings;
        private SLanguageSettings languageSettings;

        private readonly Texture2D guiBackgroundTexture;

        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont digitalDiscoSpriteFont;

        private readonly string titleName = SLocalization.GUI_Menu_OptionsMenu_Title;

        private readonly string[] sectionNames = [
            SLocalization.GUI_Menu_OptionsMenu_Section_Video,
            SLocalization.GUI_Menu_OptionsMenu_Section_Language
        ];

        private readonly string[] systemButtonNames = [
            SLocalization.Statements_Return,
            SLocalization.Statements_Save,
        ];

        private readonly Action[] systemButtonActions;

        public SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");

            this.systemButtonActions = [
                ReturnButton,
                SaveButton
            ];

            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.DIGITAL_DISCO);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            SelectSection(0);

            // Load Settings
            LoadVideoSettings();
            LoadLanguageSettings();
        }

        private void LoadVideoSettings()
        {
            this.videoSettings = SSettingsManager.LoadSettings<SVideoSettings>();

            this.videoSectionOptionSelectors[(byte)SVideoSetting.Resolution].Select((uint)Array.IndexOf(SScreenConstants.RESOLUTIONS, this.videoSettings.Resolution));
            this.videoSectionOptionSelectors[(byte)SVideoSetting.Fullscreen].Select((uint)(this.videoSettings.FullScreen ? 1 : 0));
            this.videoSectionOptionSelectors[(byte)SVideoSetting.VSync].Select((uint)(this.videoSettings.VSync ? 1 : 0));
            this.videoSectionOptionSelectors[(byte)SVideoSetting.Borderless].Select((uint)(this.videoSettings.Borderless ? 1 : 0));

            this.videoSectionButtons[(byte)SVideoSetting.Resolution].SetTextualContent(this.videoSectionOptionSelectors[(byte)SVideoSetting.Resolution].ToString());
            this.videoSectionButtons[(byte)SVideoSetting.Fullscreen].SetTextualContent(this.videoSectionOptionSelectors[(byte)SVideoSetting.Fullscreen].ToString());
            this.videoSectionButtons[(byte)SVideoSetting.VSync].SetTextualContent(this.videoSectionOptionSelectors[(byte)SVideoSetting.VSync].ToString());
            this.videoSectionButtons[(byte)SVideoSetting.Borderless].SetTextualContent(this.videoSectionOptionSelectors[(byte)SVideoSetting.Borderless].ToString());
        }

        private void LoadLanguageSettings()
        {
            this.languageSettings = SSettingsManager.LoadSettings<SLanguageSettings>();
            this.selectedLanguageIndex = (byte)Array.FindIndex(SLocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.Language == this.languageSettings.Language );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // General
            UpdateButtons();

            // Sections
            switch ((SMenuSection)this.selectedSectionIndex)
            {
                case SMenuSection.Video:
                    UpdateVideoSection();
                    break;

                case SMenuSection.Language:
                    UpdateLanguageSection();
                    break;

                default:
                    break;
            }
        }

        private void UpdateButtons()
        {
            for (byte i = 0; i < this.sectionButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.sectionButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    SelectSection(i);
                }

                if (this.selectedSectionIndex.Equals(i))
                {
                    labelElement.Color = Color.Yellow;
                    continue;
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? Color.Yellow : Color.White;
            }

            for (byte i = 0; i < this.systemButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    this.systemButtonActions[i].Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? Color.Yellow : Color.White;
            }
        }

        private void UpdateVideoSection()
        {
            for (int i = 0; i < this.videoSectionButtons.Count; i++)
            {
                SGUILabelElement labelElement = this.videoSectionButtons[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    this.videoSectionOptionSelectors[i].Next();
                    labelElement.SetTextualContent(this.videoSectionOptionSelectors[i].ToString());
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? Color.Yellow : Color.White;
            }
        }

        private void UpdateLanguageSection()
        {
            for (byte i = 0; i < this.languageSectionButtons.Count; i++)
            {
                SGUILabelElement labelElement = this.languageSectionButtons[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    this.selectedLanguageIndex = i;
                }

                if (this.selectedLanguageIndex.Equals(i))
                {
                    labelElement.Color = Color.Yellow;
                    continue;
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? Color.Yellow : Color.White;
            }
        }

        private void SelectSection(byte index)
        {
            this.selectedSectionIndex = byte.Clamp(index, 0, (byte)(this.sectionNames.Length - 1));

            for (byte i = 0; i < this.sectionContainers.Length; i++)
            {
                if (this.selectedSectionIndex.Equals(i))
                {
                    this.sectionContainers[i].Active();
                    continue;
                }

                this.sectionContainers[i].Disable();
            }
        }
    }
}
