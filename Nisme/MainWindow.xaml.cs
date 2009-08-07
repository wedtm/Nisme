using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;
using System.Net;
using Un4seen.Bass;
using System.IO;
using System.Runtime.InteropServices;
using Lala.API;
using Vimae;

namespace Nisme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer progressTimer;
        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(MainWindow_SourceInitialized);
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            //GlassHelper.ExtendGlassFrame(this, new Thickness(0, 30, 0, 0));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BassNet.Registration("miles@vimae.com", "2X22292815172922");
            nowPlaying.Artist.Text = String.Empty;
            nowPlaying.Song.Text = String.Empty;
            Player p = new Player();
            p.
            Player.Events.Play += new EventHandler<Player.Events.PlayEventArgs>(Events_Play);
            Player.Events.Stop += new EventHandler<Player.Events.StopEventArgs>(Events_Stop);
            nowPlaying.parent = this;
            menuBar.parent = this;
            menuBar.NickName.Text = Lala.API.Instance.CurrentUser.Username;
            LoadLibrary();
            progressTimer = new DispatcherTimer();
            progressTimer.Interval = new TimeSpan(1000); // This equals to 1 second, some tweaking may be necessary. //- WedTM
            progressTimer.Tick += new EventHandler(progressTimer_Tick);
            progressTimer.Start();
        }

        void Events_Stop(object sender, Player.Events.StopEventArgs e)
        {
            nowPlaying.Visibility = Visibility.Collapsed;
        }

        void Events_Play(object sender, Player.Events.PlayEventArgs e)
        {
            nowPlaying.Visibility = Visibility.Visible;
            throw new NotImplementedException();
        }


        void progressTimer_Tick(object sender, EventArgs e)
        {
            Player.Controls.Update();
        }

        private void LoadLibrary()
        {
            Lala.API.Functions.LoadUser(false); // Loads the current user and populates Lala.API.Instance.CurrentUser with their data.
            Lala.API.Instance.CurrentUser.Library.Playing = Lala.API.Instance.CurrentUser.Library.Playlists[0];
            //dataGrid1.DataContext = UserLibrary.Playing.Songs;
            dataGrid1.ItemsSource = from song in Lala.API.Instance.CurrentUser.Library.Playing.Songs select song;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, null);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Lala.API.Song> newList = Lala.API.Instance.CurrentUser.Library.Playing.Songs.FindAll(LikeTrack);
            dataGrid1.ItemsSource = newList;
        }

        private bool LikeTrack(Song s)
        {
            if (s.Title.ToLower().Contains(SearchBox.Text.ToLower()) || s.Artist.ToLower().Contains(SearchBox.Text.ToLower()) || s.DiscTitle.ToLower().Contains(SearchBox.Text.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            progressTimer.IsEnabled = true;
            Song selected = (Song)dataGrid1.SelectedItem;
            Player.Controls.Play(selected);
            nowPlaying.Artist.Text = selected.Artist;
            nowPlaying.Song.Text = selected.Title;
            nowPlaying.AlbumImage.Source = new BitmapImage(new Uri(selected.AlbumImage));
        }


        public void PlayNext()
        {
            Song selected = (Song)Lala.API.Instance.CurrentUser.Queue[0];

            nowPlaying.Artist.Text = selected.Artist;
            nowPlaying.Song.Text = selected.Title;
            nowPlaying.AlbumImage.Source = new BitmapImage(new Uri(selected.AlbumImage));
        }


        private void dataGrid1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Add the currently selected track to the Q and update the nowPlaying bar to reflect the number of items in the Q.
            if (e.Key == Key.Q)
            {
                Song sng = (Song)dataGrid1.SelectedItem;
                Player.Controls.Queue.AddSongToQueue(sng);
            }
        }
    }
}
