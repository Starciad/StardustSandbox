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
using StardustSandbox.Core.Managers;

using System;

namespace StardustSandbox.Core.Databases
{
    internal sealed class ElementDatabase
    {
        private Element[] elements;

        internal void Load(AchievementManager achievementManager)
        {
            elements = [
                // [000] Dirt
                new Elements.Solids.Movables.Dirt(
                    ElementIndex.Dirt,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 0),
                    AAP64ColorPalette.Clay,
                    achievementManager
                ),

                // [001] Mud
                new Elements.Solids.Movables.Mud(
                    ElementIndex.Mud,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 32),
                    new(87, 44, 45),
                    achievementManager
                ),

                // [002] Water
                new Elements.Liquids.Water(
                    ElementIndex.Water,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 64),
                    new(8, 120, 184),
                    achievementManager
                ),

                // [003] Stone
                new Elements.Solids.Movables.Stone(
                    ElementIndex.Stone,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 96),
                    new(66, 65, 65),
                    achievementManager
                ),

                // [004] Grass
                new Elements.Solids.Movables.Grass(
                    ElementIndex.Grass,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 128),
                    new(69, 110, 55),
                    achievementManager
                ),
                
                // [005] Ice
                new Elements.Solids.Movables.Ice(
                    ElementIndex.Ice,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 160),
                    new(34, 112, 255),
                    achievementManager
                ),

                // [006] Sand
                new Elements.Solids.Movables.Sand(
                    ElementIndex.Sand,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 192),
                    new(248, 246, 68),
                    achievementManager
                ),

                // [007] Snow
                new Elements.Solids.Movables.Snow(
                    ElementIndex.Snow,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 224),
                    new(189, 237, 246),
                    achievementManager
                ),

                // [008] Movable Corruption
                new Elements.Solids.Movables.MovableCorruption(
                    ElementIndex.MovableCorruption,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruption |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 256),
                    AAP64ColorPalette.PurpleGray,
                    achievementManager
                ),
                
                // [009] Lava
                new Elements.Liquids.Lava(
                    ElementIndex.Lava,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(0, 288),
                    AAP64ColorPalette.OrangeRed,
                    achievementManager
                ),

                // [010] Acid
                new Elements.Liquids.Acid(
                    ElementIndex.Acid,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 0),
                    new(59, 167, 5),
                    achievementManager
                ),

                // [011] Glass
                new Elements.Solids.Immovables.Glass(
                    ElementIndex.Glass,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 32),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [012] Iron
                new Elements.Solids.Immovables.Iron(
                    ElementIndex.Iron,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(160, 64),
                    new(66, 66, 66),
                    achievementManager
                ),

                // [013] Wall
                new Elements.Solids.Immovables.Wall(
                    ElementIndex.Wall,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.IsExplosionImmune,
                    ElementRenderingType.Blob,
                    new(160, 96),
                    new(22, 99, 50),
                    achievementManager
                ),

                // [014] Wood
                new Elements.Solids.Immovables.Wood(
                    ElementIndex.Wood,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 128),
                    new(92, 62, 0),
                    achievementManager
                ),

                // [015] Gas Corruption
                new Elements.Gases.GasCorruption(
                    ElementIndex.GasCorruption,
                    ElementCategory.Gas,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruption |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 160),
                    AAP64ColorPalette.PurpleGray,
                    achievementManager
                ),

                // [016] Liquid Corruption
                new Elements.Liquids.LiquidCorruption(
                    ElementIndex.LiquidCorruption,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruption |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 192),
                    AAP64ColorPalette.PurpleGray,
                    achievementManager
                ),

                // [017] Immovable Corruption
                new Elements.Solids.Immovables.ImmovableCorruption(
                    ElementIndex.ImmovableCorruption,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruption |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 224),
                    AAP64ColorPalette.PurpleGray,
                    achievementManager
                ),

                // [018] Steam
                new Elements.Gases.Steam(
                    ElementIndex.Steam,
                    ElementCategory.Gas,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 256),
                    new(171, 208, 218),
                    achievementManager
                ),

                // [019] Smoke
                new Elements.Gases.Smoke(
                    ElementIndex.Smoke,
                    ElementCategory.Gas,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(160, 288),
                    new(48, 48, 48),
                    achievementManager
                ),

                // [020] Red Brick
                new Elements.Solids.Immovables.RedBrick(
                    ElementIndex.RedBrick,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(320, 0),
                    AAP64ColorPalette.Crimson,
                    achievementManager
                ),

                // [021] Tree Leaf
                new Elements.Solids.Immovables.TreeLeaf(
                    ElementIndex.TreeLeaf,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(320, 32),
                    AAP64ColorPalette.MossGreen,
                    achievementManager
                ),

                // [022] Mounting Block
                new Elements.Solids.Immovables.MountingBlock(
                    ElementIndex.MountingBlock,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(0, 320),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [023] Fire
                new Elements.Energies.Fire(
                    ElementIndex.Fire,
                    ElementCategory.Energy,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsExplosionImmune |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(32, 320),
                    AAP64ColorPalette.Amber,
                    achievementManager
                ),

                // [024] Lamp On
                new Elements.Solids.Immovables.LampOn(
                    ElementIndex.LampOn,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(64, 320),
                    AAP64ColorPalette.Sand,
                    achievementManager
                ),

                // [025] Void
                new Elements.Solids.Immovables.Void(
                    ElementIndex.Void,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsExplosionImmune,
                    ElementRenderingType.Blob,
                    new(320, 64),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [026] Clone
                new Elements.Solids.Immovables.Clone(
                    ElementIndex.Clone,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsExplosionImmune,
                    ElementRenderingType.Blob,
                    new(320, 96),
                    AAP64ColorPalette.Amber,
                    achievementManager
                ),

                // [027] Oil
                new Elements.Liquids.Oil(
                    ElementIndex.Oil,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(320, 128),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [028] Salt
                new Elements.Solids.Movables.Salt(
                    ElementIndex.Salt,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(320, 160),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [029] Saltwater
                new Elements.Liquids.Saltwater(
                    ElementIndex.Saltwater,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(320, 192),
                    new(69, 188, 255),
                    achievementManager
                ),

                // [030] Bomb
                new Elements.Solids.Movables.Bomb(
                    ElementIndex.Bomb,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(96, 320),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [031] Dynamite
                new Elements.Solids.Movables.Dynamite(
                    ElementIndex.Dynamite,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(128, 320),
                    AAP64ColorPalette.Crimson.Darken(0.05f),
                    achievementManager
                ),

                // [032] TNT
                new Elements.Solids.Movables.Tnt(
                    ElementIndex.Tnt,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsExplosive |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(160, 320),
                    AAP64ColorPalette.Crimson.Darken(0.1f),
                    achievementManager
                ),

                // [033] Dry Sponge
                new Elements.Solids.Immovables.DrySponge(
                    ElementIndex.DrySponge,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(320, 224),
                    AAP64ColorPalette.Amber,
                    achievementManager
                ),

                // [034] Wet Sponge
                new Elements.Solids.Immovables.WetSponge(
                    ElementIndex.WetSponge,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(320, 256),
                    AAP64ColorPalette.Amber.Darken(0.1f),
                    achievementManager
                ),

                // [035] Gold
                new Elements.Solids.Immovables.Gold(
                    ElementIndex.Gold,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Single,
                    new(192, 320),
                    AAP64ColorPalette.LemonYellow,
                    achievementManager
                ),

                // [036] Heater
                new Elements.Solids.Immovables.Heater(
                    ElementIndex.Heater,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(224, 320),
                    AAP64ColorPalette.DarkRed,
                    achievementManager
                ),

                // [037] Freezer
                new Elements.Solids.Immovables.Freezer(
                    ElementIndex.Freezer,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(256, 320),
                    AAP64ColorPalette.NavyBlue,
                    achievementManager
                ),

                // [038] Ash
                new Elements.Solids.Movables.Ash(
                    ElementIndex.Ash,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(320, 288),
                    new(30, 33, 38),
                    achievementManager
                ),

                // [039] Anti-Corruption
                new Elements.Gases.AntiCorruption(
                    ElementIndex.AntiCorruption,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 0),
                    AAP64ColorPalette.Crimson,
                    achievementManager
                ),

                // [040] Devourer
                new Elements.Solids.Immovables.Devourer(
                    ElementIndex.Devourer,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(288, 320),
                    AAP64ColorPalette.Coal,
                    achievementManager
                ),

                // [041] Upward Pusher
                new Elements.Solids.Immovables.Pusher(
                    ElementIndex.UpwardPusher,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible,
                    ElementRenderingType.Single,
                    PusherDirection.Up,
                    new(320, 320),
                    AAP64ColorPalette.Rust,
                    achievementManager
                ),

                // [042] Rightward Pusher
                new Elements.Solids.Immovables.Pusher(
                    ElementIndex.RightwardPusher,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible,
                    ElementRenderingType.Single,
                    PusherDirection.Right,
                    new(352, 320),
                    AAP64ColorPalette.Rust,
                    achievementManager
                ),

                // [043] Downward Pusher
                new Elements.Solids.Immovables.Pusher(
                    ElementIndex.DownwardPusher,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible,
                    ElementRenderingType.Single,
                    PusherDirection.Down,
                    new(384, 320),
                    AAP64ColorPalette.Rust,
                    achievementManager
                ),

                // [044] Leftward Pusher
                new Elements.Solids.Immovables.Pusher(
                    ElementIndex.LeftwardPusher,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible,
                    ElementRenderingType.Single,
                    PusherDirection.Left,
                    new(416, 320),
                    AAP64ColorPalette.Rust,
                    achievementManager
                ),

                // [045] Cloud
                new Elements.Gases.Cloud(
                    ElementIndex.Cloud,
                    ElementCategory.Gas,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 32),
                    AAP64ColorPalette.LightGrayBlue,
                    achievementManager
                ),

                // [046] Charged Cloud
                new Elements.Gases.ChargedCloud(
                    ElementIndex.ChargedCloud,
                    ElementCategory.Gas,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(480, 64),
                    AAP64ColorPalette.Slate,
                    achievementManager
                ),

                // [047] Lightning Head
                new Elements.Energies.LightningHead(
                    ElementIndex.LightningHead,
                    ElementCategory.Energy,
                    ElementCharacteristics.None,
                    ElementRenderingType.Single,
                    new(448, 320),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [048] Lightning Body
                new Elements.Energies.LightningBody(
                    ElementIndex.LightningBody,
                    ElementCategory.Energy,
                    ElementCharacteristics.AffectsNeighbors |
                                      ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsExplosionImmune,
                    ElementRenderingType.Single,
                    new(448, 320),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [049] Dry Wool (Black)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryBlackWool,
                    ElementIndex.WetBlackWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 96),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [050] Dry Wool (White)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryWhiteWool,
                    ElementIndex.WetWhiteWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 128),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [051] Dry Wool (Red)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryRedWool,
                    ElementIndex.WetRedWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 160),
                    AAP64ColorPalette.Crimson,
                    achievementManager
                ),

                // [052] Dry Wool (Orange)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryOrangeWool,
                    ElementIndex.WetOrangeWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 192),
                    AAP64ColorPalette.Orange,
                    achievementManager
                ),

                // [053] Dry Wool (Yellow)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryYellowWool,
                    ElementIndex.WetYellowWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 224),
                    AAP64ColorPalette.Gold,
                    achievementManager
                ),

                // [054] Dry Wool (Green)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryGreenWool,
                    ElementIndex.WetGreenWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 256),
                    AAP64ColorPalette.ForestGreen,
                    achievementManager
                ),

                // [055] Dry Wool (Gray)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryGrayWool,
                    ElementIndex.WetGrayWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(480, 288),
                    AAP64ColorPalette.Gunmetal,
                    achievementManager
                ),

                // [056] Dry Wool (Blue)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryBlueWool,
                    ElementIndex.WetBlueWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(640, 0),
                    AAP64ColorPalette.Cyan,
                    achievementManager
                ),

                // [057] Dry Wool (Violet)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryVioletWool,
                    ElementIndex.WetVioletWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(640, 32),
                    AAP64ColorPalette.Violet,
                    achievementManager
                ),

                // [058] Dry Wool (Brown)
                new Elements.Solids.Immovables.DryWool(
                    ElementIndex.DryBrownWool,
                    ElementIndex.WetBrownWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(640, 64),
                    AAP64ColorPalette.Brown,
                    achievementManager
                ),

                // [059] Wet Wool (Black)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetBlackWool,
                    ElementIndex.DryBlackWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 96),
                    AAP64ColorPalette.DarkGray.Darken(0.65f),
                    achievementManager
                ),

                // [060] Wet Wool (White)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetWhiteWool,
                    ElementIndex.DryWhiteWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 128),
                    AAP64ColorPalette.White.Darken(0.65f),
                    achievementManager
                ),

                // [061] Wet Wool (Red)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetRedWool,
                    ElementIndex.DryRedWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 160),
                    AAP64ColorPalette.Crimson.Darken(0.65f),
                    achievementManager
                ),

                // [062] Wet Wool (Orange)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetOrangeWool,
                    ElementIndex.DryOrangeWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 192),
                    AAP64ColorPalette.Orange.Darken(0.65f),
                    achievementManager
                ),

                // [063] Wet Wool (Yellow)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetYellowWool,
                    ElementIndex.DryYellowWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 224),
                    AAP64ColorPalette.Gold.Darken(0.65f),
                    achievementManager
                ),

                // [064] Wet Wool (Green)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetGreenWool,
                    ElementIndex.DryGreenWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 256),
                    AAP64ColorPalette.ForestGreen.Darken(0.65f),
                    achievementManager
                ),

                // [065] Wet Wool (Gray)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetGrayWool,
                    ElementIndex.DryGrayWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(640, 288),
                    AAP64ColorPalette.Gunmetal.Darken(0.65f),
                    achievementManager
                ),

                // [066] Wet Wool (Blue)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetBlueWool,
                    ElementIndex.DryBlueWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(800, 0),
                    AAP64ColorPalette.Cyan.Darken(0.65f),
                    achievementManager
                ),

                // [067] Wet Wool (Violet)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetVioletWool,
                    ElementIndex.DryVioletWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(800, 32),
                    AAP64ColorPalette.Violet.Darken(0.65f),
                    achievementManager
                ),

                // [068] Wet Wool (Brown)
                new Elements.Solids.Immovables.WetWool(
                    ElementIndex.WetBrownWool,
                    ElementIndex.DryBrownWool,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(800, 64),
                    AAP64ColorPalette.Brown.Darken(0.65f),
                    achievementManager
                ),

                // [069] Fertile Soil
                new Elements.Solids.Movables.FertileSoil(
                    ElementIndex.FertileSoil,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(800, 96),
                    AAP64ColorPalette.Burgundy,
                    achievementManager
                ),

                // [070] Seed
                new Elements.Solids.Movables.Seed(
                    ElementIndex.Seed,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(480, 320),
                    AAP64ColorPalette.DarkGreen,
                    achievementManager
                ),

                // [071] Sapling
                new Elements.Solids.Movables.Sapling(
                    ElementIndex.Sapling,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(512, 320),
                    AAP64ColorPalette.DarkTeal,
                    achievementManager
                ),

                // [072] Moss
                new Elements.Solids.Immovables.Moss(
                    ElementIndex.Moss,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(800, 128),
                    AAP64ColorPalette.PineGreen,
                    achievementManager
                ),

                // [073] Gunpowder
                new Elements.Solids.Movables.Gunpowder(
                    ElementIndex.Gunpowder,
                    ElementCategory.MovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(800, 160),
                    AAP64ColorPalette.Graphite,
                    achievementManager
                ),

                // [074] Liquefied Petroleum Gas
                new Elements.Gases.LiquefiedPetroleumGas(
                    ElementIndex.LiquefiedPetroleumGas,
                    ElementCategory.Gas,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsFlammable |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Blob,
                    new(800, 192),
                    AAP64ColorPalette.Amber,
                    achievementManager
                ),

                // [075] Obsidian
                new Elements.Solids.Immovables.Obsidian(
                    ElementIndex.Obsidian,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsExplosionImmune,
                    ElementRenderingType.Blob,
                    new(800, 224),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [076] Paint (Black)
                new Elements.Liquids.Paint(
                    ElementIndex.BlackPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(800, 256),
                    AAP64ColorPalette.DarkGray,
                    achievementManager
                ),

                // [077] Paint (White)
                new Elements.Liquids.Paint(
                    ElementIndex.WhitePaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(800, 288),
                    AAP64ColorPalette.White,
                    achievementManager
                ),

                // [078] Paint (Red)
                new Elements.Liquids.Paint(
                    ElementIndex.RedPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 0),
                    AAP64ColorPalette.Crimson,
                    achievementManager
                ),

                // [079] Paint (Orange)
                new Elements.Liquids.Paint(
                    ElementIndex.OrangePaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 32),
                    AAP64ColorPalette.Orange,
                    achievementManager
                ),

                // [080] Paint (Yellow)
                new Elements.Liquids.Paint(
                    ElementIndex.YellowPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 64),
                    AAP64ColorPalette.Gold,
                    achievementManager
                ),

                // [081] Paint (Green)
                new Elements.Liquids.Paint(
                    ElementIndex.GreenPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 96),
                    AAP64ColorPalette.ForestGreen,
                    achievementManager
                ),

                // [082] Paint (Cyan)
                new Elements.Liquids.Paint(
                    ElementIndex.CyanPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 128),
                    AAP64ColorPalette.Cyan,
                    achievementManager
                ),

                // [083] Paint (Gray)
                new Elements.Liquids.Paint(
                    ElementIndex.GrayPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 160),
                    AAP64ColorPalette.Gunmetal,
                    achievementManager
                ),

                // [084] Paint (Violet)
                new Elements.Liquids.Paint(
                    ElementIndex.VioletPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 192),
                    AAP64ColorPalette.Violet,
                    achievementManager
                ),

                // [085] Paint (Brown)
                new Elements.Liquids.Paint(
                    ElementIndex.BrownPaint,
                    ElementCategory.Liquid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsFlammable,
                    ElementRenderingType.Blob,
                    new(960, 224),
                    AAP64ColorPalette.Brown,
                    achievementManager
                ),

                // [086] Mercury
                new Elements.Liquids.Mercury(
                    ElementIndex.Mercury,
                    ElementCategory.Liquid,
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsConductive,
                    ElementRenderingType.Blob,
                    new(960, 256),
                    AAP64ColorPalette.Slate,
                    achievementManager
                ),

                // [087] Electricity
                new Elements.Energies.Electricity(
                    ElementIndex.Electricity,
                    ElementCategory.Energy,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsElectrified,
                    ElementRenderingType.Blob,
                    new(960, 288),
                    AAP64ColorPalette.Gold,
                    achievementManager
                ),

                // [088] Battery
                new Elements.Solids.Immovables.Battery(
                    ElementIndex.Battery,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable |
                    ElementCharacteristics.IsElectrified,
                    ElementRenderingType.Single,
                    new(576, 320),
                    AAP64ColorPalette.Orange,
                    achievementManager
                ),

                // [089] Lamp (Off)
                new Elements.Solids.Immovables.LampOff(
                    ElementIndex.LampOff,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(544, 320),
                    AAP64ColorPalette.Brown,
                    achievementManager
                ),

                // [090] Energy Transmitter
                new Elements.Solids.Immovables.EnergyTransmitter(
                    ElementIndex.EnergyTransmitter,
                    ElementCategory.ImmovableSolid,
                    ElementCharacteristics.AffectsNeighbors |
                    ElementCharacteristics.HasTemperature |
                    ElementCharacteristics.IsCorruptible |
                    ElementCharacteristics.IsPushable,
                    ElementRenderingType.Single,
                    new(608, 320),
                    AAP64ColorPalette.Brown,
                    achievementManager
                )
            ];
        }

        internal Element GetElement(ElementIndex index)
        {
            return elements[(int)index];
        }
    }
}
