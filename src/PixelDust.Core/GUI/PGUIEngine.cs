using MLEM;
using MLEM.Ui;
using MLEM.Ui.Style;
using MLEM.Input;
using MLEM.Misc;
using MLEM.Font;

using PixelDust.Core.Engine;

using Microsoft.Xna.Framework;
using MLEM.Ui.Elements;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PixelDust.Core.GUI
{
    public static class PGUIEngine
    {
        internal static UiSystem UiSystem { get; private set; }
        private static readonly Dictionary<Type, PGUI> _GUIInstances = new();

        internal static void Initialize()
        {
            UntexturedStyle defaultStyle = new(PGraphics.SpriteBatch)
            {
                Font = new GenericSpriteFont(PFonts.Arial),
            };

            UiSystem = new(PEngine.Instance, defaultStyle, new InputHandler(PEngine.Instance))
            {
                AutoScaleReferenceSize = new((int)PScreen.DefaultResolution.X, (int)PScreen.DefaultResolution.Y),
                AutoScaleWithScreen = true,
                GlobalScale = 1.5f,
            };

            foreach (Type guiType in PEngine.Instance.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(PGUI))))
            {
                PGUI GUITargetInstance = (PGUI)Activator.CreateInstance(guiType);

                GUITargetInstance.Build();
                _GUIInstances.Add(guiType, GUITargetInstance);
            }
        }
        internal static void Update()
        {
            //UiSystem.Update(PTime.UpdateGameTime);
        }
        internal static void Draw()
        {
            UiSystem.Draw(PTime.DrawGameTime, PGraphics.SpriteBatch);
        }

        // Utilities
        public static void ActiveGUI<T>() where T : PGUI
        {
            ActiveGUI(typeof(T));
        }
        public static void ActiveGUI(Type guiType)
        {
            UiSystem.Add(guiType.Name, _GUIInstances[guiType].Root);
        }

        public static void DisableGUI<T>() where T : PGUI
        {
            DisableGUI(typeof(T));
        }
        public static void DisableGUI(Type guiType)
        {
            UiSystem.Remove(guiType.Name);
        }
    }
}
