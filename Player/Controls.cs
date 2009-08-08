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
        public class Controls
        {
            public Controls() { }
            public void Next()
            {
                Song next = (Song)Lala.API.Instance.CurrentUser.Queue[0];
                Queue.RemoveSongFromQueue(0);
                Play(next);
            }
            public void Previous()
            {
            }
            public void Stop()
            {
                Bass.BASS_ChannelStop(Instance.Channel);
            }
            public void Pause()
            {
                Bass.BASS_ChannelPause(Instance.Channel);
            }
            public void Resume()
            {
                Bass.BASS_ChannelPlay(Instance.Channel, false);
            }
            public void Play(Lala.API.Song song)
            {
                Stop();
                Lala.API.Instance.CurrentUser.Library.Playing.CurrentSong = song;
                Player.Internals.PlaySong(song.PlayLink);
            }

            public void Update()
            {
                if (Bass.BASS_ChannelIsActive(Instance.Channel) != BASSActive.BASS_ACTIVE_STOPPED)
                {
                    Instance.CurrentSong.TotalLength = Bass.BASS_StreamGetFilePosition(Instance.Channel, BASSStreamFilePosition.BASS_FILEPOS_END);
                    Instance.CurrentSong.CurrentPosition = Bass.BASS_StreamGetFilePosition(Instance.Channel, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD);
                    Instance.CurrentSong.CurrentPositionInSeconds = Bass.BASS_ChannelBytes2Seconds(Instance.Channel, Bass.BASS_ChannelGetPosition(Instance.Channel));
                    Instance.CurrentSong.TotalLengthInSeconds = Bass.BASS_ChannelBytes2Seconds(Instance.Channel, Bass.BASS_ChannelGetLength(Instance.Channel));
                }
                else
                {
                    if (Lala.API.Instance.CurrentUser.Queue.Count > 0)
                        Controls.Next();
                }
            }

            public class Queue
            {
                public void RemoveSongFromQueue(int position)
                {
                    Player.Queue.RemoveSongFromQueue(position);
                }

                public void AddSongToQueue(Song sng)
                {
                    Player.Queue.AddSongToQueue(sng);
                }
            }
        }
    }
}