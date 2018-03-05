using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace MRMS.Controls
{
    public class DataFormClearableComboBoxField : DataFormComboBoxField
    {
        public DataFormClearableComboBoxField() : base() { }
        protected override Control GetControl()
        {
            var cBox = (RadComboBox)base.GetControl();
            cBox.ClearSelectionButtonVisibility = System.Windows.Visibility.Visible;
            cBox.ClearSelectionButtonContent = "清除";
            return cBox;
        }
    }
}
