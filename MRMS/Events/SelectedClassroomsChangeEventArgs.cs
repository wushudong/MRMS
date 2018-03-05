using MRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRMS.Events
{
    public delegate void SelectedClassroomsChangedHandle(object sender, SelectedClassroomsChangedEventArgs e);
    public class SelectedClassroomsChangedEventArgs : EventArgs
    {
        private ICollection<ClassRoom> beforeChange;
        private ICollection<ClassRoom> afterChange;
        public SelectedClassroomsChangedEventArgs(ICollection<ClassRoom> beforeChange, ICollection<ClassRoom> afterChange)
        {
            this.beforeChange = beforeChange;
            this.afterChange = afterChange;
        }
        public ICollection<ClassRoom> BeforeChange
        {
            get { return beforeChange; }
        }
        public ICollection<ClassRoom> AfterChange
        {
            get { return afterChange; }
        }
    }
}
