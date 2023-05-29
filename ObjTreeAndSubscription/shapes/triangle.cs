using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop
{
    public class triangle : IShape
    {
        public override string SimpleName => "Triangle";
        public override Size size { get => new(offsetX * 2, offsetY * 3 / 2); }
        private Point[] pa;
        public triangle(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            offsetX = w; offsetY = w;
            pa = new Point[3];
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
            pa[0].X = position.X; pa[0].Y = position.Y - offsetY;
            pa[1].X = position.X - offsetX; pa[1].Y = position.Y + offsetY / 2;
            pa[2].X = position.X + offsetX; pa[2].Y = position.Y + offsetY / 2;

            g.DrawPolygon(pen, pa);
            g.FillPolygon(brush, pa);

            watcher.changeArrows(g);
            dungeonMaster.ToDefault();

        }
        public override bool check(int x, int y)
        {
            if (IsPointInTriangle(new Point(x, y), pa))
            {
                IsSelected = true;
                return true;
            }
            return false;
        }

        private static bool IsPointInPolygon(Point p, Point[] polygon)
        {
            double minX = polygon[0].X;
            double maxX = polygon[0].X;
            double minY = polygon[0].Y;
            double maxY = polygon[0].Y;
            int size = polygon.Length;
            for (int i = 1; i < size; i++)
            {
                Point q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
                return false;

            for (int i = 0, j = size - 1; i < size; j = i++)
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                     p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y)
                     / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                    return true;

            return false;
        }

        private static bool IsPointInTriangle(Point p, Point[] poly)
        {
            int a1 = (poly[0].X - p.X) * (poly[1].Y - poly[0].Y) - (poly[1].X - poly[0].X) * (poly[0].Y - p.Y);
            int a2 = (poly[1].X - p.X) * (poly[2].Y - poly[1].Y) - (poly[2].X - poly[1].X) * (poly[1].Y - p.Y);
            int a3 = (poly[2].X - p.X) * (poly[0].Y - poly[2].Y) - (poly[0].X - poly[2].X) * (poly[2].Y - p.Y);
            if ((a1 > 0 && a2 > 0 && a3 > 0) || (a1 < 0 && a2 < 0 && a3 < 0)) return true;
            return false;

        }

    }
}
