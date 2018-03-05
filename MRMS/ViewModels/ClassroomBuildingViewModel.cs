using MRMS.Dialogs;
using MRMS.Factories;
using MRMS.Helpers;
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
    public class ClassroomBuildingViewModel : ViewModelBase, IClose
    {
        private IEditorFactory editorFactory;
        private RestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public ClassroomBuilding SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<ClassroomBuilding> Items { get; private set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }

        public void Close()
        {
        }
        public void QueryClassroomBuildings()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"BuildingName\":\"{1}\",\"Location\":\"{2}\"{3}"
                 , "{", BuildingName, Location, "}"));
            int total = 0;
            try
            {
                Collection<ClassroomBuilding> classroomBuildings =
                    restConnection.GetPageValues<ClassroomBuilding>("api/ClassroomBuilding/QueryClassroomBuildingsIncludeClassRooms"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != classroomBuildings)
                {
                    Items.Clear();
                    foreach (var classroomBuilding in classroomBuildings)
                    {
                        Items.Add(classroomBuilding);
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
                JObject jo = restConnection.Post("api/ClassroomBuilding/CreatClassroomBuilding", data);
                if (null != jo)
                {
                    ClassroomBuilding classroomBuilding = jo.Value<JObject>("data").ToObject<ClassroomBuilding>();
                    SelectedItem.Id = classroomBuilding.Id;
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
                JObject jo = restConnection.Post("api/ClassroomBuilding/UpdateClassroomBuilding", data);
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
                JObject jo = restConnection.Post("api/ClassroomBuilding/DeleteClassroomBuilding", data);
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
            QueryClassroomBuildings();
        }
        public void AddOp(object o)
        {
            editorFactory.ShowNewDialog(this, AddRecord);
        }
        public void EditOp(object o)
        {
            if (null != o && o is ClassroomBuilding)
            {
                SelectedItem = o as ClassroomBuilding;
            }
            if (null == SelectedItem) return;
            editorFactory.ShowEditDialog(this, UpdateRecord);
        }
        public void DeleteOp(object o)
        {
            if (null != o && o is ClassroomBuilding)
            {
                SelectedItem = o as ClassroomBuilding;
            }
            if (null == SelectedItem) return;
            if (editorFactory.ShowDeleteDialog(SelectedItem.BuildingName))
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

        public ClassroomBuildingViewModel(RestConnection restConnection)
        {
            this.restConnection = restConnection;
            this.editorFactory = new DataFormEditorFactory<ClassroomBuildingEditor>();
            Items = new ObservableCollection<ClassroomBuilding>();
            QueryCommand = new DelegateCommand(QueryOp);
            AddCommand = new DelegateCommand(AddOp);
            EditCommand = new DelegateCommand(EditOp);
            DeleteCommand = new DelegateCommand(DeleteOp);
        }
    }
}
