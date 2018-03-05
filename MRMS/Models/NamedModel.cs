using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace MRMS.Models
{
    public class NamedModel : ViewModelBase, ICloneable
    {

        public string Name { get; set; }

        public ImageSource Image { get; set; }

        public bool IsSelected { get; set; }


        public bool IsExpanded { get; set; }

        public bool? IsChecked { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
