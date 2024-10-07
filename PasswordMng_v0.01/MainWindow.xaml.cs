using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


namespace PasswordMng_v0._01
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<PasswordObject> data = new List<PasswordObject>();
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }

       

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            SQLParser.CreateConnection();
            data = SQLParser.getData();
            foreach (PasswordObject P in data)
            {
                CreatePanel(P, Wrap_Panel);
            }
        }

        public List<PasswordObject> GetData()
        {
            return data;
        }
        public void CreatePanel(PasswordObject P, WrapPanel wrapPanel)
        {

            this.Resources["P"] = P;

            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(24),
                Height = 100,
                Width = 400,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD7E8D0")),
                Margin = new Thickness(0, 30, 0, 0),
            };
            
          
           
            border.MouseEnter += (sender, e) => { border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA8B7A1")); };
            border.MouseLeave += (sender, e) => { border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD7E8D0")); };
            border.MouseLeftButtonDown += (sender, e) => { ClipboardCopyEvent(border, e, P); };
            border.MouseRightButtonUp += (sender, e) => { contextMenu(border, e,P); };

            StackPanel contentPanel = new StackPanel
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(10)
            };

            Label webLabel = new Label
            {
                Content = "URL",
                FontFamily = new FontFamily("Bahnschrift Condensed"),
                FontSize = 15.75,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(15, 0, 0, 0)
            };

            Label pwLabel = new Label
            {
                Content = "Password",
                FontFamily = new FontFamily("Bahnschrift Condensed"),
                FontSize = 15.75,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(15, 0, 0, -100)
            };

            Label userLabel = new Label
            {
                Content = "Username",
                FontFamily = new FontFamily("Bahnschrift Condensed"),
                FontSize = 15.75,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(15, 0, 0, 0)
            };

            Label webSite = new Label
            {
                Content = P.Website,
                FontSize = 14.25,
                Margin = new Thickness(90, -61, 0, 0)
            };

            Label user = new Label
            {
                Content = P.Username,
                FontSize = 14.25,
                Margin = new Thickness(90, -31, 0, 0)
            };


            Label passwrd = new Label
            {
                FontSize = 14.25,
                Margin = new Thickness(90, -2, 0, -100)
            };

            Binding passBinding = new Binding("MaskedPassword")
            {
                Source = P,
            };
            passwrd.SetBinding(Label.ContentProperty, passBinding);

            Binding webBinding = new Binding("Website")
            {
                Source = P,
            };
            webSite.SetBinding(Label.ContentProperty, webBinding);

            Binding usrBinding = new Binding("Username")
            {
                Source = P,
            };
            user.SetBinding(Label.ContentProperty, usrBinding);


            contentPanel.Children.Add(webLabel);
            contentPanel.Children.Add(userLabel);
            contentPanel.Children.Add(pwLabel);
            contentPanel.Children.Add(webSite);
            contentPanel.Children.Add(user);
            contentPanel.Children.Add(passwrd);

            border.Child = contentPanel;

            wrapPanel.Children.Add(border);


        }

        public void ClipboardCopyEvent(object sender, MouseButtonEventArgs e, PasswordObject P)
        {
            if(e.ClickCount == 1)
            {
                Clipboard.SetText(P.Password);
            }else if(e.ClickCount == 2)
            {
                Clipboard.SetText(P.Username);
            }
        }
        private string MaskPassword(string password)
        {
            return new string('*', password.Length);
        }

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public void contextMenu(Border dataPanel, MouseButtonEventArgs e, PasswordObject P)
        {
            Context_Menu context = new Context_Menu(dataPanel, P);

            // Ermitteln der aktuellen Mausposition
            Point mousePosition = e.GetPosition(this);

            // Setzen der Position des Kontextmenüs
            context.Left = mousePosition.X + this.Left;
            context.Top = mousePosition.Y + this.Top;

            context.ShowDialog();
        }

       

        private void Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchWindow search = new SearchWindow(data);
            search.ShowDialog();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {


            using(AddPass add = new AddPass())
            {

                
                if (add.ShowDialog() == true)
                {
                    PasswordObject P = new PasswordObject(add.GetWebsite(), add.GetUsername(), add.GetPassword());
                    data.Add(P);
                    CreatePanel(P, Wrap_Panel);
                  
                }
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordObject P = new PasswordObject();
            Encryption enc = new Encryption();
            byte[] iv = enc.GenerateRandomIV();
            foreach (PasswordObject PD in P.GetData())
            {
                byte[] key = SQLParser.retrieveKey();
                byte[] p_cypher = enc.Encrypt(PD.Password, key, iv);
                byte[] u_cypher = enc.Encrypt(PD.Username, key, iv);
                SQLParser.insertData(p_cypher, u_cypher, PD.Website, iv);

            }
            this.Close();
            
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Anwendung vollständig beenden
        }
    }


}
