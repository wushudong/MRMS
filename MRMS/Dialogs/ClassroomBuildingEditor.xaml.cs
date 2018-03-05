
using MRMS.Interfaces;
using Telerik.Windows.Controls;

namespace MRMS.Dialogs
{
    /// <summary>
    /// Interaction logic for ClassroomBuildingEditor.xaml
    /// </summary>
    public partial class ClassroomBuildingEditor : IHasDataForm
    {
        public ClassroomBuildingEditor()
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
            if (e.PropertyName.Equals("BuildingName"))
            {
                e.Order = 1;
            }
            else if (e.PropertyName.Equals("Location"))
            {
                e.Order = 2;
            }
            else if (e.PropertyName.Equals("Ord"))
            {
                e.Order = 3;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
