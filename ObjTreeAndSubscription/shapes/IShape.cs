using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop
{
    public abstract class IShape
    {
        public DungeonMaster dungeonMaster { get; protected set; } = new();
        public Listener watcher { get; protected set; } = new();
        private const int movingSpeed = 1;
        public virtual Point position { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual int offsetX { get; set; }
        public virtual int offsetY { get; set; }
        public int boundX { get; set; }
        public int boundY { get; set; }
        public virtual void updBounds(int x, int y)
        {
            boundX = x;
            boundY = y;
        }
        public Color colorMain { get; set; }
        public abstract string SimpleName { get; }

        //point of closest boundary
        public virtual Point b { get; set; }
        public abstract Size size { get; }
        public abstract void draw(Graphics g);
        public abstract bool check(int x, int y);
        public virtual void move(int x, int y)
        {
            position = new(position.X + x, position.Y + y);
            dungeonMaster.NotifyObservers(x, y);
        }
        public virtual void resize(int x, int y)
        {
            offsetX += x;
            offsetY += y;
        }
        public virtual void recolor(Color choice)
        {
            colorMain = choice;
        }

        public virtual void Save(StreamWriter sw)
        {
            sw.WriteLine(SimpleName);
            sw.WriteLine("{0} {1} {2} {3} {4} {5} {6}",
            position.X, position.Y, offsetX, offsetY, colorMain.R, colorMain.G, colorMain.B);
        }

        public virtual void Load(StreamReader sr)
        {
            try
            {
                string[] data = sr.ReadLine().Split(' ');
                position = new(int.Parse(data[0]), int.Parse(data[1]));
                offsetX = int.Parse(data[2]); offsetY = int.Parse(data[3]);
                colorMain = Color.FromArgb(
                    int.Parse(data[4]),
                    int.Parse(data[5]),
                    int.Parse(data[6]));
            }
            catch (Exception)
            {
                Console.WriteLine("failed to load");
            }

        }
        

        public virtual Point closestBoundary()
        {
            int distanceTop = position.Y - offsetY;
            int distanceLeft = position.X - offsetX;
            int distanceBot = boundY - distanceTop - size.Height;
            int distanceRight = boundX - distanceLeft - size.Width;
            bool bot = false, right = false;
            if (distanceTop > distanceBot) bot = true;
            if (distanceLeft > distanceRight) right = true;
            if (bot)
            {
                if (right)
                {
                    if (distanceBot > distanceRight)
                        return new(boundX, position.Y);
                    return new(position.X, boundY);
                }
                if (distanceBot > distanceLeft)
                    return new(0, position.Y);
                return new(position.X, boundY);
            }

            if (right)
            {
                if (distanceTop > distanceRight)
                    return new(boundX, position.Y);
                return new(position.X, 0);
            }
            if (distanceTop > distanceLeft)
                return new(0, position.Y);
            return new(position.X, 0);

        }

        public virtual bool IsOnEdge()
        {
            b = closestBoundary();
            return check(b.X, b.Y);
        }
        public virtual bool checkOrFixOutOfBounds()
        {

            if (IsOnEdge())
            {
                Point vec = new();

                if (b.X <= 0) vec.X = 1;
                if (b.Y <= 0) vec.Y = 1;
                if (b.X >= boundX) vec.X = -1;
                if (b.Y >= boundY) vec.Y = -1;
                move(vec.X * movingSpeed, vec.Y * movingSpeed);
                return true;
            }
            return false;
        }

    }

}
