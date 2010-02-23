using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lala.API;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Vimae
{
    public partial class Player
    {
        public void Next()
        {
            Song next = (Song)Lala.API.Instance.CurrentUser.Queue[0];
            NowPlaying = next;
            RemoveSongFromQueue(0);
            Play(next);
        }
        public void Previous()
        {
        }
        public void Stop()
        {
            try
            {
                BassMix.BASS_Mixer_ChannelRemove(this.Channel);
            }
            catch (DllNotFoundException)
            {
                Functions.DownloadDlls();
            }
        }
        public void Pause()
        {
            BassMix.BASS_Mixer_ChannelPause(this.Channel);
        }
        public void Resume()
        {
            BassMix.BASS_Mixer_ChannelPlay(this.Channel);
        }
        public void Play(Lala.API.Song song)
        {
            Stop();
            Lala.API.Instance.CurrentUser.Library.Playing.CurrentSong = song;
            NowPlaying = song;
            PlaySong(song.PlayLink);
        }

        public CurrentSong Song = new CurrentSong();

        public void Update()
        {
            if (Bass.BASS_ChannelIsActive(this.Channel) != BASSActive.BASS_ACTIVE_STOPPED)
            {
                Song.TotalLength = Bass.BASS_StreamGetFilePosition(Channel, BASSStreamFilePosition.BASS_FILEPOS_END);
                Song.CurrentPosition = Bass.BASS_StreamGetFilePosition(Channel, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD);
                Song.CurrentPositionInSeconds = Bass.BASS_ChannelBytes2Seconds(Channel, Bass.BASS_ChannelGetPosition(Channel));
                Song.TotalLengthInSeconds = Bass.BASS_ChannelBytes2Seconds(Channel, Bass.BASS_ChannelGetLength(Channel));
            }
        }
    }
}