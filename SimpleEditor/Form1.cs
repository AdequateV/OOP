
namespace Lab6_oop
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        List<IShape> shapes;
        List<IShape> deleteBuffer;
        int mouseX, mouseY;
        
        SelectedShape sh;
        ICreator factoryCircle, factorySquare, factoryTriangle;
        int boundsX, boundsY;
        public Form1()
        {
            InitializeComponent();
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.PaleTurquoise);
            pictureBox1.Image = bm;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            shapes = new List<IShape>();
            factoryCircle = new circleFactory();
            factorySquare = new squareFactory();
            factoryTriangle = new triangleFactory();
            boundsX = pictureBox1.Size.Width;
            boundsY = pictureBox1.Size.Height;
            label4.Text = boundsX.ToString() + " " + boundsY.ToString();
        }
        
        Pen pen = new(Color.Black, 1);
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
            bool multi = checkBoxMulti.Checked && Form.ModifierKeys == Keys.Control;
            bool joints = checkBoxJoints.Checked;
            bool createNew = true;
            foreach(IShape c in shapes)
            {
                if(c.check(mouseX, mouseY))
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

                IShape? s = null;
                
                if (sh == SelectedShape.Circle)
                {
                    s = factoryCircle.FactoryMethod(mouseX, mouseY);
                }
                if (sh == SelectedShape.Square)
                {
                    s = factorySquare.FactoryMethod(mouseX, mouseY);
                }
                if(sh == SelectedShape.Triangle)
                {
                    s = factoryTriangle.FactoryMethod(mouseX, mouseY);
                }
                if(sh == SelectedShape.Section)
                {
                    s = factorySquare.FactoryMethod(mouseX, mouseY);
                    s.resize(0, -24);
                }
                
                s.IsSelected = true;
                shapes.Add(s);
            }
            foreach (IShape c in shapes)
                c.draw(g);
            
            pictureBox1.Refresh();

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
	
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
        int d = 0;
        public void deleteSelected()
        {
            
            deleteBuffer = new List<IShape>();

            foreach (IShape c in shapes)
                if (!c.IsSelected)
                    deleteBuffer.Add(c); //saved all non-selected
            shapes.Clear();
            g.Clear(Color.PaleTurquoise);
            foreach (IShape c in deleteBuffer)
                shapes.Add(c);

            foreach (IShape c in shapes)
                c.draw(g);
            label1.Text += "j";
            pictureBox1.Refresh();
        }
        public void deleteSelected2()
        {

            foreach(IShape c in shapes)
                if(c.IsSelected)
                    shapes.Remove(c);
            
            g.Clear(Color.PaleTurquoise);
            foreach (IShape c in shapes)
                c.draw(g);
            label1.Text += "j";
            pictureBox1.Refresh();
        }

        int moveDirX = 0, moveDirY = 0;
        int movingSpeed = 1;
        int resizeDirX = 0, resizeDirY = 0;
        int sizingSpeed = 1;

        
        public void checkOutOfBounds(IShape c)
        {
            var b = c.closestBoundary(boundsX, boundsY);
            label4.Text = b.ToString();
            label2.Text = c.position.ToString();
            label3.Text = "A";
            if (c.check(b.X, b.Y))
            {
                label3.Text = "KJD";
                var vec = new Point();
                if (b.X == 0) vec.X = 1;
                if (b.Y == 0) vec.Y = 1;
                if (b.X == boundsX) vec.X = -1;
                if (b.Y == boundsY) vec.Y = -1;
                c.move(vec.X * 2, vec.Y * 2);
                
            }
            
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            
            //pictureBox1.Size = new Size(Size.Width, Size.Height);
            //Console.WriteLine(pictureBox1.Size);
            
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (IShape c in shapes)
            {
                var size = c.size;
                Console.WriteLine("Width: " + size.Width + ", Height: " + size.Height);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                deleteSelected();
            }
            if (Form.ModifierKeys == Keys.Control)
            {
                if (d++ % 5 == 0)
                    label1.Text += "-";
                if (d > 25)
                {
                    d = 0;
                    label1.Text = "|Ctrl|";
                }
            }
            if(e.KeyCode == Keys.R)
            {
                foreach(IShape c in shapes)
                {
                    c.IsSelected = false;
                    c.draw(g);
                }
                pictureBox1.Refresh();
            }
            
            
            ///
            if (e.KeyCode == Keys.A) moveDirX = -1; //left
            if (e.KeyCode == Keys.D) moveDirX = 1;  //right
            if (e.KeyCode == Keys.S) moveDirY = 1;  //down
            if (e.KeyCode == Keys.W) moveDirY = -1; //up
            if (moveDirX != 0 || moveDirY != 0)
            {
                g.Clear(Color.PaleTurquoise);
                foreach(IShape shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        var b = shape.checkMovDirection(boundsX, boundsY);
                        moveDirX -= b.X; moveDirY -= b.Y; 
                        shape.move(moveDirX * movingSpeed, moveDirY * movingSpeed);
                        //adjust position when the shape is out of bounds
                        checkOutOfBounds(shape); 
                    }
                    
                    shape.draw(g);
                }
                pictureBox1.Refresh();
                moveDirX = 0; moveDirY = 0;
            }

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
                        if (resizeDirX > 0 && shape.size.Width < 200)
                            shape.resize(resizeDirX * sizingSpeed, resizeDirY * sizingSpeed);
                        if(resizeDirY > 0 && shape.size.Height < 200)
                            shape.resize(resizeDirX * sizingSpeed, resizeDirY * sizingSpeed);
                        checkOutOfBounds(shape);
                    }
                    shape.draw(g);
                }
                pictureBox1.Refresh();
                resizeDirY = 0; resizeDirX = 0;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Circle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Square;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Triangle;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sh = SelectedShape.Section;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        { 
            
        }

        

        
        
    }
    public class circle : IShape
    {
        public Point position { get; set; }
        public int radiusX { get; set; }
        public int radiusY { get; set; }
        public bool IsSelected { get; set; } = false;
        public Color colorMain { get; set; }
        public Size size { get => new Size(radiusX * 2, radiusY * 2); }

        public circle(int x, int y, int r = 25)
        {
            position = new Point(x, y);
            radiusX = r; radiusY = r;
        }
        public void draw(Graphics g)
        {
            var pen = new Pen(colorMain, 2);
            var brush = new SolidBrush(Color.FromArgb(128, 255, 204, 153));

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            g.DrawEllipse(pen,   position.X - radiusX, position.Y - radiusY, radiusX * 2, radiusY * 2);
            g.FillEllipse(brush, position.X - radiusX, position.Y - radiusY, radiusX * 2, radiusY * 2);
        }
        public bool check(int x, int y)
        {

            double p = ((double)Math.Pow(x - position.X, 2) / (double)Math.Pow(radiusX, 2)) +
                ((double)Math.Pow(y - position.Y, 2) / (double)Math.Pow(radiusY, 2));

            if (p <= 1)
            {
                IsSelected = true;
                return true;
            }
            return false;

        }

        public void move(int x, int y)
        {
            position = new Point(position.X + x, position.Y + y);
        }

        public void resize(int x, int y)
        {
            radiusX += x;
            radiusY += y;
        }
        

    }

    public class square : IShape
    {
        public Point position { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool IsSelected { get; set; } = false;
        public Color colorMain { get; set; }
        public Size size { get => new Size(width, height); }

        Point[] pa;

        public square(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            width = w; height = w;
            colorMain = Color.Yellow;
        }
        public void draw(Graphics g)
        {
            var pen = new Pen(colorMain, 2);
            var brush = new SolidBrush(Color.FromArgb(128, 255, 204, 153));

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            
            pa = new Point[] {
                new Point(position.X - width, position.Y - height), //left up
                new Point(position.X + width, position.Y - height), //right up
                new Point(position.X + width, position.Y + height), //right down
                new Point(position.X - width, position.Y + height)  //left down
            };
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

        public void move(int x, int y)
        {
            position = new Point(position.X + x, position.Y + y);
        }

        public void resize(int x, int y)
        {
            width += x;
            height += y;
        }

    }
    
    public class triangle : IShape
    {
        public Point position { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool IsSelected { get; set; } = false;

        public Color colorMain { get; set; }
        public Size size { get => new Size(offsetX, offsetY); }

        public Color colorMain2;
        public Color colorSelected;
        public Color colorSelected2;
        Point[] pa;

        public triangle(int x, int y, int w = 25)
        {
            position = new Point(x, y);
            offsetX = w; offsetY = w;
            pa = new Point[] {
                new Point(position.X, position.Y - offsetY),
                new Point(position.X - offsetX, position.Y + offsetY / 2),
                new Point(position.X + offsetX, position.Y + offsetY / 2)
            };

            colorSelected = Color.FromArgb(128, 166, 166, 166);
            colorSelected2 = Color.Black;
            colorMain = Color.FromArgb(128, 255, 204, 153);
            colorMain2 = Color.Yellow;
        }
        public void draw(Graphics g)
        {
            var pen = new Pen(colorMain2, 2);
            var brush = new SolidBrush(colorMain);

            if (IsSelected)
            {
                pen = new Pen(colorSelected2, 2);
                brush = new SolidBrush(colorSelected); ;
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

        public void move(int x, int y)
        {
            position = new Point(position.X + x, position.Y + y);
        }

        public void resize(int x, int y)
        {
            offsetX += x;
            offsetY += y;
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


            /// https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            for (int i = 0, j = size - 1; i < size; j = i++)
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                     p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                    return true;
            
            return false;
        }

    }


    public interface IShape
    {
        public Color colorMain { get; set; }
        public Point position { get; set; }
        public bool IsSelected { get; set; }
        public void draw(Graphics g);
        public bool check(int x, int y);
        public void move(int x, int y);
        public void resize(int x, int y);
        public Size size { get; }
        /// <summary>
        /// checks if desired movement is possible
        /// </summary>
        /// <param name="boX">width bounds</param>
        /// <param name="boY">height bounds</param>
        /// <returns>allowed movement vector</returns>
        public Point checkMovDirection(int boX, int boY)
        {
            int a = 0, b = 0;
            if (position.X <= 0)    a = -1; //no left movement
            if (position.X >= boX)  a = 1;  //no right
            if (position.Y <= 0)    b = -1; //no up
            if (position.Y >= boY)  b = 1;  //no down
            
            return new Point(a, b);
        }
        public Point closestBoundary1(int boundX, int boundY)
        {
            Point result = new(0, 0);
            int distanceTop = position.Y;
            int distanceBot = boundY - position.Y;
            int distanceLeft = position.X;
            int distanceRight = boundX - position.X;
            bool bot = false, right = false;
            if (distanceTop > distanceBot) bot = true;
            if (distanceLeft > distanceRight) right = true;

            if (bot)
            {
                if (right)
                {
                    if (distanceBot > distanceRight)
                        result = new(boundX, position.Y);
                    else
                        result = new(position.X, boundY);

                }
                if (!right)
                {
                    if(distanceBot > distanceLeft)
                        result = new(0, position.Y);
                    else
                        result = new(position.X, boundY);
                }
            }
            if (!bot)
            {
                if (right)
                {
                    if (distanceTop > distanceRight)
                        result = new(boundX, position.Y);
                    else
                        result = new(position.X, 0);
                    
                }
                if (!right)
                {
                    if (distanceTop > distanceLeft)
                        result = new(0, position.Y);
                    else
                        result = new(position.X, 0);
                }
                
            }
            return result;
        }
        public Point closestBoundary(int boundX, int boundY)
        {
            int distanceTop = position.Y;
            int distanceBot = boundY - position.Y;
            int distanceLeft = position.X;
            int distanceRight = boundX - position.X;

            Point closest = new(boundX, position.Y);

            if (distanceTop < Math.Min(distanceBot, Math.Min(distanceLeft, distanceRight)))
                closest = new(position.X, 0);
            else if (distanceBot < Math.Min(distanceTop, Math.Min(distanceLeft, distanceRight)))
                closest = new(position.X, boundY);
            else if (distanceLeft < Math.Min(distanceTop, Math.Min(distanceBot, distanceRight)))
                closest = new(0, position.Y);

            return closest;
        }
        
        public void recolor(Color main)
        {
            colorMain = main;
        }


    }
    interface ICreator
    {
        IShape FactoryMethod(int x, int y);
    }
    public class circleFactory : ICreator
    {
        public IShape FactoryMethod(int x, int y)
        {
            return new circle(x, y);
        }
    }
    public class squareFactory : ICreator
    {
        public IShape FactoryMethod(int x, int y)
        {
            return new square(x, y);
        }
    }
    public class triangleFactory : ICreator
    {
        public IShape FactoryMethod(int x, int y)
        {
            return new triangle(x, y);
        }
    }

    
    }
}
