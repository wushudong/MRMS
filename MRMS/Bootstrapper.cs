using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Configuration;
using MRMS.Dialogs;
using MRMS.ViewModels;
using System.Configuration;
using System.Windows;
using Telerik.Windows.Controls;

namespace MRMS
{
    public sealed class Bootstrapper : UnityBootstrapper
    {
        public static Bootstrapper Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly Bootstrapper instance = new Bootstrapper();
        }

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            LoginDialog loginDialog = ServiceLocator.Current.GetInstance<LoginDialog>();
            ((LoginDialogModel)loginDialog.DataContext).LoginCompleted += (s, ev) =>
            {
                loginDialog.Close();
                ((RadWindow)this.Shell).Show();
            };
            ((LoginDialogModel)loginDialog.DataContext).ConfigCompleted += (s, ev) =>
            {
                ServiceLocator.Current.GetInstance<ConfigDialog>().ShowDialog();
            };
            loginDialog.ShowDialog();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            UnityConfigurationSection configuration = (UnityConfigurationSection) ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            configuration.Configure(this.Container, "defaultContainer");
        }

        //
        // 摘要:
        //     Creates the Microsoft.Practices.Prism.Modularity.IModuleCatalog used by Prism.
        //
        // 备注:
        //     The base implementation returns a new ModuleCatalog.
        /*
        protected override IModuleCatalog CreateModuleCatalog()
        {
            DirectoryModuleCatalog catelog = new DirectoryModuleCatalog();
            //catelog.ModulePath = @".\Modules";
            return catelog;
        }
        */
    }
}
