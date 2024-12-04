using StardustSandbox.Core.Interfaces.General;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Tools.Options
{
    public sealed class SOptionSelector : ISReset
    {
        public string DisplayName { get; }
        public IReadOnlyList<object> Values { get; }
        public object SelectedValue => this.Values[(int)this.selectedValueIndex];
        public uint Length => (uint)this.Values.Count;
        public uint SelectedValueIndex => this.selectedValueIndex;

        private uint selectedValueIndex;

        public SOptionSelector(string displayName, uint selectedValueIndex, params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("The values array cannot be null or empty.", nameof(values));
            }

            this.DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            this.Values = values;
            this.selectedValueIndex = selectedValueIndex % (uint)values.Length;
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
            return string.Concat(this.DisplayName, ": ", this.SelectedValue);
        }
    }
}
