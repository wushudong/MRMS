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
    public class FacultyModel : NamedModel, IEditableObject
    {
        private FacultyModel back;
        public FacultyModel()
        {
            ClassesAndGrades = new List<ClassAndGradeModel>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [Display(Name = "院系名称")]
        public string FacultyName { get; set; }

        [DataMember]
        [Display(Name = "序号")]
        public int Ord { get; set; }

        [DataMember]
        public ICollection<ClassAndGradeModel> ClassesAndGrades { set; get; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as FacultyModel;
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
