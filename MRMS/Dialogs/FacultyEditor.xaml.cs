using MRMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace MRMS.Dialogs
{
    /// <summary>
    /// Interaction logic for FacultyEditor.xaml
    /// </summary>
    public partial class FacultyEditor : IHasDataForm
    {
        public FacultyEditor()
        {
            InitializeComponent();
        }

        public RadDataForm DataForm
        {
            get
            {
                return dataForm;
            }
        }

        private void dataForm_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName.Equals("FacultyName"))
            {
                e.Order = 1;
            }
            else if (e.PropertyName.Equals("Ord"))
            {
                e.Order = 2;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
