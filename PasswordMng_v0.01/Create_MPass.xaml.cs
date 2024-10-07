using System;
using System.Collections.Generic;
using System.Data.SQLite;
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


namespace PasswordMng_v0._01
{
    /// <summary>
    /// Interaktionslogik für Create_MPass.xaml
    /// </summary>
    public partial class Create_MPass : Window
    {
        public Create_MPass()
        {
            InitializeComponent();
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            Encryption enc = new Encryption();
            SQLiteConnection conn = SQLParser.CreateMConnection();
            SQLiteCommand sqlcmd = conn.CreateCommand();
            

            if (PassBox.Text == RPassBox.Text)
            {
                //Generate IV
                byte[] iv = enc.GenerateRandomIV();
                sqlcmd.CommandText = "INSERT INTO master(iv) VALUES(@iv); ";
                sqlcmd.Parameters.AddWithValue("@iv", iv);
                sqlcmd.ExecuteNonQuery();

                PasswordObject P = new PasswordObject();
                
                //Create Key
                byte[] key = P.create_Key(PassBox.Text);
                SQLParser.insertKeyData(key);
               
                

                //Encrypt Master Password
                byte[] getKey = SQLParser.retrieveKey();
                byte[] mpass = enc.Encrypt(PassBox.Text, getKey, iv);
                SQLParser.insertMPData(mpass);

                MainWindow main = new MainWindow();
                this.Hide();
                main.ShowDialog();
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }
    }
}
