using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        // Settings
        public string Id { get; set; }

        // System
        protected PGUILayout GUILayout { get; private set; }

        // Readonly
        public PGUIElementStyle Style => this.style;
        public Vector2 Position => this.position;

        // Parental
        public PGUIElement Parent => this.parent;
        public PGUIElement[] Children => [.. this.children];
        public bool HasChildren => this.children.Count > 0;

        private readonly PGUIElementStyle style;
        private readonly List<PGUIElement> children = [];
        private PGUIElement parent;
        private Vector2 position;

        public PGUIElement()
        {
            this.style = new(this);
            this.Id = string.Empty;
        }

        public PGUIElement(string id)
        {
            this.style = new(this);
            this.Id = id;
        }

        internal void SetGUILayout(PGUILayout layout)
        {
            this.GUILayout = layout;
        }

        public void AppendChild(PGUIElement element)
        {
            element.parent?.RemoveChild(element);
            element.parent = this;
            element.position = element.style.GetPosition();

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
            element.parent = this.GUILayout.RootElement;
            this.children.Remove(element);
        }
    }
}
