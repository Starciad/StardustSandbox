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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.WorldSystem;
using StardustSandbox.Core.WorldSystem.Components;
using StardustSandbox.Core.WorldSystem.Handlers;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements
{
    internal sealed class ElementContext
    {
        internal Layer Layer => this.layer;
        internal Point Position => this.position;
        internal Slot Slot => this.slot;

        private Layer layer;
        private Point position;
        private Slot slot;

        private readonly ChunkHandler chunkHandler;
        private readonly ExplosionHandler explosionHandler;
        private readonly TileMap tileMap;
        private readonly World world;

        internal ElementContext(World world)
        {
            this.chunkHandler = world.ChunkHandler;
            this.explosionHandler = world.ExplosionHandler;
            this.tileMap = world.TileMap;
            this.world = world;
        }

        internal void Initialize(Point position, Layer layer)
        {
            this.layer = layer;
            this.position = position;

            if (this.tileMap.IsWithinBounds(position))
            {
                this.slot = this.tileMap[position];
                this.slot.Position = position;
            }
        }

        #region UTILITIES

        // All utility methods defined within each of the regions below are arranged
        // alphabetically. For sets of methods with overloads, the organization consists
        // of grouping them in descending order based on the number of parameters.
        // Finally, methods following the Try-Catch pattern must always appear first,
        // followed by standard methods.

        #region WORLD

        internal Point GetWorldSize()
        {
            return this.tileMap.Size;
        }
        internal Temperature GetWorldTemperature()
        {
            return this.world.Temperature;
        }

        #endregion

        #region ELEMENTS

        #region Try Methods

        internal bool TryDestroy(Point position, Layer layer)
        {
            return this.tileMap.TryDestroy(position, layer);
        }
        internal bool TryDestroy(Point position)
        {
            return TryDestroy(position, this.layer);
        }
        internal bool TryDestroy()
        {
            return TryDestroy(this.position);
        }

        internal bool TryGetColorModifier(Point position, Layer layer, out Color value)
        {
            return this.tileMap.TryGetColorModifier(position, layer, out value);
        }
        internal bool TryGetColorModifier(Point position, out Color value)
        {
            return TryGetColorModifier(position, this.layer, out value);
        }
        internal bool TryGetColorModifier(out Color value)
        {
            return TryGetColorModifier(this.position, out value);
        }

        internal bool TryGetDissipatingState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetDissipatingState(position, layer, out value);
        }
        internal bool TryGetDissipatingState(Point position, out bool value)
        {
            return TryGetDissipatingState(position, this.layer, out value);
        }
        internal bool TryGetDissipatingState(out bool value)
        {
            return TryGetDissipatingState(this.position, out value);
        }

        internal bool TryGetElement(Point position, Layer layer, out Element value)
        {
            return this.tileMap.TryGetElement(position, layer, out value);
        }
        internal bool TryGetElement(Point position, out Element value)
        {
            return TryGetElement(position, this.layer, out value);
        }
        internal bool TryGetElement(out Element value)
        {
            return TryGetElement(this.position, out value);
        }

        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex value)
        {
            return this.tileMap.TryGetElementIndex(position, layer, out value);
        }
        internal bool TryGetElementIndex(Point position, out ElementIndex value)
        {
            return TryGetElementIndex(position, this.layer, out value);
        }
        internal bool TryGetElementIndex(out ElementIndex value)
        {
            return TryGetElementIndex(this.position, out value);
        }

        internal bool TryGetFallingState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetFallingState(position, layer, out value);
        }
        internal bool TryGetFallingState(Point position, out bool value)
        {
            return TryGetFallingState(position, this.layer, out value);
        }
        internal bool TryGetFallingState(out bool value)
        {
            return TryGetFallingState(this.position, out value);
        }

        internal bool TryGetPushedState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetPushedState(position, layer, out value);
        }
        internal bool TryGetPushedState(Point position, out bool value)
        {
            return TryGetPushedState(position, this.layer, out value);
        }
        internal bool TryGetPushedState(out bool value)
        {
            return TryGetPushedState(this.position, out value);
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            return this.tileMap.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.position, out value);
        }

        internal bool TryGetStepCycleFlag(Point position, Layer layer, out UpdateCycleFlag value)
        {
            return this.tileMap.TryGetStepCycleFlag(position, layer, out value);
        }
        internal bool TryGetStepCycleFlag(Point position, out UpdateCycleFlag value)
        {
            return TryGetStepCycleFlag(position, this.layer, out value);
        }
        internal bool TryGetStepCycleFlag(out UpdateCycleFlag value)
        {
            return TryGetStepCycleFlag(this.position, out value);
        }

        internal bool TryGetStoredElement(Point position, Layer layer, out Element value)
        {
            return this.tileMap.TryGetStoredElement(position, layer, out value);
        }
        internal bool TryGetStoredElement(Point position, out Element value)
        {
            return TryGetStoredElement(position, this.layer, out value);
        }
        internal bool TryGetStoredElement(out Element value)
        {
            return TryGetStoredElement(this.position, out value);
        }

        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex value)
        {
            return this.tileMap.TryGetStoredElementIndex(position, layer, out value);
        }
        internal bool TryGetStoredElementIndex(Point position, out ElementIndex value)
        {
            return TryGetStoredElementIndex(position, this.layer, out value);
        }
        internal bool TryGetStoredElementIndex(out ElementIndex value)
        {
            return TryGetStoredElementIndex(this.position, out value);
        }

        internal bool TryGetTemperature(Point position, Layer layer, out float value)
        {
            return this.tileMap.TryGetTemperature(position, layer, out value);
        }
        internal bool TryGetTemperature(Point position, out float value)
        {
            return TryGetTemperature(position, this.layer, out value);
        }
        internal bool TryGetTemperature(out float value)
        {
            return TryGetTemperature(this.position, out value);
        }

        internal bool TryHasElement(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryHasElement(position, layer, out value);
        }
        internal bool TryHasElement(Point position, out bool value)
        {
            return TryHasElement(position, this.layer, out value);
        }
        internal bool TryHasElement(out bool value)
        {
            return TryHasElement(this.position, out value);
        }

        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryHasStoredElement(position, layer, out value);
        }
        internal bool TryHasStoredElement(Point position, out bool value)
        {
            return TryHasStoredElement(position, this.layer, out value);
        }
        internal bool TryHasStoredElement(out bool value)
        {
            return TryHasStoredElement(this.position, out value);
        }

        internal bool TryInstantiate(Point position, Layer layer, ElementIndex value)
        {
            return this.tileMap.TryInstantiate(position, layer, value);
        }
        internal bool TryInstantiate(Point position, ElementIndex value)
        {
            return TryInstantiate(position, this.layer, value);
        }
        internal bool TryInstantiate(ElementIndex value)
        {
            return TryInstantiate(this.position, value);
        }

        internal bool TryRemove(Point position, Layer layer)
        {
            return this.tileMap.TryRemove(position, layer);
        }
        internal bool TryRemove(Point position)
        {
            return TryRemove(position, this.layer);
        }
        internal bool TryRemove()
        {
            return TryRemove(this.position);
        }

        internal bool TryReplace(Point position, Layer layer, ElementIndex value)
        {
            return this.tileMap.TryReplace(position, layer, value);
        }
        internal bool TryReplace(Point position, ElementIndex value)
        {
            return TryReplace(position, this.layer, value);
        }
        internal bool TryReplace(ElementIndex value)
        {
            return TryReplace(this.position, value);
        }

        internal bool TrySetColorModifier(Point position, Layer layer, Color value)
        {
            return this.tileMap.TrySetColorModifier(position, layer, value);
        }
        internal bool TrySetColorModifier(Point position, Color value)
        {
            return TrySetColorModifier(position, this.layer, value);
        }
        internal bool TrySetColorModifier(Color value)
        {
            return TrySetColorModifier(this.position, value);
        }

        internal bool TrySetDissipatingState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetDissipatingState(position, layer, value);
        }
        internal bool TrySetDissipatingState(Point position, bool value)
        {
            return TrySetDissipatingState(position, this.layer, value);
        }
        internal bool TrySetDissipatingState(bool value)
        {
            return TrySetDissipatingState(this.position, value);
        }

        internal bool TrySetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            return this.tileMap.TrySetElementIndex(position, layer, value);
        }
        internal bool TrySetElementIndex(Point position, ElementIndex value)
        {
            return TrySetElementIndex(position, this.layer, value);
        }
        internal bool TrySetElementIndex(ElementIndex value)
        {
            return TrySetElementIndex(this.position, value);
        }

        internal bool TrySetFallingState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetFallingState(position, layer, value);
        }
        internal bool TrySetFallingState(Point position, bool value)
        {
            return TrySetFallingState(position, this.layer, value);
        }
        internal bool TrySetFallingState(bool value)
        {
            return TrySetFallingState(this.position, value);
        }

        internal bool TrySetPosition(Point newPosition, Layer layer)
        {
            if (this.tileMap.TryUpdatePosition(this.position, newPosition, layer))
            {
                this.slot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.layer);
        }

        internal bool TrySetPushedState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetPushedState(position, layer, value);
        }
        internal bool TrySetPushedState(Point position, bool value)
        {
            return TrySetPushedState(position, this.layer, value);
        }
        internal bool TrySetPushedState(bool value)
        {
            return TrySetPushedState(this.position, value);
        }

        internal bool TrySetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value)
        {
            return this.tileMap.TrySetStepCycleFlag(position, layer, value);
        }
        internal bool TrySetStepCycleFlag(Point position, UpdateCycleFlag value)
        {
            return TrySetStepCycleFlag(position, this.layer, value);
        }
        internal bool TrySetStepCycleFlag(UpdateCycleFlag value)
        {
            return TrySetStepCycleFlag(this.position, value);
        }

        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex value)
        {
            return this.tileMap.TrySetStoredElementIndex(position, layer, value);
        }
        internal bool TrySetStoredElementIndex(Point position, ElementIndex value)
        {
            return TrySetStoredElementIndex(position, this.layer, value);
        }
        internal bool TrySetStoredElementIndex(ElementIndex value)
        {
            return TrySetStoredElementIndex(this.position, value);
        }

        internal bool TrySetTemperature(Point position, Layer layer, float value)
        {
            return this.tileMap.TrySetTemperature(position, layer, value);
        }
        internal bool TrySetTemperature(Point position, float value)
        {
            return TrySetTemperature(position, this.layer, value);
        }
        internal bool TrySetTemperature(float value)
        {
            return TrySetTemperature(this.position, value);
        }

        internal bool TrySwap(Point position1, Point position2, Layer layer)
        {
            return this.tileMap.TrySwap(position1, position2, layer);
        }
        internal bool TrySwap(Point position1, Point position2)
        {
            return TrySwap(position1, position2, this.layer);
        }
        internal bool TrySwap(Point targetPosition)
        {
            return TrySwap(this.position, targetPosition);
        }

        internal bool TryUpdatePosition(Point oldPosition, Point newPosition, Layer layer)
        {
            return this.tileMap.TryUpdatePosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdatePosition(Point oldPosition, Point newPosition)
        {
            return TryUpdatePosition(oldPosition, newPosition, this.layer);
        }
        internal bool TryUpdatePosition(Point newPosition)
        {
            return TryUpdatePosition(this.position, newPosition);
        }

        #endregion

        #region Action Methods

        internal void Destroy(Point position, Layer layer)
        {
            this.tileMap.Destroy(position, layer);
        }
        internal void Destroy(Point position)
        {
            Destroy(position, this.layer);
        }
        internal void Destroy()
        {
            Destroy(this.position);
        }

        internal Color GetColorModifier(Point position, Layer layer)
        {
            return this.tileMap.GetColorModifier(position, layer);
        }
        internal Color GetColorModifier(Point position)
        {
            return GetColorModifier(position, this.layer);
        }
        internal Color GetColorModifier()
        {
            return GetColorModifier(this.position, this.layer);
        }

        internal bool GetDissipatingState(Point position, Layer layer)
        {
            return this.tileMap.GetDissipatingState(position, layer);
        }
        internal bool GetDissipatingState(Point position)
        {
            return GetDissipatingState(position, this.layer);
        }
        internal bool GetDissipatingState()
        {
            return GetDissipatingState(this.position, this.layer);
        }

        internal Element GetElement(Point position, Layer layer)
        {
            return this.tileMap.GetElement(position, layer);
        }
        internal Element GetElement(Point position)
        {
            return GetElement(position, this.layer);
        }
        internal Element GetElement()
        {
            return GetElement(this.position);
        }

        internal ElementIndex GetElementIndex(Point position, Layer layer)
        {
            return this.tileMap.GetElementIndex(position, layer);
        }
        internal ElementIndex GetElementIndex(Point position)
        {
            return GetElementIndex(position, this.layer);
        }
        internal ElementIndex GetElementIndex()
        {
            return GetElementIndex(this.position);
        }

        internal bool GetFallingState(Point position, Layer layer)
        {
            return this.tileMap.GetFallingState(position, layer);
        }
        internal bool GetFallingState(Point position)
        {
            return GetFallingState(position, this.layer);
        }
        internal bool GetFallingState()
        {
            return GetFallingState(this.position, this.layer);
        }

        internal ElementNeighbors GetNeighboringSlots(Point position)
        {
            return this.tileMap.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.position);
        }

        internal bool GetPushedState(Point position, Layer layer)
        {
            return this.tileMap.GetPushedState(position, layer);
        }
        internal bool GetPushedState(Point position)
        {
            return GetPushedState(position, this.layer);
        }
        internal bool GetPushedState()
        {
            return GetPushedState(this.position, this.layer);
        }

        internal Slot GetSlot(Point position)
        {
            return this.tileMap.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.position);
        }

        internal UpdateCycleFlag GetStepCycleFlag(Point position, Layer layer)
        {
            return this.tileMap.GetStepCycleFlag(position, layer);
        }
        internal UpdateCycleFlag GetStepCycleFlag(Point position)
        {
            return GetStepCycleFlag(position, this.layer);
        }
        internal UpdateCycleFlag GetStepCycleFlag()
        {
            return GetStepCycleFlag(this.position, this.layer);
        }

        internal Element GetStoredElement(Point position, Layer layer)
        {
            return this.tileMap.GetStoredElement(position, layer);
        }
        internal Element GetStoredElement(Point position)
        {
            return GetStoredElement(position, this.layer);
        }
        internal Element GetStoredElement()
        {
            return GetStoredElement(this.position);
        }

        internal ElementIndex GetStoredElementIndex(Point position, Layer layer)
        {
            return this.tileMap.GetStoredElementIndex(position, layer);
        }
        internal ElementIndex GetStoredElementIndex(Point position)
        {
            return GetStoredElementIndex(position, this.layer);
        }
        internal ElementIndex GetStoredElementIndex()
        {
            return GetStoredElementIndex(this.position);
        }

        internal float GetTemperature(Point position, Layer layer)
        {
            return this.tileMap.GetTemperature(position, layer);
        }
        internal float GetTemperature(Point position)
        {
            return GetTemperature(position, this.layer);
        }
        internal float GetTemperature()
        {
            return GetTemperature(this.position);
        }

        internal void Instantiate(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.Instantiate(position, layer, index);
        }
        internal void Instantiate(Point position, ElementIndex index)
        {
            Instantiate(position, this.layer, index);
        }
        internal void Instantiate(ElementIndex index)
        {
            Instantiate(this.position, index);
        }

        internal void Remove(Point position, Layer layer)
        {
            this.tileMap.Remove(position, layer);
        }
        internal void Remove(Point position)
        {
            Remove(position, this.layer);
        }
        internal void Remove()
        {
            Remove(this.position, this.layer);
        }

        internal void Replace(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.Replace(position, layer, index);
        }
        internal void Replace(Point position, ElementIndex index)
        {
            Replace(position, this.layer, index);
        }
        internal void Replace(ElementIndex index)
        {
            Replace(this.position, index);
        }

        internal bool HasElement(Point position, Layer layer)
        {
            return this.tileMap.HasElement(position, layer);
        }
        internal bool HasElement(Point position)
        {
            return HasElement(position, this.layer);
        }
        internal bool HasElement()
        {
            return HasElement(this.position);
        }

        internal bool HasStoredElement(Point position, Layer layer)
        {
            return this.tileMap.HasStoredElement(position, layer);
        }
        internal bool HasStoredElement(Point position)
        {
            return HasStoredElement(position, this.layer);
        }
        internal bool HasStoredElement()
        {
            return HasStoredElement(this.position);
        }

        internal void SetPosition(Point newPosition, Layer layer)
        {
            if (this.tileMap.TryUpdatePosition(this.position, newPosition, layer))
            {
                this.slot = GetSlot(newPosition);
            }
        }
        internal void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.layer);
        }

        internal void SetColorModifier(Point position, Layer layer, Color value)
        {
            this.tileMap.SetColorModifier(position, layer, value);
        }
        internal void SetColorModifier(Point position, Color value)
        {
            SetColorModifier(position, this.layer, value);
        }
        internal void SetColorModifier(Color value)
        {
            SetColorModifier(this.position, value);
        }

        internal void SetDissipatingState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetDissipatingState(position, layer, value);
        }
        internal void SetDissipatingState(Point position, bool value)
        {
            SetDissipatingState(position, this.layer, value);
        }
        internal void SetDissipatingState(bool value)
        {
            SetDissipatingState(this.position, value);
        }

        internal void SetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            this.tileMap.SetElementIndex(position, layer, value);
        }
        internal void SetElementIndex(Point position, ElementIndex value)
        {
            SetElementIndex(position, this.layer, value);
        }
        internal void SetElementIndex(ElementIndex value)
        {
            SetElementIndex(this.position, value);
        }

        internal void SetFallingState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetFallingState(position, layer, value);
        }
        internal void SetFallingState(Point position, bool value)
        {
            SetFallingState(position, this.layer, value);
        }
        internal void SetFallingState(bool value)
        {
            SetFallingState(this.position, value);
        }

        internal void SetPushedState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetPushedState(position, layer, value);
        }
        internal void SetPushedState(Point position, bool value)
        {
            SetPushedState(position, this.layer, value);
        }
        internal void SetPushedState(bool value)
        {
            SetPushedState(this.position, value);
        }

        internal void SetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value)
        {
            this.tileMap.SetStepCycleFlag(position, layer, value);
        }
        internal void SetStepCycleFlag(Point position, UpdateCycleFlag value)
        {
            SetStepCycleFlag(position, this.layer, value);
        }
        internal void SetStepCycleFlag(UpdateCycleFlag value)
        {
            SetStepCycleFlag(this.position, value);
        }

        internal void SetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.SetStoredElementIndex(position, layer, index);
        }
        internal void SetStoredElementIndex(Point position, ElementIndex index)
        {
            SetStoredElementIndex(position, this.layer, index);
        }
        internal void SetStoredElementIndex(ElementIndex index)
        {
            SetStoredElementIndex(this.position, index);
        }

        internal void SetTemperature(Point position, Layer layer, float value)
        {
            this.tileMap.SetTemperature(position, layer, value);
        }
        internal void SetTemperature(Point position, float value)
        {
            SetTemperature(position, this.layer, value);
        }
        internal void SetTemperature(float value)
        {
            SetTemperature(this.position, value);
        }

        internal void Swap(Point position1, Point position2, Layer layer)
        {
            this.tileMap.Swap(position1, position2, layer);
        }
        internal void Swap(Point position1, Point position2)
        {
            Swap(position1, position2, this.layer);
        }
        internal void Swap(Point targetPosition)
        {
            Swap(this.position, targetPosition);
        }

        internal void UpdatePosition(Point oldPosition, Point newPosition, Layer layer)
        {
            this.tileMap.UpdatePosition(oldPosition, newPosition, layer);
        }
        internal void UpdatePosition(Point oldPosition, Point newPosition)
        {
            UpdatePosition(oldPosition, newPosition, this.layer);
        }
        internal void UpdatePosition(Point newPosition)
        {
            UpdatePosition(this.position, newPosition);
        }

        #endregion

        #endregion

        #region CHUNKING

        internal bool TryGetChunkUpdateState(Point position, out bool value)
        {
            return this.chunkHandler.TryGetChunkUpdateState(position, out value);
        }
        internal bool TryNotifyChunk(Point position)
        {
            return this.chunkHandler.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.position);
        }

        internal bool GetChunkUpdateState(Point position)
        {
            return this.chunkHandler.GetChunkUpdateState(position);
        }
        internal void NotifyChunk(Point position)
        {
            this.chunkHandler.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiate(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            return this.explosionHandler.TryInstantiate(position, layer, explosionBuilder);
        }
        internal bool TryInstantiate(Point position, ExplosionBuilder explosionBuilder)
        {
            return TryInstantiate(position, this.layer, explosionBuilder);
        }
        internal bool TryInstantiate(ExplosionBuilder explosionBuilder)
        {
            return TryInstantiate(this.position, explosionBuilder);
        }

        internal void Instantiate(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            this.explosionHandler.Instantiate(position, layer, explosionBuilder);
        }
        internal void Instantiate(Point position, ExplosionBuilder explosionBuilder)
        {
            Instantiate(position, this.layer, explosionBuilder);
        }
        internal void Instantiate(ExplosionBuilder explosionBuilder)
        {
            Instantiate(this.position, explosionBuilder);
        }

        #endregion

        #endregion
    }
}
