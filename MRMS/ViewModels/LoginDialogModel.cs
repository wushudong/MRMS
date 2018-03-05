using Microsoft.Practices.Unity;
using MRMS.Helpers;
using MRMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class LoginDialogModel : ViewModelBase
    {
        private IUnityContainer container;
        public event EventHandler LoginCompleted;
        public event EventHandler ConfigCompleted;
        [Dependency]
        public ISystemConfig Config { get; set; }

        public void RaiseLoginCompleted()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }
        public void RaiseConfigCompleted()
        {
            ConfigCompleted?.Invoke(this, EventArgs.Empty);
        }

        private string username;
        private string password;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }

        public string Password
        {
            private get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        public void LoginOp(object o)
        {
            string msg;
            IRestConnection restCon = container.Resolve<IRestConnection>(new ParameterOverride("baseUrl", Config.GetConfig<string>("ServiceAddress")));
            //new RestConnection(Config.GetConfig<string>("ServiceAddress"));
            string password = (o as RadPasswordBox)?.Password;
            if (restCon.Login(@"/Account/Login", "LoginName", username, "Password", password, out msg))
            {
                RaiseLoginCompleted();
            }
            else
            {
                RadWindow.Alert(new DialogParameters
                {
                    OkButtonContent = "确定",
                    Content = msg,
                    Owner = App.Current.MainWindow,
                    Header = "错误"
                });
            }
        }
        public void ConfigOp(object o)
        {
            RaiseConfigCompleted();
        }

        public ICommand LoginCommand { get; set; }
        public ICommand ConfigCommand { get; set; }

        public LoginDialogModel(IUnityContainer container)
        {
            this.container = container;
            LoginCommand = new DelegateCommand(LoginOp);
            OnPropertyChanged("LoginOp");
            ConfigCommand = new DelegateCommand(ConfigOp);
            OnPropertyChanged("ConfigOp");
        }
    }
}
