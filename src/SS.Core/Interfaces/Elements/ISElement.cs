using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Interfaces.Elements.Contexts;

namespace StardustSandbox.Core.Interfaces.Elements
{
    public interface ISElement
    {
        string Identifier { get; }

        Texture2D Texture { get; }
        Color ReferenceColor { get; }

        bool EnableDefaultBehaviour { get; }
        bool EnableNeighborsAction { get; }
        bool EnableTemperature { get; }
        bool EnableFlammability { get; }
        bool EnableLightEmission { get; }

        bool IsExplosionImmune { get; }

        int DefaultDispersionRate { get; }
        short DefaultTemperature { get; }
        short DefaultFlammabilityResistance { get; }
        short DefaultDensity { get; }
        byte DefaultLuminousIntensity { get; }
        float DefaultExplosionResistance { get; }

        SElementRendering Rendering { get; }
        ISElementContext Context { get; set; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Instantiate();
        void Destroy();

        void Steps();
    }
}
