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

namespace Nisme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Lala.API.Library UserLibrary;
        Lala.API.User User;
        Thread PlayerThread;
        int MainChannel;
        Song CurrentTrack;
        private static Stream _fs;
        private static long _fsLength;
        private BASS_FILEPROCS _myStreamCreateUser;
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
            nowPlaying.parent = this;
            menuBar.parent = this;
            LoadLibrary();
            progressTimer = new DispatcherTimer();
            progressTimer.Interval = new TimeSpan(1000); // This equals to 1 second, some tweaking may be necessary. //- WedTM
            progressTimer.Tick += new EventHandler(progressTimer_Tick);
            progressTimer.Start();
        }


        void progressOfTrack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((ProgressBar)sender);
            SeekToPoint(1000);
        }

        void progressTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                BASS_INFO info = Bass.BASS_GetInfo();
            }
            catch (DllNotFoundException)
            {
                MessageBoxResult res = MessageBox.Show("Could not find the appropriate bass.dll file within Nisme's directory. Would you like to download the appropriate version now?", "Missing DLL's", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    string url = String.Empty;
                    if (IntPtr.Size == 4)
                        url = "http://nisme.googlecode.com/files/bass.x32";
                    else
                        url = "http://nisme.googlecode.com/files/bass.x64";

                    WebClient Client = new WebClient();
                    Client.DownloadFile(url, "bass.dll");
                    MessageBox.Show("File has been successfully downloaded.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please either re-run and allow Nisme to download bass.dll, or manually download it to Nisme's directory. The application will now exit.", "Exiting", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    return;
                }
            }
            if (Bass.BASS_ChannelIsActive(MainChannel) != BASSActive.BASS_ACTIVE_STOPPED)
            {
                progressTimer.IsEnabled = true; // Adding this in in case the else statement is hit before BASS actually starts playing, thusly causing the timer to be disabled //- WedTM
                if (nowPlaying.Visibility == Visibility.Collapsed)
                    nowPlaying.Visibility = Visibility.Visible;
                double max = Bass.BASS_StreamGetFilePosition(MainChannel, BASSStreamFilePosition.BASS_FILEPOS_END);
                double current = Bass.BASS_StreamGetFilePosition(MainChannel, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD);
                double currentTime = Bass.BASS_ChannelBytes2Seconds(MainChannel, Bass.BASS_ChannelGetPosition(MainChannel));
                double maxTime = Bass.BASS_ChannelBytes2Seconds(MainChannel, Bass.BASS_ChannelGetLength(MainChannel));
                double percentageOfTrack = currentTime / maxTime;
                double trackBarLength = nowPlaying.trackBarBg.ActualWidth;
                nowPlaying.playedAmount.Width = percentageOfTrack * trackBarLength;
                nowPlaying.TotalTime.Text = Lala.API.Functions.SecondsToTime(maxTime, false);
                nowPlaying.CurrentTime.Text = Lala.API.Functions.SecondsToTime(currentTime, false);
                double percentageOfDownload = current / max;
                nowPlaying.loadedAmount.Width = percentageOfDownload * trackBarLength;
            }
            else
            {
                if (nowPlaying.Visibility == Visibility.Visible)
                    nowPlaying.Visibility = Visibility.Collapsed;
                if (Lala.API.Instance.CurrentUser.Queue.Count > 0)
                {
                    PlayNext();
                }
                else
                    progressTimer.IsEnabled = false; // Doesn't appear to be needed, this one is for the release team! //- WedTM

            }
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

        protected void SeekToPoint(long point)
        {
            //
        }

        protected void PlaySong(string PlayURL)
        {
            progressTimer.IsEnabled = true;
            Bass.BASS_ChannelStop(MainChannel);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PlayURL);
            req.Headers.Add("Cookie:" + Lala.API.Instance.Cookie);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                _fs = resp.GetResponseStream();
                _fsLength = resp.ContentLength;
                _myStreamCreateUser = new BASS_FILEPROCS(
                            new FILECLOSEPROC(MyFileProcUserClose),
                            new FILELENPROC(MyFileProcUserLength),
                            new FILEREADPROC(MyFileProcUserRead),
                            new FILESEEKPROC(MyFileProcUserSeek));

                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                MainChannel = Bass.BASS_StreamCreateFileUser(BASSStreamSystem.STREAMFILE_BUFFER, BASSFlag.BASS_STREAM_AUTOFREE, _myStreamCreateUser, IntPtr.Zero);
                Bass.BASS_ChannelPlay(MainChannel, false);
        }

        private void MyFileProcUserClose(IntPtr user)
        {
            if (_fs == null)
                return;
            _fs.Close();
            Console.WriteLine("File Closed");
        }

        private long MyFileProcUserLength(IntPtr user)
        {
            if (_fs == null)
                return 0L;
            return _fsLength;
        }

        private int MyFileProcUserRead(IntPtr buffer, int length, IntPtr user)
        {
            if (_fs == null)
                return 0;
            try
            {
                // at first we need to create a byte[] with the size of the requested length
                byte[] data = new byte[length];
                // read the file into data
                int bytesread = _fs.Read(data, 0, length);
                // and now we need to copy the data to the buffer
                // we write as many bytes as we read via the file operation
                Marshal.Copy(data, 0, buffer, bytesread);
                return bytesread;
            }
            catch { return 0; }
        }

        private bool MyFileProcUserSeek(long offset, IntPtr user)
        {
            if (_fs == null)
                return false;
            try
            {
                long pos = _fs.Seek(offset, SeekOrigin.Begin);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User.Queue.Clear();
            Song selected = (Song)dataGrid1.SelectedItem;
            int StartIndex = dataGrid1.Items.IndexOf(selected);
            User.Queue.Add(selected);
            for (int i = 1; i < 50; i++)
            {
                if((i + StartIndex) <= (dataGrid1.Items.Count - StartIndex) - 1)
                User.Queue.Add((Song)dataGrid1.Items[i + StartIndex]);
            }
            PlayNext();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, null);
        }

        private void menu1_MouseEnter(object sender, MouseEventArgs e)
        {
            //Menu m = (Menu)sender;
           // m.Visibility = Visibility.Visible;
        }

        private void menu1_MouseLeave(object sender, MouseEventArgs e)
        {
           // Menu m = (Menu)sender;
           // m.Visibility = Visibility.Hidden;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Lala.API.Song> newList = UserLibrary.Playing.Songs.FindAll(LikeTrack);
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
            UserLibrary.Playing.CurrentSong = selected;
            PlaySong(selected.PlayLink);
            nowPlaying.Artist.Text = selected.Artist;
            nowPlaying.Song.Text = selected.Title;
            nowPlaying.AlbumImage.Source = new BitmapImage(new Uri(selected.AlbumImage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, null);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

            }
            this.DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void dataGrid1_Sorting(object sender, Microsoft.Windows.Controls.DataGridSortingEventArgs e)
        {
            
        }

        public void PlayNext()
        {
            Song selected = (Song)Lala.API.Instance.CurrentUser.Queue[0];
            RemoveSongFromQueue(0);
            Lala.API.Instance.CurrentUser.Library.Playing.CurrentSong = selected;
            PlaySong(selected.PlayLink);
            nowPlaying.Artist.Text = selected.Artist;
            nowPlaying.Song.Text = selected.Title;
            SetLCDScreen(selected.Title, "by " + selected.Artist);
            nowPlaying.AlbumImage.Source = new BitmapImage(new Uri(selected.AlbumImage));
        }

        public void SetLCDScreen(string Row1, string Row2)
        {
            if (Row1.Length > 20)
                Row1 = Row1.Substring(0, 17) + "...";
            if (Row2.Length > 20)
                Row2 = Row2.Substring(0, 17) + "...";
        }

        private void dataGrid1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Add the currently selected track to the Q and update the nowPlaying bar to reflect the number of items in the Q.
            if (e.Key == Key.Q)
            {
                Song sng = (Song)dataGrid1.SelectedItem;
                AddSongToQueue(sng);
            }
        }

        public void AddSongToQueue(Song sng)
        {
            Lala.API.Instance.CurrentUser.Queue.Add(sng);
            nowPlaying.QueueLength.Text = Lala.API.Instance.CurrentUser.Queue.Count.ToString();
                if (Bass.BASS_ChannelIsActive(MainChannel) == BASSActive.BASS_ACTIVE_STOPPED)
                    PlayNext();
        }

        public void RemoveSongFromQueue(int Index)
        {
            Lala.API.Instance.CurrentUser.Queue.RemoveAt(0);
            nowPlaying.QueueLength.Text = Lala.API.Instance.CurrentUser.Queue.Count.ToString();
        }
    }
}
