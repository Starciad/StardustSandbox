using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.Resources.Elements.Bundle.Energies;
using StardustSandbox.Game.Resources.Elements.Bundle.Gases;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
{
    public sealed class SElementDatabase(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly List<ISElement> _registeredElements = [];

        public override void Initialize()
        {
            // ID : 00
            RegisterElement(new SDirt(this.SGameInstance));

            // ID : 01
            RegisterElement(new SMud(this.SGameInstance));

            // ID : 02
            RegisterElement(new SWater(this.SGameInstance));

            // ID : 03
            RegisterElement(new SStone(this.SGameInstance));

            // ID : 04
            RegisterElement(new SGrass(this.SGameInstance));

            // ID : 05
            RegisterElement(new SIce(this.SGameInstance));

            // ID : 06
            RegisterElement(new SSand(this.SGameInstance));

            // ID : 07
            RegisterElement(new SSnow(this.SGameInstance));

            // ID : 08
            RegisterElement(new SMCorruption(this.SGameInstance));

            // ID : 09
            RegisterElement(new SLava(this.SGameInstance));

            // ID : 10
            RegisterElement(new SAcid(this.SGameInstance));

            // ID : 11
            RegisterElement(new SGlass(this.SGameInstance));

            // ID : 12
            RegisterElement(new SMetal(this.SGameInstance));

            // ID : 13
            RegisterElement(new SWall(this.SGameInstance));

            // ID : 14
            RegisterElement(new SWood(this.SGameInstance));

            // ID : 15
            RegisterElement(new SGCorruption(this.SGameInstance));

            // ID : 16
            RegisterElement(new SLCorruption(this.SGameInstance));

            // ID : 17
            RegisterElement(new SIMCorruption(this.SGameInstance));

            // ID : 18
            RegisterElement(new SSteam(this.SGameInstance));

            // ID : 19
            RegisterElement(new SSmoke(this.SGameInstance));

            // ID : 20
            RegisterElement(new SRedBrick(this.SGameInstance));

            // ID : 21
            RegisterElement(new STreeLeaf(this.SGameInstance));

            // ID : 22
            RegisterElement(new SMountingBlock(this.SGameInstance));

            // ID : 23
            RegisterElement(new SFire(this.SGameInstance));
        }

        private void RegisterElement(SElement element)
        {
            element.Initialize();
            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint id) where T : ISElement
        {
            return (T)GetElementById(id);
        }

        public ISElement GetElementById(uint id)
        {
            return this._registeredElements[(int)id];
        }

        public uint GetIdOfElementType<T>() where T : ISElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Id;
        }

        public T GetElementByType<T>() where T : ISElement
        {
            return (T)GetElementByType(typeof(T));
        }

        public ISElement GetElementByType(Type type)
        {
            return this._registeredElements.Find(x => x.GetType() == type);
        }
    }
}
