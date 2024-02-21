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
using static System.Net.Mime.MediaTypeNames;
using XamlAnimatedGif.Decoding;
using XamlAnimatedGif;
using WpfApp1.CustomControls;
using System.Reflection;
using WpfApp1.UserControls;
using WpfApp1.Algorithms.Dijkstra;
using WpfApp1.Algorithms;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            LoadButtons();

        }
        public static UCButton? LastButton { get; set; }
        private int count = -1;
        private int i = 0;
        Control? _ActiveControl;
        Point point;
        bool isShiftActive = false;

        public static List<UserControl> uCBoxes = new List<UserControl>();
        private static List<UCNode> _Nodes = new List<UCNode>();
        private static List<UCLine> _Lines = new List<UCLine>();
        private static List<UCButton> uCButtons = new List<UCButton>();

        public static List<UCNode> GetNodes { get => _Nodes; }
        public static List<UCLine> GetLines { get => _Lines; }
        public static List<UCButton> GetButtons { get => uCButtons; }
        private string SetName()
        {
            string[] abc = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L",
                "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string nName;
            if (i >= abc.Length)
            {
                i = 0;
                count++;
            }
            if (count > -1)
                nName = String.Format(abc[count] + abc[i]);
            else
                nName = String.Format(abc[i]);
            i++;
            return nName;

        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
            {
                foreach (var n in _Nodes)
                {
                    n.ShiftEnabled = false;
                    isShiftActive = false;
                }
            }

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
            {
                if (uCBoxes.Count > 0)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        c1.Children.Remove(ucb);
                    }
                    uCBoxes.Clear();
                }
                foreach (var n in _Nodes)
                {
                    n.ShiftEnabled = true;
                    isShiftActive = true;
                }
            }
            else if (e.Key == Key.Space)
            {
                List<UCNode> nodes = _Nodes.FindAll(x => x.IsSelected);
                if (nodes.Count == 2)
                {
                    if (!_Lines.Exists(x => x.ExistsConection(nodes[0], nodes[1])))
                    {
                        UCLine UCLine = new UCLine(nodes[0], nodes[1]);

                        _Lines.Add(UCLine);
                        c1.Children.Add(UCLine);
                        nodes[0].AddLine(UCLine);
                        nodes[1].AddLine(UCLine);
                        foreach (var n in nodes)
                        {
                            Canvas.SetZIndex(n, c1.Children.Count + 1);
                            n.UnSelect();
                        }
                        UCLine.MouseDown += UCLine_MouseDown;
                    }
                }
            }
            if (e.Key == Key.F11)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    Topmost = false;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    Topmost = true;

                }

            }

        }

        private void UCLine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                foreach (var ucb in uCBoxes)
                {
                    c1.Children.Remove(ucb);
                }
                uCBoxes.Clear();
                UCEditLineBox uCEditLineBox = new UCEditLineBox((UCLine)sender);
                uCBoxes.Add(uCEditLineBox);
                c1.Children.Add(uCEditLineBox);
                Canvas.SetLeft(uCEditLineBox, (c1.ActualWidth / 2) - 150);
                Canvas.SetZIndex(uCEditLineBox, c1.Children.Count + 1);
            }
            else if (e.ChangedButton == MouseButton.Right /*&& cbAlgorithm.SelectedIndex !=2*/)
            {
                UCLine ucr = (UCLine)sender;
                foreach (var n in ucr.GetNodes)
                {
                    n.RemoveLine(ucr);
                }
                c1.Children.Remove(ucr);
                _Lines.Remove(ucr);
            }
        }

        private void c1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.OriginalSource == c1 && e.ClickCount == 2 && e.GetPosition(c1).Y > 30)
            {
                var n = new UCNode(SetName());
                c1.Children.Add(n);
                _Nodes.Add(n);
                Point point = new Point();
                point.X = e.GetPosition(c1).X;
                point.Y = e.GetPosition(c1).Y;
                if ((double)point.X % 20 <= 10)
                    point.X -= point.X % 20;
                else point.X += (20 - point.X % 20);

                if ((double)point.Y % 20 <= 10)
                    point.Y -= point.Y % 20;
                else
                    point.Y += (20 - point.Y % 20);
                Canvas.SetLeft(n, point.X - 30);
                Canvas.SetTop(n, point.Y - 30);
                n.MouseDown += N_MouseDown;
                n.MouseMove += N_MouseMove;
                n.MouseUp += N_MouseUp;
            }
            else if (_Nodes.Count >= 1 && e.OriginalSource == c1)
            {
                if (uCBoxes.Count > 0)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        c1.Children.Remove(ucb);
                    }
                    uCBoxes.Clear();
                }
                var n = _Nodes.FindAll(x => x.IsSelected);
                if (n.Count >= 1)
                {
                    foreach (var ns in n)
                    {
                        ns.UnSelect();
                    }
                }
            }
        }
        private bool isDragging = false;
        private void N_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                if (isDragging)
                {
                    _ActiveControl = (UCNode)sender;
                }
                else if (!isShiftActive)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        c1.Children.Remove(ucb);
                    }
                    uCBoxes.Clear();
                    UCEditNodeBox uCEditNodeBox = new UCEditNodeBox((UCNode)sender);
                    uCBoxes.Add(uCEditNodeBox);
                    c1.Children.Add(uCEditNodeBox);
                    Canvas.SetLeft(uCEditNodeBox, (c1.ActualWidth / 2) - 150);
                    Canvas.SetZIndex(uCEditNodeBox, c1.Children.Count + 1);
                }
                isDragging = false;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                UCNode ucn = (UCNode)sender;
                foreach (var r in ucn.GetLines)
                {
                    c1.Children.Remove(r);
                    _Lines.Remove(r);
                }
                c1.Children.Remove(ucn);
                _Nodes.Remove(ucn);
                if (uCBoxes.Count > 0)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        c1.Children.Remove(ucb);
                    }
                    uCBoxes.Clear();
                }
            }

            _ActiveControl = null;
        }

        private void N_MouseMove(object sender, MouseEventArgs e)
        {
            isDragging = true;
        }

        private void N_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _ActiveControl = (UCNode)sender;
                isDragging = false;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public void LoadButtons()
        {
            
            btnPlay.IsControl = false;
            uCButtons.Add(btnPlay);
            btnReset.IsControl = false;
            uCButtons.Add(btnReset);
            btnEnd.IsControl = false;
            btnEnd.UnableButton();
        }

        private void btnManual_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnPlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var n in _Nodes)
            {
                n.LoadNodes();
            }
            
            List<UCNode> endNodes = _Nodes.FindAll(x => x.GetNodeType == Enums.NodeType.End);
            if (endNodes.Count > 0)
            {
                Traveler traveler = new Traveler(endNodes[0]);
                lblLastCalculation.Content = traveler.Run();
                foreach (var button in uCButtons)
                {
                    button.UnableButton();
                }
                btnEnd.EnableButton();
            }
        }

        private void btnEnd_MouseDown(object sender, MouseButtonEventArgs e)
        {

            foreach (var l in _Lines)
            {
                l.Visibility = Visibility.Visible;
                l.ChangeToLine();
            }
            foreach (var n in _Nodes)
            {
                n.DefaultValues();
            }
            foreach (var buton in uCButtons)
            {
                buton.EnableButton();
            }
            btnEnd.UnableButton();
        }

        private void cbAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ClearNodesAndLines()
        {
            foreach (var n in _Nodes)
            {
                c1.Children.Remove(n);
            }
            _Nodes.Clear();
            foreach (var l in _Lines)
            {
                c1.Children.Remove(l);
            }
            _Lines.Clear();
            i = 0;
            count = -1;
        }

        private void btnReset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearNodesAndLines();
        }

        private void c1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_ActiveControl == null)
            {
                return;
            }
            var n = (UCNode)_ActiveControl;
            if (n.IsSelected)
            {
                Point point = new Point();
                point.X = e.GetPosition(c1).X;
                point.Y = e.GetPosition(c1).Y;

                if (point.Y > 30)
                {
                    if ((double)point.X % 20 <= 10)
                        point.X -= point.X % 20;
                    else point.X += (20 - point.X % 20);

                    if ((double)point.Y % 20 <= 10)
                        point.Y -= point.Y % 20;
                    else
                        point.Y += (20 - point.Y % 20);

                    Canvas.SetLeft(_ActiveControl, point.X - 30);
                    Canvas.SetTop(_ActiveControl, point.Y - 30);
                    List<UCLine> Lines = _Lines.FindAll(x => x.GetNodes.Exists(y => y == _ActiveControl));
                    foreach (var r in Lines)
                    {
                        r.ReDraw((UCNode)_ActiveControl);
                    }
                }
            }
        }

        private void btnBars_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnBars_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UCSchemes uCSchemes = new UCSchemes();
            OverCanva.Children.Add(uCSchemes);
            Canvas.SetLeft(uCSchemes, c1.ActualWidth / 2 - 375);
            Canvas.SetTop(uCSchemes, OverCanva.ActualHeight / 2 - 250);
            Canvas.SetZIndex(uCSchemes, OverCanva.Children.Count + 1);
        }
    }
}
