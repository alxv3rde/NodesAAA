using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Xml.Linq;

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
            DataContext = this;
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(UCSchemeButton));
        private bool selected;
        private bool viewed;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (selected)
                {
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5d5d5d"));
                    btnDelete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    buttonBorder.Background = Brushes.Transparent;
                }
            }
        }
        public bool Viewed
        {
            get { return viewed; }
            set
            {
                viewed = value;
                if (viewed && !selected)
                {
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#49496c"));
                }
                else if (viewed && selected)
                {
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#49496c"));
                }
                else if (!viewed && !selected)
                {
                    buttonBorder.Background = Brushes.Transparent;
                }
                else if (!viewed && selected)
                {
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5d5d5d"));
                }

            }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                btnClone.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Visible;
            }
            else
            {
                btnClone.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnEdit.Visibility = Visibility.Visible;
                if (!viewed && !selected)
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c68e45"));
            }
        }


        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClone.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;
            if (!viewed && !selected)
                buttonBorder.Background = Brushes.Transparent;
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

        private void btnClone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btnEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btnDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel? btnParent = this.Parent as StackPanel;
            var res = MessageBox.Show("Are you sure you want to delete "+GetName()+"?","Confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                File.Delete($@"..\..\..\Schemes\{GetName()}.xml");
                
                btnParent.Children.Remove(this);
            }
            e.Handled = true;
        }
        // desing------------------------------

        private void btnClone_MouseEnter(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb70b"));
        }

        private void btnEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb70b"));
        }

        private void btnDelete_MouseEnter(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb70b"));
        }

        private void btnDelete_MouseLeave(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = Brushes.White;
        }

        private void btnEdit_MouseLeave(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = Brushes.White;
        }

        private void btnClone_MouseLeave(object sender, MouseEventArgs e)
        {
            IconImage ic = (IconImage)sender;
            ic.Foreground = Brushes.White;
        }
    }
}
