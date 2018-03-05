﻿using MRMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace MRMS.Dialogs
{
    /// <summary>
    /// Interaction logic for ClassAndGradeEditor.xaml
    /// </summary>
    public partial class ClassAndGradeEditor : IHasDataForm
    {
        public ClassAndGradeEditor()
        {
            InitializeComponent();
        }

        public RadDataForm DataForm
        {
            get
            {
                return dataForm;
            }
        }
    }
}
