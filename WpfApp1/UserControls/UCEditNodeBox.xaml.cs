using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;
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

        private async void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
           await ApplyChanges();
        }

        private async void gEditNode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               await ApplyChanges();
            }

        }
        private async Task ApplyChanges()
        {
            StreamReader file = new StreamReader($@"..\..\..\Schemes\lastScheme.txt");
            string rl = file.ReadLine();
            file.Dispose();
            file.Close();
            XDocument xDoc = XDocument.Load($@"..\..\..\Schemes\{rl}.xml");
            XElement? updateType = xDoc.Root.Element("Nodes")
                    .Elements("Node").FirstOrDefault(n => (string)n.Element("Name") == node.GetName);

            
            if (nodeType == NodeType.End)
                node.NodeTypeToEnd();
            else
                node.NodeTypeToCommon();

            if (MainWindow.GetNodes.Exists(x => x.GetName == tbName.Text) && tbName.Text != node.GetName)
            {
                lblWarning.Visibility = Visibility.Visible;
                return;
            }
            string tempName = node.GetName;
            if (tbName.Text != "")
            {

                node.ChangeName(tbName.Text);
            }
            updateType.Element("Type").Value = node.GetNodeType.ToString();
            updateType.Element("Name").Value = node.GetName;
            if(node.GetLines.Count > 0)
            {
                XElement? updateIDLine = xDoc.Root.Element("Conections");
                foreach (var line in updateIDLine.Elements("Line"))
                {
                    UCLine nodeLine = node.lines.FirstOrDefault(x => x.ID == line.Element("ID").Value);
                    if (line.Element("Node1").Value == tempName)
                    {
                        string id= node.GetName + line.Element("Node2").Value;
                        line.Element("ID").Value = id;
                        nodeLine.ID = id;
                        line.Element("Node1").Value = node.GetName;
                        
                    }
                    else if (line.Element("Node2").Value == tempName)
                    {
                        string id = line.Element("Node1").Value + node.GetName;
                        line.Element("ID").Value = id;
                        line.Element("Node2").Value = node.GetName;
                        nodeLine.ID = id;
                    }
                }
            }
            xDoc.Descendants("LastEditDate").FirstOrDefault().Value = DateTime.Now.ToString();
            xDoc.Save($@"..\..\..\Schemes\{xDoc.Descendants("Name").FirstOrDefault().Value}.xml");
            var panel = Parent as Grid;
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
