using MRMS.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;

namespace MRMS.Factories
{
    public class ClassAndGradeEditorFactory : DataFormEditorFactory<ClassAndGradeEditor>
    {
        protected override RadWindow CreateDialog(object dataContext)
        {
            var dialog = new ClassAndGradeEditor();
            dialog.DataContext = dataContext;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.Owner = Application.Current.MainWindow;
            return dialog;
        }
    }
}
