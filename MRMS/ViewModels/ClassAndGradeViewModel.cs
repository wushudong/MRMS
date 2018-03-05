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
    public class ClassAndGradeViewModel : ViewModelBase, IClose
    {
        private IEditorFactory editorFactory;
        private IRestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public ClassAndGradeModel SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<ClassAndGradeModel> Items { get; private set; }
        [DoNotNotify]
        public ObservableCollection<FacultyModel> Faculties { get; private set; }
        public string ClassAndGradeName { get; set; }
        public string MajorName { get; set; }
        public string EntranceDate { get; set; }
        public string GraduationDate { get; set; }
        public string FacultyId { get; set; }
        public void Close()
        {
        }
        public void Query()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"ClassAndGradeName\":\"{1}\",\"MajorName\":\"{2}\", \"EntranceDate\":\"{3}\",\"GraduationDate\":\"{4}\",\"FacultyId\":\"{5}\"{6}"
                 , "{", ClassAndGradeName, MajorName, EntranceDate, GraduationDate, FacultyId, "}"));
            int total = 0;
            try
            {
                Collection<ClassAndGradeModel> classesAndGrades =
                    restConnection.GetPageValues<ClassAndGradeModel>("api/ClassAndGrade/Query"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != classesAndGrades)
                {
                    Items.Clear();
                    foreach (var classAndGrade in classesAndGrades)
                    {
                        //使classAndGrade中的Faculty与Faculties集合中的对象为同一对象
                        //解决在下拉列表框中无法选中Id相同对象的问题
                        classAndGrade.Faculty = Faculties.Where(p => p.Id == classAndGrade.Faculty.Id).FirstOrDefault();
                        Items.Add(classAndGrade);
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
        public void GetAllFaculties()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            int total = 0;
            try
            {
                Collection<FacultyModel> faculties =
                    restConnection.GetPageValues<FacultyModel>("api/Faculty/GetAll"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != faculties)
                {
                    Faculties = new ObservableCollection<FacultyModel>(faculties);
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
                JObject jo = restConnection.Post("api/ClassAndGrade/Create", data);
                if (null != jo)
                {
                    ClassAndGradeModel classAndGradeModel = jo.Value<JObject>("data").ToObject<ClassAndGradeModel>();
                    SelectedItem.Id = classAndGradeModel.Id;
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
                JObject jo = restConnection.Post("api/ClassAndGrade/Update", data);
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
                JObject jo = restConnection.Post("api/ClassAndGrade/Delete", data);
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
            if (null != o && o is ClassAndGradeModel)
            {
                SelectedItem = o as ClassAndGradeModel;
            }
            if (null == SelectedItem) return;
            editorFactory.ShowEditDialog(this, UpdateRecord);
        }
        public void DeleteOp(object o)
        {
            if (null != o && o is ClassAndGradeModel)
            {
                SelectedItem = o as ClassAndGradeModel;
            }
            if (null == SelectedItem) return;
            if (editorFactory.ShowDeleteDialog(SelectedItem.Faculty?.FacultyName + "[" + SelectedItem.ClassAndGradeName + "]"))
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

        public ClassAndGradeViewModel(IRestConnection restConnection)
        {
            this.restConnection = restConnection;
            this.editorFactory = new DataFormEditorFactory<ClassAndGradeEditor>();
            Items = new ObservableCollection<ClassAndGradeModel>();
            QueryCommand = new DelegateCommand(QueryOp);
            AddCommand = new DelegateCommand(AddOp);
            EditCommand = new DelegateCommand(EditOp);
            DeleteCommand = new DelegateCommand(DeleteOp);
            GetAllFaculties();
        }
    }
}
