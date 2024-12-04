using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Localization.Resources;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu : SGUISystem
    {
        private enum SMenuSection : byte
        {
            General = 0,
            Video = 1,
            Volume = 2,
            Cursor = 3,
            Language = 4
        }

        private enum SSystemButton : byte
        {
            Return = 0,
            Save = 1
        }

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;

        private readonly string titleName;
        private readonly string[] sectionNames;
        private readonly string[] systemButtonNames;

        private byte selectedSectionIndex;

        public SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");

            this.titleName = SLocalization.GUI_Menu_OptionsMenu_Title;
            this.sectionNames = [
                SLocalization.GUI_Menu_OptionsMenu_Section_General,
                SLocalization.GUI_Menu_OptionsMenu_Section_Video,
                SLocalization.GUI_Menu_OptionsMenu_Section_Volume,
                SLocalization.GUI_Menu_OptionsMenu_Section_Cursor,
                SLocalization.GUI_Menu_OptionsMenu_Section_Language
            ];
            this.systemButtonNames = [
                SLocalization.Statements_Return,
                SLocalization.Statements_Save,
            ];
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            SelectSection(0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            for (byte i = 0; i < this.sectionButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.sectionButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetMeasureStringSize()))
                {
                    SelectSection(i);
                }

                if (this.selectedSectionIndex.Equals(i))
                {
                    labelElement.Color = Color.Yellow;
                    continue;
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetMeasureStringSize()) ? Color.Yellow : Color.White;
            }

            for (byte i = 0; i < this.systemButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetMeasureStringSize()))
                {
                    // (Actions will still be added.)
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetMeasureStringSize()) ? Color.Yellow : Color.White;
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
