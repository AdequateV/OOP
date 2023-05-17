using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_oop
{
    public class square : IShape
    {
        public override Size size { get => new(offsetX * 2, offsetY * 2); }

        private Point[] pa;

        public square(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            offsetX = w; offsetY = w;
            pa = new Point[4];
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

            pa[0].X = (position.X - offsetX); pa[0].Y = (position.Y - offsetY); //left up
            pa[1].X = (position.X + offsetX); pa[1].Y = (position.Y - offsetY); //right up
            pa[2].X = (position.X + offsetX); pa[2].Y = (position.Y + offsetY); //right down
            pa[3].X = (position.X - offsetX); pa[3].Y = (position.Y + offsetY); //left down            

            g.DrawPolygon(pen, pa);
            g.FillPolygon(brush, pa);

        }
        public override bool check(int x, int y)
        {
            //
            if (x >= pa[0].X && x <= pa[1].X && y >= pa[0].Y && y <= pa[3].Y)
            {
                IsSelected = true;
                return true;
            }
            return false;
        }

        public override void Save(StreamWriter sw)
        {
            throw new NotImplementedException();
        }

        public override void Load(StreamReader sr)
        {
            throw new NotImplementedException();
        }
    }
}
