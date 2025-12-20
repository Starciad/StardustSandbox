using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("VideoSettings")]
    public readonly struct StatusSettings : ISettingsModule
    {
        [XmlElement("TheRestartAfterSavingSettingsWarningWasDisplayed", typeof(bool))]
        public readonly bool TheRestartAfterSavingSettingsWarningWasDisplayed { get; init; }

        [XmlElement("TheMovementTutorialWasDisplayed", typeof(bool))]
        public readonly bool TheMovementTutorialWasDisplayed { get; init; }

        public StatusSettings()
        {
            this.TheRestartAfterSavingSettingsWarningWasDisplayed = false;
            this.TheMovementTutorialWasDisplayed = false;
        }

        public StatusSettings(StatusSettings statusSettings)
        {
            this.TheRestartAfterSavingSettingsWarningWasDisplayed = statusSettings.TheRestartAfterSavingSettingsWarningWasDisplayed;
            this.TheMovementTutorialWasDisplayed = statusSettings.TheMovementTutorialWasDisplayed;
        }
    }
}
