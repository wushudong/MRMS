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
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class ClassroomMaintViewModel : ViewModelBase, IClose
    {
        private IEditorFactory editorFactory;
        private RestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public ClassRoom SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<ClassRoom> Items { get; private set; }
        [DoNotNotify]
        public ObservableCollection<ClassroomBuilding> ClassroomBuildings { get; private set; }
        [DoNotNotify]
        public ObservableCollection<dynamic> UsageModes { get; private set; }
        public string RoomNum { get; set; }
        public string BuildingId { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string IdentifyMode { get; set; }
        public void Close()
        {
        }
        public void QueryClassrooms()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"RoomNum\":\"{1}\",\"BuildingId\":\"{2}\", \"TerminalId\":\"{3}\",\"TerminalIp\":\"{4}\",\"IdentifyMode\":\"{5}\"{6}"
                 , "{", RoomNum, BuildingId, TerminalId, TerminalIp, IdentifyMode, "}"));
            int total = 0;
            try
            {
                Collection<ClassRoom> classrooms =
                    restConnection.GetPageValues<ClassRoom>("api/ClassRoomInfoMaint/QueryClassRooms"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != classrooms)
                {
                    Items.Clear();
                    foreach (var classroom in classrooms)
                    {
                        //使classroom中的ClassroomBuilding与ClassroomBuildings集合中的对象为同一对象
                        //解决在下拉列表框中无法选中Id相同对象的问题
                        classroom.ClassroomBuilding = ClassroomBuildings.Where(p => p.Id == classroom.ClassroomBuilding.Id).FirstOrDefault();
                        Items.Add(classroom);
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
        public void GetAllClassroomBuildings()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            int total = 0;
            try
            {
                Collection<ClassroomBuilding> classroomBuildings =
                    restConnection.GetPageValues<ClassroomBuilding>("api/ClassroomBuilding/GetAllClassroomBuildings"
                    ,1, 0, 1000000, parameters, ref total);
                if (null != classroomBuildings)
                {
                    ClassroomBuildings = new ObservableCollection<ClassroomBuilding>(classroomBuildings);
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
                JObject jo = restConnection.Post("api/ClassRoomInfoMaint/CreatClassRoom", data);
                if (null != jo)
                {
                    ClassRoom classroom = jo.Value<JObject>("data").ToObject<ClassRoom>();
                    SelectedItem.Id = classroom.Id;
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
                JObject jo = restConnection.Post("api/ClassRoomInfoMaint/UpdateClassRoom", data);
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
                JObject jo = restConnection.Post("api/ClassRoomInfoMaint/DeleteClassRoom", data);
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
        public void InitUsageModes()
        {
            UsageModes = new ObservableCollection<dynamic>();
            UsageModes.Add(new { Code = "1", Name = "公共刷卡" });
            UsageModes.Add(new { Code = "2", Name = "指定刷卡" });
            UsageModes.Add(new { Code = "3", Name = "管理模式" });
            UsageModes.Add(new { Code = "9", Name = "试运行模式" });
        }
        public void QueryOp(object o)
        {
            QueryClassrooms();
        }
        public void AddOp(object o)
        {
            editorFactory.ShowNewDialog(this, AddRecord);
        }
        public void EditOp(object o)
        {
            if (null != o && o is ClassRoom)
            {
                SelectedItem = o as ClassRoom;
            }
            if (null == SelectedItem) return;
            editorFactory.ShowEditDialog(this, UpdateRecord);
        }
        public void DeleteOp(object o)
        {
            if (null != o && o is ClassRoom)
            {
                SelectedItem = o as ClassRoom;
            }
            if (null == SelectedItem) return;
            if (editorFactory.ShowDeleteDialog(SelectedItem.ClassroomBuilding?.BuildingName + "[" + SelectedItem.RoomNum + "]"))
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

        public ClassroomMaintViewModel(RestConnection restConnection)
        {
            this.restConnection = restConnection;
            this.editorFactory = new DataFormEditorFactory<ClassroomEditor>();
            Items = new ObservableCollection<ClassRoom>();
            QueryCommand = new DelegateCommand(QueryOp);
            AddCommand = new DelegateCommand(AddOp);
            EditCommand = new DelegateCommand(EditOp);
            DeleteCommand = new DelegateCommand(DeleteOp);
            GetAllClassroomBuildings();
            InitUsageModes();
        }
    }
}
