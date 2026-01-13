/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Collections;

namespace StardustSandbox.WorldSystem
{
    public sealed class Slot : IPoolableObject
    {
        internal bool IsEmpty => this.IsBackgroundEmpty && this.IsForegroundEmpty;

        internal bool IsForegroundEmpty => this.Foreground.IsEmpty;
        internal bool IsBackgroundEmpty => this.Background.IsEmpty;

        internal Point Position => this.position;

        internal SlotLayer Foreground => this.foreground;
        internal SlotLayer Background => this.background;

        private Point position;

        private readonly SlotLayer foreground = new();
        private readonly SlotLayer background = new();

        internal SlotLayer GetLayer(in Layer layer)
        {
            return layer switch
            {
                Layer.Foreground => this.foreground,
                Layer.Background => this.background,
                _ => null,
            };
        }

        internal void SetPosition(in Point position)
        {
            this.position = position;
        }

        internal void Instantiate(in Layer layer, in ElementIndex index)
        {
            GetLayer(layer).Instantiate(index);
        }

        internal void Destroy(in Layer layer)
        {
            GetLayer(layer).Destroy();
        }

        internal void Copy(in Layer layer, in SlotLayer valueToCopy)
        {
            GetLayer(layer).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(in Layer layer, in float value)
        {
            GetLayer(layer).SetTemperatureValue(value);
        }

        internal void SetColorModifier(in Layer layer, in Color value)
        {
            GetLayer(layer).SetColorModifier(value);
        }

        internal void SetStoredElement(in Layer layer, in ElementIndex index)
        {
            GetLayer(layer).SetStoredElement(index);
        }

        internal bool HasState(in Layer layer, in ElementStates value)
        {
            return GetLayer(layer).HasState(value);
        }

        internal void SetState(in Layer layer, in ElementStates value)
        {
            GetLayer(layer).SetState(value);
        }

        internal void RemoveState(in Layer layer, in ElementStates value)
        {
            GetLayer(layer).RemoveState(value);
        }

        internal void ClearStates(in Layer layer)
        {
            GetLayer(layer).ClearStates();
        }

        internal void ToggleState(in Layer layer, in ElementStates value)
        {
            GetLayer(layer).ToggleState(value);
        }

        internal void Reset(in Layer layer)
        {
            GetLayer(layer).Reset();
        }

        public void Reset()
        {
            this.foreground.Reset();
            this.background.Reset();
        }
    }
}
