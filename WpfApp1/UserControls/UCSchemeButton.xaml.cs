using System;
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
                    btnClone.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    buttonBorder.Background = Brushes.Transparent;
                    btnClone.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;
                    btnDelete.Visibility = Visibility.Hidden;
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
                    btnClone.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                }else if (viewed && selected)
                {
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#49496c"));
                    btnClone.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Collapsed;
                }
                else if(!viewed &&!selected)
                {
                    buttonBorder.Background = Brushes.Transparent;
                    btnClone.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;
                    btnDelete.Visibility = Visibility.Hidden;
                }else if (!viewed && selected)
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
            if (viewed && selected)
            {

            }
            else
            {
                if(!viewed)
                    buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c68e45"));
            }
        }
                

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
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
    }
}
