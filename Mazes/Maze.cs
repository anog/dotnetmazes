using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Mazes
{
    class Room
    {
        public int col_, row_;
        public Room(int col, int row) { row_ = row; col_ = col; }
        public Image Draw(Pen p)
        {
            Bitmap img = new Bitmap(11, 11);
            Graphics g = Graphics.FromImage(img);
            Brush b = Brushes.Black;
            g.FillRectangle(b, 0, 0, 1, 1);
            g.FillRectangle(b, 0, 10, 1, 1);
            g.FillRectangle(b, 10, 0, 1, 1);
            g.FillRectangle(b, 10, 10, 1, 1);

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
            Point[] dot_points = new Point[5];
            dot_points[0] = new Point(5, 5 - 2);
            dot_points[1] = new Point(5 + 2, 5);
            dot_points[2] = new Point(5, 5 + 2);
            dot_points[3] = new Point(5 - 2, 5);
            dot_points[4] = new Point(5, 5 - 2);
            g.FillClosedCurve(Brushes.Chocolate, dot_points);
        }

        Boolean visited_;
        public Boolean Visited
        {
            get { return visited_; }
            set { visited_ = value; }
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

        Room current_room_;
        UInt32 rows_, cols_;
        public Maze(UInt32 cols, UInt32 rows)
        {
            cols_ = cols;
            rows_ = rows;
            rooms = new Room[cols, rows];
            current_room_ = rooms[0, 0];
            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    rooms[col, row] = new Mazes.Room(col, row);
                }
            }
            for (int col = 0; col < cols - 1; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    if (row < rows - 1)
                    {
                        rooms[col, row].South = rooms[col, row + 1];
                    }
                    rooms[col, row].East = rooms[col + 1, row];
                }
            }
            RandomWalk();
        }
        Room[,] rooms;

        Random r = new Random(42);
        Room GetRandomUnvisitedRoom()
        {
            List<Room> unvisited_rooms = new List<Room>();
            for (int col = 0; col < cols_; ++col)
            {
                for (int row = 0; row < rows_; ++row)
                {
                    if (rooms[col, row].Visited == false)
                    {
                        unvisited_rooms.Add(rooms[col, row]);
                    }
                }
            }
            Debug.WriteLine("Unvisited count: {0}", unvisited_rooms.Count);
            if (unvisited_rooms.Count() == 0)
            {
                return null;
            }

            return unvisited_rooms.ElementAt(r.Next(0, unvisited_rooms.Count));
        }

        void RandomWalk()
        {
            uint start_col = 0;
            uint start_row = rows_ - 1;
            Room room = rooms[start_col, start_row];
            Room prev_room = room;
            while (true)
            {
                if (room.Visited == true)
                {
                    room = GetRandomUnvisitedRoom();
                    prev_room = room;
                    if (room == null)
                    {
                        break;
                    }
                }
                room.Visited = true;
                switch (r.Next(0, 2))
                {
                    case 0:
                        // North
                        if (room.North != null && room.North != prev_room)
                        {
                            room.OpenNorth = true;
                            prev_room = room;
                            room = room.North;

                        }
                        else
                        {
                            if (room.East != null && room.East != prev_room)
                            {
                                room.OpenEast = true;
                                prev_room = room;
                                room = room.East;
                            }
                        }
                        break;
                    case 1:
                        // East
                        if (room.East != null && room.East != prev_room)
                        {
                            room.OpenEast = true;
                            prev_room = room;
                            room = room.East;
                        }
                        else
                        {
                            if (room.North != null && room.North != prev_room)
                            {
                                room.OpenNorth = true;
                                prev_room = room;
                                room = room.North;

                            }
                        }
                        break;
                    case 2:
                        // South
                        if (room.South != null && room.South != prev_room)
                        {
                            room.OpenSouth = true;
                            prev_room = room;
                            room = room.South;
                        }
                        break;
                    case 3:
                        // West
                        if (room.West != null && room.West != prev_room)
                        {
                            room.OpenWest = true;
                            prev_room = room;
                            room = room.West;
                        }
                        break;
                }
            }
        }

        public Image Draw()
        {
            Pen p = new Pen(Color.Black, 1);
            Bitmap img = new Bitmap((int)cols_ * 10 + 1, (int)rows_ * 10 + 1);
            Graphics g = Graphics.FromImage(img);
            for (int col = 0; col < cols_; col++)
            {
                for (int row = 0; row < rows_; row++)
                {
                    g.DrawImage(rooms[col, row].Draw(p), col * 10, row * 10);
                    Debug.WriteLine("I: {0}, J:{1}", col, row);
                }
            }
            Debug.WriteLine("Height: {0} Width: {1}", img.Height, img.Width);
            g.Flush();
            return img;
        }
    }
}
