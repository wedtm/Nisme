using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Nisme
{
    public partial class Loading : Form
    {
        public int timer = 0;
        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }

        public void ShowModeless(ThreadStart tsDelegate)
        {
            // Setup thread.
            Thread thread = new Thread(tsDelegate);
            timer1.Start();
            // Start it.
            thread.Start();
           // Show this dialog in modeless state.
            this.Show();
            // Process events until thread has died.
            while (thread.IsAlive)
                Application.DoEvents();
            // Hide window.
            timer1.Stop();
            this.Hide();
            this.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string text = "Loading your library. Please wait";
            timer++;
            if (timer == 1)
                text += ".";
            if (timer == 2)
                text += "..";
            if (timer == 3)
                text += "...";
            if (timer == 4)
            {
                text += "....";
                timer = 0;
            }
            label1.Text = text;
            Thread.Sleep(300);
        }

    }
}
