using Lab6_oop;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Lab6_oop
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        Container<IShape> shapes;
        Container<IShape> deleteBuffer;
        int mouseX, mouseY;
        int toolbarHeight = 100;
        SelectedShape sh;
        shapeFactory sf;
        int boundsX, boundsY;
        int desiredDimension = 25;
        //mouse
        bool multi, joints, createNew;
        //settings
        int moveDirX = 0, moveDirY = 0, movingSpeed = 4;
        int resizeDirX = 0, resizeDirY = 0, sizingSpeed = 3;


        public Form1()
        {
            InitializeComponent();
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.PaleTurquoise);
            pictureBox1.Image = bm;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            shapes = new Container<IShape>();
            sf = new shapeFactory();
            boundsX = pictureBox1.Size.Width;
            boundsY = pictureBox1.Size.Height;
        }

        enum SelectedShape
        {
            Circle,
            Square,
            Triangle,
            Section
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            multi = (Form.ModifierKeys == Keys.Control); //&& checkBoxMulti.Checked;
            joints = checkBoxJoints.Checked;
            createNew = true;
            foreach (IShape c in shapes)
            {
                if (c.check(mouseX, mouseY))
                {
                    createNew = false;
                    if (multi || joints)
                    {
                        if (!multi)
                            foreach (IShape c2 in shapes)
                                if (!c2.check(mouseX, mouseY))
                                    c2.IsSelected = false;

                        c.IsSelected = true;
                        continue;
                    }

                    foreach (IShape c1 in shapes)
                        c1.IsSelected = false;
                    c.IsSelected = true;
                }
            }
            if (createNew)
            {
                if (!multi)
                    foreach (IShape c in shapes)
                        c.IsSelected = false;
                IShape s = shapeFactory.FactoryMethod(mouseX, mouseY, (int)sh);
                if (sh == SelectedShape.Section)
                    s.resize(0, -(desiredDimension - 1));


                s.IsSelected = true;
                shapes.Add(s);
            }
            foreach (IShape c in shapes)
                c.draw(g);

            pictureBox1.Refresh();

        }

        public void deleteSelected()
        {
            deleteBuffer = new Container<IShape>();
            foreach (IShape c in shapes)
                if (!c.IsSelected)
                    deleteBuffer.Add(c);
            //saved all non-selected
            shapes.Clear();
            g.Clear(Color.PaleTurquoise);
            foreach (IShape c in deleteBuffer)
                shapes.Add(c);
            foreach (IShape c in shapes)
                c.draw(g);
            Console.WriteLine("deleted");
            pictureBox1.Refresh();
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width, ClientSize.Height - toolbarHeight);
            boundsX = pictureBox1.Size.Width;
            boundsY = pictureBox1.Size.Height;
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            pictureBox1.Image = bm;
            foreach (IShape c in shapes)
            {
                c.draw(g);
                c.checkOutOfBounds(boundsX, boundsY, SystemInformation.MouseSpeed);
            }
            pictureBox1.Refresh();

        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (IShape c in shapes) c.draw(g);
            pictureBox1.Refresh();
        }


        private void btnStats_Click(object sender, EventArgs e)
        {
            foreach (IShape c in shapes)
            {
                Console.WriteLine("Size (w, h): " + c.size);
                Console.WriteLine(c.closestBoundary(boundsX, boundsY));

            }
        }
        int d = 0;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                deleteSelected();
            }
            if (Form.ModifierKeys == Keys.Control)
            {
                if (d++ % 2 == 0) label1.Text += "<";
                if (d > 8){ label1.Text = "|Ctrl|"; d = 0; }
            }
            MoveHandle(e);
            ResizeHandle(e);

        }
        private void MoveHandle(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) moveDirX = -1; //left
            if (e.KeyCode == Keys.D) moveDirX = 1;  //right
            if (e.KeyCode == Keys.S) moveDirY = 1;  //down
            if (e.KeyCode == Keys.W) moveDirY = -1; //up
            if (moveDirX != 0 || moveDirY != 0)
            {
                g.Clear(Color.PaleTurquoise);
                foreach (IShape shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        shape.move(moveDirX * movingSpeed, moveDirY * movingSpeed);
                        //adjust position when the shape is out of bounds
                        shape.checkOutOfBounds(boundsX, boundsY, movingSpeed);
                    }

                    shape.draw(g);
                }
                pictureBox1.Refresh();
                moveDirX = 0; moveDirY = 0;
            }
        }
        private void ResizeHandle(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Q) resizeDirX = -1; //x axis decr
            if (e.KeyCode == Keys.E) resizeDirX = 1;  //x axis incr
            if (e.KeyCode == Keys.Z) resizeDirY = -1; //y decr
            if (e.KeyCode == Keys.C) resizeDirY = 1;  //y incr
            if (resizeDirX != 0 || resizeDirY != 0)
            {
                g.Clear(Color.PaleTurquoise);
                foreach (IShape shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        if (resizeDirX > 0 && shape.size.Width >= boundsX / 2 ||
                            resizeDirY > 0 && shape.size.Height >= boundsY / 2)
                        {
                            shape.draw(g);
                            continue;
                        }

                        shape.resize(resizeDirX * sizingSpeed, resizeDirY * sizingSpeed);
                        shape.checkOutOfBounds(boundsX, boundsY, movingSpeed);
                    }
                    shape.draw(g);
                }
                pictureBox1.Refresh();
                resizeDirY = 0; resizeDirX = 0;
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label5.Text = e.Location.ToString();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }



        public void unselectDropdownMenuItems()
        {
            foreach (ToolStripMenuItem a in toolStripDropDownButton1.DropDownItems)
                a.BackColor = Color.White;
        }

        private void circleellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Circle;
            unselectDropdownMenuItems();
            circleellipseToolStripMenuItem.BackColor = Color.Wheat;
        }

        private void squarerectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Square;
            unselectDropdownMenuItems();
            squarerectangleToolStripMenuItem.BackColor = Color.Wheat;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Triangle;
            unselectDropdownMenuItems();
            triangleToolStripMenuItem.BackColor = Color.Wheat;
        }

        private void sectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Section;
            unselectDropdownMenuItems();
            sectionToolStripMenuItem.BackColor = Color.Wheat;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            foreach (IShape shape in shapes)
            {
                if (shape.IsSelected)
                {
                    shape.recolor(colorDialog1.Color);
                    shape.IsSelected = false;
                    shape.draw(g);
                }
            }
            pictureBox1.Refresh();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            g.Clear(Color.PaleTurquoise);
            pictureBox1.Refresh();

        }

        private void unselectShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //unselect everything
            foreach (IShape c in shapes)
            {
                c.IsSelected = false;
                c.draw(g);
            }
            pictureBox1.Refresh();
        }
    }
    public class circle : IShape
    {
        public Point position { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool IsSelected { get; set; } = false;
        public Color colorMain { get; set; }
        public Size size { get => new(offsetX * 2, offsetY * 2); }

        public circle(int x, int y, int r = 25)
        {
            position = new Point(x, y);
            offsetX = r; offsetY = r;
            colorMain = Color.FromArgb(128, 255, 204, 200);
        }
        public void draw(Graphics g)
        {
            var pen = new Pen(Color.FromArgb(128, 255, 204, 153), 2);
            var brush = new SolidBrush(colorMain);

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            g.DrawEllipse(pen,   position.X - offsetX, position.Y - offsetY, offsetX * 2, offsetY * 2);
            g.FillEllipse(brush, position.X - offsetX, position.Y - offsetY, offsetX * 2, offsetY * 2);
        }
        public bool check(int x, int y)
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


    }

    public class square : IShape
    {
        public Point position { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool IsSelected { get; set; } = false;
        public Color colorMain { get; set; }
        public Size size { get => new(offsetX * 2, offsetY * 2); }

        private Point[] pa;

        public square(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            offsetX = w; offsetY = w;
            pa = new Point[4];
            colorMain = Color.FromArgb(128, 255, 204, 200);
        }
        public void draw(Graphics g)
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
        public bool check(int x, int y)
        {
            //
            if (x >= pa[0].X && x <= pa[1].X && y >= pa[0].Y && y <= pa[3].Y)
            {
                IsSelected = true;
                return true;
            }
            return false;
        }



    }

    public class triangle : IShape
    {
        public Point position { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool IsSelected { get; set; } = false;

        public Color colorMain { get; set; }
        public Size size { get => new(offsetX * 2, offsetY * 3 / 2); }
        private Point[] pa;

        public triangle(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            offsetX = w; offsetY = w;
            pa = new Point[3];
            colorMain = Color.FromArgb(128, 255, 204, 200);
        }
        public void draw(Graphics g)
        {
            var pen = new Pen(Color.FromArgb(128, 255, 204, 153), 2);
            var brush = new SolidBrush(colorMain);

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            pa[0].X = position.X;           pa[0].Y = position.Y - offsetY;
            pa[1].X = position.X - offsetX; pa[1].Y = position.Y + offsetY / 2;
            pa[2].X = position.X + offsetX; pa[2].Y = position.Y + offsetY / 2;

            g.DrawPolygon(pen, pa);
            g.FillPolygon(brush, pa);

        }
        public bool check(int x, int y)
        {
            if (IsPointInPolygon(new Point(x, y), pa))
            {
                IsSelected = true;
                return true;
            }
            return false;
        }

        private static bool IsPointInPolygon(Point p, Point[] polygon)
        {
            double minX = polygon[0].X, maxX = polygon[0].X;
            double minY = polygon[0].Y, maxY = polygon[0].Y;
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


    }


    public interface IShape
    {
        public Color colorMain { get; set; }
        public Point position { get; set; }
        public Size size { get; }
        public bool IsSelected { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }


        public void draw(Graphics g);
        public bool check(int x, int y);
        public void move(int x, int y)
        {
            position = new Point(position.X + x, position.Y + y);
        }
        public void resize(int x, int y)
        {
            offsetX += x;
            offsetY += y;
        }
        public void recolor(Color main)
        {
            colorMain = main;
        }

        public Point closestBoundary(int boundX, int boundY)
        {
            int distanceTop   = position.Y - offsetY;
            int distanceLeft  = position.X - offsetX;
            int distanceBot   = boundY - distanceTop - size.Height;
            int distanceRight = boundX - distanceLeft - size.Width;
            bool bot = false, right = false;
            if (distanceTop > distanceBot)    bot = true;
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

        public void checkOutOfBounds(int boundsX, int boundsY, int movingSpeed)
        {
            Point b = closestBoundary(boundsX, boundsY);
            if (check(b.X, b.Y))
            {
                Point vec = new();
                if (b.X == 0)       vec.X = 1;
                if (b.Y == 0)       vec.Y = 1;
                if (b.X == boundsX) vec.X = -1;
                if (b.Y == boundsY) vec.Y = -1;
                move(vec.X * 2 * movingSpeed, vec.Y * 2 * movingSpeed);

            }

        }


    }

    public class shapeFactory
    {

        public static IShape FactoryMethod(int x, int y, int type)
        {
            if (type == 0) return new circle  (x, y);
            if (type == 1) return new square  (x, y);
            if (type == 2) return new triangle(x, y);
            if (type == 3) return new square  (x, y);
            return new circle(x, y);
        }
        public static IShape FactoryMethod(int x, int y, int type, int dimensions)
        {
            if (type == 0) return new circle  (x, y, dimensions);
            if (type == 1) return new square  (x, y, dimensions);
            if (type == 2) return new triangle(x, y, dimensions);
            if (type == 3) return new square  (x, y, dimensions);
            return new circle(x, y);
        }
    }
}
