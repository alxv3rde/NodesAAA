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
using WpfApp1.Enums;
using XamlAnimatedGif;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCNode.xaml
    /// </summary>
    public partial class UCNode : UserControl
    {
        public UCNode(string name)
        {
            InitializeComponent();
            type = NodeType.Common;
            ChangeName(name);
            Visited = false;
        }
        public void DefaultValues()
        {
            VFinal = null;
            VTemp = null;
            Visited = false;
        }
        public bool Visited { get; set; }
        public int? VFinal { get; set; }
        public int? VTemp { get; set; }
        private string name;
        private bool shiftEnabled = false;
        public bool ShiftEnabled { get => shiftEnabled; set => shiftEnabled = value; }
        public List<UCNode> nodes = new List<UCNode>();
        public List<UCLine> lines = new List<UCLine>();
        private bool selected = false;
        private NodeType type;
        public NodeType GetNodeType { get => type; }
        public string GetName { get => name; }
        public bool IsSelected { get => selected; }
        public void LoadNodes()
        {
            nodes = MainWindow.GetNodes.FindAll(x=>x.GetLines.Exists(y=>lines.Exists(z => z == y)) && x != this);
        }
        public void ChangeName(string name)
        {
            this.name = name;
            lblName.Content = name;
        }
        public void NodeTypeToEnd()
        {
            type = NodeType.End;
            n2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/NEndNode.png"));
        }
        public void NodeTypeToCommon()
        {
            type = NodeType.Common;
            n2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/NCommonNode.png"));
        }
        public void UnSelect()
        {
            if (type == NodeType.Common)
            {
                n2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/NCommonNode.png"));

            }
            else
            {
                n2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/NEndNode.png"));
            }

            selected = false;
        }


        public void AddLine(UCLine uCRama)
        {
            lines.Add(uCRama);
        }
        public void RemoveLine(UCLine uCRama)
        {
            lines.Remove(uCRama);
        }
        public List<UCLine> GetLines { get => lines; }

        private void gNode_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void gNode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !selected)
            {
                if (type == NodeType.Common)
                {
                    if (shiftEnabled)
                    {
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedCommonNode.gif"));
                        selected = true;

                    }
                    else
                    {
                        foreach (var n in MainWindow.GetNodes)
                        {
                            n.UnSelect();
                        }
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedCommonNode.gif"));
                        selected = true;
                    }
                }
                else
                {
                    if (shiftEnabled)
                    {
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedEndNode.gif"));
                        selected = true;

                    }
                    else
                    {
                        foreach (var n in MainWindow.GetNodes)
                        {
                            n.UnSelect();
                        }
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedEndNode.gif"));
                        selected = true;
                    }

                }


            }
            else if (e.ChangedButton == MouseButton.Left && selected)
            {

                if (shiftEnabled)
                {
                    UnSelect();
                }
                else
                {
                    if (type == NodeType.Common)
                    {
                        foreach (var n in MainWindow.GetNodes)
                        {
                            n.UnSelect();
                        }
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedCommonNode.gif"));
                        selected = true;
                    }
                    else
                    {
                        foreach (var n in MainWindow.GetNodes)
                        {
                            n.UnSelect();
                        }
                        AnimationBehavior.SetSourceUri(n2, new Uri("pack://application:,,,/Resources/NSelectedEndNode.gif"));
                        selected = true;
                    }

                }
            }

        }
    }
}
