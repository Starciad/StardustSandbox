using System.Collections.Generic;
using System.Text;

namespace StardustSandbox.UI.Elements
{
    internal static class UIElementTree
    {
        private const string PIPE = "│   ";
        private const string ELBOW = "└── ";
        private const string TEE = "├── ";
        private const string SPACE_PREFIX = "    ";

        internal static string ToString(UIElement element)
        {
            StringBuilder outputString = new();

            BuildString(element, outputString, string.Empty, true);

            return outputString.ToString();
        }

        private static void BuildString(UIElement element, StringBuilder outputString, string indent, bool isLast)
        {
            _ = outputString.Append(indent);

            if (isLast)
            {
                _ = outputString.Append(ELBOW);
                indent += SPACE_PREFIX;
            }
            else
            {
                _ = outputString.Append(TEE);
                indent += PIPE;
            }

            _ = outputString.AppendLine(element.GetType().Name);

            IReadOnlyList<UIElement> children = element.Children;

            for (int i = 0; i < children.Count; i++)
            {
                BuildString(children[i], outputString, indent, i == children.Count - 1);
            }
        }
    }
}
