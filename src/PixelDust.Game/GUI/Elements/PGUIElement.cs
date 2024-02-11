using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        // Settings
        public string Id { get; set; }

        // Internals
        internal PGUIRootElement RootElement { get; private set; }
        internal bool IsOpen { get; private set; }

        // Readonly
        public PGUIElement Parent => this.parent;
        public PGUIElement[] Children => [.. this.children];
        public bool HasChildren => this.children.Count > 0;

        private readonly List<PGUIElement> children = [];
        private PGUIElement parent;

        public PGUIElement()
        {
            this.Id = string.Empty;
        }

        public PGUIElement(string id)
        {
            this.Id = id;
        }

        internal void Open()
        {
            this.IsOpen = true;
        }

        internal void Close()
        {
            this.IsOpen = false;
        }

        internal void SetRootElement(PGUIRootElement rootElement)
        {
            this.RootElement = rootElement;
        }

        public void AppendChild(PGUIElement element)
        {
            element.parent?.RemoveChild(element);
            element.parent = this;

            this.children.Add(element);
        }

        public void RemoveAllChildren()
        {
            foreach (PGUIElement element in Children)
            {
                RemoveChild(element);
            }
        }

        public void RemoveChild(PGUIElement element)
        {
            element.parent = this.RootElement;
            this.children.Remove(element);
        }
    }
}
