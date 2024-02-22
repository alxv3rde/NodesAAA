using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
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
using WpfApp1.UserControls;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para UCEditLineBox.xaml
    /// </summary>
    public partial class UCEditLineBox : UserControl
    {
        public UCEditLineBox(UCLine line)
        {
            InitializeComponent();
            _Line = line;
            tbName.Text = line.GetLineValue.ToString();
        }
        UCLine _Line;

        private void btnSave_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSave.Background = new SolidColorBrush(Color.FromRgb(77, 90, 150));
        }

        private void btnSave_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSave.Background = new SolidColorBrush(Color.FromRgb(61, 73, 128));
        }

        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            saveChanges();
        }

        private void gEditLIne_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                saveChanges();
            }
        }

        private void saveChanges()
        {
            if(tbName.Text != "")
            {
                try
                {
                    _Line.ChangeLineValue(Convert.ToDouble(tbName.Text));
                }
                catch { }
                
            }
            var panel = this.Parent as Grid;
            if (panel != null)
            {
                MainWindow.uCBoxes.Clear();
                panel.Children.Remove(this);
                return;
            }
        }

        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
        }

        private void gEditLIne_Loaded(object sender, RoutedEventArgs e)
        {
            tbName.Focus();
        }
    }
}
