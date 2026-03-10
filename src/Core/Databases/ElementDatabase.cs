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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Extensions;

using System;

namespace StardustSandbox.Core.Databases
{
    internal static class ElementDatabase
    {
        private static Element[] elements;
        private static bool isLoaded;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ElementDatabase)} is already loaded.");
            }

            elements = [
                // [000] Dirt
                new Elements.Solids.Movables.Dirt()
                {
                    Index = ElementIndex.Dirt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 0),
                    ReferenceColor = AAP64ColorPalette.Clay,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.6f,
                    DefaultExplosionResistance = 1.0f,
                },

                // [001] Mud
                new Elements.Solids.Movables.Mud()
                {
                    Index = ElementIndex.Mud,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 32),
                    ReferenceColor = new(87, 44, 45),
                    DefaultTemperature = 18.0f,
                    DefaultDensity = 1.5f,
                    DefaultExplosionResistance = 0.6f,
                },

                // [002] Water
                new Elements.Liquids.Water()
                {
                    Index = ElementIndex.Water,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 64),
                    ReferenceColor = new(8, 120, 184),
                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0f,
                    DefaultDensity = 1.0f,
                    DefaultExplosionResistance = 0.2f,
                },

                // [003] Stone
                new Elements.Solids.Movables.Stone()
                {
                    Index = ElementIndex.Stone,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 96),
                    ReferenceColor = new(66, 65, 65),
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 2.5f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [004] Grass
                new Elements.Solids.Movables.Grass()
                {
                    Index = ElementIndex.Grass,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 128),
                    ReferenceColor = new(69, 110, 55),
                    DefaultTemperature = 22.0f,
                    DefaultFlammabilityResistance = 10.0f,
                    DefaultDensity = 0.1f,
                    DefaultExplosionResistance = 0.5f,
                },
                
                // [005] Ice
                new Elements.Solids.Movables.Ice()
                {
                    Index = ElementIndex.Ice,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 160),
                    ReferenceColor = new(34, 112, 255),
                    DefaultTemperature = -25.0f,
                    DefaultDensity = 0.92f,
                    DefaultExplosionResistance = 1.2f,
                },

                // [006] Sand
                new Elements.Solids.Movables.Sand()
                {
                    Index = ElementIndex.Sand,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 192),
                    ReferenceColor = new(248, 246, 68),
                    DefaultTemperature = 22.0f,
                    DefaultDensity = 1.5f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [007] Snow
                new Elements.Solids.Movables.Snow()
                {
                    Index = ElementIndex.Snow,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 224),
                    ReferenceColor = new(189, 237, 246),
                    DefaultTemperature = -15.0f,
                    DefaultDensity = 0.1f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [008] Movable Corruption
                new Elements.Solids.Movables.MovableCorruption()
                {
                    Index = ElementIndex.MovableCorruption,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruption |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 256),
                    ReferenceColor = AAP64ColorPalette.PurpleGray,
                    DefaultDensity = 1.4f,
                    DefaultExplosionResistance = 0.8f,
                },
                
                // [009] Lava
                new Elements.Liquids.Lava()
                {
                    Index = ElementIndex.Lava,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 288),
                    ReferenceColor = AAP64ColorPalette.OrangeRed,
                    DefaultTemperature = 1500.0f,
                    DefaultDensity = 2.7f,
                    DefaultExplosionResistance = 0.4f,
                },

                // [010] Acid
                new Elements.Liquids.Acid()
                {
                    Index = ElementIndex.Acid,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 0),
                    ReferenceColor = new(59, 167, 5),
                    DefaultTemperature = 10.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.2f,
                },

                // [011] Glass
                new Elements.Solids.Immovables.Glass()
                {
                    Index = ElementIndex.Glass,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 32),
                    ReferenceColor = AAP64ColorPalette.White,
                    DefaultTemperature = 25.0f,
                    DefaultDensity = 2.5f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [012] Iron
                new Elements.Solids.Immovables.Iron()
                {
                    Index = ElementIndex.Iron,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 64),
                    ReferenceColor = new(66, 66, 66),
                    DefaultTemperature = 30.0f,
                    DefaultDensity = 7.8f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [013] Wall
                new Elements.Solids.Immovables.Wall()
                {
                    Index = ElementIndex.Wall,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 96),
                    ReferenceColor = new(22, 99, 50),

                    DefaultDensity = 2.2f,
                },

                // [014] Wood
                new Elements.Solids.Immovables.Wood()
                {
                    Index = ElementIndex.Wood,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 128),
                    ReferenceColor = new(92, 62, 0),
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.7f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [015] Gas Corruption
                new Elements.Gases.GasCorruption()
                {
                    Index = ElementIndex.GasCorruption,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruption |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 160),
                    ReferenceColor = AAP64ColorPalette.PurpleGray,
                    DefaultDensity = 0.005f,
                },

                // [016] Liquid Corruption
                new Elements.Liquids.LiquidCorruption()
                {
                    Index = ElementIndex.LiquidCorruption,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruption |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 192),
                    ReferenceColor = AAP64ColorPalette.PurpleGray,
                    DefaultDensity = 1.05f,
                    DefaultExplosionResistance = 0.1f,
                },

                // [017] Immovable Corruption
                new Elements.Solids.Immovables.ImmovableCorruption()
                {
                    Index = ElementIndex.ImmovableCorruption,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruption |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 224),
                    ReferenceColor = AAP64ColorPalette.PurpleGray,
                    DefaultDensity = 1.6f,
                    DefaultExplosionResistance = 1.2f,
                },

                // [018] Steam
                new Elements.Gases.Steam()
                {
                    Index = ElementIndex.Steam,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 256),
                    ReferenceColor = new(171, 208, 218),
                    DefaultTemperature = 200.0f,
                    DefaultDensity = 0.0006f,
                },

                // [019] Smoke
                new Elements.Gases.Smoke()
                {
                    Index = ElementIndex.Smoke,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 288),
                    ReferenceColor = new(48, 48, 48),
                    DefaultTemperature = 350.0f,
                    DefaultDensity = 0.002f,
                },

                // [020] Red Brick
                new Elements.Solids.Immovables.RedBrick()
                {
                    Index = ElementIndex.RedBrick,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 0),
                    ReferenceColor = AAP64ColorPalette.Crimson,
                    DefaultTemperature = 25.0f,
                    DefaultDensity = 2.4f,
                    DefaultExplosionResistance = 2.5f,
                },

                // [021] Tree Leaf
                new Elements.Solids.Immovables.TreeLeaf()
                {
                    Index = ElementIndex.TreeLeaf,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 32),
                    ReferenceColor = AAP64ColorPalette.MossGreen,
                    DefaultTemperature = 22.0f,
                    DefaultFlammabilityResistance = 5.0f,
                    DefaultDensity = 0.04f,
                    DefaultExplosionResistance = 0.1f,
                },

                // [022] Mounting Block
                new Elements.Solids.Immovables.MountingBlock()
                {
                    Index = ElementIndex.MountingBlock,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(0, 320),
                    ReferenceColor = AAP64ColorPalette.White,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 150.0f,
                    DefaultDensity = 1.8f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [023] Fire
                new Elements.Energies.Fire()
                {
                    Index = ElementIndex.Fire,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsExplosionImmune |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(32, 320),
                    ReferenceColor = AAP64ColorPalette.Amber,
                    DefaultTemperature = 500.0f,
                    DefaultDensity = 0.0f,
                },

                // [024] Lamp On
                new Elements.Solids.Immovables.LampOn()
                {
                    Index = ElementIndex.LampOn,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(64, 320),
                    ReferenceColor = AAP64ColorPalette.Sand,
                    DefaultTemperature = 26.0f,
                    DefaultDensity = 2.8f,
                },

                // [025] Void
                new Elements.Solids.Immovables.Void()
                {
                    Index = ElementIndex.Void,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 64),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    DefaultDensity = 0.001f,
                },

                // [026] Clone
                new Elements.Solids.Immovables.Clone()
                {
                    Index = ElementIndex.Clone,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 96),
                    ReferenceColor = AAP64ColorPalette.Amber,
                    DefaultDensity = 3.0f,
                },

                // [027] Oil
                new Elements.Liquids.Oil()
                {
                    Index = ElementIndex.Oil,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 128),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    DefaultFlammabilityResistance = 5.0f,
                    DefaultDensity = 0.92f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [028] Salt
                new Elements.Solids.Movables.Salt()
                {
                    Index = ElementIndex.Salt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 160),
                    ReferenceColor = AAP64ColorPalette.White,
                    DefaultTemperature = 22.0f,
                    DefaultDensity = 2.2f,
                    DefaultExplosionResistance = 0.7f,
                },

                // [029] Saltwater
                new Elements.Liquids.Saltwater()
                {
                    Index = ElementIndex.Saltwater,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 192),
                    ReferenceColor = new(69, 188, 255),
                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0f,
                    DefaultDensity = 1.03f,
                    DefaultExplosionResistance = 0.2f,
                },

                // [030] Bomb
                new Elements.Solids.Movables.Explosives.Bomb()
                {
                    Index = ElementIndex.Bomb,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(96, 320),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    DefaultTemperature = 25.0f,
                    DefaultDensity = 3.5f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [031] Dynamite
                new Elements.Solids.Movables.Explosives.Dynamite()
                {
                    Index = ElementIndex.Dynamite,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(128, 320),
                    ReferenceColor = AAP64ColorPalette.Crimson.Darken(0.05f),
                    DefaultTemperature = 22.0f,
                    DefaultDensity = 2.4f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [032] TNT
                new Elements.Solids.Movables.Explosives.Tnt()
                {
                    Index = ElementIndex.Tnt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsExplosive |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(160, 320),
                    ReferenceColor = AAP64ColorPalette.Crimson.Darken(0.1f),
                    DefaultTemperature = 22.0f,
                    DefaultDensity = 2.8f,
                    DefaultExplosionResistance = 0.35f,
                },

                // [033] Dry Sponge
                new Elements.Solids.Immovables.DrySponge()
                {
                    Index = ElementIndex.DrySponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 224),
                    ReferenceColor = AAP64ColorPalette.Amber,
                    DefaultTemperature = 25.0f,
                    DefaultFlammabilityResistance = 10.0f,
                    DefaultDensity = 0.055f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [034] Wet Sponge
                new Elements.Solids.Immovables.WetSponge()
                {
                    Index = ElementIndex.WetSponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 256),
                    ReferenceColor = AAP64ColorPalette.Amber.Darken(0.1f),
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.8f,
                },

                // [035] Gold
                new Elements.Solids.Immovables.Gold()
                {
                    Index = ElementIndex.Gold,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(192, 320),
                    ReferenceColor = AAP64ColorPalette.LemonYellow,
                    DefaultTemperature = 22.0f,
                    DefaultDensity = 19.3f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [036] Heater
                new Elements.Solids.Immovables.Heater()
                {
                    Index = ElementIndex.Heater,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(224, 320),
                    ReferenceColor = AAP64ColorPalette.DarkRed,
                    DefaultTemperature = 0.0f,
                    DefaultDensity = 1.5f,
                    DefaultExplosionResistance = 2.5f,
                },

                // [037] Freezer
                new Elements.Solids.Immovables.Freezer()
                {
                    Index = ElementIndex.Freezer,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(256, 320),
                    ReferenceColor = AAP64ColorPalette.NavyBlue,
                    DefaultTemperature = 0.0f,
                    DefaultDensity = 1.5f,
                },

                // [038] Ash
                new Elements.Solids.Movables.Ash()
                {
                    Index = ElementIndex.Ash,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 288),
                    ReferenceColor = new(30, 33, 38),
                    DefaultTemperature = 40.0f,
                    DefaultDensity = 0.35f,
                    DefaultExplosionResistance = 0.0f,
                },

                // [039] Anti-Corruption
                new Elements.Gases.AntiCorruption()
                {
                    Index = ElementIndex.AntiCorruption,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 0),
                    ReferenceColor = AAP64ColorPalette.Crimson,
                    DefaultDensity = 0.5f,
                },

                // [040] Devourer
                new Elements.Solids.Immovables.Devourer()
                {
                    Index = ElementIndex.Devourer,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(288, 320),
                    ReferenceColor = AAP64ColorPalette.Coal,
                    DefaultTemperature = 35.0f,
                    DefaultDensity = 3.5f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [041] Upward Pusher
                new Elements.Solids.Immovables.Pusher()
                {
                    Index = ElementIndex.UpwardPusher,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(320, 320),
                    ReferenceColor = AAP64ColorPalette.Rust,
                    Direction = PusherDirection.Up,
                    DefaultDensity = 2.0f,
                },

                // [042] Rightward Pusher
                new Elements.Solids.Immovables.Pusher()
                {
                    Index = ElementIndex.RightwardPusher,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(352, 320),
                    ReferenceColor = AAP64ColorPalette.Rust,
                    Direction = PusherDirection.Right,
                    DefaultDensity = 2.0f,
                },

                // [043] Downward Pusher
                new Elements.Solids.Immovables.Pusher()
                {
                    Index = ElementIndex.DownwardPusher,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(384, 320),
                    ReferenceColor = AAP64ColorPalette.Rust,
                    Direction = PusherDirection.Down,
                    DefaultDensity = 2.0f,
                },

                // [044] Leftward Pusher
                new Elements.Solids.Immovables.Pusher()
                {
                    Index = ElementIndex.LeftwardPusher,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(416, 320),
                    ReferenceColor = AAP64ColorPalette.Rust,
                    Direction = PusherDirection.Left,
                    DefaultDensity = 2.0f,
                },

                // [045] Cloud
                new Elements.Gases.Cloud()
                {
                    Index = ElementIndex.Cloud,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 32),
                    ReferenceColor = AAP64ColorPalette.LightGrayBlue,
                    DefaultTemperature = 15.0f,
                    DefaultFlammabilityResistance = 10.0f,
                    DefaultDensity = 0.15f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [046] Charged Cloud
                new Elements.Gases.ChargedCloud()
                {
                    Index = ElementIndex.ChargedCloud,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 64),
                    ReferenceColor = AAP64ColorPalette.Slate,
                    DefaultTemperature = 10.0f,
                    DefaultFlammabilityResistance = 10.0f,
                    DefaultDensity = 0.2f,
                    DefaultExplosionResistance = 0.7f,
                },

                // [047] Lightning Head
                new Elements.Energies.LightningHead()
                {
                    Index = ElementIndex.LightningHead,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.None,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(448, 320),
                    ReferenceColor = AAP64ColorPalette.White,
                    DefaultTemperature = TemperatureConstants.MAX_CELSIUS_VALUE,
                    DefaultDensity = 0.0f,
                },

                // [048] Lightning Body
                new Elements.Energies.LightningBody()
                {
                    Index = ElementIndex.LightningBody,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(448, 320),
                    ReferenceColor = AAP64ColorPalette.White,
                    DefaultTemperature = TemperatureConstants.MAX_CELSIUS_VALUE,
                    DefaultDensity = 0.0f,
                },

                // [049] Dry Wool (Black)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryBlackWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 96),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    WetWoolIndex = ElementIndex.WetBlackWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [050] Dry Wool (White)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryWhiteWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 128),
                    ReferenceColor = AAP64ColorPalette.White,
                    WetWoolIndex = ElementIndex.WetWhiteWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [051] Dry Wool (Red)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryRedWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 160),
                    ReferenceColor = AAP64ColorPalette.Crimson,
                    WetWoolIndex = ElementIndex.WetRedWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [052] Dry Wool (Orange)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryOrangeWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 192),
                    ReferenceColor = AAP64ColorPalette.Orange,
                    WetWoolIndex = ElementIndex.WetOrangeWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [053] Dry Wool (Yellow)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryYellowWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 224),
                    ReferenceColor = AAP64ColorPalette.Gold,
                    WetWoolIndex = ElementIndex.WetYellowWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [054] Dry Wool (Green)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryGreenWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 256),
                    ReferenceColor = AAP64ColorPalette.ForestGreen,
                    WetWoolIndex = ElementIndex.WetGreenWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [055] Dry Wool (Gray)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryGrayWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(480, 288),
                    ReferenceColor = AAP64ColorPalette.Gunmetal,
                    WetWoolIndex = ElementIndex.WetGrayWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [056] Dry Wool (Blue)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryBlueWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 0),
                    ReferenceColor = AAP64ColorPalette.Cyan,
                    WetWoolIndex = ElementIndex.WetBlueWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [057] Dry Wool (Violet)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryVioletWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 32),
                    ReferenceColor = AAP64ColorPalette.Violet,
                    WetWoolIndex = ElementIndex.WetVioletWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [058] Dry Wool (Brown)
                new Elements.Solids.Immovables.DryWool()
                {
                    Index = ElementIndex.DryBrownWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 64),
                    ReferenceColor = AAP64ColorPalette.Brown,
                    WetWoolIndex = ElementIndex.WetBrownWool,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 35.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [059] Wet Wool (Black)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetBlackWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 96),
                    ReferenceColor = AAP64ColorPalette.DarkGray.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryBlackWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [060] Wet Wool (White)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetWhiteWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 128),
                    ReferenceColor = AAP64ColorPalette.White.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryWhiteWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [061] Wet Wool (Red)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetRedWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 160),
                    ReferenceColor = AAP64ColorPalette.Crimson.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryRedWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [062] Wet Wool (Orange)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetOrangeWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 192),
                    ReferenceColor = AAP64ColorPalette.Orange.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryOrangeWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [063] Wet Wool (Yellow)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetYellowWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 224),
                    ReferenceColor = AAP64ColorPalette.Gold.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryYellowWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [064] Wet Wool (Green)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetGreenWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 256),
                    ReferenceColor = AAP64ColorPalette.ForestGreen.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryGreenWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [065] Wet Wool (Gray)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetGrayWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(640, 288),
                    ReferenceColor = AAP64ColorPalette.Gunmetal.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryGrayWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [066] Wet Wool (Blue)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetBlueWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 0),
                    ReferenceColor = AAP64ColorPalette.Cyan.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryBlueWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [067] Wet Wool (Violet)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetVioletWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 32),
                    ReferenceColor = AAP64ColorPalette.Violet.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryVioletWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [068] Wet Wool (Brown)
                new Elements.Solids.Immovables.WetWool()
                {
                    Index = ElementIndex.WetBrownWool,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 64),
                    ReferenceColor = AAP64ColorPalette.Brown.Darken(0.65f),
                    DryWoolIndex = ElementIndex.DryBrownWool,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.6f,
                    DefaultExplosionResistance = 1.5f,
                },

                // [069] Fertile Soil
                new Elements.Solids.Movables.FertileSoil()
                {
                    Index = ElementIndex.FertileSoil,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 96),
                    ReferenceColor = AAP64ColorPalette.Burgundy,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.4f,
                    DefaultExplosionResistance = 0.4f,
                },

                // [070] Seed
                new Elements.Solids.Movables.Seed()
                {
                    Index = ElementIndex.Seed,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(480, 320),
                    ReferenceColor = AAP64ColorPalette.DarkGreen,
                    DefaultTemperature = 25.0f,
                    DefaultFlammabilityResistance = 5.0f,
                    DefaultDensity = 0.05f,
                    DefaultExplosionResistance = 0.2f,
                },

                // [071] Sapling
                new Elements.Solids.Movables.Sapling()
                {
                    Index = ElementIndex.Sapling,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(512, 320),
                    ReferenceColor = AAP64ColorPalette.DarkTeal,
                    DefaultTemperature = 25.0f,
                    DefaultFlammabilityResistance = 15.0f,
                    DefaultDensity = 0.3f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [072] Moss
                new Elements.Solids.Immovables.Moss()
                {
                    Index = ElementIndex.Moss,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 128),
                    ReferenceColor = AAP64ColorPalette.PineGreen,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.4f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [073] Gunpowder
                new Elements.Solids.Movables.Explosives.Gunpowder()
                {
                    Index = ElementIndex.Gunpowder,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 160),
                    ReferenceColor = AAP64ColorPalette.Graphite,
                    DefaultTemperature = 22.0f,
                    DefaultFlammabilityResistance = 5.0f,
                    DefaultDensity = 0.9f,
                    DefaultExplosionResistance = 0.1f,
                },

                // [074] Liquefied Petroleum Gas
                new Elements.Gases.LiquefiedPetroleumGas()
                {
                    Index = ElementIndex.LiquefiedPetroleumGas,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsFlammable |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 192),
                    ReferenceColor = AAP64ColorPalette.Amber,
                    DefaultTemperature = -42.0f,
                    DefaultFlammabilityResistance = 1.0f,
                    DefaultDensity = 0.25f,
                    DefaultExplosionResistance = 0.2f,
                },

                // [075] Obsidian
                new Elements.Solids.Immovables.Obsidian()
                {
                    Index = ElementIndex.Obsidian,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 224),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    DefaultTemperature = 35.0f,
                    DefaultDensity = 2.4f,
                },

                // [076] Paint (Black)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.BlackPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 256),
                    ReferenceColor = AAP64ColorPalette.DarkGray,
                    DyeingColor = AAP64ColorPalette.DarkGray,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [077] Paint (White)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.WhitePaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(800, 288),
                    ReferenceColor = AAP64ColorPalette.White,
                    DyeingColor = AAP64ColorPalette.White,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [078] Paint (Red)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.RedPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 0),
                    ReferenceColor = AAP64ColorPalette.Crimson,
                    DyeingColor = AAP64ColorPalette.Crimson,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [079] Paint (Orange)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.OrangePaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 32),
                    ReferenceColor = AAP64ColorPalette.Orange,
                    DyeingColor = AAP64ColorPalette.Orange,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [080] Paint (Yellow)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.YellowPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 64),
                    ReferenceColor = AAP64ColorPalette.Gold,
                    DyeingColor = AAP64ColorPalette.Gold,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [081] Paint (Green)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.GreenPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 96),
                    ReferenceColor = AAP64ColorPalette.ForestGreen,
                    DyeingColor = AAP64ColorPalette.ForestGreen,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [082] Paint (Cyan)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.CyanPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 128),
                    ReferenceColor = AAP64ColorPalette.Cyan,
                    DyeingColor = AAP64ColorPalette.Cyan,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [083] Paint (Gray)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.GrayPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 160),
                    ReferenceColor = AAP64ColorPalette.Gunmetal,
                    DyeingColor = AAP64ColorPalette.Gunmetal,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [084] Paint (Violet)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.VioletPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 192),
                    ReferenceColor = AAP64ColorPalette.Violet,
                    DyeingColor = AAP64ColorPalette.Violet,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [085] Paint (Brown)
                new Elements.Liquids.Paint()
                {
                    Index = ElementIndex.BrownPaint,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsFlammable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 224),
                    ReferenceColor = AAP64ColorPalette.Brown,
                    DyeingColor = AAP64ColorPalette.Brown,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 0.3f,
                },

                // [086] Mercury
                new Elements.Liquids.Mercury()
                {
                    Index = ElementIndex.Mercury,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsConductive,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 256),
                    ReferenceColor = AAP64ColorPalette.Slate,
                    DefaultTemperature = 10.0f,
                    DefaultDensity = 13.5f,
                    DefaultExplosionResistance = 0.5f,
                    DefaultDispersionRate = 3,
                },

                // [087] Electricity
                new Elements.Energies.Electricity()
                {
                    Index = ElementIndex.Electricity,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsElectrified,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(960, 288),
                    ReferenceColor = AAP64ColorPalette.Gold,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 0.0f,
                    DefaultExplosionResistance = 0.0f,
                    DefaultDispersionRate = 8,
                },

                // [088] Battery
                new Elements.Solids.Immovables.Battery()
                {
                    Index = ElementIndex.Battery,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable |
                                      ElementCharacteristics.IsElectrified,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(576, 320),
                    ReferenceColor = AAP64ColorPalette.Orange,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 25.0f,
                    DefaultDensity = 1.2f,
                    DefaultExplosionResistance = 1.0f,
                },

                // [089] Lamp (Off)
                new Elements.Solids.Immovables.LampOff()
                {
                    Index = ElementIndex.LampOff,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(544, 320),
                    ReferenceColor = AAP64ColorPalette.Brown,
                    DefaultTemperature = 20.0f,
                    DefaultFlammabilityResistance = 20.0f,
                    DefaultDensity = 1.0f,
                    DefaultExplosionResistance = 0.5f,
                },

                // [090] Energy Transmitter
                new Elements.Solids.Immovables.EnergyTransmitter()
                {
                    Index = ElementIndex.EnergyTransmitter,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(608, 320),
                    ReferenceColor = AAP64ColorPalette.Brown,
                    DefaultTemperature = 25.0f,
                    DefaultFlammabilityResistance = 30.0f,
                    DefaultDensity = 1.3f,
                    DefaultExplosionResistance = 1.2f,
                }
            ];

            isLoaded = true;
        }

        internal static Element GetElement(in ElementIndex index)
        {
            return elements[(int)index];
        }
    }
}
