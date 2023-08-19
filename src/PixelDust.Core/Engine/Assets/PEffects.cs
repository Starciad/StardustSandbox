using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PixelDust.Core
{
    public static class PEffects
    {
        public static IReadOnlyDictionary<string, Effect> Effects => _effects;
        private static readonly Dictionary<string, Effect> _effects = new();

        internal static void Load()
        {
            _effects.Add("Global", PContent.Load<Effect>("Effects/Global"));
        }

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
