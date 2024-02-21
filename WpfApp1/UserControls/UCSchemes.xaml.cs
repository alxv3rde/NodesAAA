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
    /// Lógica de interacción para UCSchemes.xaml
    /// </summary>
    public partial class UCSchemes : UserControl
    {
        public UCSchemes()
        {
            InitializeComponent();
        }
        private event EventHandler btnCloseClicked;
        private bool isDragging = false;
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c68e45"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383840"));
        }


        private Point offset;
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            offset = e.GetPosition(this);
            Topbar.CaptureMouse();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePosition = e.GetPosition(Parent as Canvas);
                double newX = mousePosition.X - offset.X;
                double newY = mousePosition.Y - offset.Y;

                // Actualiza la posición del Grid arrastrable
                Canvas.SetLeft(this, newX);
                Canvas.SetTop(this, newY);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                Topbar.ReleaseMouseCapture();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var canvas = (Parent as Canvas);
            canvas.Children.Remove(this);
        }
    }
}
