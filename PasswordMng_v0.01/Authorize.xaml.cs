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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace PasswordMng_v0._01
{
    /// <summary>
    /// Interaktionslogik für Authorize.xaml
    /// </summary>
    public partial class Authorize : Window
    {
        public Authorize()
        {
            InitializeComponent();
        }

        

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            SQLParser sQLParser = new SQLParser();
            String mpass = sQLParser.getMasterPass();

            if (LoginBox.Text == mpass)
            {
                MainWindow main = new MainWindow();
                this.Hide();
                main.ShowDialog();

            }
            else { 
                noEntry.Foreground = new SolidColorBrush(Colors.Red);
                noEntry.Content = "Wrong Password";
            }
        }
        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
