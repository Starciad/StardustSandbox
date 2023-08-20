using MLEM.Ui;
using MLEM.Ui.Elements;

using PixelDust.Core.Engine;

namespace PixelDust.Core.GUI
{
    public abstract class PGUI
    {
        public Group Root => root;

        private readonly PGUIBuilder builder;
        private readonly Group root;

        public PGUI()
        {
            root = new(Anchor.Center, new(1,1));
            builder = new(root);

            Build();
        }

        internal RootElement Build()
        {
            OnBuild(builder);
            return default;
        }

        protected abstract void OnBuild(IPGUIBuilder builder);
    }
}