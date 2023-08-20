using MLEM.Ui.Elements;

using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Core.GUI
{
    internal sealed class PGUIBuilder : IPGUIBuilder
    {
        private readonly Group GUIRoot;

        private readonly List<(PGUIBuildElement, Element)> openedElements = new();
        private (PGUIBuildElement, Element) currentElementOpened;

        public PGUIBuilder(Group root)
        {
            GUIRoot = root;
            openedElements.Add((new(), GUIRoot));

            currentElementOpened = openedElements[0];
        }

        public PGUIBuildElement Create(Element UIElement)
        {
            while (currentElementOpened.Item1.IsClosed)
            {
                openedElements.Remove(openedElements.Last());
                currentElementOpened = openedElements.Last();
            }

            PGUIBuildElement buildElement = new();

            var value = (buildElement, currentElementOpened.Item2.AddChild(UIElement));
            openedElements.Add(value);
            currentElementOpened = value;
            
            return buildElement;
        }

        public void CreateClosed(Element UIElement)
        {
            Create(UIElement).Dispose();
        }
    }
}
