using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordMng_v0._01
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            SQLiteCommand sqlite_cmd;
            SQLiteConnection connKey = SQLParser.CreateConnectionKey();

            // Überprüfen, ob die Tabelle leer ist
            sqlite_cmd = connKey.CreateCommand();
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM key";
            long rowCount = (long)sqlite_cmd.ExecuteScalar();
            if(rowCount == 0)
            {
                Create_MPass create_MPass = new Create_MPass();
                create_MPass.ShowDialog();
            }
            else
            {
                Authorize auth = new Authorize();
                auth.ShowDialog();
            }
        }
    }
}
