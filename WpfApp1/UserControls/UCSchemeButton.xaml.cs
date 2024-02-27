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

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCSchemeButton.xaml
    /// </summary>
    public partial class UCSchemeButton : UserControl
    {
        public UCSchemeButton()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(UCSchemeButton));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c68e45"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383840"));
        }
        public void SetName(string item)
        {
            lblNombre.Content = item;
        }
        public string GetName()
        {
            return lblNombre.Content.ToString();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
