using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lala.API;
using Un4seen.Bass;

namespace Vimae
{
    public partial class Player
    {
        public void Next()
        {
            Song next = (Song)Lala.API.Instance.CurrentUser.Queue[0];
            RemoveSongFromQueue(0);
            Play(next);
        }
        public void Previous()
        {
        }
        public void Stop()
        {
            Bass.BASS_ChannelStop(Channel);
        }
        public void Pause()
        {
            Bass.BASS_ChannelPause(Channel);
        }
        public void Resume()
        {
            Bass.BASS_ChannelPlay(Channel, false);
        }
        public void Play(Lala.API.Song song)
        {
            Stop();
            Lala.API.Instance.CurrentUser.Library.Playing.CurrentSong = song;
            PlaySong(song.PlayLink);
        }

        public CurrentSong Song = new CurrentSong();

        public void Update()
        {
            if (Bass.BASS_ChannelIsActive(Channel) != BASSActive.BASS_ACTIVE_STOPPED)
            {
                Song.TotalLength = Bass.BASS_StreamGetFilePosition(Channel, BASSStreamFilePosition.BASS_FILEPOS_END);
                Song.CurrentPosition = Bass.BASS_StreamGetFilePosition(Channel, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD);
                Song.CurrentPositionInSeconds = Bass.BASS_ChannelBytes2Seconds(Channel, Bass.BASS_ChannelGetPosition(Channel));
                Song.TotalLengthInSeconds = Bass.BASS_ChannelBytes2Seconds(Channel, Bass.BASS_ChannelGetLength(Channel));
            }
            else
            {
                if (Lala.API.Instance.CurrentUser.Queue.Count > 0)
                    Next();
            }
        }
    }
}