using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            Maze m = new Maze(30, 20);
            Image maze_image = m.Draw();
            pictureBox1.Height = maze_image.Height;
            pictureBox1.Width = maze_image.Width;
            pictureBox1.Image = maze_image;
            Debug.WriteLine("Height: {0} Width: {1}", pictureBox1.Height, pictureBox1.Width);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(11, 11);
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.Black, 1);
            g.DrawLine(p, new Point(0, 0), new Point(0, 10));
            pictureBox1.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
