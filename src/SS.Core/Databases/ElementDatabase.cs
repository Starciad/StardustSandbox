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
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Extensions;

namespace StardustSandbox.Core.Databases
{
    internal sealed class ElementDatabase
    {
        private Element[] elements;

        internal void Load(GameEvents gameEvents)
        {
            this.elements = [
                // [000] Dirt
                new Elements.Common.Solids.Movables.Dirt(
                    ElementIndex.Dirt,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Clay)
                    {
                        TextureOriginOffset = new(0, 0),
                    },
                    gameEvents
                ),

                // [001] Mud
                new Elements.Common.Solids.Movables.Mud(
                    ElementIndex.Mud,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(87, 44, 45))
                    {
                        TextureOriginOffset = new(0, 32),
                    },
                    gameEvents
                ),

                // [002] Water
                new Elements.Common.Liquids.Water(
                    ElementIndex.Water,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, new(8, 120, 184))
                    {
                        TextureOriginOffset = new(0, 64),
                    },
                    gameEvents
                ),

                // [003] Stone
                new Elements.Common.Solids.Movables.Stone(
                    ElementIndex.Stone,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(66, 65, 65))
                    {
                        TextureOriginOffset = new(0, 96),
                    },
                    gameEvents
                ),

                // [004] Grass
                new Elements.Common.Solids.Movables.Grass(
                    ElementIndex.Grass,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(69, 110, 55))
                    {
                        TextureOriginOffset = new(0, 128),
                    },
                    gameEvents
                ),
                
                // [005] Ice
                new Elements.Common.Solids.Movables.Ice(
                    ElementIndex.Ice,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(34, 112, 255))
                    {
                        TextureOriginOffset = new(0, 160),
                    },
                    gameEvents
                ),

                // [006] Sand
                new Elements.Common.Solids.Movables.Sand(
                    ElementIndex.Sand,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(248, 246, 68))
                    {
                        TextureOriginOffset = new(0, 192),
                    },
                    gameEvents
                ),

                // [007] Snow
                new Elements.Common.Solids.Movables.Snow(
                    ElementIndex.Snow,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(189, 237, 246))
                    {
                        TextureOriginOffset = new(0, 224),
                    },
                    gameEvents
                ),

                // [008] Movable Corruption
                new Elements.Common.Solids.Movables.MovableCorruption(
                    ElementIndex.MovableCorruption,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.PurpleGray)
                    {
                        TextureOriginOffset = new(0, 256),
                    },
                    gameEvents
                ),
                
                // [009] Lava
                new Elements.Common.Liquids.Lava(
                    ElementIndex.Lava,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.OrangeRed)
                    {
                        TextureOriginOffset = new(0, 288),
                    },
                    gameEvents
                ),

                // [010] Acid
                new Elements.Common.Liquids.Acid(
                    ElementIndex.Acid,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, new(59, 167, 5))
                    {
                        TextureOriginOffset = new(160, 0),
                    },
                    gameEvents
                ),

                // [011] Glass
                new Elements.Common.Solids.Immovables.Glass(
                    ElementIndex.Glass,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(160, 32),
                    },
                    gameEvents
                ),

                // [012] Iron
                new Elements.Common.Solids.Immovables.Iron(
                    ElementIndex.Iron,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, new(66, 66, 66))
                    {
                        TextureOriginOffset = new(160, 64),
                    },
                    gameEvents
                ),

                // [013] Wall
                new Elements.Common.Solids.Immovables.Wall(
                    ElementIndex.Wall,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, new(22, 99, 50))
                    {
                        TextureOriginOffset = new(160, 96),
                    },
                    gameEvents
                ),

                // [014] Wood
                new Elements.Common.Solids.Immovables.Wood(
                    ElementIndex.Wood,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, new(92, 62, 0))
                    {
                        TextureOriginOffset = new(160, 128),
                    },
                    gameEvents
                ),

                // [015] Gas Corruption
                new Elements.Common.Gases.GasCorruption(
                    ElementIndex.GasCorruption,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.PurpleGray)
                    {
                        TextureOriginOffset = new(160, 160),
                    },
                    gameEvents
                ),

                // [016] Liquid Corruption
                new Elements.Common.Liquids.LiquidCorruption(
                    ElementIndex.LiquidCorruption,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.PurpleGray)
                    {
                        TextureOriginOffset = new(160, 192),
                    },
                    gameEvents
                ),

                // [017] Immovable Corruption
                new Elements.Common.Solids.Immovables.ImmovableCorruption(
                    ElementIndex.ImmovableCorruption,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.PurpleGray)
                    {
                        TextureOriginOffset = new(160, 224),
                    },
                    gameEvents
                ),

                // [018] Steam
                new Elements.Common.Gases.Steam(
                    ElementIndex.Steam,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, new(171, 208, 218))
                    {
                        TextureOriginOffset = new(160, 256),
                    },
                    gameEvents
                ),

                // [019] Smoke
                new Elements.Common.Gases.Smoke(
                    ElementIndex.Smoke,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, new(48, 48, 48))
                    {
                        TextureOriginOffset = new(160, 288),
                    },
                    gameEvents
                ),

                // [020] Brick
                new Elements.Common.Solids.Immovables.Brick(
                    ElementIndex.Brick,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Crimson)
                    {
                        TextureOriginOffset = new(320, 0),
                    },
                    gameEvents
                ),

                // [021] Leaf
                new Elements.Common.Solids.Immovables.Leaf(
                    ElementIndex.Leaf,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.MossGreen)
                    {
                        TextureOriginOffset = new(320, 32),
                    },
                    gameEvents
                ),

                // [022] Mounting Block
                new Elements.Common.Solids.Immovables.MountingBlock(
                    ElementIndex.MountingBlock,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(0, 320),
                    },
                    gameEvents
                ),

                // [023] Fire
                new Elements.Common.Energies.Fire(
                    ElementIndex.Fire,
                    ElementCategory.Energy,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Amber)
                    {
                        TextureOriginOffset = new(32, 320),
                    },
                    gameEvents
                ),

                // [024] Lamp On
                new Elements.Common.Solids.Immovables.LampOn(
                    ElementIndex.LampOn,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Sand)
                    {
                        TextureOriginOffset = new(64, 320),
                    },
                    gameEvents
                ),

                // [025] Void
                new Elements.Common.Solids.Immovables.Void(
                    ElementIndex.Void,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(320, 64),
                    },
                    gameEvents
                ),

                // [026] Clone
                new Elements.Common.Solids.Immovables.Clone(
                    ElementIndex.Clone,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Amber)
                    {
                        TextureOriginOffset = new(320, 96),
                    },
                    gameEvents
                ),

                // [027] Oil
                new Elements.Common.Liquids.Oil(
                    ElementIndex.Oil,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(320, 128),
                    },
                    gameEvents
                ),

                // [028] Salt
                new Elements.Common.Solids.Movables.Salt(
                    ElementIndex.Salt,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(320, 160),
                    },
                    gameEvents
                ),

                // [029] Saltwater
                new Elements.Common.Liquids.Saltwater(
                    ElementIndex.Saltwater,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, new(69, 188, 255))
                    {
                        TextureOriginOffset = new(320, 192),
                    },
                    gameEvents
                ),

                // [030] Bomb
                new Elements.Common.Solids.Movables.Bomb(
                    ElementIndex.Bomb,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(96, 320),
                    },
                    gameEvents
                ),

                // [031] Dynamite
                new Elements.Common.Solids.Movables.Dynamite(
                    ElementIndex.Dynamite,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Crimson.Darken(0.05f))
                    {
                        TextureOriginOffset = new(128, 320),
                    },
                    gameEvents
                ),

                // [032] TNT
                new Elements.Common.Solids.Movables.Tnt(
                    ElementIndex.Tnt,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Crimson.Darken(0.1f))
                    {
                        TextureOriginOffset = new(160, 320),
                    },
                    gameEvents
                ),

                // [033] Dry Sponge
                new Elements.Common.Solids.Immovables.DrySponge(
                    ElementIndex.DrySponge,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Amber)
                    {
                        TextureOriginOffset = new(320, 224),
                    },
                    gameEvents
                ),

                // [034] Wet Sponge
                new Elements.Common.Solids.Immovables.WetSponge(
                    ElementIndex.WetSponge,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Amber.Darken(0.1f))
                    {
                        TextureOriginOffset = new(320, 256),
                    },
                    gameEvents
                ),

                // [035] Gold
                new Elements.Common.Solids.Immovables.Gold(
                    ElementIndex.Gold,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.LemonYellow)
                    {
                        TextureOriginOffset = new(192, 320),
                    },
                    gameEvents
                ),

                // [036] Heater
                new Elements.Common.Solids.Immovables.TemperatureModifier(
                    ElementIndex.Heater,
                    ElementCategory.ImmovableSolid,
                    TemperatureModifierMode.Warming,
                    new(ElementRenderingType.Single, AAP64ColorPalette.DarkRed)
                    {
                        TextureOriginOffset = new(224, 320),
                    },
                    gameEvents
                ),

                // [037] Freezer
                new Elements.Common.Solids.Immovables.TemperatureModifier(
                    ElementIndex.Freezer,
                    ElementCategory.ImmovableSolid,
                    TemperatureModifierMode.Cooling,
                    new(ElementRenderingType.Single, AAP64ColorPalette.NavyBlue)
                    {
                        TextureOriginOffset = new(256, 320),
                    },
                    gameEvents
                ),

                // [038] Ash
                new Elements.Common.Solids.Movables.Ash(
                    ElementIndex.Ash,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, new(30, 33, 38))
                    {
                        TextureOriginOffset = new(320, 288),
                    },
                    gameEvents
                ),

                // [039] Anti-Corruption
                new Elements.Common.Gases.AntiCorruption(
                    ElementIndex.AntiCorruption,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Crimson)
                    {
                        TextureOriginOffset = new(480, 0),
                    },
                    gameEvents
                ),

                // [040] Devourer
                new Elements.Common.Solids.Immovables.Devourer(
                    ElementIndex.Devourer,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Coal)
                    {
                        TextureOriginOffset = new(288, 320),
                    },
                    gameEvents
                ),

                // [041] Upward Pusher
                new Elements.Common.Solids.Immovables.Pusher(
                    ElementIndex.UpwardPusher,
                    ElementCategory.ImmovableSolid,
                    PusherDirection.Up,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Rust)
                    {
                        TextureOriginOffset = new(320, 320),
                    },
                    gameEvents
                ),

                // [042] Rightward Pusher
                new Elements.Common.Solids.Immovables.Pusher(
                    ElementIndex.RightwardPusher,
                    ElementCategory.ImmovableSolid,
                    PusherDirection.Right,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Rust)
                    {
                        TextureOriginOffset = new(352, 320),
                    },
                    gameEvents
                ),

                // [043] Downward Pusher
                new Elements.Common.Solids.Immovables.Pusher(
                    ElementIndex.DownwardPusher,
                    ElementCategory.ImmovableSolid,
                    PusherDirection.Down,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Rust)
                    {
                        TextureOriginOffset = new(384, 320),
                    },
                    gameEvents
                ),

                // [044] Leftward Pusher
                new Elements.Common.Solids.Immovables.Pusher(
                    ElementIndex.LeftwardPusher,
                    ElementCategory.ImmovableSolid,
                    PusherDirection.Left,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Rust)
                    {
                        TextureOriginOffset = new(416, 320),
                    },
                    gameEvents
                ),

                // [045] Cloud
                new Elements.Common.Gases.Cloud(
                    ElementIndex.Cloud,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.LightGrayBlue)
                    {
                        TextureOriginOffset = new(480, 32),
                    },
                    gameEvents
                ),

                // [046] Charged Cloud
                new Elements.Common.Gases.ChargedCloud(
                    ElementIndex.ChargedCloud,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Slate)
                    {
                        TextureOriginOffset = new(480, 64),
                    },
                    gameEvents
                ),

                // [047] Lightning Head
                new Elements.Common.Energies.LightningHead(
                    ElementIndex.LightningHead,
                    ElementCategory.Energy,
                    new(ElementRenderingType.Single, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(448, 320),
                    },
                    gameEvents
                ),

                // [048] Lightning Body
                new Elements.Common.Energies.LightningBody(
                    ElementIndex.LightningBody,
                    ElementCategory.Energy,
                    new(ElementRenderingType.Single, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(448, 320),
                    },
                    gameEvents
                ),

                // [049] Dry Wool (Black)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryBlackWool,
                    ElementIndex.WetBlackWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(480, 96),
                    },
                    gameEvents
                ),

                // [050] Dry Wool (White)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryWhiteWool,
                    ElementIndex.WetWhiteWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(480, 128),
                    },
                    gameEvents
                ),

                // [051] Dry Wool (Red)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryRedWool,
                    ElementIndex.WetRedWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Crimson)
                    {
                        TextureOriginOffset = new(480, 160),
                    },
                    gameEvents
                ),

                // [052] Dry Wool (Orange)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryOrangeWool,
                    ElementIndex.WetOrangeWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Orange)
                    {
                        TextureOriginOffset = new(480, 192),
                    },
                    gameEvents
                ),

                // [053] Dry Wool (Yellow)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryYellowWool,
                    ElementIndex.WetYellowWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gold)
                    {
                        TextureOriginOffset = new(480, 224),
                    },
                    gameEvents
                ),

                // [054] Dry Wool (Green)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryGreenWool,
                    ElementIndex.WetGreenWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.ForestGreen)
                    {
                        TextureOriginOffset = new(480, 256),
                    },
                    gameEvents
                ),

                // [055] Dry Wool (Gray)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryGrayWool,
                    ElementIndex.WetGrayWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gunmetal)
                    {
                        TextureOriginOffset = new(480, 288),
                    },
                    gameEvents
                ),

                // [056] Dry Wool (Blue)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryBlueWool,
                    ElementIndex.WetBlueWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Cyan)
                    {
                        TextureOriginOffset = new(640, 0),
                    },
                    gameEvents
                ),

                // [057] Dry Wool (Violet)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryVioletWool,
                    ElementIndex.WetVioletWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Violet)
                    {
                        TextureOriginOffset = new(640, 32),
                    },
                    gameEvents
                ),

                // [058] Dry Wool (Brown)
                new Elements.Common.Solids.Immovables.DryWool(
                    ElementIndex.DryBrownWool,
                    ElementIndex.WetBrownWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Brown)
                    {
                        TextureOriginOffset = new(640, 64),
                    },
                    gameEvents
                ),

                // [059] Wet Wool (Black)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetBlackWool,
                    ElementIndex.DryBlackWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 96),
                    },
                    gameEvents
                ),

                // [060] Wet Wool (White)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetWhiteWool,
                    ElementIndex.DryWhiteWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.White.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 128),
                    },
                    gameEvents
                ),

                // [061] Wet Wool (Red)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetRedWool,
                    ElementIndex.DryRedWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Crimson.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 160),
                    },
                    gameEvents
                ),

                // [062] Wet Wool (Orange)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetOrangeWool,
                    ElementIndex.DryOrangeWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Orange.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 192),
                    },
                    gameEvents
                ),

                // [063] Wet Wool (Yellow)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetYellowWool,
                    ElementIndex.DryYellowWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gold.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 224),
                    },
                    gameEvents
                ),

                // [064] Wet Wool (Green)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetGreenWool,
                    ElementIndex.DryGreenWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.ForestGreen.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 256),
                    },
                    gameEvents
                ),

                // [065] Wet Wool (Gray)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetGrayWool,
                    ElementIndex.DryGrayWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gunmetal.Darken(0.65f))
                    {
                        TextureOriginOffset = new(640, 288),
                    },
                    gameEvents
                ),

                // [066] Wet Wool (Blue)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetBlueWool,
                    ElementIndex.DryBlueWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Cyan.Darken(0.65f))
                    {
                        TextureOriginOffset = new(800, 0),
                    },
                    gameEvents
                ),

                // [067] Wet Wool (Violet)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetVioletWool,
                    ElementIndex.DryVioletWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Violet.Darken(0.65f))
                    {
                        TextureOriginOffset = new(800, 32),
                    },
                    gameEvents
                ),

                // [068] Wet Wool (Brown)
                new Elements.Common.Solids.Immovables.WetWool(
                    ElementIndex.WetBrownWool,
                    ElementIndex.DryBrownWool,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Brown.Darken(0.65f))
                    {
                        TextureOriginOffset = new(800, 64),    
                    },
                    gameEvents
                ),

                // [069] Fertile Soil
                new Elements.Common.Solids.Movables.FertileSoil(
                    ElementIndex.FertileSoil,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Burgundy)
                    {
                        TextureOriginOffset = new(800, 96),
                    },
                    gameEvents
                ),

                // [070] Seed
                new Elements.Common.Solids.Movables.Seed(
                    ElementIndex.Seed,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.DarkGreen)
                    {
                        TextureOriginOffset = new(480, 320),    
                    },
                    gameEvents
                ),

                // [071] Sapling
                new Elements.Common.Solids.Movables.Sapling(
                    ElementIndex.Sapling,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.DarkTeal)
                    {
                        TextureOriginOffset = new(512, 320),
                    },
                    gameEvents
                ),

                // [072] Moss
                new Elements.Common.Solids.Immovables.Moss(
                    ElementIndex.Moss,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.PineGreen)
                    {
                        TextureOriginOffset = new(800, 128),
                    },
                    gameEvents
                ),

                // [073] Gunpowder
                new Elements.Common.Solids.Movables.Gunpowder(
                    ElementIndex.Gunpowder,
                    ElementCategory.MovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Graphite)
                    {
                        TextureOriginOffset = new(800, 160),
                    },
                    gameEvents
                ),

                // [074] Liquefied Petroleum Gas
                new Elements.Common.Gases.LiquefiedPetroleumGas(
                    ElementIndex.LiquefiedPetroleumGas,
                    ElementCategory.Gas,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Amber)
                    {
                        TextureOriginOffset = new(800, 192),
                    },
                    gameEvents
                ),

                // [075] Obsidian
                new Elements.Common.Solids.Immovables.Obsidian(
                    ElementIndex.Obsidian,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(800, 224),
                    },
                    gameEvents
                ),

                // [076] Paint (Black)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.BlackPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.DarkGray)
                    {
                        TextureOriginOffset = new(800, 256),
                    },
                    gameEvents
                ),

                // [077] Paint (White)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.WhitePaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.White)
                    {
                        TextureOriginOffset = new(800, 288),
                    },
                    gameEvents
                ),

                // [078] Paint (Red)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.RedPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Crimson)
                    {
                        TextureOriginOffset = new(960, 0),
                    },
                    gameEvents
                ),

                // [079] Paint (Orange)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.OrangePaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Orange)
                    {
                        TextureOriginOffset = new(960, 32),
                    },
                    gameEvents
                ),

                // [080] Paint (Yellow)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.YellowPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gold)
                    {
                        TextureOriginOffset = new(960, 64),
                    },
                    gameEvents
                ),

                // [081] Paint (Green)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.GreenPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.ForestGreen)
                    {
                        TextureOriginOffset = new(960, 96),
                    },
                    gameEvents
                ),

                // [082] Paint (Cyan)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.CyanPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Cyan)
                    {
                        TextureOriginOffset = new(960, 128),
                    },
                    gameEvents
                ),

                // [083] Paint (Gray)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.GrayPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gunmetal)
                    {
                        TextureOriginOffset = new(960, 160),
                    },
                    gameEvents
                ),

                // [084] Paint (Violet)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.VioletPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Violet)
                    {
                        TextureOriginOffset = new(960, 192),
                    },
                    gameEvents
                ),

                // [085] Paint (Brown)
                new Elements.Common.Liquids.Paint(
                    ElementIndex.BrownPaint,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Brown)
                    {
                        TextureOriginOffset = new(960, 224),
                    },
                    gameEvents
                ),

                // [086] Mercury
                new Elements.Common.Liquids.Mercury(
                    ElementIndex.Mercury,
                    ElementCategory.Liquid,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Slate)
                    {
                        TextureOriginOffset = new(960, 256),
                    },
                    gameEvents
                ),

                // [087] Electricity
                new Elements.Common.Energies.Electricity(
                    ElementIndex.Electricity,
                    ElementCategory.Energy,
                    new(ElementRenderingType.Blob, AAP64ColorPalette.Gold)
                    {
                        TextureOriginOffset = new(960, 288),
                    },
                    gameEvents
                ),

                // [088] Battery
                new Elements.Common.Solids.Immovables.Battery(
                    ElementIndex.Battery,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Orange)
                    {
                        TextureOriginOffset = new(576, 320),
                    },
                    gameEvents
                ),

                // [089] Lamp (Off)
                new Elements.Common.Solids.Immovables.LampOff(
                    ElementIndex.LampOff,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Brown)
                    {
                        TextureOriginOffset = new(544, 320),
                    },
                    gameEvents
                ),

                // [090] Energy Transmitter
                new Elements.Common.Solids.Immovables.EnergyTransmitter(
                    ElementIndex.EnergyTransmitter,
                    ElementCategory.ImmovableSolid,
                    new(ElementRenderingType.Single, AAP64ColorPalette.Brown)
                    {
                        TextureOriginOffset = new(608, 320),
                    },
                    gameEvents
                )
            ];
        }

        internal Element GetElement(ElementIndex index)
        {
            return index is ElementIndex.None ? null : this.elements[((byte)index) - 1];
        }
    }
}
