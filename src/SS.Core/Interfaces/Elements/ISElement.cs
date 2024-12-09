using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Elements.Rendering;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElement
    {
        uint Id { get; }
        Texture2D Texture { get; }

        Color ReferenceColor { get; }

        int DefaultDispersionRate { get; }
        short DefaultTemperature { get; }
        short DefaultFlammabilityResistance { get; }

        bool EnableDefaultBehaviour { get; }
        bool EnableNeighborsAction { get; }
        bool EnableTemperature { get; }
        bool EnableFlammability { get; }

        SElementRendering Rendering { get; }
        ISElementContext Context { get; set; }
    }
}
