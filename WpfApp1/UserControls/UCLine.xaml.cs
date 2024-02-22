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
    /// Lógica de interacción para UCLine.xaml
    /// </summary>
    public partial class UCLine : UserControl
    {
        public UCLine(UCNode n1, UCNode n2)
        {
            InitializeComponent();
            _Nodes.Add(n1);
            _Nodes.Add(n2);
            DrawControl();
        }
        private double value;
        bool way = false;
        public bool IsWay { get => way; }
        public bool ConectionIsWay(UCNode n1, UCNode n2)
        {
            if (_Nodes.Contains(n1) && _Nodes.Contains(n2))
            {
                return IsWay;
            }
            return false;
        }
        public void ChangeToWayline()
        {
            way = true;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Wayline.png"));
        }
        public void ChangeToLine()
        {
            way = false;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Line.png"));
        }
        public void ChangeToTry()
        {
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/End.png"));
        }
        public double GetLineValue { get => value; }
        List<UCNode> _Nodes = new List<UCNode>();
        public void ChangeLineValue(double value)
        {
            this.value = value;
            lblValue.Content = value.ToString();
        }
        public bool ExistsConection(UCNode n1, UCNode n2)
        {
            if (_Nodes.Contains(n1) && _Nodes.Contains(n2))
            {
                return true;
            }
            return false;
        }

        private void DrawControl()
        {
            double co = Canvas.GetLeft(_Nodes[0]) - Canvas.GetLeft(_Nodes[1]);
            double ca = Canvas.GetTop(_Nodes[0]) - Canvas.GetTop(_Nodes[1]);
            Height = 12;
            Width = Math.Sqrt((co * co) + (ca * ca));
            Canvas.SetLeft(this, (Canvas.GetLeft(_Nodes[0]) + Canvas.GetLeft(_Nodes[1]) + _Nodes[0].Width - Width) / 2);
            Canvas.SetTop(this, (Canvas.GetTop(_Nodes[0]) + Canvas.GetTop(_Nodes[1]) + _Nodes[0].Width - Height) / 2);
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = new RotateTransform(Math.Atan(ca / co) * 180 / Math.PI);
        }
        public void ReDraw(UCNode n)
        {
            for (int i = 0; i < _Nodes.Count; i++)
            {
                if (_Nodes[i] == n)
                {
                    _Nodes[i] = n;
                }
            }
            DrawControl();
        }
        public List<UCNode> GetNodes { get => _Nodes; }
    }
}
