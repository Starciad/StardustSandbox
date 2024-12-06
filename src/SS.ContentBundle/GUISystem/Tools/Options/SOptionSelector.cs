using StardustSandbox.Core.Interfaces.General;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.Tools.Options
{
    public sealed class SOptionSelector : ISReset
    {
        public string DisplayName { get; }
        public string[] Values { get; }
        public string SelectedValue => this.Values[(int)this.selectedValueIndex];
        public uint Length => (uint)this.Values.Length;
        public uint SelectedValueIndex => this.selectedValueIndex;

        private uint selectedValueIndex;

        public SOptionSelector(string displayName, uint selectedValueIndex, string[] values)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("The values array cannot be null or empty.", nameof(values));
            }

            this.DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            this.Values = values;
            this.selectedValueIndex = selectedValueIndex % (uint)values.Length;
        }

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
