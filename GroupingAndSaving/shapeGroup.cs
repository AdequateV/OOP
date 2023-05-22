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
        public List<IShape> shapes;
        int LARGE = 9999;
        public shapeGroup()
        {
            shapes = new();
        }
        public shapeGroup(IShape s)
        {
            shapes = new();
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
            LeftUp.X    = LARGE;  LeftUp.Y    = LARGE;
            RightDown.X = -LARGE; RightDown.Y = -LARGE;
            foreach (IShape shape in shapes)
            {
                RightDown.X = Math.Max(RightDown.X, shape.position.X + shape.size.Width / 2);
                RightDown.Y = Math.Max(RightDown.Y, shape.position.Y + shape.size.Height / 2);
                LeftUp.X    = Math.Min(LeftUp.X,    shape.position.X - shape.size.Width / 2);
                LeftUp.Y    = Math.Min(LeftUp.Y,    shape.position.Y - shape.size.Height / 2);
            }
            offsetX = (RightDown.X - LeftUp.X) / 2;
            offsetY = (RightDown.Y - LeftUp.Y) / 2;
            return new Size(offsetX * 2, offsetY * 2);
        }

        public void AddShape(IShape shape)
        {
            shapes.Add(shape);
            calcSize();
            position = new Point(RightDown.X - size.Width / 2, RightDown.Y - size.Height / 2);
            
        }
        public void Ungroup(List<IShape> dest)
        {
            foreach (IShape s in shapes)
            {
                dest.Add(s);
            }
            //shapes.Clear();
        }

        public override void resize(int x, int y)
        {
            base.resize(x, y);
            foreach (IShape s in shapes)
            {
                s.resize(x, y);
            }
        }
        public override bool check(int x, int y)
        {
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
            g.DrawRectangle(new Pen(Color.FromArgb(128, Color.Gray), 2),
                position.X - size.Width / 2, position.Y - size.Height / 2, size.Width, size.Height);
        }
        public override void recolor(Color main)
        {
            foreach (IShape shape in shapes)
            {
                shape.recolor(main);
            }
        }
        public override void move(int x, int y)
        {
            Console.WriteLine("RD " + RightDown);
            Console.WriteLine("LU " + LeftUp);
            Console.WriteLine("SIZE " + size);
            //position = new Point(RightDown.X - size.Width / 2, RightDown.Y - size.Height / 2);
            base.move(x, y);
            Console.WriteLine("gr pos" + position);
            foreach (IShape shape in shapes)
            {
                shape.move(x, y);
            }

        }
        public override void updBounds(int x, int y)
        {
            base.updBounds(x, y);
            foreach (var shape in shapes)
            {
                shape.updBounds(x, y);
            }
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
