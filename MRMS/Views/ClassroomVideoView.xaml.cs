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
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += (s, f) =>
            {
                timer.Stop();
                vlcPlayer.LoadMedia((vlcPlayer.DataContext as ClassRoom).VedioAddress);
                vlcPlayer.Play();
            };
            timer.Start();
        }
    }
}
