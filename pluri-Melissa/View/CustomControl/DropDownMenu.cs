﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.View.CustomControl
{
    
    public class DropDownMenu : ContentControl
    {


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(DropDownMenu), new PropertyMetadata(false));


        static DropDownMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownMenu), new FrameworkPropertyMetadata(typeof(DropDownMenu)));
        }
    }
}
