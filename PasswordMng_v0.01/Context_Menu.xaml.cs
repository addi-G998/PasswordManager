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
    /// <summary>
    /// Interaktionslogik für Context_Menu.xaml
    /// </summary>
    public partial class Context_Menu : Window, IDisposable
    {
        private bool disposed = false;
        private Border dataPanel;
        PasswordObject P;
        
        public Context_Menu(Border border, PasswordObject P)
        {
            InitializeComponent();
            dataPanel = border;
            this.P = P;
            
        }

        private void delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(dataPanel != null)
            {

                // Entferne das Panel aus dem übergeordneten Container
                var parent = dataPanel.Parent as Panel;
                if (parent != null)
                {
                    SQLParser.deleteData(P.GetID());
                    parent.Children.Remove(dataPanel);
                    P.GetData().Remove(P);

                    

                }

            }
            this.Close();
        }

        private void edit_Btn_Click(object sender, RoutedEventArgs e)
        {
            Editor editor = new Editor(dataPanel, P);
            editor.ShowDialog();
            this.Close();
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

        ~Context_Menu()
        {
            Dispose(false);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
