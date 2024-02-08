using System;

namespace PixelDust.Core.Managers.Attributes
{
    /// <summary>
    /// Attribute used by the <see cref="PManagersHandler"/> class as a guide for registering managers during game startup.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PManagerRegisterAttribute : Attribute { }
}
