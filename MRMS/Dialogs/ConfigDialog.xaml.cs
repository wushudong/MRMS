using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MRMS.Helpers;
using MRMS.Interfaces;
using MRMS.ViewModels;

namespace MRMS.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfigDialog.xaml
    /// </summary>
    public partial class ConfigDialog
    {
        public ConfigDialog(IUnityContainer container)
        {
            InitializeComponent();
            //IUnityContainer container  = ServiceLocator.Current.GetInstance<IUnityContainer>();
            DataContext = container.Resolve<ConfigDialogModel>(new ParameterOverrides
                {
                  {"view", this},
                  {"systemConfig", Bootstrapper.Instance.Container.Resolve<ISystemConfig>()}
                }/*new ParameterOverride("view", this)*/);
        }
    }
}
