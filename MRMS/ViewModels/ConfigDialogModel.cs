using Microsoft.Practices.Unity;
using MRMS.Helpers;
using MRMS.Interfaces;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class ConfigDialogModel : ViewModelBase
    {

        private ISystemConfig systemConfig;
        public event EventHandler OkCompleted;
        public event EventHandler CancelCompleted;
        public RadWindow currentView;

        public void RaiseOkCompleted()
        {
            if (OkCompleted != null)
            {
                OkCompleted(this, EventArgs.Empty);
            }
        }
        public void RaiseCancelCompleted()
        {
            if (CancelCompleted != null)
            {
                CancelCompleted(this, EventArgs.Empty);
            }
        }

        private string serviceAdr;

        public string ServiceAdr
        {
            get { return serviceAdr; }
            set { serviceAdr = value; OnPropertyChanged("ServiceAdr"); }
        }

        public void OkOp(object o)
        {
            systemConfig.SetConfig<string>("ServiceAddress", serviceAdr);
            systemConfig.WriteConfig();
            RaiseOkCompleted();
            currentView.DialogResult = true;
            currentView.Close();
        }
        public void CancelOp(object o)
        {
            RaiseCancelCompleted();
            currentView.DialogResult = false;
            currentView.Close();
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ConfigDialogModel(RadWindow view, ISystemConfig config)
        {
            currentView = view;
            systemConfig = config; 
            ServiceAdr = systemConfig.GetConfig<string>("ServiceAddress");
            OkCommand = new DelegateCommand(OkOp);
            OnPropertyChanged("OkCommand");
            CancelCommand = new DelegateCommand(CancelOp);
            OnPropertyChanged("CancelCommand");
        }
    }
}
