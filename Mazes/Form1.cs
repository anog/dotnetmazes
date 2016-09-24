using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mazes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Maze m = new Maze();
            pictureBox1.Image = m.Draw();   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(11, 11);
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.Black, 1);
            g.DrawLine(p, new Point(0, 0), new Point(0, 10));
            pictureBox1.Image = img;
        }
    }
}
