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
    class IcCardViewModel : ViewModelBase, IClose
    {
        private IEditorFactory editorFactory;
        private IRestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public IcCardModel SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<IcCardModel> Items { get; private set; }
        [DoNotNotify]
        public ObservableCollection<FacultyModel> Faculties { get; private set; }
        [DoNotNotify]
        public ObservableCollection<dynamic> IcCardTypes { get; private set; }
        [DoNotNotify]
        public ObservableCollection<dynamic> SexTypes { get; private set; }

        public string FacultyId { set; get; }
        public string HexCode { get; set; }
        public string CardNum { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Sex { get; set; }
        public string Career { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CardType { get; set; }

        public void Close()
        {
        }
        public void Query()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"FacultyId\":\"{1}\",\"HexCode\":\"{2}\", \"CardNum\":\"{3}\",\"PersonId\":\"{4}\",\"Sex\":\"{5}\",\"Career\":\"{6}\",\"Email\":\"{7}\",\"Phone\":\"{8}\",\"CardType\":\"{9}\",\"Name\":\"{10}\"{11}"
                 , "{", FacultyId, HexCode, CardNum, PersonId, Sex, Career, Email, Phone, CardType, PersonName, "}"));
            int total = 0;
            try
            {
                Collection<IcCardModel> icCards =
                    restConnection.GetPageValues<IcCardModel>("api/IcCardInfoMaint/QueryIcCards"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != icCards)
                {
                    Items.Clear();
                    foreach (var icCard in icCards)
                    {
                        //使icCard中的FacultyModel与Faculties集合中的对象为同一对象
                        //解决在下拉列表框中无法选中Id相同对象的问题
                        if (null != icCard.Faculty)
                        {
                            icCard.Faculty = Faculties.Where(p => p.Id == icCard.Faculty.Id).FirstOrDefault();
                        }
                        Items.Add(icCard);
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
                JObject jo = restConnection.Post("api/IcCardInfoMaint/CreatIcCard", data);
                if (null != jo)
                {
                    IcCardModel icCard = jo.Value<JObject>("data").ToObject<IcCardModel>();
                    SelectedItem.Id = icCard.Id;
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
                JObject jo = restConnection.Post("api/IcCardInfoMaint/UpdateIcCard", data);
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
                JObject jo = restConnection.Post("api/IcCardInfoMaint/DeleteIcCard", data);
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
        public void InitIcCardTypes()
        {
            IcCardTypes = new ObservableCollection<dynamic>();
            IcCardTypes.Add(new { Code = "A", Name = "管理员卡" });
            IcCardTypes.Add(new { Code = "T", Name = "教师卡" });
            IcCardTypes.Add(new { Code = "S", Name = "学生卡" });
            IcCardTypes.Add(new { Code = "L", Name = "临时卡" });
        }
        public void InitSexTypes()
        {
            SexTypes = new ObservableCollection<dynamic>();
            SexTypes.Add(new { Code = "男", Name = "男" });
            SexTypes.Add(new { Code = "女", Name = "女" });
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
            if (null != o && o is IcCardModel)
            {
                SelectedItem = o as IcCardModel;
            }
            if (null == SelectedItem) return;
            editorFactory.ShowEditDialog(this, UpdateRecord);
        }
        public void DeleteOp(object o)
        {
            if (null != o && o is IcCardModel)
            {
                SelectedItem = o as IcCardModel;
            }
            if (null == SelectedItem) return;
            if (editorFactory.ShowDeleteDialog(SelectedItem.PersonName + "[" + SelectedItem.PersonId + "]"))
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

        public IcCardViewModel(IRestConnection restConnection)
        {
            this.restConnection = restConnection;
            this.editorFactory = new DataFormEditorFactory<IcCardEditor>();
            Items = new ObservableCollection<IcCardModel>();
            QueryCommand = new DelegateCommand(QueryOp);
            AddCommand = new DelegateCommand(AddOp);
            EditCommand = new DelegateCommand(EditOp);
            DeleteCommand = new DelegateCommand(DeleteOp);
            GetAllFaculties();
            InitIcCardTypes();
            InitSexTypes();
        }
    }
}
