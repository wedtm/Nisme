using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lala.API;

namespace Vimae
{
    public partial class Player
    {
        interface IControls
        {
            void Next();
            void Previous();
            void Stop();
            void Pause();
            void Play();
            void Play(Lala.API.Song song);

        }
    }
}