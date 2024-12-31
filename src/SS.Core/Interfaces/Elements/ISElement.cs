using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElement
    {
        string Identifier { get; }
        Texture2D Texture { get; }

        Color ReferenceColor { get; }

        int DefaultDispersionRate { get; }
        short DefaultTemperature { get; }
        short DefaultFlammabilityResistance { get; }
        short DefaultDensity { get; }

        bool EnableDefaultBehaviour { get; }
        bool EnableNeighborsAction { get; }
        bool EnableTemperature { get; }
        bool EnableFlammability { get; }

        SElementRendering Rendering { get; }
        ISElementContext Context { get; set; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void InstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer);
        void Steps();
    }
}
