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
        public XDocument ActualScheme = new XDocument();
        public MainWindow()
        {
            InitializeComponent();

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            LoadButtons();

        }
        private bool isDraggingSV = false;
        private Point lastMousePosition;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SVPanel.ScrollToHorizontalOffset(SVPanel.ScrollableWidth / 2);
            SVPanel.ScrollToVerticalOffset(SVPanel.ScrollableHeight / 2);
        }
        public static UCButton? LastButton { get; set; }
        private int count = -1;
        private int i = 0;
        Control? _ActiveControl;
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
                    this.ResizeMode = ResizeMode.CanResize;
                    Topmost = false;
                }
                else
                {
                    this.ResizeMode = ResizeMode.NoResize;
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
                var n = _Nodes.FindAll(x => x.IsSelected);
                if (n.Count >= 1)
                {
                    foreach (var ns in n)
                    {
                        ns.UnSelect();
                    }
                }
                foreach (var ucb in uCBoxes)
                {
                    PropertiesBox.Children.Remove(ucb);
                }
                uCBoxes.Clear();
                UCEditLineBox uCEditLineBox = new UCEditLineBox((UCLine)sender);
                uCBoxes.Add(uCEditLineBox);
                PropertiesBox.Children.Add(uCEditLineBox);
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
                if (uCBoxes.Count > 0)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        PropertiesBox.Children.Remove(ucb);
                    }
                    uCBoxes.Clear();
                }
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
                UCEditNodeBox uCEditNodeBox = new UCEditNodeBox(n);
                uCBoxes.Add(uCEditNodeBox);
                PropertiesBox.Children.Add(uCEditNodeBox);
            }
            else if (_Nodes.Count >= 1 && e.OriginalSource == c1)
            {
                if (uCBoxes.Count > 0)
                {
                    foreach (var ucb in uCBoxes)
                    {
                        PropertiesBox.Children.Remove(ucb);
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
                if (!isShiftActive)
                {

                    _ActiveControl = (UCNode)sender;
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
                        PropertiesBox.Children.Remove(ucb);
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
            foreach (var ucb in uCBoxes)
            {
                PropertiesBox.Children.Remove(ucb);
            }
            uCBoxes.Clear();

            if (e.ChangedButton == MouseButton.Left)
            {
                if (_Nodes.FindAll(x => x.IsSelected).Count <= 1)
                {

                    UCEditNodeBox uCEditNodeBox = new UCEditNodeBox((UCNode)sender);
                    uCBoxes.Add(uCEditNodeBox);
                    PropertiesBox.Children.Add(uCEditNodeBox);
                }
                _ActiveControl = (UCNode)sender;
                isDragging = false;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public void LoadButtons()
        {
            btnSchemas.IsControl = false;
            btnAdyacencia.IsControl = false;
            btnBars.IsControl = false;
            uCButtons.Add(btnBars);
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
                DateTime inicio = DateTime.Now;
                double res = traveler.Run();
                DateTime fin = DateTime.Now;
                lblLastCalculation.Content = string.Format("{0:0.00}", res);
                TimeSpan duracion = fin - inicio;
                lblLastMS.Content = duracion.TotalSeconds + "s";
                string tot = "";
                string first = endNodes[0].GetName;
                foreach (var item in traveler.lines)
                {
                    List<string> temp = new List<string>();
                    foreach (var i in item.Item1)
                    {
                        if (!temp.Contains(i.GetNodes[0].GetName) && !temp.Contains(i.GetNodes[1].GetName))
                        {
                            tot += i.GetNodes[0].GetName + " -> " + i.GetNodes[1].GetName + " -> ";
                            temp.Add(i.GetNodes[0].GetName);
                            temp.Add(i.GetNodes[1].GetName);
                        }
                        else
                        {
                            if (!temp.Contains(i.GetNodes[0].GetName))
                            {
                                tot += i.GetNodes[0].GetName + " -> ";
                                temp.Add(i.GetNodes[0].GetName);
                            }
                            else if (!temp.Contains(i.GetNodes[1].GetName))
                            {
                                tot += i.GetNodes[1].GetName + " -> ";
                                temp.Add(i.GetNodes[1].GetName);
                            }
                            else
                            {
                                tot += endNodes[0].GetName + " = " + string.Format("{0:0.00}", item.Item2);
                            }
                        }
                    }
                    tot += "\n";
                }
                MessageBox.Show(tot);
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


        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Right) && e.OriginalSource == c1)
            {
                isDraggingSV = true;
                lastMousePosition = e.GetPosition(SVPanel);
                SVPanel.CaptureMouse();
            }

        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Right)
            {
                isDraggingSV = false;
                SVPanel.ReleaseMouseCapture();
            }

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingSV)
            {
                Point currentMousePosition = e.GetPosition(SVPanel);
                double deltaX = currentMousePosition.X - lastMousePosition.X;
                double deltaY = currentMousePosition.Y - lastMousePosition.Y;

                SVPanel.ScrollToHorizontalOffset(SVPanel.HorizontalOffset - deltaX);
                SVPanel.ScrollToVerticalOffset(SVPanel.VerticalOffset - deltaY);

                lastMousePosition = currentMousePosition;
            }
        }
        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (OverCanva.Children.Count > 0)
            {
                Canvas.SetLeft(OverCanva.Children[0], OverCanva.ActualWidth / 2 - 375);
                Canvas.SetTop(OverCanva.Children[0], OverCanva.ActualHeight / 2 - 250);

            }
            double contentWidth = SVPanel.ExtentWidth;
            double contentHeight = SVPanel.ExtentHeight;

            double horizontalOffsetRatio = SVPanel.HorizontalOffset / Math.Max(1, contentWidth - e.PreviousSize.Width);
            double verticalOffsetRatio = SVPanel.VerticalOffset / Math.Max(1, contentHeight - e.PreviousSize.Height);

            double newHorizontalOffset = horizontalOffsetRatio * Math.Max(0, contentWidth - e.NewSize.Width);
            double newVerticalOffset = verticalOffsetRatio * Math.Max(0, contentHeight - e.NewSize.Height);

            SVPanel.ScrollToHorizontalOffset(newHorizontalOffset);
            SVPanel.ScrollToVerticalOffset(newVerticalOffset);
        }

        private void btnSchemas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (OverCanva.Children.OfType<UCSchemes>().Count() <= 0)
            {
                UCSchemes uCSchemes = new UCSchemes();
                OverCanva.Children.Add(uCSchemes);
                Canvas.SetLeft(uCSchemes, OverCanva.ActualWidth / 2 - 375);
                Canvas.SetTop(uCSchemes, OverCanva.ActualHeight / 2 - 250);
            }
        }

        private void btnAdyacencia_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnAdyacencia_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
