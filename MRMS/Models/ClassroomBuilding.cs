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
    public class ClassroomBuilding : NamedModel, IEditableObject
    {
        private ClassroomBuilding back;
        public ClassroomBuilding()
        {
            ClassRooms = new List<ClassRoom>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "教学楼名称")]
        public string BuildingName { get; set; }

        [DataMember]
        [Display(Name = "位置")]
        public string Location { get; set; }

        [DataMember]
        [Display(Name = "序号")]
        public int Ord { get; set; }

        [DataMember]
        public ICollection<ClassRoom> ClassRooms { set; get; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as ClassroomBuilding;
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
