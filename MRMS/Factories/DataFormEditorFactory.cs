using MRMS.Interfaces;
using MRMS.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;

namespace MRMS.Factories
{
    public class DataFormEditorFactory <T> : IEditorFactory where T : RadWindow, new()
    {
        public virtual bool ShowDeleteDialog(string msg)
        {
            bool result = false;

            RadWindow.Confirm(new DialogParameters
            {
                CancelButtonContent = "取消",
                OkButtonContent = "确定",
                Content = string.Format("确定要删除记录 {0}?", msg),
                Closed = (s, e) => result = e.DialogResult ?? false,
                Owner = Application.Current.MainWindow,
                Header = "提示"
            });
            return result;
        }

        public virtual bool ShowEditDialog(object dataContext, CommitEditOp commitEditOp)
        {
            var dialog = CreateDialog(dataContext);
            dialog.Header = "编辑";
            if (dialog is IHasDataForm)
            {
                (dialog as IHasDataForm).DataForm.Loaded += 
                    new RoutedEventHandler((s, e) => { (dialog as IHasDataForm).DataForm.BeginEdit(); });
                (dialog as IHasDataForm).DataForm.CommandProvider = 
                    new CommonDataFormCommandProvider((dialog as IHasDataForm).DataForm, commitEditOp);
            }
            dialog.ShowDialog();
            return dialog.DialogResult ?? false;
        }

        public virtual bool ShowNewDialog(object dataContext, CommitEditOp commitEditOp)
        {
            var dialog = CreateDialog(dataContext);
            dialog.Header = "添加";
            if (dialog is IHasDataForm)
            {
                (dialog as IHasDataForm).DataForm.Loaded += 
                    new RoutedEventHandler((s, e) => { (dialog as IHasDataForm).DataForm.AddNewItem(); });
                (dialog as IHasDataForm).DataForm.CommandProvider = 
                    new CommonDataFormCommandProvider((dialog as IHasDataForm).DataForm, commitEditOp);
            }
            dialog.ShowDialog();
            return dialog.DialogResult ?? false;
        }

        protected virtual RadWindow CreateDialog(object dataContext)
        {
            var dialog = new T();
            dialog.DataContext = dataContext;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.Owner = Application.Current.MainWindow;
            return dialog;
        }
    }
}
