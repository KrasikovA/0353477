using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Text;
namespace _0353477
{
    public partial class Form1 : Form
    {
        public static byte[][] TUNES = {Properties.Resources._01, Properties.Resources._02,
            Properties.Resources._03, Properties.Resources._04,
            Properties.Resources._05_s, Properties.Resources._06};
        public Label[] tracks = new Label[6];
        public static int CURRENT_TUNE = 0;
        public static int SECONDS = 0;
        public static int MINUTES = 0;
        public static bool PLAY_DELTA = false;
        public Label secLabel;
        public Label minLabel;
        public PrivateFontCollection pahanFont = new PrivateFontCollection();
        public OnTuneEndsDelegate WHEN_TUNE_ENDS;
        public bool isPlaying = true;
        public int songPing = 0;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            pahanFont.AddFontFile("BPtypewrite.ttf");
            pahanFont.AddFontFile("BPtypewriteDamagedSlashed.ttf");
            secLabel = this.label3;
            minLabel = this.label1;
            tracks[0] = label12;
            tracks[1] = label13;
            tracks[2] = label14;
            tracks[3] = label15;
            tracks[4] = label16;
            tracks[5] = label17;
            foreach (Control c in this.Controls)
            {
                c.BackColor = Color.Transparent;
                c.Font = new Font(pahanFont.Families[0], 9, FontStyle.Regular);
                c.ForeColor = Color.White;

            }
            WHEN_TUNE_ENDS = new OnTuneEndsDelegate(onTuneEnds);
            playTune(); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PahanPlayer.BASSMOD_Free();
        }

        public void crossCurrent()
        {
           foreach (Label l in tracks)
            {
                l.Font = new Font(pahanFont.Families[0], 9, FontStyle.Regular);
            }
           tracks[CURRENT_TUNE].Font = new Font(pahanFont.Families[1], 9, FontStyle.Regular);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }
        private void playTune()
         {
            PahanPlayer.BASSMOD_Free();
            PahanPlayer.BASSMOD_Init(-1, 11025, BASSMOD_BASSInint.BASS_DEVICE_DEFAULT);
            PahanPlayer.BASSMOD_MusicLoad(true, TUNES[CURRENT_TUNE], 0, 0, 
                BASSMOD_BASSMusic.BASS_MUSIC_RAMP | BASSMOD_BASSMusic.BASS_MUSIC_NONINTER |
                BASSMOD_BASSMusic.BASS_MUSIC_CALCLEN | BASSMOD_BASSMusic.BASS_MUSIC_FT2MOD);
            PahanPlayer.BASSMOD_MusicSetSync(BASSMOD_BASSMusic.BASS_SYNC_END, 0,WHEN_TUNE_ENDS, 0);
            if (isPlaying)
            {
                PahanPlayer.BASSMOD_MusicPlay();
            }
            crossCurrent();
        }
        private void onTuneEnds(int handle, int data, int user)
        {
            Action action = () =>
            {
                if (CURRENT_TUNE < 5)
                {
                    CURRENT_TUNE++;
                }
                else
                {
                    CURRENT_TUNE = 0;
                }
                cleanTimer();
                playTune();
            };
            if (PLAY_DELTA)
                return;
            if (this.InvokeRequired)
            {
                PLAY_DELTA = true;
                this.Invoke(action);
            };    
        }

        private void cleanTimer()
        {
            minLabel.Text = "00";
            secLabel.Text = "00";
            SECONDS = 0;
            MINUTES = 0;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string localSec = "";
            string localMin = "";
            SECONDS++;
            //отсчитываем тут 50 секунд чтобы разрешить песне закончиться
            if (SECONDS > 50)
            {
                PLAY_DELTA = false;
            }
            if (SECONDS == 60)
            {
                SECONDS = 0;
                MINUTES++;
            }
            if (SECONDS < 10)
            {
                localSec = "0" + SECONDS.ToString();
            }
            else
            {
                localSec = SECONDS.ToString();
            }

            if (MINUTES < 10)
            {
                localMin = "0" + MINUTES.ToString();
            }
            else
            {
                localMin = MINUTES.ToString();
            }
            this.label3.Text = localSec;
            this.label1.Text = localMin;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (SECONDS < 4 && SECONDS > 1)
            {
                cleanTimer();
                playTune();
                return;
            }
            if (CURRENT_TUNE > 0)
            {
                CURRENT_TUNE--;
            }
            else
            {
                CURRENT_TUNE = 5;
            }
            cleanTimer();
            playTune();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                PahanPlayer.BASSMOD_MusicStop();
                this.label7.Text = "play";
                this.timer1.Stop();
            }
            else
            {
                PahanPlayer.BASSMOD_MusicPlay();
                this.label7.Text = "stop";
                this.timer1.Start();
            }
            isPlaying = !isPlaying;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            if (CURRENT_TUNE < 5)
            {
                CURRENT_TUNE++;
            }
            else
            {
                CURRENT_TUNE = 0;
            }
            cleanTimer();
            playTune();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label6_DoubleClick(object sender, EventArgs e)
        {
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
