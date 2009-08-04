using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Lala.API;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nisme
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private String UsersPassword = String.Empty;
        public Login()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(Window1_SourceInitialized);
            this.listBox1.DataContext = this;
            Users = new ObservableCollection<User>();
            string filename = "USERS.NDB";
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Object us = bf.Deserialize(fs);
                Users = (ObservableCollection<User>)us;
                fs.Close();
            }
        }

        public ObservableCollection<User> Users
        { get; set; }

        void Window1_SourceInitialized(object sender, EventArgs e)
        {
            //GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        }

        private void AttemptLogin(string email, bool newUser)
        {
            User currentUser = (User)listBox1.SelectedItem;
            if (!Lala.API.HTTPRequests.GetLoginCookie(email, UsersPassword))
            {
               // passwordBox1.Password = String.Empty;
                MessageBox.Show("Incorrect Login Credentials.");
            }
            else
            {
                if (newUser)
                {
                    Users.Add(new User(email, "http://user-images.lala.com/servlet/UserImageServlet?userToken=" + Lala.API.Constants.UserID));
                }
                FileStream fs = new FileStream("USERS.NDB", FileMode.OpenOrCreate, FileAccess.Write);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, Users);
                }
                finally
                {
                    fs.Close();
                }
                this.Hide();
                MainWindow main = new MainWindow();
                main.Show();
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = (PasswordBox)sender;
            UsersPassword = box.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User currentUser = (User)listBox1.SelectedItem;
            string eml = currentUser.EmailAddress;
            AttemptLogin(eml, false);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UsersPassword = NewUserPassword.Password;            
            AttemptLogin(Email.Text, true);
        }
    }

    [Serializable]
    public class User
    {
        public String Username { get; set; }
        public DateTime LastLoginDate { get; set; }
        public String ProfileImage { get; set; }
        public String EmailAddress { get; set; }
        public User(string UserName, string ProfileImage)
        {
            this.Username = UserName;
            this.ProfileImage = ProfileImage;
            this.LastLoginDate = DateTime.Now;
            this.EmailAddress = "miles@vimae.com";
        }
    }
}
