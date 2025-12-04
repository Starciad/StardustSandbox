using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;

using System;

namespace StardustSandbox.Databases
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
                new Elements.Solids.Movables.Dirt()
                {
                    Index = ElementIndex.Dirt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 0),
                    ReferenceColor = new Color(181, 140, 90),

                    DefaultTemperature = 20.0,
                    DefaultDensity = 1.6,
                    DefaultExplosionResistance = 1.0,
                },
                new Elements.Solids.Movables.Mud()
                {
                    Index = ElementIndex.Mud,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 32),
                    ReferenceColor = new Color(120, 90, 60),

                    DefaultTemperature = 18.0,
                    DefaultDensity = 1.5,
                    DefaultExplosionResistance = 0.6,
                },
                new Elements.Liquids.Water()
                {
                    Index = ElementIndex.Water,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 64),
                    ReferenceColor = new Color(100, 180, 255),

                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0,
                    DefaultDensity = 1.0,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Movables.Stone()
                {
                    Index = ElementIndex.Stone,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 96),
                    ReferenceColor = new Color(150, 150, 160),

                    DefaultTemperature = 20.0,
                    DefaultDensity = 2.5,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Grass()
                {
                    Index = ElementIndex.Grass,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 128),
                    ReferenceColor = new Color(120, 200, 120),

                    DefaultTemperature = 22.0,
                    DefaultFlammabilityResistance = 10.0,
                    DefaultDensity = 0.1,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Ice()
                {
                    Index = ElementIndex.Ice,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 160),
                    ReferenceColor = new Color(180, 220, 255),

                    DefaultTemperature = -25.0,
                    DefaultDensity = 0.92,
                    DefaultExplosionResistance = 1.2,
                },
                new Elements.Solids.Movables.Sand()
                {
                    Index = ElementIndex.Sand,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 192),
                    ReferenceColor = new Color(240, 220, 170),

                    DefaultTemperature = 22.0,
                    DefaultDensity = 1.5,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Snow()
                {
                    Index = ElementIndex.Snow,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 224),
                    ReferenceColor = new Color(230, 240, 255),

                    DefaultTemperature = -15.0,
                    DefaultDensity = 0.1,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.MovableCorruption()
                {
                    Index = ElementIndex.MovableCorruption,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 256),
                    ReferenceColor = new Color(90, 40, 120),

                    DefaultDensity = 1.4,
                    DefaultExplosionResistance = 0.8,
                },
                new Elements.Liquids.Lava()
                {
                    Index = ElementIndex.Lava,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 288),
                    ReferenceColor = new Color(255, 120, 60),

                    DefaultTemperature = 1000.0,
                    DefaultDensity = 2.7,
                    DefaultExplosionResistance = 0.4,
                },
                new Elements.Liquids.Acid()
                {
                    Index = ElementIndex.Acid,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 0),
                    ReferenceColor = new Color(180, 255, 120),

                    DefaultTemperature = 10.0,
                    DefaultDensity = 1.2,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Immovables.Glass()
                {
                    Index = ElementIndex.Glass,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 32),
                    ReferenceColor = new Color(200, 220, 255),

                    DefaultTemperature = 25.0,
                    DefaultDensity = 2.5,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Immovables.Iron()
                {
                    Index = ElementIndex.Iron,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 64),
                    ReferenceColor = new Color(180, 180, 200),

                    DefaultTemperature = 30.0,
                    DefaultDensity = 7.8,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Immovables.Wall()
                {
                    Index = ElementIndex.Wall,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 96),
                    ReferenceColor = new Color(120, 120, 120),

                    DefaultDensity = 2.2,
                },
                new Elements.Solids.Immovables.Wood()
                {
                    Index = ElementIndex.Wood,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 128),
                    ReferenceColor = new Color(170, 120, 70),

                    DefaultTemperature = 20.0,
                    DefaultFlammabilityResistance = 35.0,
                    DefaultDensity = 0.7,
                    DefaultExplosionResistance = 1.5,
                },
                new Elements.Gases.GasCorruption()
                {
                    Index = ElementIndex.GasCorruption,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 160),
                    ReferenceColor = new Color(110, 60, 150),

                    DefaultDensity = 0.005,
                },
                new Elements.Liquids.LiquidCorruption()
                {
                    Index = ElementIndex.LiquidCorruption,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 192),
                    ReferenceColor = new Color(100, 50, 130),

                    DefaultDensity = 1.05,
                    DefaultExplosionResistance = 0.1,
                },
                new Elements.Solids.Immovables.ImmovableCorruption()
                {
                    Index = ElementIndex.ImmovableCorruption,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 224),
                    ReferenceColor = new Color(80, 30, 100),

                    DefaultDensity = 1.6,
                    DefaultExplosionResistance = 1.2,
                },
                new Elements.Gases.Steam()
                {
                    Index = ElementIndex.Steam,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 256),
                    ReferenceColor = new Color(210, 220, 230),

                    DefaultTemperature = 200.0,
                    DefaultDensity = 0.0006,
                },
                new Elements.Gases.Smoke()
                {
                    Index = ElementIndex.Smoke,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 288),
                    ReferenceColor = new Color(120, 120, 120),

                    DefaultTemperature = 350.0,
                    DefaultDensity = 0.002,
                },
                new Elements.Solids.Immovables.RedBrick()
                {
                    Index = ElementIndex.RedBrick,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 0),
                    ReferenceColor = new Color(200, 80, 60),

                    DefaultTemperature = 25.0,
                    DefaultDensity = 2.4,
                    DefaultExplosionResistance = 2.5,
                },
                new Elements.Solids.Immovables.TreeLeaf()
                {
                    Index = ElementIndex.TreeLeaf,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 32),
                    ReferenceColor = new Color(140, 200, 100),

                    DefaultTemperature = 22.0,
                    DefaultFlammabilityResistance = 5.0,
                    DefaultDensity = 0.04,
                    DefaultExplosionResistance = 0.1,
                },
                new Elements.Solids.Immovables.MountingBlock()
                {
                    Index = ElementIndex.MountingBlock,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(0, 320),
                    ReferenceColor = new Color(180, 160, 120),

                    DefaultTemperature = 20.0,
                    DefaultFlammabilityResistance = 150.0,
                    DefaultDensity = 1.8,
                    DefaultExplosionResistance = 1.5,
                },
                new Elements.Energies.Fire()
                {
                    Index = ElementIndex.Fire,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsExplosionImmune | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(32, 320),
                    ReferenceColor = new Color(255, 180, 60),

                    DefaultTemperature = 500.0,
                    DefaultDensity = 0.0,
                },
                new Elements.Solids.Immovables.Lamp()
                {
                    Index = ElementIndex.Lamp,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(64, 320),
                    ReferenceColor = new Color(255, 255, 200),

                    DefaultTemperature = 26.0,
                    DefaultDensity = 2.8,
                },
                new Elements.Solids.Immovables.Void()
                {
                    Index = ElementIndex.Void,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 64),
                    ReferenceColor = new Color(0, 0, 0),

                    DefaultDensity = 0.001,
                },
                new Elements.Solids.Immovables.Clone()
                {
                    Index = ElementIndex.Clone,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 96),
                    ReferenceColor = new Color(255, 230, 120),

                    DefaultDensity = 3.0,
                },
                new Elements.Liquids.Oil()
                {
                    Index = ElementIndex.Oil,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 128),
                    ReferenceColor = new Color(60, 60, 40),

                    DefaultFlammabilityResistance = 5.0,
                    DefaultDensity = 0.92,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.Salt()
                {
                    Index = ElementIndex.Salt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 160),
                    ReferenceColor = new Color(230, 230, 255),

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2.2,
                    DefaultExplosionResistance = 0.7,
                },
                new Elements.Liquids.Saltwater()
                {
                    Index = ElementIndex.Saltwater,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 192),
                    ReferenceColor = new Color(160, 200, 255),

                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0,
                    DefaultDensity = 1.03,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Movables.Bomb()
                {
                    Index = ElementIndex.Bomb,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(96, 320),
                    ReferenceColor = new Color(80, 80, 80),

                    DefaultTemperature = 25.0,
                    DefaultDensity = 3.5,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.Dynamite()
                {
                    Index = ElementIndex.Dynamite,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(128, 320),
                    ReferenceColor = new Color(220, 60, 60),

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2.4,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Tnt()
                {
                    Index = ElementIndex.Tnt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsExplosive | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(160, 320),
                    ReferenceColor = new Color(255, 80, 80),

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2.8,
                    DefaultExplosionResistance = 0.35,
                },
                new Elements.Solids.Immovables.DrySponge()
                {
                    Index = ElementIndex.DrySponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 224),
                    ReferenceColor = new Color(255, 240, 120),

                    DefaultTemperature = 25.0,
                    DefaultFlammabilityResistance = 10.0,
                    DefaultDensity = 0.055,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Immovables.WetSponge()
                {
                    Index = ElementIndex.WetSponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 256),
                    ReferenceColor = new Color(200, 220, 120),

                    DefaultTemperature = 20.0,
                    DefaultDensity = 1.2,
                    DefaultExplosionResistance = 0.8,
                },
                new Elements.Solids.Immovables.Gold()
                {
                    Index = ElementIndex.Gold,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(192, 320),
                    ReferenceColor = new Color(255, 215, 80),

                    DefaultTemperature = 22.0,
                    DefaultDensity = 19.3,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Immovables.Heater()
                {
                    Index = ElementIndex.Heater,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(224, 320),
                    ReferenceColor = new Color(255, 120, 60),

                    DefaultTemperature = 0.0,
                    DefaultDensity = 1.5,
                    DefaultExplosionResistance = 2.5,
                },
                new Elements.Solids.Immovables.Freezer()
                {
                    Index = ElementIndex.Freezer,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(256, 320),
                    ReferenceColor = new Color(180, 220, 255),

                    DefaultTemperature = 0.0,
                    DefaultDensity = 1.5,
                },
                new Elements.Solids.Movables.Ash()
                {
                    Index = ElementIndex.Ash,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 288),
                    ReferenceColor = new Color(180, 180, 180),

                    DefaultTemperature = 40.0,
                    DefaultDensity = 0.35,
                    DefaultExplosionResistance = 0.0,
                },
            ];

            isLoaded = true;
        }

        internal static Element GetElement(ElementIndex index)
        {
            return elements[(int)index];
        }

        internal static Element GetElement(Type type)
        {
            return Array.Find(elements, x => x.GetType() == type);
        }
    }
}
