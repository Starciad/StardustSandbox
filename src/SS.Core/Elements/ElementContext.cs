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
        internal Layer CurrentLayer { get; private set; }
        internal Point CurrentPosition { get; private set; }
        internal Slot CurrentSlot { get; private set; }

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
            this.CurrentLayer = layer;
            this.CurrentPosition = position;

            if (this.tileMap.IsWithinBounds(position))
            {
                this.CurrentSlot = this.tileMap[position];
                this.CurrentSlot.Position = position;
            }
        }

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

        internal bool TrySetPosition(Point newPosition, Layer layer)
        {
            if (this.tileMap.TryUpdateElementPosition(this.CurrentPosition, newPosition, layer))
            {
                this.CurrentSlot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.CurrentLayer);
        }

        internal bool TryInstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return this.tileMap.TryInstantiateElementIndex(position, layer, index);
        }
        internal bool TryInstantiateElementIndex(Point position, ElementIndex index)
        {
            return TryInstantiateElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TryInstantiateElementIndex(ElementIndex index)
        {
            return TryInstantiateElementIndex(this.CurrentPosition, index);
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            return this.tileMap.TryUpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition)
        {
            return TryUpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            return this.tileMap.TrySwappingElements(element1Position, element2Position, layer);
        }
        internal bool TrySwappingElements(Point element1Position, Point element2Position)
        {
            return TrySwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(this.CurrentPosition, targetPosition);
        }

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            return this.tileMap.TryDestroyElement(position, layer);
        }
        internal bool TryDestroyElement(Point position)
        {
            return TryDestroyElement(position, this.CurrentLayer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.CurrentPosition);
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            return this.tileMap.TryRemoveElement(position, layer);
        }
        internal bool TryRemoveElement(Point position)
        {
            return TryRemoveElement(position, this.CurrentLayer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.CurrentPosition);
        }

        internal bool TryReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return this.tileMap.TryReplaceElementIndex(position, layer, index);
        }
        internal bool TryReplaceElementIndex(Point position, ElementIndex index)
        {
            return TryReplaceElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TryReplaceElementIndex(ElementIndex index)
        {
            return TryReplaceElementIndex(this.CurrentPosition, index);
        }

        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            return this.tileMap.TryGetElementIndex(position, layer, out index);
        }
        internal bool TryGetElementIndex(Point position, out ElementIndex index)
        {
            return TryGetElementIndex(position, this.CurrentLayer, out index);
        }
        internal bool TryGetElementIndex(out ElementIndex index)
        {
            return TryGetElementIndex(this.CurrentPosition, out index);
        }

        internal bool TryGetElement(Point position, Layer layer, out Element element)
        {
            return this.tileMap.TryGetElement(position, layer, out element);
        }
        internal bool TryGetElement(Point position, out Element element)
        {
            return TryGetElement(position, this.CurrentLayer, out element);
        }
        internal bool TryGetElement(out Element element)
        {
            return TryGetElement(this.CurrentPosition, out element);
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            return this.tileMap.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.CurrentPosition, out value);
        }

        internal bool TrySetElementTemperature(Point position, Layer layer, float value)
        {
            return this.tileMap.TrySetElementTemperature(position, layer, value);
        }
        internal bool TrySetElementTemperature(Point position, float value)
        {
            return TrySetElementTemperature(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementTemperature(float value)
        {
            return TrySetElementTemperature(this.CurrentPosition, value);
        }

        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            return this.tileMap.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(Point position, Color value)
        {
            return TrySetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementColorModifier(Color value)
        {
            return TrySetElementColorModifier(this.CurrentPosition, value);
        }

        internal bool TryHasElement(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryHasElement(position, layer, out value);
        }
        internal bool TryHasElement(Point position, out bool value)
        {
            return TryHasElement(position, this.CurrentLayer, out value);
        }
        internal bool TryHasElement(out bool value)
        {
            return TryHasElement(this.CurrentPosition, out value);
        }

        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryHasStoredElement(position, layer, out value);
        }
        internal bool TryHasStoredElement(Point position, out bool value)
        {
            return TryHasStoredElement(position, this.CurrentLayer, out value);
        }
        internal bool TryHasStoredElement(out bool value)
        {
            return TryHasStoredElement(this.CurrentPosition, out value);
        }

        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return this.tileMap.TrySetStoredElementIndex(position, layer, index);
        }
        internal bool TrySetStoredElementIndex(Point position, ElementIndex index)
        {
            return TrySetStoredElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TrySetStoredElementIndex(ElementIndex index)
        {
            return TrySetStoredElementIndex(this.CurrentPosition, index);
        }

        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            return this.tileMap.TryGetStoredElementIndex(position, layer, out index);
        }
        internal bool TryGetStoredElementIndex(Point position, out ElementIndex index)
        {
            return TryGetStoredElementIndex(position, this.CurrentLayer, out index);
        }
        internal bool TryGetStoredElementIndex(out ElementIndex index)
        {
            return TryGetStoredElementIndex(this.CurrentPosition, out index);
        }

        internal bool TryGetStoredElement(Point position, Layer layer, out Element element)
        {
            return this.tileMap.TryGetStoredElement(position, layer, out element);
        }
        internal bool TryGetStoredElement(Point position, out Element element)
        {
            return TryGetStoredElement(position, this.CurrentLayer, out element);
        }
        internal bool TryGetStoredElement(out Element element)
        {
            return TryGetStoredElement(this.CurrentPosition, out element);
        }

        internal bool TryGetColorModifier(Point position, Layer layer, out Color value)
        {
            return this.tileMap.TryGetColorModifier(position, layer, out value);
        }
        internal bool TryGetColorModifier(Point position, out Color value)
        {
            return TryGetColorModifier(position, this.CurrentLayer, out value);
        }
        internal bool TryGetColorModifier(out Color value)
        {
            return TryGetColorModifier(this.CurrentPosition, out value);
        }

        internal bool TryGetDissipatingState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetDissipatingState(position, layer, out value);
        }
        internal bool TryGetDissipatingState(Point position, out bool value)
        {
            return TryGetDissipatingState(position, this.CurrentLayer, out value);
        }
        internal bool TryGetDissipatingState(out bool value)
        {
            return TryGetDissipatingState(this.CurrentPosition, out value);
        }

        internal bool TryGetFallingState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetFallingState(position, layer, out value);
        }
        internal bool TryGetFallingState(Point position, out bool value)
        {
            return TryGetFallingState(position, this.CurrentLayer, out value);
        }
        internal bool TryGetFallingState(out bool value)
        {
            return TryGetFallingState(this.CurrentPosition, out value);
        }

        internal bool TryGetPushedState(Point position, Layer layer, out bool value)
        {
            return this.tileMap.TryGetPushedState(position, layer, out value);
        }
        internal bool TryGetPushedState(Point position, out bool value)
        {
            return TryGetPushedState(position, this.CurrentLayer, out value);
        }
        internal bool TryGetPushedState(out bool value)
        {
            return TryGetPushedState(this.CurrentPosition, out value);
        }

        internal bool TryGetStepCycleFlag(Point position, Layer layer, out UpdateCycleFlag value)
        {
            return this.tileMap.TryGetStepCycleFlag(position, layer, out value);
        }
        internal bool TryGetStepCycleFlag(Point position, out UpdateCycleFlag value)
        {
            return TryGetStepCycleFlag(position, this.CurrentLayer, out value);
        }
        internal bool TryGetStepCycleFlag(out UpdateCycleFlag value)
        {
            return TryGetStepCycleFlag(this.CurrentPosition, out value);
        }

        internal bool TrySetDissipatingState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetDissipatingState(position, layer, value);
        }
        internal bool TrySetDissipatingState(Point position, bool value)
        {
            return TrySetDissipatingState(position, this.CurrentLayer, value);
        }
        internal bool TrySetDissipatingState(bool value)
        {
            return TrySetDissipatingState(this.CurrentPosition, value);
        }

        internal bool TrySetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            return this.tileMap.TrySetElementIndex(position, layer, value);
        }
        internal bool TrySetElementIndex(Point position, ElementIndex value)
        {
            return TrySetElementIndex(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementIndex(ElementIndex value)
        {
            return TrySetElementIndex(this.CurrentPosition, value);
        }

        internal bool TrySetFallingState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetFallingState(position, layer, value);
        }
        internal bool TrySetFallingState(Point position, bool value)
        {
            return TrySetFallingState(position, this.CurrentLayer, value);
        }
        internal bool TrySetFallingState(bool value)
        {
            return TrySetFallingState(this.CurrentPosition, value);
        }

        internal bool TrySetPushedState(Point position, Layer layer, bool value)
        {
            return this.tileMap.TrySetPushedState(position, layer, value);
        }
        internal bool TrySetPushedState(Point position, bool value)
        {
            return TrySetPushedState(position, this.CurrentLayer, value);
        }
        internal bool TrySetPushedState(bool value)
        {
            return TrySetPushedState(this.CurrentPosition, value);
        }

        internal bool TrySetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value) 
        {
            return this.tileMap.TrySetStepCycleFlag(position, layer, value);
        }
        internal bool TrySetStepCycleFlag(Point position, UpdateCycleFlag value)
        {
            return TrySetStepCycleFlag(position, this.CurrentLayer, value);
        }
        internal bool TrySetStepCycleFlag(UpdateCycleFlag value)
        {
            return TrySetStepCycleFlag(this.CurrentPosition, value);
        }

        #endregion

        #region Internal Methods

        internal void SetPosition(Point newPosition, Layer layer)
        {
            _ = TrySetPosition(newPosition, layer);
        }
        internal void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.CurrentLayer);
        }

        internal void InstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.InstantiateElementIndex(position, layer, index);
        }
        internal void InstantiateElementIndex(Point position, ElementIndex index)
        {
            InstantiateElementIndex(position, this.CurrentLayer, index);
        }
        internal void InstantiateElementIndex(ElementIndex index)
        {
            InstantiateElementIndex(this.CurrentPosition, index);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            this.tileMap.UpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal void UpdateElementPosition(Point oldPosition, Point newPosition)
        {
            UpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            this.tileMap.SwappingElements(element1Position, element2Position, layer);
        }
        internal void SwappingElements(Point element1Position, Point element2Position)
        {
            SwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal void SwappingElements(Point targetPosition)
        {
            SwappingElements(this.CurrentPosition, targetPosition);
        }

        internal void DestroyElement(Point position, Layer layer)
        {
            this.tileMap.DestroyElement(position, layer);
        }
        internal void DestroyElement(Point position)
        {
            DestroyElement(position, this.CurrentLayer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.CurrentPosition);
        }

        internal void RemoveElement(Point position, Layer layer)
        {
            this.tileMap.RemoveElement(position, layer);
        }
        internal void RemoveElement(Point position)
        {
            RemoveElement(position, this.CurrentLayer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.CurrentPosition, this.CurrentLayer);
        }

        internal void ReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.ReplaceElementIndex(position, layer, index);
        }
        internal void ReplaceElementIndex(Point position, ElementIndex index)
        {
            ReplaceElementIndex(position, this.CurrentLayer, index);
        }
        internal void ReplaceElementIndex(ElementIndex index)
        {
            ReplaceElementIndex(this.CurrentPosition, index);
        }

        internal void SetElementTemperature(Point position, Layer layer, float value)
        {
            this.tileMap.SetElementTemperature(position, layer, value);
        }
        internal void SetElementTemperature(Point position, float value)
        {
            SetElementTemperature(position, this.CurrentLayer, value);
        }
        internal void SetElementTemperature(float value)
        {
            SetElementTemperature(this.CurrentPosition, value);
        }

        internal void SetElementColorModifier(Point position, Layer layer, Color value)
        {
            this.tileMap.SetElementColorModifier(position, layer, value);
        }
        internal void SetElementColorModifier(Point position, Color value)
        {
            SetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal void SetElementColorModifier(Color value)
        {
            SetElementColorModifier(this.CurrentPosition, value);
        }

        internal void SetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            this.tileMap.SetStoredElementIndex(position, layer, index);
        }
        internal void SetStoredElementIndex(Point position, ElementIndex index)
        {
            SetStoredElementIndex(position, this.CurrentLayer, index);
        }
        internal void SetStoredElementIndex(ElementIndex index)
        {
            SetStoredElementIndex(this.CurrentPosition, index);
        }

        internal ElementIndex GetStoredElementIndex(Point position, Layer layer)
        {
            return this.tileMap.GetStoredElementIndex(position, layer);
        }
        internal ElementIndex GetStoredElementIndex(Point position)
        {
            return GetStoredElementIndex(position, this.CurrentLayer);
        }
        internal ElementIndex GetStoredElementIndex()
        {
            return GetStoredElementIndex(this.CurrentPosition);
        }

        internal Element GetStoredElement(Point position, Layer layer)
        {
            return this.tileMap.GetStoredElement(position, layer);
        }
        internal Element GetStoredElement(Point position)
        {
            return GetStoredElement(position, this.CurrentLayer);
        }
        internal Element GetStoredElement()
        {
            return GetStoredElement(this.CurrentPosition);
        }

        internal bool HasElement(Point position, Layer layer)
        {
            return this.tileMap.HasElement(position, layer);
        }
        internal bool HasElement(Point position)
        {
            return HasElement(position, this.CurrentLayer);
        }
        internal bool HasElement()
        {
            return HasElement(this.CurrentPosition);
        }

        internal bool HasStoredElement(Point position, Layer layer)
        {
            return this.tileMap.HasStoredElement(position, layer);
        }
        internal bool HasStoredElement(Point position)
        {
            return HasStoredElement(position, this.CurrentLayer);
        }
        internal bool HasStoredElement()
        {
            return HasStoredElement(this.CurrentPosition);
        }

        internal ElementIndex GetElementIndex(Point position, Layer layer)
        {
            return this.tileMap.GetElementIndex(position, layer);
        }
        internal ElementIndex GetElementIndex(Point position)
        {
            return GetElementIndex(position, this.CurrentLayer);
        }
        internal ElementIndex GetElementIndex()
        {
            return GetElementIndex(this.CurrentPosition);
        }

        internal Element GetElement(Point position, Layer layer)
        {
            return this.tileMap.GetElement(position, layer);
        }
        internal Element GetElement(Point position)
        {
            return GetElement(position, this.CurrentLayer);
        }
        internal Element GetElement()
        {
            return GetElement(this.CurrentPosition);
        }

        internal Slot GetSlot(Point position)
        {
            return this.tileMap.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.CurrentPosition);
        }

        internal ElementNeighbors GetNeighboringSlots(Point position)
        {
            return this.tileMap.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.CurrentPosition);
        }

        internal bool IsEmpty(Point position, Layer layer)
        {
            return this.tileMap.IsEmpty(position, layer);
        }
        internal bool IsEmpty(Point position)
        {
            return IsEmpty(position, this.CurrentLayer);
        }
        internal bool IsEmpty()
        {
            return IsEmpty(this.CurrentPosition, this.CurrentLayer);
        }

        internal Color GetColorModifier(Point position, Layer layer) 
        {
            return this.tileMap.GetColorModifier(position, layer);
        }
        internal Color GetColorModifier(Point position)
        {
            return GetColorModifier(position, this.CurrentLayer);
        }
        internal Color GetColorModifier()
        {
            return GetColorModifier(this.CurrentPosition, this.CurrentLayer);
        }

        internal bool GetDissipatingState(Point position, Layer layer)
        {
            return this.tileMap.GetDissipatingState(position, layer);
        }
        internal bool GetDissipatingState(Point position)
        {
            return GetDissipatingState(position, this.CurrentLayer);
        }
        internal bool GetDissipatingState()
        {
            return GetDissipatingState(this.CurrentPosition, this.CurrentLayer);
        }

        internal bool GetFallingState(Point position, Layer layer)
        {
            return this.tileMap.GetFallingState(position, layer);
        }
        internal bool GetFallingState(Point position)
        {
            return GetFallingState(position, this.CurrentLayer);
        }
        internal bool GetFallingState()
        {
            return GetFallingState(this.CurrentPosition, this.CurrentLayer);
        }

        internal bool GetPushedState(Point position, Layer layer)
        {
            return this.tileMap.GetPushedState(position, layer);
        }
        internal bool GetPushedState(Point position)
        {
            return GetPushedState(position, this.CurrentLayer);
        }
        internal bool GetPushedState()
        {
            return GetPushedState(this.CurrentPosition, this.CurrentLayer);
        }

        internal UpdateCycleFlag GetStepCycleFlag(Point position, Layer layer)
        {
            return this.tileMap.GetStepCycleFlag(position, layer);
        }
        internal UpdateCycleFlag GetStepCycleFlag(Point position)
        {
            return GetStepCycleFlag(position, this.CurrentLayer);
        }
        internal UpdateCycleFlag GetStepCycleFlag()
        {
            return GetStepCycleFlag(this.CurrentPosition, this.CurrentLayer);
        }

        internal void SetDissipatingState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetDissipatingState(position, layer, value);
        }
        internal void SetDissipatingState(Point position, bool value)
        {
            SetDissipatingState(position, this.CurrentLayer, value);
        }
        internal void SetDissipatingState(bool value)
        {
            SetDissipatingState(this.CurrentPosition, value);
        }

        internal void SetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            this.tileMap.SetElementIndex(position, layer, value);
        }
        internal void SetElementIndex(Point position, ElementIndex value)
        {
            SetElementIndex(position, this.CurrentLayer, value);
        }
        internal void SetElementIndex(ElementIndex value)
        {
            SetElementIndex(this.CurrentPosition, value);
        }

        internal void SetFallingState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetFallingState(position, layer, value);
        }
        internal void SetFallingState(Point position, bool value)
        {
            SetFallingState(position, this.CurrentLayer, value);
        }
        internal void SetFallingState(bool value)
        {
            SetFallingState(this.CurrentPosition, value);
        }

        internal void SetPushedState(Point position, Layer layer, bool value)
        {
            this.tileMap.SetPushedState(position, layer, value);
        }
        internal void SetPushedState(Point position, bool value)
        {
            SetPushedState(position, this.CurrentLayer, value);
        }
        internal void SetPushedState(bool value)
        {
            SetPushedState(this.CurrentPosition, value);
        }

        internal void SetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value)
        {
            this.tileMap.SetStepCycleFlag(position, layer, value);
        }
        internal void SetStepCycleFlag(Point position, UpdateCycleFlag value)
        {
            SetStepCycleFlag(position, this.CurrentLayer, value);
        }
        internal void SetStepCycleFlag(UpdateCycleFlag value)
        {
            SetStepCycleFlag(this.CurrentPosition, value);
        }

        #endregion

        #endregion

        #region CHUNKING

        internal bool TryNotifyChunk(Point position)
        {
            return this.chunkHandler.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.CurrentPosition);
        }

        internal void NotifyChunk(Point position)
        {
            this.chunkHandler.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.CurrentPosition);
        }

        internal bool TryGetChunkUpdateState(Point position, out bool result)
        {
            return this.chunkHandler.TryGetChunkUpdateState(position, out result);
        }
        internal bool GetChunkUpdateState(Point position)
        {
            return this.chunkHandler.GetChunkUpdateState(position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            return this.explosionHandler.TryInstantiateExplosion(position, layer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        internal void InstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            this.explosionHandler.InstantiateExplosion(position, layer, explosionBuilder);
        }
        internal void InstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal void InstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        #endregion
    }
}
