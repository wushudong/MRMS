using Microsoft.Practices.ServiceLocation;
using MRMS.Helpers;
using MRMS.Interfaces;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataForm;

namespace MRMS.Providers
{
    public class CommonDataFormCommandProvider : DataFormCommandProvider
    {
        private RestConnection restConnection = ServiceLocator.Current.GetInstance<RestConnection>();
        private CommitEditOp commitEditOp;
        public CommonDataFormCommandProvider(RadDataForm dataForm, CommitEditOp commitEditOp) :base(dataForm)
        {
            this.commitEditOp = commitEditOp;   
        }
        protected override void CommitEdit()
        {
            if (DataForm != null && DataForm.ValidateItem())
            {
                if (commitEditOp(DataForm.CurrentItem))
                {
                    DataForm.CommitEdit();
                    (DataForm.ParentOfType<RadWindow>()).DialogResult = true;
                    (DataForm.ParentOfType<RadWindow>()).Close();
                }
            }
        }

        protected override void CancelEdit()
        {
            if (DataForm != null)
            {
                DataForm.CancelEdit();
                (DataForm.ParentOfType<RadWindow>()).DialogResult = false;
                (DataForm.ParentOfType<RadWindow>()).Close();
            }
        }
    }
}
