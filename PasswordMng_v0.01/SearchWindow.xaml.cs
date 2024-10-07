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
    /// Interaktionslogik für SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        MainWindow main = new MainWindow();
        private List<PasswordObject> passwordList = new List<PasswordObject>();
        private List<PasswordObject> results = new List<PasswordObject>();
        public SearchWindow(List<PasswordObject> List)
        {
            
            InitializeComponent();
            this.passwordList = List;
            this.main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search_Panel.Children.Clear();
            results.Clear();
            foreach (PasswordObject P in passwordList)
            {
                if (P.Website.ToUpper().Contains(SearchBox.Text.ToUpper()))
                {
                    if (SearchBox.Text == "")
                    {
                        break;
                    }
                    if (!results.Any(resultObject => resultObject.GetID() == P.GetID()))
                    {
                        results.Add(P);
                        if (results.Count != 0)
                        {
                            main.CreatePanel(P, Search_Panel);
                        }
                    }
                }
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
