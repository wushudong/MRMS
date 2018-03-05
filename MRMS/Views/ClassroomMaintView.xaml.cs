using Microsoft.Practices.ServiceLocation;
using MRMS.Helpers;
using MRMS.ViewModels;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.Views
{
    /// <summary>
    /// ClassroomMaintView.xaml 的交互逻辑
    /// </summary>
    public partial class ClassroomMaintView : UserControl
    {
        public ClassroomMaintView()
        {
            this.DataContext = ServiceLocator.Current.GetInstance<ClassroomMaintViewModel>();
            InitializeComponent();
            this.grid.KeyboardCommandProvider = new CustomGridKeyboardCommandProvider(this.grid);
        }

        private bool GridIsInEditMode(RadGridView grid)
        {
            var currentCell = grid.CurrentCell;
            if ((currentCell != null) &&
                (currentCell.ParentRow != null && currentCell.ParentRow.Equals(grid.RowInEditMode)))
            {
                if ((currentCell != null) && object.Equals(currentCell.DataContext ?? currentCell.ParentRow.Item, grid.RowInEditMode.DataContext))
                {
                    return grid.RowInEditMode.IsInEditMode;
                }
            }
            return false;
        }
        private bool ShouldPerformSelection(RadGridView grid)
        {
            return
                  grid.SelectionMode != System.Windows.Controls.SelectionMode.Multiple &&
                  grid.CanUserSelect;
        }
        private void ExecSelectionCommandIfNeeded(RadGridView grid)
        {
            if (ShouldPerformSelection(grid))
            {
                return;
            }
            RadGridViewCommands.SelectCurrentUnit.Execute(grid);
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            var a = e;
        }

        private void GridViewDataColumn_KeyDown(object sender, KeyEventArgs e)
        {
            var a = e;
        }

        private void grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!grid.Items.IsEditingItem) return;
            switch (e.Key)
            {
                case Key.Left:
                    {
                        var editBox = grid.CurrentCell.ChildrenOfType<TextBox>().FirstOrDefault();
                        if (null != editBox)
                        {
                            //编辑状态，光标在首位或文本全先才移动焦点
                            if (!(editBox.CaretIndex == 0 || editBox.SelectionLength == editBox.Text.Length))
                            {
                                return;
                            }
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.MovePrevious);
                        if (!ShouldPerformSelection(grid))
                        {
                            grid.PendingCommands.Add(RadGridViewCommands.SelectCurrentUnit);
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.BeginEdit);
                        grid.ExecutePendingCommand();
                        break;
                    }
                case Key.Right:
                    {
                        var editBox = grid.CurrentCell.ChildrenOfType<TextBox>().FirstOrDefault();
                        if (null != editBox)
                        {
                            //编辑状态，光标在末尾或文本全先才移动焦点
                            if (!(editBox.CaretIndex == editBox.Text.Length || editBox.SelectionLength == editBox.Text.Length))
                            {
                                return;
                            }
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.MoveNext);
                        if (!ShouldPerformSelection(grid))
                        {
                            grid.PendingCommands.Add(RadGridViewCommands.SelectCurrentUnit);
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.BeginEdit);
                        grid.ExecutePendingCommand();
                        break;
                    }
                case Key.Up:
                    {
                        grid.PendingCommands.Add(RadGridViewCommands.MoveUp);
                        if (!ShouldPerformSelection(grid))
                        {
                            grid.PendingCommands.Add(RadGridViewCommands.SelectCurrentUnit);
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.BeginEdit);
                        grid.ExecutePendingCommand();
                        break;
                    }
                case Key.Down:
                    {
                        grid.PendingCommands.Add(RadGridViewCommands.MoveDown);
                        if (!ShouldPerformSelection(grid))
                        {
                            grid.PendingCommands.Add(RadGridViewCommands.SelectCurrentUnit);
                        }
                        grid.PendingCommands.Add(RadGridViewCommands.BeginEdit);
                        grid.ExecutePendingCommand();
                        break;
                    }
            }
        }
    }
}
