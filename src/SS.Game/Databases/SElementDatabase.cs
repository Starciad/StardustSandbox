using StardustSandbox.Game.Elements;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
{
    public sealed class SElementDatabase(SGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly List<SElement> _registeredElements = [];

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
        }

        private void RegisterElement(SElement element)
        {
            element.Initialize();
            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint id) where T : SElement
        {
            return (T)GetElementById(id);
        }

        public SElement GetElementById(uint id)
        {
            return this._registeredElements[(int)id];
        }

        public uint GetIdOfElementType<T>() where T : SElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Id;
        }

        public T GetElementByType<T>() where T : SElement
        {
            return (T)GetElementByType(typeof(T));
        }

        public SElement GetElementByType(Type type)
        {
            return this._registeredElements.Find(x => x.GetType() == type);
        }
    }
}
