using Meta.Vlc.Wpf;
using MRMS.Interfaces;
using MRMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace MRMS.ViewModels
{
    public class ClassroomVideoViewModel : ViewModelBase, IClose
    {
        private ISystemConfig systemConfig;
        private ClassroomTreeViewModel treeViewModel;
        private int _ColumnWidth = 400;
        private int _RowHeight = 300;
        private int _MinimizedColumnWidth = 200;
        private int _MinimizedRowHeight = 150;
        public int ColumnWidth
        {
            get
            {
                return _ColumnWidth;
            }
            set
            {
                if (!Equals(this._ColumnWidth, value))
                {
                    this._ColumnWidth = value;
                    this.OnPropertyChanged("ColumnWidth");
                }
            }
        }
        public int RowHeight
        {
            get
            {
                return _RowHeight;
            }
            set
            {
                if (!Equals(this._RowHeight, value))
                {
                    this._RowHeight = value;
                    this.OnPropertyChanged("RowHeight");
                }
            }
        }
        public int MinimizedColumnWidth
        {
            get
            {
                return _MinimizedColumnWidth;
            }
            set
            {
                if (!Equals(this._MinimizedColumnWidth, value))
                {
                    this._MinimizedColumnWidth = value;
                    this.OnPropertyChanged("MinimizedColumnWidth");
                }
            }
        }
        public int MinimizedRowHeight
        {
            get
            {
                return _MinimizedRowHeight;
            }
            set
            {
                if (!Equals(this._MinimizedRowHeight, value))
                {
                    this._MinimizedRowHeight = value;
                    this.OnPropertyChanged("MinimizedRowHeight");
                }
            }
        }
        private void readVideoSize()
        {
            dynamic videoSize = new { ColumnWidth = _ColumnWidth, RowHeight = _RowHeight };
            videoSize = systemConfig.GetConfig<dynamic>("VideoSize");
            if (null != videoSize && videoSize.ColumnWidth >= 200 && videoSize.RowHeight >= 150)
            {
                ColumnWidth = videoSize.ColumnWidth;
                RowHeight = videoSize.RowHeight;
            }
        }
        private void writeVedioSize()
        {
            systemConfig.SetConfig<dynamic>("VideoSize", new { ColumnWidth = _ColumnWidth, RowHeight = _RowHeight });
            systemConfig.WriteConfig();
        }
        private void sortClassrooms()
        {

            ICollection<ClassRoom> sortClassrooms = Classrooms.OrderBy(p => p.ClassroomBuilding.Ord).ThenBy(p => p.RoomNum).ToList();
            for (int i = 0; i < sortClassrooms.Count; i++)
            {
                Classrooms.Move(Classrooms.IndexOf(sortClassrooms.ElementAt(i)), i);
            }

        }
        public ClassroomTreeViewModel TreeViewModel
        {
            get
            {
                return this.treeViewModel;
            }
            set
            {
                if (null != value && this.treeViewModel != value)
                {
                    this.treeViewModel = value;
                    treeViewModel.ReadSelectClassroomsConfig("VideoClassrooms");
                    setVideoClassrooms();
                    sortClassrooms();
                    treeViewModel.SelectedClassroomsChanged += TreeViewModel_SelectedClassroomsChanged;
                }
            }
        }
        //设置已选择的教室信息
        private void setVideoClassrooms()
        {
            foreach (var classroom in TreeViewModel.SelectedClassrooms)
            {
                if (Classrooms.IndexOf(classroom) < 0)
                {
                    Classrooms.Add(classroom);
                }
            }
            ICollection<ClassRoom> removes = Classrooms.Where(p => !TreeViewModel.SelectedClassrooms.Contains(p)).ToList();
            foreach (var item in removes)
            {
                if (null != item.VlcPlayer)
                {
                    item.VlcPlayer.Dispose();
                    item.VlcPlayer = null;
                }
                Classrooms.Remove(item);
            }
        }
        private void TreeViewModel_SelectedClassroomsChanged(object sender, Events.SelectedClassroomsChangedEventArgs e)
        {
            setVideoClassrooms();
            sortClassrooms();
            treeViewModel.WriteSelectClassroomsConfig("VideoClassrooms");
        }
        public ObservableCollection<ClassRoom> Classrooms { get; private set; }
        public ICommand PlayCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        private void OnPlayCommandExecuted(object obj)
        {
            var vlcPlayer = obj as VlcPlayer;
            if (vlcPlayer != null)
            {
                vlcPlayer.Play();
            }
        }
        private void OnPauseCommandExecuted(object obj)
        {
            var vlcPlayer = obj as VlcPlayer;
            if (vlcPlayer != null)
            {
                vlcPlayer.PauseOrResume();
            }
        }

        public void Close()
        {
            writeVedioSize();
            foreach(var classroom in Classrooms)
            {
                if (classroom.VlcPlayer != null)
                {
                    classroom.VlcPlayer.Dispose();
                }
            }
        }

        public ClassroomVideoViewModel(ISystemConfig config)
        {
            systemConfig = config;
            readVideoSize();
            Classrooms = new ObservableCollection<ClassRoom>();
            PlayCommand = new DelegateCommand(OnPlayCommandExecuted);
            PauseCommand = new DelegateCommand(OnPauseCommandExecuted);
        }
    }
}
