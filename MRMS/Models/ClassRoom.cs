using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace MRMS.Models
{
    [DataContract]
    public class ClassRoom : NamedModel, IEditableObject
    {
        private ClassRoom back;
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        [Required]
        [Display(Name = "教室编号")]
        public string RoomNum { get; set; }
        
        [DataMember]
        [Display(Name = "终端编号")]
        public string TerminalId { get; set; }

        [DataMember]
        [Display(Name = "终端IP")]
        public string TerminalIp { get; set; }

        [DataMember]
        [Display(Name = "视频地址")]
        public string VedioAddress { get; set; }

        [DataMember]
        [Display(Name = "使用方式")]
        public string IdentifyMode { get; set; }

        public Meta.Vlc.Interop.Media.MediaState VedioState { get; set; }

        private Meta.Vlc.Wpf.VlcPlayer _VlcPlayer;

        public Meta.Vlc.Wpf.VlcPlayer VlcPlayer
        {
            get { return _VlcPlayer; }
            set
            {
                if (null == _VlcPlayer)
                {
                    _VlcPlayer = value;
                    _VlcPlayer.IsMuteChanged += (sender, e) => {
                        //var timer = new System.Timers.Timer(1000) { AutoReset = false };
                        //timer.Elapsed += delegate {
                        //    timer.Dispose();
                        //    this.OnPropertyChanged("IsMute");
                        //};
                        //timer.Start();
                        this.OnPropertyChanged("IsMute");
                    };
                    _VlcPlayer.VolumeChanged += (sender, e) => { this.OnPropertyChanged("Volume"); };
                }
            }
        }

        public bool IsMute
        {
            get { return null != VlcPlayer ? VlcPlayer.IsMute : false; }
            set
            {
                if (null != VlcPlayer && !Equals(VlcPlayer.IsMute, value))
                {
                    VlcPlayer.IsMute = value;
                }
            }
        }
        public int Volume
        {
            get { return VlcPlayer != null ? VlcPlayer.Volume : 0; }
            set
            {
                if (VlcPlayer != null && !Equals(VlcPlayer.Volume, value))
                {
                    VlcPlayer.Volume = value;
                }
            }
        }

        [DataMember]
        [Display(Name = "教学楼")]
        [Required]
        public ClassroomBuilding ClassroomBuilding { get; set; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as ClassRoom;
        }

        public void EndEdit()
        {

        }

        public void CancelEdit()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(this, property.GetValue(back, null), null);
            }
        }
    }
}
