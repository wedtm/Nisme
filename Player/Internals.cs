using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net;
using Un4seen.Bass;
using Lala.API;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;

namespace Vimae
{
    public partial class Player
    {
        internal class Internals
        {
            private static Stream _fs;
            private static long _fsLength;
            private static BASS_FILEPROCS _myStreamCreateUser;
            private static Timer timer;

            public void Init()
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
            }

            internal void PlayNext()
            {
                
                Song selected = (Song)Lala.API.Instance.CurrentUser.Queue[0];
                RemoveSongFromQueue(0);
                Lala.API.Instance.CurrentUser.Library.Playing.CurrentSong = selected;
                PlaySong(selected.PlayLink);
            }

            internal void PlaySong(string PlayURL)
            {
                Bass.BASS_ChannelStop(Instance.Channel);
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
                Instance.Channel = Bass.BASS_StreamCreateFileUser(BASSStreamSystem.STREAMFILE_BUFFER, BASSFlag.BASS_STREAM_AUTOFREE, _myStreamCreateUser, IntPtr.Zero);
                Bass.BASS_ChannelPlay(Instance.Channel, false);
            }

            private void MyFileProcUserClose(IntPtr user)
            {
                if (_fs == null)
                    return;
                _fs.Close();
                Console.WriteLine("File Closed, starting Timer.");
                timer = new Timer();
                timer.Interval = 500;
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();

            }

            void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (Bass.BASS_ChannelIsActive(Instance.Channel) != BASSActive.BASS_ACTIVE_STOPPED)
                    return;
                Events.OnStop(new Events.StopEventArgs());
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

            private void RemoveSongFromQueue(int p)
            {
                throw new NotImplementedException();
            }
        }
    }
}