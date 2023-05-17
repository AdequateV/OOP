using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_oop
{
    
    public class shapeGroup : IShape
    {
        private List<IShape> shapes;
        private List<IShape> buff;
        int LARGE = 9999;
        public shapeGroup()
        {
            shapes = new();
            buff = new();
        }
        public shapeGroup(IShape s)
        {
            shapes = new();
            buff = new();
            AddShape(s);
        }

        Point LeftUp = new(), RightDown = new();
        public override Size size => calcSize();
        public override bool IsSelected
        {
            get
            {
                foreach (IShape s in shapes)
                    if (s.IsSelected) return true;
                return false;
            }
            set
            {
                foreach (IShape s in shapes)
                    s.IsSelected = value;
            }
        }
        public Size calcSize()
        {
            LeftUp.X = LARGE; LeftUp.Y = LARGE; RightDown.X = -LARGE; RightDown.Y = -LARGE;
            foreach (IShape shape in buff)
            {
                RightDown.X = Math.Max(RightDown.X, shape.position.X + shape.size.Width / 2);
                RightDown.Y = Math.Max(RightDown.Y, shape.position.Y + shape.size.Height / 2);

                LeftUp.X = Math.Min(LeftUp.X, shape.position.X - shape.size.Width / 2);
                LeftUp.Y = Math.Min(LeftUp.Y, shape.position.Y - shape.size.Height / 2);
            }

            return new Size(RightDown.X - LeftUp.X, RightDown.Y - LeftUp.Y);
        }

        private void recurBufferize(IShape s)
        {
            if (s is shapeGroup group)
            {
                foreach (IShape b in group.shapes)
                {
                    Console.WriteLine(" group");
                    if (b is not shapeGroup)
                    {
                        Console.Write(" shape added");
                        buff.Add(b);
                    }
                    recurBufferize(b);
                }

            }
        }
        public void AddShape(IShape shape)
        {
            calcSize();
            shapes.Add(shape);
            recurBufferize(shape);
        }
        public void Ungroup(List<IShape> dest)
        {
            foreach (IShape s in shapes)
            {
                dest.Add(s);
            }
            buff.Clear();
            shapes.Clear();
        }
        
        public override void resize(int x, int y)
        {
            Console.WriteLine("group resize//////////////////////////////");
            foreach(IShape s in shapes)
            {
                s.resize(x, y);
            }
        }
        public override bool check(int x, int y)
        {
            Console.WriteLine("group check//////////////////////////////");
            foreach (IShape shape in shapes)
                if (shape.check(x, y))
                    return true;
            return false;

        }

        public override void draw(Graphics g)
        {
            foreach (IShape shape in shapes)
            {
                shape.draw(g);
            }
            g.DrawRectangle(new Pen(Color.Gray, 2),
                RightDown.X - size.Width, RightDown.Y - size.Height, size.Width, size.Height);
        }
        public override void recolor(Color main)
        {
            foreach(IShape shape in shapes)
            {
                shape.recolor(main);
            }
        }
        public override void move(int x, int y)
        {
            Console.WriteLine("group move///////////////////");
            
            foreach (IShape shape in shapes)
            {
                shape.move(x, y);
            }
        }
        
        public override bool checkOrFixOutOfBounds(int boundsX, int boundsY, int movingSpeed)
        {
            foreach(IShape s in buff)
            {
                Point b = s.closestBoundary(boundsX, boundsY);
                if(s.check(b.X, b.Y))
                {
                    Point vec = new();
                    if (b.X == 0) vec.X = 1;
                    if (b.Y == 0) vec.Y = 1;
                    if (b.X == boundsX) vec.X = -1;
                    if (b.Y == boundsY) vec.Y = -1;
                    move(vec.X * 2 * movingSpeed, vec.Y * 2 * movingSpeed);
                    return true;
                }
            }
            return false;
        }

        public override void Save(StreamWriter sw)
        {
            sw.WriteLine("Group");
            sw.WriteLine(shapes.Count);
            foreach (IShape shape in shapes)
            {
                shape.Save(sw);
            }
        }

        public override void Load(StreamReader sr)
        {
            return;
        }
        
    }

    
}
