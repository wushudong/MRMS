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
    public class IcCardModel : NamedModel, IEditableObject
    {
        private IcCardModel back;
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "IC卡代码")]
        [Required(ErrorMessage = "不能为空值。")]
        public string HexCode { get; set; }
        [DataMember]
        [Display(Name = "IC卡编号")]
        [Required(ErrorMessage = "不能为空值。")]
        public string CardNum { get; set; }
        [DataMember]
        [Display(Name = "工号")]
        [Required(ErrorMessage = "不能为空值。")]
        public string PersonId { get; set; }
        [DataMember(Name = "Name")]
        [Display(Name = "性别")]
        public string PersonName { get; set; }
        [DataMember]
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [DataMember]
        [Display(Name = "院系名称")]
        public FacultyModel Faculty { set; get; }
        [DataMember]
        [Display(Name = "工作单位")]
        public string Career { get; set; }
        [DataMember]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        [DataMember]
        [Display(Name = "电话")]
        public string Phone { get; set; }
        [DataMember]
        [Display(Name = "卡片类别")]
        [Required(ErrorMessage = "不能为空值。")]
        //A.管理员卡
        //T.教师卡
        //S.学生卡
        //L.临时卡
        public string CardType { get; set; }

        public void BeginEdit()
        {
            back = this.MemberwiseClone() as IcCardModel;
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
