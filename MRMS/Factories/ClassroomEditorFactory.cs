using MRMS.Dialogs;
using MRMS.Interfaces;
using MRMS.Providers;
using System.Windows;
using Telerik.Windows.Controls;

namespace MRMS.Factories
{
    public class ClassroomEditorFactory : DataFormEditorFactory<ClassroomEditor>
    {
        protected override RadWindow CreateDialog(object dataContext)
        {
            var dialog = new ClassroomEditor();
            dialog.DataContext = dataContext;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.Owner = Application.Current.MainWindow;
            return dialog;
        }
    }
}
