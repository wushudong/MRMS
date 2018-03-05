using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MRMS.Helpers;
using MRMS.ViewModels;

namespace MRMS.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog
    {
        public LoginDialog()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<LoginDialogModel>();
        }
    }
}
