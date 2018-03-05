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

        public Meta.Vlc.Wpf.VlcPlayer VlcPlayer { get; set; }

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
