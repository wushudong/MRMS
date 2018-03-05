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
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace MRMS
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : RadWindow
    {
        public Shell()
        {
            this.DataContext = ServiceLocator.Current.GetInstance<ShellModel>();
            InitializeComponent();
        }
    }
}
