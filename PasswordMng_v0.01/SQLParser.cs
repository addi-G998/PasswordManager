using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordMng_v0._01
{
    public class SQLParser
    {

        //SQLiteConnection sqlite_conn;
        static SQLParser()
        {
            // Set the DataDirectory path
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
        }

        public static SQLiteConnection CreateConnection()
        {


            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            string dbPath = "|DataDirectory|\\resources\\passVault.db";
            sqlite_conn = new SQLiteConnection($"Data Source={dbPath}; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
            return sqlite_conn;
        }

        public static SQLiteConnection CreateConnectionKey()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection: 
            string dbPath = "|DataDirectory|\\resources\\key_test.db";

            sqlite_conn = new SQLiteConnection($"Data Source={dbPath}; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
            return sqlite_conn;
        }

        public static void alterData(PasswordObject P)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "Select Id from passwrdData Where Id = @id";
            sqlite_cmd.Parameters.AddWithValue("@id", P.GetID());
            
            if (sqlite_cmd.ExecuteScalar() == null)
            {
                System.Windows.MessageBox.Show("No data found");
                conn.Close();
                return;
            }



            if (P != null)
            {
                Encryption enc = new Encryption();
                byte[] iv = enc.GenerateRandomIV();
                byte[] key = retrieveKey();
                //byte[] web = enc.Encrypt(P.Website, key, iv);
                byte[] usr = enc.Encrypt(P.Username, key, iv);
                byte[] pwrd = enc.Encrypt(P.Password, key, iv);
               
                sqlite_cmd.CommandText = "UPDATE passwrdData SET Website = @web, u_cypher = @usr, p_cypher = @pwrd, iv = @iv WHERE Id = @id";
                sqlite_cmd.Parameters.AddWithValue("@web", P.Website);
                sqlite_cmd.Parameters.AddWithValue("@usr", usr);
                sqlite_cmd.Parameters.AddWithValue("@pwrd", pwrd);
                sqlite_cmd.Parameters.AddWithValue("@iv", iv);
                try
                {
                    sqlite_cmd.ExecuteNonQuery();

                }
                catch(Exception ex)
                {
                   
                    System.Windows.MessageBox.Show(ex.Message);
                }finally
                {
                    conn.Close();
                }
            }
        }

        public static SQLiteConnection CreateMConnection()
        {


            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            string dbPath = "|DataDirectory|\\resources\\master.db";
            sqlite_conn = new SQLiteConnection($"Data Source={dbPath}; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);

            }
            return sqlite_conn;
        }

        public static void insertData(byte[] pwrd, byte[] usr, String web, byte[] iv)
        {


            SQLiteCommand sqlite_cmd;
            SQLiteConnection conn = CreateConnection();

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO passwrdData(Website, u_cypher, p_cypher, iv) VALUES(@web, @usr, @pwrd, @iv); ";
            sqlite_cmd.Parameters.AddWithValue("@web", web);
            sqlite_cmd.Parameters.AddWithValue("@usr", usr);
            sqlite_cmd.Parameters.AddWithValue("@pwrd", pwrd);
            sqlite_cmd.Parameters.AddWithValue("@iv", iv);

            sqlite_cmd.ExecuteNonQuery();

        }

        public static void insertKeyData(byte[] key)
        {
            SQLiteCommand sqlite_cmd;
            SQLiteConnection connKey = CreateConnectionKey();

            // Überprüfen, ob die Tabelle leer ist
            sqlite_cmd = connKey.CreateCommand();
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM key";
            long rowCount = (long)sqlite_cmd.ExecuteScalar();

            if (rowCount == 0)
            {
                // Tabelle ist leer, Einfügeabfrage ausführen
                sqlite_cmd.CommandText = "INSERT INTO key(keyvalue) VALUES(@key);";
                sqlite_cmd.Parameters.AddWithValue("@key", key);
                sqlite_cmd.ExecuteNonQuery();
            }
            else
            {
                // Tabelle ist nicht leer, keine Aktion erforderlich
                Console.WriteLine("Die Tabelle 'key' ist nicht leer. Einfügen abgebrochen.");
            }
        }

        public static void insertMPData(byte[] mp)
        {
            SQLiteCommand sqlite_cmd;
            SQLiteConnection connKey = CreateMConnection();

            try
            {
                // Überprüfen, ob die Tabelle leer ist
                sqlite_cmd = connKey.CreateCommand();
                sqlite_cmd.CommandText = "SELECT COUNT(*) FROM master";
                long rowCount = (long)sqlite_cmd.ExecuteScalar();

                if (rowCount == 0)
                {
                    // Tabelle ist leer, Einfügeabfrage ausführen
                    sqlite_cmd.CommandText = "INSERT INTO master(m_pw) VALUES(@mp);";
                    sqlite_cmd.Parameters.AddWithValue("@mp", mp);
                    sqlite_cmd.ExecuteNonQuery();
                    Console.WriteLine("Data inserted successfully into the master table.");
                }
                else
                {
                    // Tabelle ist nicht leer, Aktualisierungsabfrage ausführen
                    sqlite_cmd.CommandText = "UPDATE master SET m_pw = @mp WHERE rowid = 1;";
                    sqlite_cmd.Parameters.AddWithValue("@mp", mp);
                    sqlite_cmd.ExecuteNonQuery();
                    Console.WriteLine("Data updated successfully in the master table.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connKey.Close();
            }
        }

        public static byte[] retrieveKey()
        {
            SQLiteConnection connKey = CreateConnectionKey(); // Key connection
            SQLiteCommand sQLiteCommand = connKey.CreateCommand();
            sQLiteCommand.CommandText = "SELECT * FROM key";
            SQLiteDataReader sqliteDataReader = sQLiteCommand.ExecuteReader();
            if (!sqliteDataReader.Read())
            {
                throw new InvalidOperationException("No key data found.");
            }
            byte[] keyBlob = GetKeyBlobData(sqliteDataReader, "keyvalue");
            return keyBlob;
        }

        private static byte[] GetBlobData(SQLiteDataReader reader, string columnName)
        {
            long length = reader.GetBytes(reader.GetOrdinal(columnName), 0, null, 0, 0); // Get the length of the data
            byte[] blobData = new byte[length];
            reader.GetBytes(reader.GetOrdinal(columnName), 0, blobData, 0, (int)length);
            return blobData;
        }

        private static byte[] GetKeyBlobData(SQLiteDataReader reader, string columnName)
        {

            if (!reader.HasRows)
            {
                throw new InvalidOperationException("No current row");
            }
            long length = reader.GetBytes(reader.GetOrdinal(columnName), 0, null, 0, 0); // Get the length of the data
            byte[] blobData = new byte[length];
            reader.GetBytes(reader.GetOrdinal(columnName), 0, blobData, 0, (int)length);
            return blobData;
        }

        private SQLiteDataReader sQLiteDataReader()
        {

            SQLiteConnection conn = SQLParser.CreateMConnection();
            SQLiteCommand sqlcmd = conn.CreateCommand();
            sqlcmd.CommandText = "SELECT * FROM master";
            SQLiteDataReader sqlite_datareader = sqlcmd.ExecuteReader();
            return sqlite_datareader;
        }

        public String getMasterPass()
        {
            Encryption enc = new Encryption();
            SQLiteDataReader reader = sQLiteDataReader();
            reader.Read();

            byte[] mpass = GetBlobData(reader, "m_pw");
            byte[] iv = GetBlobData(reader, "iv");

            String pass = enc.Decrypt(mpass, retrieveKey(), iv);

            return pass;


        }

        public static List<PasswordObject> getData()
        {
            Encryption encrypt = new Encryption();
            
            // Get the key data
           byte[] keyBlob = retrieveKey();

            // Get the password data
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM passwrdData";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            List<PasswordObject> data = new List<PasswordObject>();

            while (sqlite_datareader.Read())
            {
                byte[] passwordBlob = GetBlobData(sqlite_datareader, "p_cypher");
                byte[] usernameBlob = GetBlobData(sqlite_datareader, "u_cypher");
                byte[] ivBlob = GetBlobData(sqlite_datareader, "iv");
                string website = sqlite_datareader.GetString(1);
                int id = sqlite_datareader.GetInt32(0);


                data.Add(new PasswordObject(passwordBlob, usernameBlob,ivBlob,keyBlob, website, id));

            }
            return data;
        }

        public static void deleteData(int id)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM passwrdData WHERE Id = @id";
            sqlite_cmd.Parameters.AddWithValue("@id", id);
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}
