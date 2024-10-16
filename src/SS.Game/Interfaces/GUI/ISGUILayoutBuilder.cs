﻿using StardustSandbox.Game.GameContent.GUI.Elements;
using StardustSandbox.Game.GUI.Elements;

namespace StardustSandbox.Game.Interfaces.GUI
{
    public interface ISGUILayoutBuilder
    {
        SGUIRootElement RootElement { get; }

        T CreateElement<T>() where T : SGUIElement;
    }
}
