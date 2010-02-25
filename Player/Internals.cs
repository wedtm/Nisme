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
using Un4seen.Bass.AddOn.Mix;
using System.Threading;

namespace Vimae
{
    public partial class Player
    {
            private static Stream _fs;
            private static long _fsLength;
            private static BASS_FILEPROCS _myStreamCreateUser;
            private static System.Timers.Timer timer;
            public Song NowPlaying = null;


            internal void PlaySong(string PlayURL)
            {
                if(this.Channel != 0)
                    Bass.BASS_ChannelStop(this.Channel);
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
                this.Channel = Bass.BASS_StreamCreateFileUser(BASSStreamSystem.STREAMFILE_BUFFER, BASSFlag.BASS_STREAM_AUTOFREE, _myStreamCreateUser, IntPtr.Zero);
                if (this.Channel == 0)
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
                 Bass.BASS_ChannelPlay(this.Channel, false);
               this.Played(this, new EventArgs());
            }

            private void MyFileProcUserClose(IntPtr user)
            {
                return;
                if (_fs == null)
                    return;
                _fs.Close();
                Console.WriteLine("File Closed, starting Timer.");
                timer = new System.Timers.Timer();
                timer.Interval = 500;
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();

            }

            void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (Bass.BASS_ChannelIsActive(Channel) != BASSActive.BASS_ACTIVE_STOPPED)
                    return;
                Bass.BASS_ChannelStop(this.Channel);
                this.Stopped(this, new EventArgs());
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
        }
    }