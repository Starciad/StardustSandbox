
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.Selectors
{
    internal sealed class SOptionSelector(string displayName, uint selectedValueIndex, string[] values) : ISReset
    {
        internal string DisplayName { get; } = displayName;
        internal string[] Values { get; } = values;
        internal string SelectedValue => this.Values[(int)this.selectedValueIndex];
        internal uint Length => (uint)this.Values.Length;
        internal uint SelectedValueIndex => this.selectedValueIndex;

        private uint selectedValueIndex = selectedValueIndex % (uint)values.Length;

        internal void Select(uint index)
        {
            this.selectedValueIndex = uint.Clamp(index, 0, this.Length - 1);
        }

        internal void Next()
        {
            this.selectedValueIndex = (this.selectedValueIndex + 1) % this.Length;
        }

        internal void Previous()
        {
            this.selectedValueIndex = this.selectedValueIndex == 0 ? this.Length - 1 : this.selectedValueIndex - 1;
        }

        public void Reset()
        {
            this.selectedValueIndex = 0;
        }

        public override string ToString()
        {
            return string.Concat(this.DisplayName, ": ", this.SelectedValue.ToLower());
        }
    }
}
