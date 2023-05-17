using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_oop
{
    public class circle : IShape
    {
        public override Size size { get => new(offsetX * 2, offsetY * 2); }

        public circle(int x, int y, int r = 25)
        {
            position = new Point(x, y);
            offsetX = r; offsetY = r;
            colorMain = Color.FromArgb(128, 255, 204, 200);
        }
        public override void draw(Graphics g)
        {
            var pen = new Pen(Color.FromArgb(128, 255, 204, 153), 2);
            var brush = new SolidBrush(colorMain);

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            g.DrawEllipse(pen, position.X - offsetX, position.Y - offsetY, offsetX * 2, offsetY * 2);
            g.FillEllipse(brush, position.X - offsetX, position.Y - offsetY, offsetX * 2, offsetY * 2);
        }
        public override bool check(int x, int y)
        {

            double p = ((double)Math.Pow(x - position.X, 2) / (double)Math.Pow(offsetX, 2)) +
                ((double)Math.Pow(y - position.Y, 2) / (double)Math.Pow(offsetY, 2));

            if (p <= 1)
            {
                IsSelected = true;
                return true;
            }
            return false;

        }

        public override void Save(StreamWriter sw)
        {
            sw.WriteLine("Circle");
            sw.WriteLine("{0} {1} {2} {3} {4} {5} {6}",
            position.X, position.Y, offsetX, offsetY, colorMain.R, colorMain.G, colorMain.B);
        }

        public override void Load(StreamReader sr)
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

                
            }


        }
    }
}
