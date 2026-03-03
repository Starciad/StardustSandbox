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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class TemperatureSettingsUI
    {
        private enum TemperatureIndex : byte
        {
            None = 0,
            VeryCold = 1,
            Cold = 2,
            Normal = 3,
            Hot = 4,
            VeryHot = 5,
        }

        private sealed class Section
        {
            internal string Title => this.title;
            internal TimeSpan StartTime => this.startTime;
            internal TimeSpan EndTime => this.endTime;
            internal TemperatureIndex Index { get => this.index; set => this.index = value; }

            internal SlotInfo[] ButtonSlotInfos => this.buttonSlotInfos;
            internal ButtonInfo[] ButtonInfos => this.buttonInfos;

            private TemperatureIndex index;

            private readonly SlotInfo[] buttonSlotInfos;
            private readonly ButtonInfo[] buttonInfos;

            private readonly string title;
            private readonly TimeSpan startTime;
            private readonly TimeSpan endTime;

            internal Section(string title, TimeSpan startTime, TimeSpan endTime, World world)
            {
                this.title = title;
                this.startTime = startTime;
                this.endTime = endTime;

                this.buttonInfos =
                [
                    new(TextureIndex.IconUI, new(224, 192, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_None_Name, Localization_GUIs.TemperatureSettings_Temperature_None_Description, () =>
                    {
                        this.Index = TemperatureIndex.None;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_NONE_TEMPERATURE, false);
                    }),

                    new(TextureIndex.IconUI, new(0, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_VeryCold_Name, Localization_GUIs.TemperatureSettings_Temperature_VeryCold_Description, () =>
                    {
                        this.Index = TemperatureIndex.VeryCold;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_VERY_COLD_TEMPERATURE, true);
                    }),

                    new(TextureIndex.IconUI, new(32, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Cold_Name, Localization_GUIs.TemperatureSettings_Temperature_Cold_Description, () =>
                    {
                        this.Index = TemperatureIndex.Cold;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_COLD_TEMPERATURE, true);
                    }),

                    new(TextureIndex.IconUI, new(64, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Normal_Name, Localization_GUIs.TemperatureSettings_Temperature_Normal_Description, () =>
                    {
                        this.Index = TemperatureIndex.Normal;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_NORMAL_TEMPERATURE, true);
                    }),

                    new(TextureIndex.IconUI, new(96, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Hot_Name, Localization_GUIs.TemperatureSettings_Temperature_Hot_Description, () =>
                    {
                        this.Index = TemperatureIndex.Hot;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_HOT_TEMPERATURE, true);
                    }),

                    new(TextureIndex.IconUI, new(128, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_VeryHot_Name, Localization_GUIs.TemperatureSettings_Temperature_VeryHot_Description, () =>
                    {
                        this.Index = TemperatureIndex.VeryHot;
                        world.Temperature.SetTemperatureValue(this.StartTime, TemperatureConstants.WORLD_VERY_HOT_TEMPERATURE, true);
                    }),
                ];

                this.buttonSlotInfos = new SlotInfo[this.buttonInfos.Length];

                for (int i = 0; i < this.buttonInfos.Length; i++)
                {
                    SlotInfo buttonSlotInfo = UIBuilderUtility.BuildButtonSlot(new(0, 0), this.buttonInfos[i]);

                    buttonSlotInfo.Background.Alignment = UIDirection.Southwest;
                    buttonSlotInfo.Icon.Alignment = UIDirection.Center;

                    buttonSlotInfo.Icon.TextureIndex = this.buttonInfos[i].TextureIndex;
                    buttonSlotInfo.Icon.SourceRectangle = this.buttonInfos[i].TextureSourceRectangle;

                    buttonSlotInfo.Background.AddChild(buttonSlotInfo.Icon);

                    this.buttonSlotInfos[i] = buttonSlotInfo;
                }
            }
        }
    }
}
