using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace PasswordMng_v0._01
{
    public class PasswordObject : INotifyPropertyChanged
    {
        private string website;
        public string Website 
        { 
            get 
            {
                return website;
            }
            set 
            {
                website = value;
                OnPropertyChanged("Website");
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set 
            {
                username = value;
                OnPropertyChanged("Username");
            }   
        }

        private string password;
        public string Password 
        {
            get
            {
                return password;
            }
            set 
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

       // private int id;
        private int ID { get; set; }
        public string MaskedPassword
        {
            get
            {
                return new string('*', password.Length);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //List for temporary storage of PasswordObjects
        private static List<PasswordObject> data = new List<PasswordObject>();

        public PasswordObject()
        {
            
        }
        public PasswordObject(string website, string username, string password)
        {
            Website = website;
            Username = username;
            Password = password;
            data.Add(this);
        }

        public PasswordObject(byte[] password, byte[] username,byte[] iv, byte[]key, string website, int id)
        {
            Encryption enc = new Encryption();
            this.Password = enc.Decrypt(password, key,iv);
            this.Username = enc.Decrypt(username, key, iv);
            this.Website = website;
            this.ID = id;

        }
        public byte[] create_Key(String masterpass)
        {
            Encryption enc = new Encryption();
            byte[] salt = enc.GenerateRandomSalt();
            byte[] key = enc.GenerateKeyFromPassword(masterpass, salt, 1000, 16);

            return key;
        }

        public int GetID()
        {
            return ID;
        }
        
        public List<PasswordObject> GetData()
        {
            return data;
        }

        public void SetList(List<PasswordObject> list)
        {
            data = list;
        }

        public void SetUser(string username)
        {
            Username = username;
        }

        public void SetPass(string password) {
            Password = password;
        }

        public void SetWeb(string website)
        {
            Website = website;
        }
    }
}
