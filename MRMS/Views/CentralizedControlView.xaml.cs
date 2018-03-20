using Microsoft.Practices.ServiceLocation;
using MRMS.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MRMS.Views
{
    /// <summary>
    /// CentralizedControlView.xaml 的交互逻辑
    /// </summary>
    public partial class CentralizedControlView : UserControl
    {
        public CentralizedControlView()
        {
            this.DataContext = ServiceLocator.Current.GetInstance<CentralizedControlViewModel>();
            InitializeComponent();
        }
    }
}
