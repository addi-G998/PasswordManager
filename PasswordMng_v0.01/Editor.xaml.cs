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
using System.Windows.Shapes;

namespace PasswordMng_v0._01
{
    /// <summary>
    /// Interaktionslogik für Editor.xaml
    /// </summary>
    public partial class Editor : Window, IDisposable
    {

        private bool disposed = false;
        private Border dataPanel;
        PasswordObject P;

        public Editor(Border border, PasswordObject P)
        {
            InitializeComponent();
            dataPanel = border;
            this.P = P;
            idBox.Text = P.Website;
            userBox.Text = P.Username;
            passBox.Text = P.Password;
        }

        private void confirmBtn_Click(object sender, MouseButtonEventArgs e)
        {
            P.SetUser(userBox.Text);
            P.SetPass(passBox.Text);
            P.SetWeb(idBox.Text);
            SQLParser.alterData(P);
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void changeData(PasswordObject P) 
        {

            if (P != null)
            {
                
            }
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

        ~Editor()
        {
            Dispose(false);
        }



    }
}

