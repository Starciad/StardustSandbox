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
                    ReferenceColor = Color.White,

                    DefaultTemperature = 20.0,
                    DefaultDensity = 1600.0,
                    DefaultExplosionResistance = 1.0,
                },
                new Elements.Solids.Movables.Mud()
                {
                    Index = ElementIndex.Mud,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 32),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 18.0,
                    DefaultDensity = 1500.0,
                    DefaultExplosionResistance = 0.6,
                },
                new Elements.Liquids.Water()
                {
                    Index = ElementIndex.Water,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 64),
                    ReferenceColor = Color.White,

                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0,
                    DefaultDensity = 1000.0,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Movables.Stone()
                {
                    Index = ElementIndex.Stone,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 96),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 20.0,
                    DefaultDensity = 2500.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Grass()
                {
                    Index = ElementIndex.Grass,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 128),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultFlammabilityResistance = 10.0,
                    DefaultDensity = 1100.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Ice()
                {
                    Index = ElementIndex.Ice,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 160),
                    ReferenceColor = Color.White,

                    DefaultTemperature = -25.0,
                    DefaultDensity = 920.0,
                    DefaultExplosionResistance = 1.2,
                },
                new Elements.Solids.Movables.Sand()
                {
                    Index = ElementIndex.Sand,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 192),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultDensity = 1500.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Snow()
                {
                    Index = ElementIndex.Snow,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 224),
                    ReferenceColor = Color.White,

                    DefaultTemperature = -15.0,
                    DefaultDensity = 600.0,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.MovableCorruption()
                {
                    Index = ElementIndex.MovableCorruption,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 256),
                    ReferenceColor = Color.White,

                    DefaultDensity = 1400.0,
                    DefaultExplosionResistance = 0.8,
                },
                new Elements.Liquids.Lava()
                {
                    Index = ElementIndex.Lava,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 288),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 1000.0,
                    DefaultDensity = 3000.0,
                    DefaultExplosionResistance = 0.4,
                },
                new Elements.Liquids.Acid()
                {
                    Index = ElementIndex.Acid,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 0),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 10.0,
                    DefaultDensity = 1100.0,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Immovables.Glass()
                {
                    Index = ElementIndex.Glass,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 32),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 25.0,
                    DefaultDensity = 2500.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Immovables.Iron()
                {
                    Index = ElementIndex.Iron,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 64),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 30.0,
                    DefaultDensity = 7800.0,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Immovables.Wall()
                {
                    Index = ElementIndex.Wall,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 96),
                    ReferenceColor = Color.White,

                    DefaultDensity = 2200.0,
                },
                new Elements.Solids.Immovables.Wood()
                {
                    Index = ElementIndex.Wood,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 128),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 20.0,
                    DefaultFlammabilityResistance = 35.0,
                    DefaultDensity = 700.0,
                    DefaultExplosionResistance = 1.5,
                },
                new Elements.Gases.GasCorruption()
                {
                    Index = ElementIndex.GasCorruption,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 160),
                    ReferenceColor = Color.White,

                    DefaultDensity = 5.0,
                },
                new Elements.Liquids.LiquidCorruption()
                {
                    Index = ElementIndex.LiquidCorruption,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 192),
                    ReferenceColor = Color.White,

                    DefaultDensity = 1050.0,
                    DefaultExplosionResistance = 0.1,
                },
                new Elements.Solids.Immovables.ImmovableCorruption()
                {
                    Index = ElementIndex.ImmovableCorruption,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 224),
                    ReferenceColor = Color.White,

                    DefaultDensity = 1600.0,
                    DefaultExplosionResistance = 1.2,
                },
                new Elements.Gases.Steam()
                {
                    Index = ElementIndex.Steam,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 256),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 200.0,
                    DefaultDensity = 1.0,
                },
                new Elements.Gases.Smoke()
                {
                    Index = ElementIndex.Smoke,
                    Category = ElementCategory.Gas,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(160, 288),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 350.0,
                    DefaultDensity = 2.0,
                },
                new Elements.Solids.Immovables.RedBrick()
                {
                    Index = ElementIndex.RedBrick,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 0),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 25.0,
                    DefaultDensity = 2400.0,
                    DefaultExplosionResistance = 2.5,
                },
                new Elements.Solids.Immovables.TreeLeaf()
                {
                    Index = ElementIndex.TreeLeaf,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 32),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultFlammabilityResistance = 5.0,
                    DefaultDensity = 400.0,
                    DefaultExplosionResistance = 0.1,
                },
                new Elements.Solids.Immovables.MountingBlock()
                {
                    Index = ElementIndex.MountingBlock,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(0, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 20.0,
                    DefaultFlammabilityResistance = 150.0,
                    DefaultDensity = 1800.0,
                    DefaultExplosionResistance = 1.5,
                },
                new Elements.Energies.Fire()
                {
                    Index = ElementIndex.Fire,
                    Category = ElementCategory.Energy,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsExplosionImmune | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(32, 320),
                    ReferenceColor = Color.White,

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
                    ReferenceColor = Color.White,

                    DefaultTemperature = 26.0,
                    DefaultDensity = 2800.0,
                },
                new Elements.Solids.Immovables.Void()
                {
                    Index = ElementIndex.Void,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(360, 64),
                    ReferenceColor = Color.White,

                    DefaultDensity = 220.0,
                },
                new Elements.Solids.Immovables.Clone()
                {
                    Index = ElementIndex.Clone,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsExplosionImmune,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(360, 96),
                    ReferenceColor = Color.White,

                    DefaultDensity = 3000.0,
                },
                new Elements.Liquids.Oil()
                {
                    Index = ElementIndex.Oil,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 128),
                    ReferenceColor = Color.White,

                    DefaultFlammabilityResistance = 5.0,
                    DefaultDensity = 980.0,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.Salt()
                {
                    Index = ElementIndex.Salt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 160),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2200.0,
                    DefaultExplosionResistance = 0.7,
                },
                new Elements.Liquids.Saltwater()
                {
                    Index = ElementIndex.Saltwater,
                    Category = ElementCategory.Liquid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 192),
                    ReferenceColor = Color.White,

                    DefaultDispersionRate = 3,
                    DefaultTemperature = 25.0,
                    DefaultDensity = 1200.0,
                    DefaultExplosionResistance = 0.2,
                },
                new Elements.Solids.Movables.Bomb()
                {
                    Index = ElementIndex.Bomb,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(96, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 25.0,
                    DefaultDensity = 3500.0,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Movables.Dynamite()
                {
                    Index = ElementIndex.Dynamite,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(128, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2400.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Movables.Tnt()
                {
                    Index = ElementIndex.Tnt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsExplosive | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(160, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultDensity = 2800.0,
                    DefaultExplosionResistance = 0.35,
                },
                new Elements.Solids.Immovables.DrySponge()
                {
                    Index = ElementIndex.DrySponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 224),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 25.0,
                    DefaultFlammabilityResistance = 10.0,
                    DefaultDensity = 550.0,
                    DefaultExplosionResistance = 0.5,
                },
                new Elements.Solids.Immovables.WetSponge()
                {
                    Index = ElementIndex.WetSponge,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 256),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 20.0,
                    DefaultDensity = 1200.0,
                    DefaultExplosionResistance = 0.8,
                },
                new Elements.Solids.Immovables.Gold()
                {
                    Index = ElementIndex.Gold,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(192, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 22.0,
                    DefaultDensity = 17_150.0,
                    DefaultExplosionResistance = 0.3,
                },
                new Elements.Solids.Immovables.Heater()
                {
                    Index = ElementIndex.Heater,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsFlammable | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(224, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 0.0,
                    DefaultDensity = 1500.0,
                    DefaultExplosionResistance = 2.5,
                },
                new Elements.Solids.Immovables.Freezer()
                {
                    Index = ElementIndex.Freezer,
                    Category = ElementCategory.ImmovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Single,
                    TextureOriginOffset = new(256, 320),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 0.0,
                    DefaultDensity = 1500.0,
                },
                new Elements.Solids.Movables.Ash()
                {
                    Index = ElementIndex.Ash,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(320, 288),
                    ReferenceColor = Color.White,

                    DefaultTemperature = 40.0,
                    DefaultDensity = 350.0,
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
