using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices; // Подключаем новое пространство имён

namespace WinFormsApp1Часики
{
    public partial class Form1 : Form
    {
        int cx = 230, cy = 230; // Центр картинки.

        int secHAND = 100, minHAND = 90, hrHAND = 75;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();


        [DllImport("user32", CharSet = CharSet.Auto)]
        internal extern static bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        internal extern static bool ReleaseCapture();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        System.Windows.Forms.Timer timer01 = new System.Windows.Forms.Timer();
        SoundPlayer sp = new SoundPlayer("D:\\Jeremy-Soule-The-Elder-Scrolls-III-Morrowind-Main-Theme-kissvk.com.wav");
        bool b = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            this.label2.Text = DateTime.Now.ToString("MM:yyyy");
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            const uint WM_SYSCOMMAND = 0x0112;
            const uint DOMOVE = 0xF012;

            ReleaseCapture();
            PostMessage(this.Handle, WM_SYSCOMMAND, DOMOVE, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t.Interval = 1000;  //В миллисекундах
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }
        private int[] msCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }
        private int[] hrCoord(int hval, int mval, int hlen)
        {
            int[] coord = new int[2];
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }
        private void t_Tick(object sender, EventArgs e)
        {
            //Берём время.
            int s = DateTime.Now.Second;
            int m = DateTime.Now.Minute;
            int h = DateTime.Now.Hour;

            int[] handCoord = new int[2];

            Graphics g = pictureBox1.CreateGraphics();

            // Стираем предыдущее положения секундной стрелки
            handCoord = msCoord(s, secHAND + 4);
            g.DrawLine(new Pen(Color.White, 45f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            // Стираем предыдущее положение минутной стрелки
            handCoord = msCoord(m, minHAND + 4);
            g.DrawLine(new Pen(Color.White, 40f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            // Стираем предыдущее положение часовой стрелки
            handCoord = hrCoord(h % 12, m, hrHAND + 4);
            g.DrawLine(new Pen(Color.White, 20f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));


            //Отрисовка стрелки часов.
            handCoord = hrCoord(h % 12, m, hrHAND);
            g.DrawLine(new Pen(Color.Gray, 4f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));


            //Отрисовка минутной стрелки
            handCoord = msCoord(m, minHAND);
            g.DrawLine(new Pen(Color.Black, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            // Отрисовка секундной стрелки.
            handCoord = msCoord(s, secHAND);
            g.DrawLine(new Pen(Color.Yellow, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            timer01.Interval = 1000;
            timer01.Tick += new EventHandler(timer1_Tick);
            timer01.Start();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (b == false)
            {
                label3.Text = maskedTextBox1.Text;
                timer2.Start();
                maskedTextBox1.Visible = false;
                button1.Text = "Убрать будильник";
                b = true;
            }
            else if (b == true)
            {
                label3.Text = "00:00";
                timer2.Stop();
                maskedTextBox1.Visible = true;
                button1.Text = "Завести будильник";
                b = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label1.Text == label3.Text + ":00")
            {
                button2.Enabled = true;
                sp.Play();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.Stop();
            button2.Enabled = false;
            maskedTextBox1.Visible = true;
            button1.Text = "Завести будильник";
            b = false;
        }
    }
}