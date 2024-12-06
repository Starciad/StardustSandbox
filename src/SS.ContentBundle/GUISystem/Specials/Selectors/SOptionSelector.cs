
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.Specials.Selectors
{
    public sealed class SOptionSelector(string displayName, uint selectedValueIndex, string[] values) : ISReset
    {
        public string DisplayName { get; } = displayName;
        public string[] Values { get; } = values;
        public string SelectedValue => this.Values[(int)this.selectedValueIndex];
        public uint Length => (uint)this.Values.Length;
        public uint SelectedValueIndex => this.selectedValueIndex;

        private uint selectedValueIndex = selectedValueIndex % (uint)values.Length;

        public void Select(uint index)
        {
            this.selectedValueIndex = uint.Clamp(index, 0, this.Length - 1);
        }

        public void Next()
        {
            this.selectedValueIndex = (this.selectedValueIndex + 1) % this.Length;
        }

        public void Previous()
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
