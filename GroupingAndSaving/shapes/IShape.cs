using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_oop
{
    public abstract class IShape
    {
        public virtual Point position { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual int offsetX { get; set; }
        public virtual int offsetY { get; set; }
        public Color colorMain { get; set; }


        public abstract Size size { get; }
        public abstract void draw(Graphics g);
        public abstract bool check(int x, int y);
        public virtual void move(int x, int y)
        {
            position = new Point(position.X + x, position.Y + y);

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

        public abstract void Save(StreamWriter sw);
        public abstract void Load(StreamReader sr);


        public virtual Point closestBoundary(int boundX, int boundY)
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
        public virtual bool checkOrFixOutOfBounds(int boundsX, int boundsY, int movingSpeed)
        {
            Point b = closestBoundary(boundsX, boundsY);
            //Console.WriteLine("closest boundary is " + b);
            if (check(b.X, b.Y))
            {
                Point vec = new();
                if (b.X == 0) vec.X = 1;
                if (b.Y == 0) vec.Y = 1;
                if (b.X == boundsX) vec.X = -1;
                if (b.Y == boundsY) vec.Y = -1;
                move(vec.X * 2 * movingSpeed, vec.Y * 2 * movingSpeed);
                return true;
            }
            return false;
        }


    }

}
