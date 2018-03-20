using Microsoft.Practices.ServiceLocation;
using MRMS.Dialogs;
using MRMS.Interfaces;
using MRMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class CentralizedControlViewModel : ViewModelBase, IClose
    {
        private IRestConnection restConnection;
        [AlsoNotifyFor("CanEditItem")]
        public CentralizedControlModel SelectedItem { get; set; }
        public bool CanEditItem
        {
            get
            {
                return null != SelectedItem;
            }
        }
        [DoNotNotify]
        public ObservableCollection<CentralizedControlModel> Items { get; private set; }
        [DoNotNotify]
        public ObservableCollection<ClassroomBuilding> ClassroomBuildings { get; private set; }
        public string RoomNum { get; set; }
        public string BuildingId { get; set; }
        public int ProgressMinimum { get; set; }
        public int ProgressMaximum { get; set; }
        public int ProgressValue { get; set; }
        public string ProgressMessage { get; set; }
        public void Close()
        {
        }
        public void QueryClassrooms()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sWhere", string.Format(
                 "{0} \"RoomNum\":\"{1}\",\"BuildingId\":\"{2}\", {3}"
                 , "{", RoomNum, BuildingId, "}"));
            int total = 0;
            try
            {
                Collection<CentralizedControlModel> centralizedControls =
                    restConnection.GetPageValues<CentralizedControlModel>("api/TerminalInfo/QueryCentralizedControls"
                    , 1, 0, 1000000, parameters, ref total);
                if (null != centralizedControls)
                {
                    Items.Clear();
                    foreach (var centralizedControl in centralizedControls)
                    {
                        //使centralizedControl中的ClassroomBuilding与ClassroomBuildings集合中的对象为同一对象
                        //解决在下拉列表框中无法选中Id相同对象的问题
                        centralizedControl.ClassroomBuilding = ClassroomBuildings.Where(p => p.Id == centralizedControl.ClassroomBuilding.Id).FirstOrDefault();
                        Items.Add(centralizedControl);
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
                    , 1, 0, 1000000, parameters, ref total);
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
        public bool SetCentralizedControl(CentralizedControlModel data)
        {
            bool result = false;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("terminalId", data.TerminalId);
                parameters.Add("terminalIp", data.TerminalIp);
                Dictionary<string, string> switchs = new Dictionary<string, string>();
                switchs.Add("System", data.CentralizedControlSwitch ? "1" : "0");
                switchs.Add("AirConditioner", data.AirConditionerSwitch ? "1" : "0");
                switchs.Add("Lamp", data.LampSwitch ? "1" : "0");
                parameters.Add("parameters", JsonConvert.SerializeObject(switchs));
                JObject jo = restConnection.Get("api/TerminalOperate/SetParameter", parameters);
                if (null != jo && jo.Value<bool>("success"))
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
            QueryClassrooms();
        }
        public void ExecuteOp(object o)
        {
            if (null != o && o is CentralizedControlModel)
            {
                SelectedItem = o as CentralizedControlModel;
            }
            if (null == SelectedItem) return;
            for (int i = 0; i < 3; i++)
            {
                if (SetCentralizedControl(SelectedItem))
                {
                    SelectedItem.CommondExcuteStat = true;
                    return;
                }
            }
            SelectedItem.CommondExcuteStat = false;
        }
        private CancellationTokenSource cts;
        public void ExecuteAllOp(object o)
        {
            cts = new CancellationTokenSource();
            RadWindow progressDialog = ServiceLocator.Current.GetInstance<ProgressDialog>();
            progressDialog.DataContext = this;
            ProgressMinimum = 0;
            ProgressMaximum = Items.Count;
            ProgressValue = 0;
            ProgressMessage = "";
            Task<int> t = new Task<int>(() => {
                foreach (var item in Items)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (cts.IsCancellationRequested)
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            return 0;                        }
                        if (SetCentralizedControl(item))
                        {
                            item.CommondExcuteStat = true;
                            break;
                        }
                    }

                    ProgressValue++;
                    ProgressMessage = "已执行" + ProgressValue + "命令/总共" + ProgressMaximum +"命令...";
                }
                return 1;
            }, cts.Token);
            t.Start();
            t.ContinueWith(task => {
                try
                {
                    int i = task.Result;
                }
                catch (AggregateException e)
                {
                    e.Handle((err) => err is OperationCanceledException);
                }
                if (!task.IsCanceled && task.IsCompleted && !task.IsFaulted)
                {
                    InvokeOnUIThread(() =>
                    {
                        RadWindow.Alert(new DialogParameters
                        {
                            OkButtonContent = "确定",
                            Content = "命令执行完成！",
                            Owner = App.Current.MainWindow,
                            Header = "提示"
                        });

                        progressDialog.DialogResult = true;
                        progressDialog.Close();
                    });
                }                else if (task.IsCanceled)                {
                    InvokeOnUIThread(() =>
                    {
                        RadWindow.Alert(new DialogParameters
                        {
                            OkButtonContent = "确定",
                            Content = "命令被取消！",
                            Owner = App.Current.MainWindow,
                            Header = "提示"
                        });
                        progressDialog.DialogResult = false;
                        progressDialog.Close();
                    });
                }                else
                {
                    InvokeOnUIThread(() =>
                    {
                        RadWindow.Alert(new DialogParameters
                        {
                            OkButtonContent = "确定",
                            Content = "命令执行失败！",
                            Owner = App.Current.MainWindow,
                            Header = "提示"
                        });
                        progressDialog.DialogResult = false;
                        progressDialog.Close();
                    });
                }

            });
            progressDialog.ShowDialog();
        }
        public void ExecuteCancelOp(object o)
        {
            /*
            RadWindow dialog = o as RadWindow;
            if (null != dialog)
            {
                dialog.DialogResult = false;
                dialog.Close();
            }
            */
            cts.Cancel();
        }
        public ICommand QueryCommand { get; set; }
        public ICommand ExecuteCommand { get; set; }
        public ICommand ExecuteAllCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        

        public CentralizedControlViewModel(IRestConnection restConnection)
        {
            this.restConnection = restConnection;
            Items = new ObservableCollection<CentralizedControlModel>();
            QueryCommand = new DelegateCommand(QueryOp);
            ExecuteCommand = new DelegateCommand(ExecuteOp);
            ExecuteAllCommand = new DelegateCommand(ExecuteAllOp);
            CancelCommand = new DelegateCommand(ExecuteCancelOp);
            GetAllClassroomBuildings();
        }
    }
}
