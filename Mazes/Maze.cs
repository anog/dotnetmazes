using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Mazes
{
    class Room
    {
        public Image Draw(Pen p)
        {
            Bitmap img = new Bitmap(11, 11);
            Graphics g = Graphics.FromImage(img);
            Brush b = Brushes.Black;
            g.FillRectangle(b, 0, 0, 1,1);
            g.FillRectangle(b, 0, 10, 1,1);
            g.FillRectangle(b, 10, 0, 1,1);
            g.FillRectangle(b, 10, 10, 1,1);

            if (!open_west_)
            {
                g.DrawLine(p, new Point(0, 0), new Point(0, 10));
            }
            if (!open_east_)
            {
                g.DrawLine(p, new Point(10, 0), new Point(10, 10));
            }
            if (!open_north_)
            {
                g.DrawLine(p, new Point(0, 0), new Point(10, 0));
            }
            if (!open_south_)
            {
                g.DrawLine(p, new Point(0, 10), new Point(10, 10));
            }
            return img;
        }

        public void DrawDot(Graphics g)
        {
            Pen p = new Pen(Color.Chocolate);
            
        }

        Room north_ = null, south_ = null, east_ = null, west_ = null;
        Boolean open_north_, open_south_, open_east_, open_west_;

        public Boolean OpenNorth
        {
            get { return open_north_; }
            set { open_north_ = value; if (north_.OpenSouth != true) { north_.OpenSouth = true; } }
        }

        public Boolean OpenSouth
        {
            get { return open_south_; }
            set { open_south_ = value; if (south_.OpenNorth != true) { south_.OpenNorth = true; } }
        }

        public Boolean OpenWest
        {
            get { return open_west_; }
            set { open_west_ = value; if (west_.OpenEast != true) { west_.OpenEast = true; } }
        }

        public Boolean OpenEast
        {
            get { return open_east_; }
            set { open_east_ = value; if (east_.OpenWest != true) { east_.OpenWest = true; } }
        }
        public Room North
        {
            get { return north_; }
            set { north_ = value; if (north_.South != this) { north_.South = this; } }
        }

        public Room South
        {
            get { return south_; }
            set { south_ = value; if (south_.North != this) { south_.North = this; } }
        }

        public Room East
        {
            get { return east_; }
            set { east_ = value; if (east_.West != this) { east_.West = this; } }
        }

        public Room West
        {
            get { return west_; }
            set { west_ = value; if (west_.East != this) { west_.East = this; } }
        }
    }

    class Maze
    {
        public Maze()
        {
            rooms = new Room[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    rooms[i, j] = new Mazes.Room();
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    rooms[i, j].South = rooms[i, j + 1];
                    rooms[i, j].East = rooms[i+1, j];
                }
            }
            RandomWalk(rooms, 10, 10);
        }
        Room[,] rooms;

        void RandomWalk(Room[,] rooms, UInt64 numx, UInt64 numy)
        {
            UInt64 x = 0;
            UInt64 y = 0;
            Random r = new Random(42);
            for (UInt64 i = 0; i < 800; ++i){
                Int64 ri = r.Next(0, 4);
                if (ri == 0)
                {
                    if (rooms[x, y].North != null)
                    {
                        rooms[x, y].OpenNorth = true;
                        y -= 1;
                    }
                }
                if (ri == 1)
                {
                    if (rooms[x, y].South != null)
                    {
                        rooms[x, y].OpenSouth = true;
                        y += 1;
                    }
                }
                if (ri == 2)
                {
                    if (rooms[x, y].West != null)
                    {
                        rooms[x, y].OpenWest = true;
                        x -= 1;
                    }
                }
                if (ri == 3)
                {
                    if (rooms[x, y].East != null)
                    {
                        rooms[x, y].OpenEast = true;
                        x += 1;
                    }
                }


            }

        }

        public Image Draw()
        {
            Pen p = new Pen(Color.Black, 1);
            Bitmap img = new Bitmap(101, 101);
            Graphics g = Graphics.FromImage(img);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    g.DrawImage(rooms[i, j].Draw(p), i * 10, j * 10);
                }
            }
            
            g.Flush();
            return img;
        }
    }
}
