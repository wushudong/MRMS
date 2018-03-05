using MRMS.Events;
using MRMS.Helpers;
using MRMS.Interfaces;
using MRMS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class ClassroomTreeViewModel : ViewModelBase
    {
        private ISystemConfig systemConfig;
        private RestConnection restConnection;

        public event SelectedClassroomsChangedHandle SelectedClassroomsChanged;

        public NamedModel SelectedItem { get; set; }
        public ObservableCollection<ClassroomBuilding> ClassroomBuildings { get; private set; }
        //标记选择的教室
        public void MarkSelectedClassrooms(IEnumerable<int> classroomIds)
        {
            ICollection<ClassRoom> classrooms = ClassroomBuildings.SelectMany(p => p.ClassRooms).ToList();
            foreach (var classroom in classrooms)
            {
                if (classroomIds.Contains(classroom.Id))
                {
                    classroom.IsChecked = true;
                }
                else
                {
                    classroom.IsChecked = false;
                }
            }
        }
        public ICollection<ClassRoom> SelectedClassrooms
        {
            get
            {
                return ClassroomBuildings.SelectMany(p => p.ClassRooms).Where(p => p.IsChecked == true).OrderBy(p => p.ClassroomBuilding.Ord).ThenBy(p => p.RoomNum).ToList<ClassRoom>();
            }
        }
        public void RaiseSelectedClassroomsChanged(ICollection<ClassRoom> beforeChange, ICollection<ClassRoom> afterChange)
        {
            SelectedClassroomsChanged?.Invoke(this, new SelectedClassroomsChangedEventArgs(beforeChange, afterChange));
        }

        public void ClassroomCheckedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                ClassRoom classroom = sender as ClassRoom;
                if (classroom.IsChecked.HasValue)
                {
                    ICollection<ClassRoom> beforeChange = SelectedClassrooms;
                    classroom.ClassroomBuilding.PropertyChanged -= new PropertyChangedEventHandler(ClassroomBuildingCheckedChange);
                    //没有一个选中
                    if (classroom.ClassroomBuilding.ClassRooms.Count(c => c.IsChecked == true) == 0)
                    {
                        beforeChange.Add(sender as ClassRoom);
                        classroom.ClassroomBuilding.IsChecked = false;
                    }
                    //全部选中
                    else if (classroom.ClassroomBuilding.ClassRooms.Count(c => c.IsChecked != true) == 0)
                    {
                        beforeChange.Remove(sender as ClassRoom);
                        classroom.ClassroomBuilding.IsChecked = true;
                    }
                    else
                    {
                        classroom.ClassroomBuilding.IsChecked = null;
                    }
                    ICollection<ClassRoom> afterChange = SelectedClassrooms;
                    RaiseSelectedClassroomsChanged(beforeChange, afterChange);
                    classroom.ClassroomBuilding.PropertyChanged += new PropertyChangedEventHandler(ClassroomBuildingCheckedChange);
                }
            }
        }
        public void ClassroomBuildingCheckedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                ClassroomBuilding classroomBuilding = sender as ClassroomBuilding;
                if (classroomBuilding.IsChecked.HasValue)
                {
                    ICollection<ClassRoom> beforeChange = SelectedClassrooms;
                    foreach (var classroom in classroomBuilding.ClassRooms)
                    {
                        classroom.PropertyChanged -= new PropertyChangedEventHandler(ClassroomCheckedChange);
                        classroom.IsChecked = classroomBuilding.IsChecked.Value;
                        classroom.PropertyChanged += new PropertyChangedEventHandler(ClassroomCheckedChange);
                    }
                    ICollection<ClassRoom> afterChange = SelectedClassrooms;
                    RaiseSelectedClassroomsChanged(beforeChange, afterChange);
                }
            }
        }
        //读取上次已选择的教室信息
        public void ReadSelectClassroomsConfig(string key)
        {
            IEnumerable<int> classroomIds = systemConfig.GetArrayConfig<int>(key);
            if (null == classroomIds) return;
            MarkSelectedClassrooms(classroomIds);
        }
        //保存已选择的教室信息
        public void WriteSelectClassroomsConfig(string key)
        {
            systemConfig.SetArrayConfig<int>(key, SelectedClassrooms.Select(p => p.Id).ToList());
            systemConfig.WriteConfig();
        }
        public ClassroomTreeViewModel(RestConnection restConnection, ISystemConfig config)
        {
            this.restConnection = restConnection;
            systemConfig = config;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("_dc", "1502368547501");
            parameters.Add("sWhere", "{}");
            parameters.Add("page", "1");
            parameters.Add("start", "0");
            parameters.Add("limit", "10");
            JObject jo = this.restConnection.Get("api/ClassroomBuilding/QueryClassroomBuildingsIncludeClassRooms", parameters);
            
            if (jo.Value<bool>("success"))
            {
                JArray ja = jo.Value<JArray>("data");
                if (null != ja)
                {
                    ClassroomBuildings = new ObservableCollection<ClassroomBuilding>(ja.ToObject<Collection<ClassroomBuilding>>());
                    foreach (var classroomBuilding in ClassroomBuildings)
                    {
                        //初始化全部教学楼选中
                        classroomBuilding.IsChecked = true;
                        classroomBuilding.PropertyChanged += new PropertyChangedEventHandler(ClassroomBuildingCheckedChange);
                        foreach (var classroom in classroomBuilding.ClassRooms)
                        {
                            //初始化全部教室选中
                            classroom.IsChecked = true;
                            classroom.Name = classroomBuilding.BuildingName + "_" + classroom.RoomNum;
                            classroom.ClassroomBuilding = classroomBuilding;
                            classroom.PropertyChanged += new PropertyChangedEventHandler(ClassroomCheckedChange);
                        }
                    }
                }
                
            }
        }
    }
}
