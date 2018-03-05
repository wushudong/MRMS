using Microsoft.Practices.ServiceLocation;
using MRMS.ViewModels;
using System.Linq;

using Telerik.Windows.Controls;

namespace MRMS.Views
{
    /// <summary>
    /// Interaction logic for ClassroomMonitorView.xaml
    /// </summary>
    public partial class ClassroomMonitorView
    {
        public ClassroomMonitorView()
        {
            this.DataContext = ServiceLocator.Current.GetInstance<ClassroomMonitorViewModel>();
            InitializeComponent();
            (this.DataContext as ClassroomMonitorViewModel).TreeViewModel = this.ClassroomTree.DataContext as ClassroomTreeViewModel;
        }

        private void RadTileView_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTileViewItem item = e.OriginalSource as RadTileViewItem;
            if (item != null)
            {
                RadFluidContentControl fluid = item.ChildrenOfType<RadFluidContentControl>().FirstOrDefault();
                if (fluid != null)
                {
                    switch (item.TileState)
                    {
                        case TileViewItemState.Maximized:
                            fluid.State = FluidContentControlState.Large;
                            break;
                        case TileViewItemState.Minimized:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                        case TileViewItemState.Restored:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                        default:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                    }
                }
            }
        }
    }
}
