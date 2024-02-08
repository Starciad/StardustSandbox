using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine.Components;

using System.Collections.Generic;

namespace PixelDust.Core.Engine.Assets
{
    /// <summary>
    /// Static class responsible for managing, storing and configuring game effects.
    /// </summary>
    public static class PEffects
    {
        private static readonly Dictionary<string, Effect> _effects = [];

        /// <summary>
        /// Loads all the effects that will be used in the project.
        /// </summary>
        internal static void Load()
        {
            _effects.Add("Global", PContent.Effects.Load<Effect>("Global"));
        }

        /// <summary>
        /// Update and configure variables and global constant parameters internal to each loaded effect.
        /// </summary>
        internal static void Update()
        {
            // Setting global values
            foreach (Effect effect in _effects.Values)
            {
                effect.Parameters["Time"]?.SetValue((float)PTime.UpdateGameTime.TotalGameTime.TotalSeconds);
            }
        }
    }
}
