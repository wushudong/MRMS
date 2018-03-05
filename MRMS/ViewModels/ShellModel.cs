using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using MRMS.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    class ShellModel : ViewModelBase
    {
        private ViewModelBase currentViewModel;
        private IRegionManager regionManager;
        private IModuleManager moduleManager;
        private IUnityContainer container;
        private bool isBusy;

        public ShellModel(IRegionManager regionManager, IModuleManager moduleManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.moduleManager = moduleManager;
            this.container = container;
            this.IsBusy = true;
            ShowViewCommand = new DelegateCommand(ShowViewOp);
            CloseCommand = new DelegateCommand(CloseOp);
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }

            set
            {
                if (this.currentViewModel != value)
                {
                    this.currentViewModel = value;
                    this.OnPropertyChanged(() => this.CurrentViewModel);
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                if (this.isBusy != value)
                {
                    this.isBusy = value;
                    this.OnPropertyChanged(() => this.IsBusy);
                }
            }
        }
        public void ShowViewOp(object o)
        {
            Type type = System.Type.GetType(o.ToString(), false);
            if (null == type) return;
            /*
            if (o.ToString() == "ClassroomMonitorView")
            {
                type = typeof(ClassroomMonitorView);
            }
            else if (o.ToString() == "ClassroomVideoView")
            {
                type = typeof(ClassroomVideoView);
            }
            else if (o.ToString() == "ClassroomMaintView")
            {
                type = typeof(ClassroomMaintView);
            }
            else if (o.ToString() == "ClassroomBuildingView")
            {
                
                type = typeof(ClassroomBuildingView);
            }
            */
            if (null == type) return;
            IRegion mainRegion = this.regionManager.Regions["MainRegion"];
            if (mainRegion.Views.Any(m => m.GetType() == type))
            {
                mainRegion.Activate(mainRegion.Views.Where(m => m.GetType() == type).First());
            }
            else
            {
                object view = this.container.Resolve(type);
                mainRegion.Add(view);
                mainRegion.Activate(view);
            }
            //regionManager.RegisterViewWithRegion("MainRegion", typeof(ClassroomMonitorView));
        }
        public void CloseOp(object o)
        {
            IRegion mainRegion = this.regionManager.Regions["MainRegion"];
            foreach (var view in mainRegion.Views)
            {
                if((view as FrameworkElement).DataContext is IClose)
                {
                    ((view as FrameworkElement).DataContext as IClose).Close();
                }
            }
        }

        public ICommand ShowViewCommand { get; set; }
        public ICommand CloseCommand { get; set; }
    }
}
