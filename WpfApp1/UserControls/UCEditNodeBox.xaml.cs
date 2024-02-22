using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using WpfApp1.UserControls;
using XamlAnimatedGif;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para UCEditNodeBox.xaml
    /// </summary>
    public partial class UCEditNodeBox : UserControl
    {
        public UCEditNodeBox(UCNode node)
        {
            InitializeComponent();
            this.node = node;
            nodeType = node.GetNodeType;
            LoadNode();
            tbName.Text = node.GetName;
        }
        UCNode node;
        NodeType nodeType;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectionStart = tbName.SelectionStart;
            tbName.Text = tbName.Text.ToUpper();
            tbName.SelectionStart = selectionStart;
            tbName.SelectionLength = 0;
        }
        private void btnNodeStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (nodeType == NodeType.End)
            {
                AnimationBehavior.SetSourceUri(imgNode, new Uri("pack://application:,,,/Resources/NCommonNode.png"));
                nodeType = NodeType.Common;
            }
            else
            {
                AnimationBehavior.SetSourceUri(imgNode, new Uri("pack://application:,,,/Resources/NEndNode.png"));
                nodeType = NodeType.End;
            }
        }
        public void LoadNode()
        {
            if (nodeType == NodeType.Common)
            {
                AnimationBehavior.SetSourceUri(imgNode, new Uri("pack://application:,,,/Resources/NCommonNode.png"));
                nodeType = NodeType.Common;
            }
            else
            {
                AnimationBehavior.SetSourceUri(imgNode, new Uri("pack://application:,,,/Resources/NEndNode.png"));
                nodeType = NodeType.End;
            }
        }

        private void btnNodeStatus_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNodeStatus.Background = new SolidColorBrush(Colors.White) { Opacity = 0.15 };
        }

        private void btnNodeStatus_MouseLeave(object sender, MouseEventArgs e)
        {
            btnNodeStatus.Background = new SolidColorBrush(Colors.White) { Opacity = 0 };
        }

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
            ApplyChanges();
        }

        private void gEditNode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyChanges();
            }

        }
        private void ApplyChanges()
        {
            if (nodeType == NodeType.End) node.NodeTypeToEnd();
            else node.NodeTypeToCommon();

            if (MainWindow.GetNodes.Exists(x => x.GetName == tbName.Text) && tbName.Text != node.GetName)
            {
                lblWarning.Visibility = Visibility.Visible;
                return;
            }

            if (tbName.Text != "")
            {

                node.ChangeName(tbName.Text);
            }
            var panel = this.Parent as Grid;
            if (panel != null)
            {
                MainWindow.uCBoxes.Clear();
                panel.Children.Remove(this);
                return;
            }
        }

        private void gEditNode_Loaded(object sender, RoutedEventArgs e)
        {
            tbName.Focus();
        }

        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
        }
    }
}
