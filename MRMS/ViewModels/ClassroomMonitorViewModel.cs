using MRMS.Helpers;
using MRMS.Interfaces;
using MRMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class ClassroomMonitorViewModel : ViewModelBase, IClose
    {
        private RestConnection restConnection;
        private ClassroomTreeViewModel treeViewModel;
        private System.Windows.Forms.Timer timer;
        /*
        //读取上次已选择的教室信息
        public void ReadSelectClassroomsConfig()
        {
            IEnumerable<int> classroomIds = systemConfig.GetArrayConfig<int>("MonitorClassrooms");
            if (null == classroomIds) return;
            treeViewModel.MarkSelectedClassrooms(classroomIds);
            QueryTerminalInfos(TreeViewModel.SelectedClassrooms);
            sortTerminalInfos();
        }
        //保存已选择的教室信息
        public void WriteSelectClassroomsConfig()
        {
            systemConfig.SetArrayConfig<int>("MonitorClassrooms", TreeViewModel.SelectedClassrooms.Select(p => p.Id).ToList());
            systemConfig.WriteConfig();
        }
        */
        private void sortTerminalInfos()
        {
            /*
            ObservableExtensions.Sort<TerminalInfo>(TerminalInfos, (a, b) => {
                ClassRoom aClassroom = treeViewModel.SelectedClassrooms.Where(p => p.TerminalId.Equals(a.TerminalId)).FirstOrDefault();
                ClassRoom bClassroom = treeViewModel.SelectedClassrooms.Where(p => p.TerminalId.Equals(b.TerminalId)).FirstOrDefault();
                if (null != aClassroom && null != bClassroom)
                {
                    if (aClassroom.ClassroomBuilding.Ord.Equals(bClassroom.ClassroomBuilding.Ord))
                    {
                        return aClassroom.RoomNum.CompareTo(bClassroom.RoomNum);
                    }
                    else
                    {
                        return aClassroom.ClassroomBuilding.Ord.CompareTo(bClassroom.ClassroomBuilding.Ord);
                    }
                }
                else if (null == aClassroom)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            */
            
            ICollection<TerminalInfo> sortTerminalInfos = TerminalInfos.OrderBy(p => treeViewModel.SelectedClassrooms.Where(o => o.TerminalId.Equals(p.TerminalId)).Select(o => o.ClassroomBuilding).FirstOrDefault().Ord)
                .ThenBy(p => treeViewModel.SelectedClassrooms.Where(o => o.TerminalId.Equals(p.TerminalId)).Select(o => o.RoomNum).FirstOrDefault()).ToList();
            for(int i = 0; i < sortTerminalInfos.Count; i++)
            {
                TerminalInfos.Move(TerminalInfos.IndexOf(sortTerminalInfos.ElementAt(i)), i);
            }
            
        }

        public NamedModel SelectedItem { get; set; }
        //获取教室树列表数据模型
        public ClassroomTreeViewModel TreeViewModel
        {
            get
            {
                return treeViewModel;
            }
            set
            {
                if (null != value && treeViewModel != value)
                {
                    treeViewModel = value;
                    treeViewModel.ReadSelectClassroomsConfig("MonitorClassrooms");
                    QueryTerminalInfos(TreeViewModel.SelectedClassrooms);
                    sortTerminalInfos();
                    treeViewModel.SelectedClassroomsChanged += TreeViewModel_SelectedClassroomsChanged;
                }
            }
        }
        private void TreeViewModel_SelectedClassroomsChanged(object sender, Events.SelectedClassroomsChangedEventArgs e)
        {
            QueryTerminalInfos(TreeViewModel.SelectedClassrooms);
            sortTerminalInfos();
            treeViewModel.WriteSelectClassroomsConfig("MonitorClassrooms");
        }
        //更新设备状态信息
        private void QueryTerminalInfos(ICollection<ClassRoom> classrooms)
        {
            ICollection<ClassroomBuilding> classroomBuildings = new List<ClassroomBuilding>();
            foreach (var classroom in classrooms)
            {
                classroomBuildings.Add(classroom.ClassroomBuilding);
                classroom.ClassroomBuilding = null;
            }
            //JsonSerializerSettings settings = new JsonSerializerSettings();  
            //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //string jClassrooms = JsonConvert.SerializeObject(classrooms, settings);
            //JArray j = JArray.Parse(jClassrooms);
            //object cls = j.ToObject<ICollection<ClassRoom>>();
            JObject jo = restConnection.Post("api/TerminalInfo/QueryLastTerminalInfos", classrooms);
            foreach (var classroom in classrooms)
            {
                classroom.ClassroomBuilding = classroomBuildings.ElementAt(classrooms.ToList().IndexOf(classroom));
            }
            if (jo.Value<bool>("success"))
            {
                JArray ja = jo.Value<JArray>("data");
                if (null != ja)
                {
                    Collection<TerminalInfo> terminalInfos = ja.ToObject<Collection<TerminalInfo>>();
                    foreach (var terminalInfo in terminalInfos)
                    {
                        ClassRoom classroom = classrooms.Where(p => p.TerminalId.Equals(terminalInfo.TerminalId)).FirstOrDefault();
                        if (null != classroom)
                        {
                            terminalInfo.Name = classroom.ClassroomBuilding.BuildingName + "_" + classroom.RoomNum;
                        }
                    }
                    //删除未选择的教室
                    ICollection<TerminalInfo> removeItems = TerminalInfos.Where(p => !terminalInfos.Select(o => o.TerminalId).Contains(p.TerminalId)).ToList();
                    foreach (var item in removeItems)
                    {
                        TerminalInfos.Remove(item);
                    }
                    //更新原选择教室的信息
                    PropertyInfo[] properties = typeof(TerminalInfo).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                    foreach (var item in TerminalInfos)
                    {
                        TerminalInfo terminalInfo = terminalInfos.Where(p => p.TerminalId == item.TerminalId).FirstOrDefault();
                        if (null != terminalInfo)
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                property.SetValue(item, property.GetValue(terminalInfo, null), null);
                            }
                        }
                    }
                    //增加新选择的教室
                    ICollection<TerminalInfo> addItems = terminalInfos.Where(p => !TerminalInfos.Select(o => o.TerminalId).Contains(p.TerminalId)).ToList();
                    foreach (var item in addItems)
                    {
                        TerminalInfos.Add(item);
                    }
                }
            }
        }
        //刷新设备信息
        private void refreshTerminalInfos()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                try
                {
                    QueryTerminalInfos(TreeViewModel.SelectedClassrooms);

                }
                finally
                {
                    timer.Start();
                }
            };
            timer.Interval = 3000;
            timer.Start();
        }

        public void Close()
        {
        }

        public ObservableCollection<TerminalInfo> TerminalInfos { get; private set; }
        public ClassroomMonitorViewModel(RestConnection restConnection, ISystemConfig config)
        {
            this.restConnection = restConnection;
            /*
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            JObject jo = this.restConnection.Get("api/ClassRoomInfoMaint/GetAllClassRooms", parameters);

            if (jo.Value<bool>("success"))
            {
                JArray ja = jo.Value<JArray>("data");
                if (null != ja)
                {
                    Collection<ClassRoom> classrooms = ja.ToObject<Collection<ClassRoom>>();
                    jo = this.restConnection.Post("api/TerminalInfo/QueryLastTerminalInfos", classrooms);
                    if (jo.Value<bool>("success"))
                    {
                        ja = jo.Value<JArray>("data");
                        if (null != ja)
                        {
                            Collection<TerminalInfo> terminalInfos = ja.ToObject<Collection<TerminalInfo>>();
                            foreach (var terminalInfo in terminalInfos)
                            {
                                ClassRoom classroom = classrooms.Where(p => p.TerminalId.Equals(terminalInfo.TerminalId)).FirstOrDefault();
                                if (null != classroom)
                                {
                                    terminalInfo.Name = classroom.ClassroomBuilding.BuildingName + "_" + classroom.RoomNum;
                                }
                            }
                            TerminalInfos = new ObservableCollection<TerminalInfo>(terminalInfos);
                        }
                    }
                }

            }
            */
            TerminalInfos = new ObservableCollection<TerminalInfo>();
           
            refreshTerminalInfos();
        }
    }
}
