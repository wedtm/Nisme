using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimae
{
    public partial class Player
    {
        public class Events
        {
            public event EventHandler<PlayEventArgs> Play;
            public event EventHandler<StopEventArgs> Stop;
            public event EventHandler<ResumeEventArgs> Resume;

            public virtual void OnPlay(PlayEventArgs e)
            {
                if (this.Play != null)
                {
                    this.Play(this, e);
                }
            }

            public virtual void OnStop(StopEventArgs e)
            {
                if (this.Stop != null)
                {
                    this.Stop(this, e);
                }
            }

            public virtual void OnResume(ResumeEventArgs e)
            {
                if (this.Resume != null)
                {
                    this.Resume(this, e);
                }
            }




            public class PlayEventArgs : EventArgs
                {
                }

            public class StopEventArgs : EventArgs
                {
                }

            public class ResumeEventArgs : EventArgs
                {
                }
        }
    }
}