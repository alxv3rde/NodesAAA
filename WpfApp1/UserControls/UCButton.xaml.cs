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

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para UCButton.xaml
    /// </summary>
    public partial class UCButton : UserControl
    {
        public UCButton()
        {
            InitializeComponent();
            DataContext = this;   
        }
        private bool selected = false;
        private bool control = true;
        public bool IsControl { get => control; set => control = value; }
        public bool IsSelected { get => selected; }
        public void UnableButton()
        {
            this.IsEnabled = false;
            img.Opacity = 0.5;

        }
        public void EnableButton()
        {
            this.IsEnabled = true;
            img.Opacity = 1;

        }

        public static readonly DependencyProperty RutaImagenProperty =
            DependencyProperty.Register("RutaImagen", typeof(string), typeof(UCButton), new PropertyMetadata("/Resources/End.png"));

        public string RutaImagen
        {
            get { return (string)GetValue(RutaImagenProperty); }
            set { SetValue(RutaImagenProperty, value); }
        }
        public void Select()
        {
            if (IsControl)
            {
                MainWindow.LastButton = this;
                selected = true;
                border.Background = new SolidColorBrush(Colors.White) { Opacity = 0.15 };
            }
            foreach(var n in MainWindow.GetNodes.FindAll(x=>x.IsSelected))
            {
                n.UnSelect();
            }
            
        }
        public void UnSelect()
        {
            selected = false;
            border.Background = new SolidColorBrush(Colors.White) { Opacity = 0 };
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                return;
            }
            border.Background = new SolidColorBrush(Colors.White) { Opacity = 0.15 };
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                return;
            }
            border.Background = new SolidColorBrush(Colors.White) { Opacity = 0 };
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsSelected)
            {
                foreach(var b in MainWindow.GetButtons.FindAll(x=>x.IsEnabled == true))
                {
                    b.UnSelect();
                }
                Select();
            }
        }
        public void ChangeIcon(string uri)
        {
            img.Source = new BitmapImage(new Uri(uri));
        }
    }
}
