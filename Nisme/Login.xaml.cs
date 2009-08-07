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
        }

        void Window1_SourceInitialized(object sender, EventArgs e)
        {
            //GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        }

        private void AttemptLogin(string email, bool newUser)
        {
            if (!Lala.API.HTTPRequests.GetLoginCookie(email, UsersPassword))
            {
               // passwordBox1.Password = String.Empty;
                MessageBox.Show("Incorrect Login Credentials.");
            }
            else
            {
                Lala.API.Instance.CurrentUser = new Lala.API.User();
                JObject jo = Lala.API.HTTPRequests.GetUserInfo();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UsersPassword = NewUserPassword.Password;            
            AttemptLogin(Email.Text, true);
        }
    }
}
