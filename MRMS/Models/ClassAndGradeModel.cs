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
    public class ClassAndGradeModel : NamedModel, IEditableObject
    {
        private ClassAndGradeModel back;
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "院系名称")]
        [Required]
        public FacultyModel Faculty { set; get; }

        [DataMember]
        [Display(Name = "专业名称")]
        public string MajorName { get; set; }

        [DataMember]
        [Display(Name = "班级名称")]
        public string ClassAndGradeName { get; set; }

        [DataMember]
        [Display(Name = "班级人数")]
        public int NumberOfStudents { get; set; }

        [DataMember]
        [Display(Name = "入学日期")]
        public string EntranceDate { get; set; }

        [DataMember]
        [Display(Name = "毕业日期")]
        public string GraduationDate { get; set; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as ClassAndGradeModel;
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
