using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MRMS.Controls
{
    class DataFormDataField : Telerik.Windows.Controls.DataFormDataField
    {
        public DataFormDataField() : base()
        {
            var a = 1;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PlaceLabel();
        }

        public void PlaceLabel()
        {
            var label = this.GetTemplateChild("PART_Label");
            var containerGrid = this.GetTemplateChild("PART_DataFormDataFieldGrid") as Grid;

            if (label != null)
            {
                if (this.LabelPosition == Telerik.Windows.Controls.Data.DataForm.LabelPosition.Beside)
                {
                    label.SetValue(Grid.RowProperty, 1);
                    label.SetValue(Grid.ColumnProperty, 0);
                }
                else
                {
                    label.SetValue(Grid.RowProperty, 0);
                    label.SetValue(Grid.ColumnProperty, 1);
                }
            }

            if (containerGrid != null && containerGrid.ColumnDefinitions.Count > 0)
            {
                if (this.LabelPosition == Telerik.Windows.Controls.Data.DataForm.LabelPosition.Beside)
                {
                    //containerGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    containerGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
                }
            }
        }
    }
}
