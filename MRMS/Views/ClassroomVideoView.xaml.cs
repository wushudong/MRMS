using Meta.Vlc.Wpf;
using Microsoft.Practices.ServiceLocation;
using MRMS.Models;
using MRMS.ViewModels;
using System;
using System.Windows.Controls;

namespace MRMS.Views
{
    /// <summary>
    /// ClassroomVideoView.xaml 的交互逻辑
    /// </summary>
    public partial class ClassroomVideoView : UserControl
    {
        public ClassroomVideoView()
        {
            this.DataContext = ServiceLocator.Current.GetInstance<ClassroomVideoViewModel>();
            InitializeComponent();
            (this.DataContext as ClassroomVideoViewModel).TreeViewModel = this.ClassroomTree.DataContext as ClassroomTreeViewModel;
        }

        private void vlcPlayer_StateChanged(object sender, Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            VlcPlayer vlcPlayer = sender as VlcPlayer;
            if (null == vlcPlayer || null == vlcPlayer.DataContext as ClassRoom) return;
            (vlcPlayer.DataContext as ClassRoom).VedioState = e.Value;
        }

        private void vlcPlayer_Initialized(object sender, EventArgs e)
        {
            VlcPlayer vlcPlayer = sender as VlcPlayer;
            if (null == vlcPlayer || null == vlcPlayer.DataContext as ClassRoom) return;
            (vlcPlayer.DataContext as ClassRoom).VlcPlayer = vlcPlayer;
            /*
            System.Timers.Timer timer = new System.Timers.Timer(50) { AutoReset = false };
            timer.Elapsed += delegate {
                timer.Dispose();
                vlcPlayer.LoadMedia((vlcPlayer.DataContext as ClassRoom).VedioAddress);
                vlcPlayer.Play();
                vlcPlayer.IsMute = true;
            };
            */
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Start();
            timer.Interval = 1;
            timer.Tick += (s, f) =>
            {
                timer.Stop();
                vlcPlayer.LoadMedia((vlcPlayer.DataContext as ClassRoom).VedioAddress);
                vlcPlayer.Play();
            };
            timer.Start();
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 300;
            timer1.Tick += (s, f) =>
            {
                timer1.Stop();
                vlcPlayer.IsMute = true;
            };
            timer1.Start();
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            ClassRoom classRoom = e.Parameter as ClassRoom;
            if (null != classRoom)
            {
                if (null != classRoom.VlcPlayer)
                {
                    classRoom.VlcPlayer.IsMute = !classRoom.VlcPlayer.IsMute;
                }
            }
        }
    }
}
