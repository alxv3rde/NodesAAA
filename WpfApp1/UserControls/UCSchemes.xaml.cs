using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Serialization;

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
            LoadSchemeName();

        }
        private UCSchemeButton ViewedButton;
        public UCSchemeButton SelectedButton;
        private async Task LoadSchemeName()
        {
            StreamReader file = new StreamReader($@"..\..\..\Schemes\lastScheme.txt");
            string actualSchemeName = file.ReadLine();
            file.Dispose();
            file.Close();
            SchemePanel.Children.Clear();
            string folderPath = @"..\..\..\Schemes\";
            //List<string> fileNames = GetFileNamesInFolder(folderPath);
            List<string> fileNames = Directory.GetFiles(folderPath)
                                       .Where(file => System.IO.Path.GetExtension(file) == ".xml").Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
            foreach (var item in fileNames)
            {
                UCSchemeButton uCSchemeB = new UCSchemeButton();
                buttons.Add(uCSchemeB);
                uCSchemeB.SetName(item);
                SchemePanel.Children.Add(uCSchemeB);
                uCSchemeB.MouseDown += UCSchemeB_MouseDown;
                if(uCSchemeB.GetName() == actualSchemeName)
                {
                    uCSchemeB.Selected = true;
                    uCSchemeB.Viewed = true;
                    ViewedButton = uCSchemeB;
                    SelectedButton = uCSchemeB;
                    LoadSelected();
                }
            }
            
        }

        public event MouseButtonEventHandler SelectMouseDown;

        private async void UCSchemeB_MouseDown(object sender, MouseButtonEventArgs e)
        {
                     
            UCSchemeButton button = (UCSchemeButton)sender;
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
            {
                await LoadXMLInfo(button.GetName());
                foreach (var b in buttons)
                {
                    b.Viewed = false;
                }
                button.Viewed = true;
                ViewedButton = button;
            }
            else if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                foreach (var b in buttons)
                {
                    b.Selected = false;
                }
                SelectedButton = button;
                button.Selected = true;
                SelectMouseDown?.Invoke(sender, e);
            }
            
        }
        private async Task LoadXMLInfo(string schemeName)
        {
            try
            {
                XDocument xDoc = XDocument.Load($@"..\..\..\Schemes\{schemeName}.xml");
                lblNameDes.Content = schemeName;
                tbDescription.Text = xDoc.Descendants("Description").FirstOrDefault()?.Value;
                lblLastEdit.Content = DateShower(xDoc.Descendants("LastEditDate").FirstOrDefault()?.Value);
            }
            catch
            {

            }
        }
        public async Task LoadSelected()
        {
            try
            {
                XDocument xDoc = XDocument.Load($@"..\..\..\Schemes\{SelectedButton.GetName()}.xml");
                lblNameDes.Content = xDoc.Descendants("Name").FirstOrDefault()?.Value;
                tbDescription.Text = xDoc.Descendants("Description").FirstOrDefault()?.Value;
                lblLastEdit.Content = DateShower(xDoc.Descendants("LastEditDate").FirstOrDefault()?.Value);
            }
            catch { }
        }
        public async Task LoadScheme(string schemeName)
        {
            XDocument xDoc = XDocument.Load($@"..\..\..\Schemes\{schemeName}.xml");
        }

        
        List<UCSchemeButton> buttons = new List<UCSchemeButton>();

        private List<(string, string)> _basicInfo = new List<(string, string)>(); // Item1 = name, item2 = description
        public UCSchemes(string _name)
        {

        }


        private event EventHandler btnCloseClicked;
        private bool isDragging = false;
        private void Grid_MouseEnter(object sender, MouseEventArgs e) // btnNewScheme
        {
            if (!_active)
                buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c68e45"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) // btnNewScheme
        {
            if (!_active)
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
        bool _active = false;
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) // btnNewScheme
        {
            _active = true;
            lblNew.Visibility = Visibility.Collapsed;
            tbNewScheme.Visibility = Visibility.Visible;
            buttonBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383840"));
            tbName.Focus();
            e.Handled = true;
        }


        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                NewScheme();
        }

        private void tbName_LostFocus(object sender, RoutedEventArgs e)
        {
            _active = false;
            lblNew.Visibility = Visibility.Visible;
            tbNewScheme.Visibility = Visibility.Collapsed;
            tbName.Text = "";
        }
        private void NewScheme()
        {
            _active = false;
            lblNew.Visibility = Visibility.Visible;
            tbNewScheme.Visibility = Visibility.Collapsed;
            DateTime dateTime = DateTime.Now;
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"), // Declaración XML
                new XElement("Scheme", // Elemento raíz
                    new XElement("Name", tbName.Text),
                    new XElement("Description", ""),
                    new XElement("CreationDate", dateTime.ToString()),
                    new XElement("LastEditDate", dateTime.ToString()),
                    new XElement("Nodes"),
                    new XElement("Conections")
                )
            );
            xDoc.Save(@"..\..\..\Schemes\" + tbName.Text + ".xml");

            LoadSchemeName();
            tbName.Text = "";

        }
        private async Task UpdateDescription()
        {
            DateTime dateTime = DateTime.Now;
            XDocument xDoc = XDocument.Load($@"..\..\..\Schemes\{ViewedButton.GetName()}.xml");
            xDoc.Descendants("Description").FirstOrDefault().Value = tbDescription.Text;
            xDoc.Descendants("LastEditDate").FirstOrDefault().Value = dateTime.ToString();
            xDoc.Save($@"..\..\..\Schemes\{ViewedButton.GetName()}.xml");
            await LoadXMLInfo(ViewedButton.GetName());
        }

        private async void tbDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await UpdateDescription();
            }
        }

        private void cPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _active = false;
            lblNew.Visibility = Visibility.Visible;
            tbNewScheme.Visibility = Visibility.Collapsed;
            tbName.Text = "";
        }
        private string DateShower(string dateString)
        {
            string format = "dd/MM/yyyy hh:mm:ss tt";
            dateString = dateString.Replace("a. m.", "AM").Replace("p. m.", "PM");
            DateTime dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            TimeSpan timeDifference = DateTime.Now - dateTime;

            if (timeDifference.TotalSeconds < 60)
            {
                return "Just Now";
            }
            else if (timeDifference.TotalMinutes < 60)
            {
                int minutes = (int)timeDifference.TotalMinutes;
                return $"{minutes} {(minutes == 1 ? "minute" : "minutes")} ago";
            }
            else if (timeDifference.TotalHours < 24)
            {
                int hours = (int)timeDifference.TotalHours;
                return $"{hours} {(hours == 1 ? "hour" : "hours")} ago";
            }
            else
            {
                int days = (int)Math.Round(timeDifference.TotalDays);
                return $"{days} {(days == 1 ? "day" : "days")} ago";
            }
        }
    }
}
