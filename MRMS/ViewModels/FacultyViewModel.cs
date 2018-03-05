using MRMS.Dialogs;
using MRMS.Factories;
using MRMS.Interfaces;
using MRMS.Models;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    class FacultyViewModel : ViewModelBase, IClose
    {
        private IEditorFactory editorFactory;
        private IRestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public FacultyModel SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<FacultyModel> Items { get; private set; }
        public string FacultyName { get; set; }

        public void Close()
        {
        }
        public void Query()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"FacultyName\":\"{1}\"{2}"
                 , "{", FacultyName, "}"));
            int total = 0;
            try
            {
                Collection<FacultyModel> faculties =
                    restConnection.GetPageValues<FacultyModel>("api/Faculty/QueryIncludeClassesAndGrades"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != faculties)
                {
                    Items.Clear();
                    foreach (var faculty in faculties)
                    {
                        Items.Add(faculty);
                    }
                }
            }
            catch (Exception e)
            {

                RadWindow.Alert(new DialogParameters
                {
                    OkButtonContent = "确定",
                    Content = e.Message,
                    Owner = App.Current.MainWindow,
                    Header = "错误"
                });
            }
        }
        public bool AddRecord(object data)
        {
            bool result = false;
            try
            {
                JObject jo = restConnection.Post("api/Faculty/Create", data);
                if (null != jo)
                {
                    FacultyModel faculty = jo.Value<JObject>("data").ToObject<FacultyModel>();
                    SelectedItem.Id = faculty.Id;
                    result = true;
                }
            }
            catch (Exception e)
            {

                RadWindow.Alert(new DialogParameters
                {
                    OkButtonContent = "确定",
                    Content = e.Message,
                    Owner = App.Current.MainWindow,
                    Header = "错误"
                });
            }
            return result;
        }
        public bool UpdateRecord(object data)
        {
            bool result = false;
            try
            {
                JObject jo = restConnection.Post("api/Faculty/Update", data);
                if (null != jo)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {

                RadWindow.Alert(new DialogParameters
                {
                    OkButtonContent = "确定",
                    Content = e.Message,
                    Owner = App.Current.MainWindow,
                    Header = "错误"
                });
            }
            return result;
        }
        public bool DeleteRecord(object data)
        {
            bool result = false;
            try
            {
                JObject jo = restConnection.Post("api/Faculty/Delete", data);
                if (null != jo)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {

                RadWindow.Alert(new DialogParameters
                {
                    OkButtonContent = "确定",
                    Content = e.Message,
                    Owner = App.Current.MainWindow,
                    Header = "错误"
                });
            }
            return result;
        }
        public void QueryOp(object o)
        {
            Query();
        }
        public void AddOp(object o)
        {
            editorFactory.ShowNewDialog(this, AddRecord);
        }
        public void EditOp(object o)
        {
            if (null != o && o is FacultyModel)
            {
                SelectedItem = o as FacultyModel;
            }
            if (null == SelectedItem) return;
            editorFactory.ShowEditDialog(this, UpdateRecord);
        }
        public void DeleteOp(object o)
        {
            if (null != o && o is FacultyModel)
            {
                SelectedItem = o as FacultyModel;
            }
            if (null == SelectedItem) return;
            if (editorFactory.ShowDeleteDialog(SelectedItem.FacultyName))
            {
                if (DeleteRecord(SelectedItem))
                {
                    Items.Remove(SelectedItem);
                }
            }
        }
        public ICommand QueryCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public FacultyViewModel(IRestConnection restConnection)
        {
            this.restConnection = restConnection;
            this.editorFactory = new DataFormEditorFactory<FacultyEditor>();
            Items = new ObservableCollection<FacultyModel>();
            QueryCommand = new DelegateCommand(QueryOp);
            AddCommand = new DelegateCommand(AddOp);
            EditCommand = new DelegateCommand(EditOp);
            DeleteCommand = new DelegateCommand(DeleteOp);
        }
    }
}
