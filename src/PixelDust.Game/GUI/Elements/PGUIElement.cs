using Microsoft.Xna.Framework;

using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.Objects;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        // Settings
        public string Id { get; set; }

        // Readonly
        public PGUIElementStyle Style => this.style;
        public Vector2 Position => this.style.GetPosition();

        private readonly PGUIElementStyle style;

        #region DELEGATES
        public delegate void ClickEventHandler();
        public delegate void DoubleClickEventHandler();
        public delegate void MouseDownEventHandler();
        public delegate void MouseEnterEventHandler();
        public delegate void MouseLeaveEventHandler();
        public delegate void MouseMoveEventHandler();
        public delegate void MouseOutEventHandler();
        public delegate void MouseOverEventHandler();
        public delegate void MouseUpEventHandler();
        #endregion

        #region EVENTS
        public event ClickEventHandler OnClick;
        public event DoubleClickEventHandler OnDoubleClick;
        public event MouseDownEventHandler OnMouseDown;
        public event MouseEnterEventHandler OnMouseEnter;
        public event MouseLeaveEventHandler OnMouseLeave;
        public event MouseMoveEventHandler OnMouseMove;
        public event MouseOutEventHandler OnMouseOut;
        public event MouseOverEventHandler OnMouseOver;
        public event MouseUpEventHandler OnMouseUp;
        #endregion

        public PGUIElement()
        {
            this.style = new(this);
            this.Id = string.Empty;
        }

        public PGUIElement(string id)
        {
            this.style = new(this);
            this.Id = id;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }
    }
}
