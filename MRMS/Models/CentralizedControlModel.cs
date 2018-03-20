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
    public class CentralizedControlModel : NamedModel, IEditableObject
    {
        private CentralizedControlModel back;
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
        [Display(Name = "中控开关")]
        public bool CentralizedControlSwitch { get; set; }

        [DataMember]
        [Display(Name = "空调开关")]
        public bool AirConditionerSwitch { get; set; }

        [DataMember]
        [Display(Name = "照明开关")]
        public bool LampSwitch { get; set; }

        [DataMember]
        [Display(Name = "执行状态")]
        public bool? CommondExcuteStat { get; set; }

        [DataMember]
        [Display(Name = "教学楼")]
        [Required]
        public ClassroomBuilding ClassroomBuilding { get; set; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as CentralizedControlModel;
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
