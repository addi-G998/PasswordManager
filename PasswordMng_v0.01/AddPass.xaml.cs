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

namespace PasswordMng_v0._01
{

    public partial class AddPass : Window, IDisposable
    {

        private bool disposed = false;

        public AddPass()
        {
            InitializeComponent();
        }

        private string Password { get; set; }
        private string Username { get; set; }
        private string Website { get; set; }
       

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Password = Password_field.Text;
            Username = Username_field.Text;
            Website = Website_field.Text;

            this.DialogResult = true;
            this.Close();

        }

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public string GetWebsite()
        {
            return Website;
        }

        public string GetUsername()
        {
            return Username;
        }

        public string GetPassword()
        {
            return Password;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Freigabe von verwalteten Ressourcen
                    this.Close();
                }
                // Freigabe von nicht verwalteten Ressourcen
                disposed = true;
            }
        }

        ~AddPass()
        {
            Dispose(false);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
