
using Lab7_oop;
using Microsoft.VisualBasic.Devices;
using System.IO;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows.Forms;

namespace Lab7_oop
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        Container<IShape> shapes;
        Container<IShape> deleteBuffer;
        Container<IShape> groupBuffer;
        int mouseX, mouseY;
        int toolbarHeight = 100;
        SelectedShape sh;
        shapeCreator sf;
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
            shapes = new();

            sf = new shapeCreator();
            boundsX = pictureBox1.Size.Width;
            boundsY = pictureBox1.Size.Height;
            label4.Text = boundsX.ToString() + " " + boundsY.ToString();

            commands[Keys.A] = new MoveCommand(-1 * movingSpeed, 0);
            commands[Keys.D] = new MoveCommand(1 * movingSpeed, 0);
            commands[Keys.W] = new MoveCommand(0, -1 * movingSpeed);
            commands[Keys.S] = new MoveCommand(0, 1 * movingSpeed);

            commands[Keys.G] = new ResizeCommand(-1 * sizingSpeed, 0);
            commands[Keys.J] = new ResizeCommand(1 * sizingSpeed, 0);
            commands[Keys.Y] = new ResizeCommand(0, 1 * sizingSpeed);
            commands[Keys.H] = new ResizeCommand(0, -1 * sizingSpeed);

            commands[Keys.Delete] = new DeleteShapeCommand(shapes, g, pictureBox1);

        }

        enum SelectedShape
        {
            Circle,
            Square,
            Triangle,
            Section
        }
        int shapesIndex = 0;

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
                IShape? s = null;
                ICommand cmd = new CreateShapeCommand(mouseX, mouseY, (int)sh, shapesIndex, shapes, g, pictureBox1);
                cmd.execute(s);
                history.Push(cmd);
                s = cmd.GetShape();
                // = shapeCreator.createShapeMethod(mouseX, mouseY, (int)sh);
                if (sh == SelectedShape.Section)
                    s.resize(0, -(desiredDimension - 1));

                //s.IsSelected = true;
                //shapes.Add(s);
                shapesIndex++;
            }
            foreach (IShape c in shapes)
                c.draw(g);

            pictureBox1.Refresh();

        }


        public void deleteSelected()
        {
            ICommand cmd = new DeleteShapeCommand(shapes, g, pictureBox1);
            cmd.execute(null);
            history.Push(cmd);
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
                c.checkOrFixOutOfBounds(boundsX, boundsY, SystemInformation.MouseSpeed);
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

            if (e.KeyCode == Keys.Delete) deleteSelected();

            if (Form.ModifierKeys == Keys.Control)
            {
                if (d++ % 2 == 0) label1.Text += "<";
                if (d > 8) { label1.Text = "|Ctrl|"; d = 0; }
            }
            MoveAndResizeHandle(e);

        }

        Stack<ICommand> history = new();
        Stack<ICommand> canceledHistory = new();
        Dictionary<Keys, ICommand> commands = new();

        private void MoveAndResizeHandle(KeyEventArgs e)
        {
            ICommand? cmd = null;
            try
            { cmd = commands[e.KeyCode]; }
            catch (Exception)
            { Console.WriteLine("no key"); }

            if (cmd != null)
            {
                g.Clear(Color.PaleTurquoise);
                foreach (IShape shape in shapes)
                {
                    if (shape.IsSelected)
                    {

                        if ((cmd is ResizeCommand rc) &&
                            (((rc._x > 0 && shape.size.Width >= boundsX) ||
                             (rc._y > 0 && shape.size.Height >= boundsY)) &&
                             (shape is not shapeGroup)))
                        {
                            Console.WriteLine("AAAAAAAAAAa");
                            shape.draw(g);
                            break;
                        }

                        ICommand newCmd = cmd.clone();
                        newCmd.execute(shape);
                        history.Push(newCmd);
                        shape.checkOrFixOutOfBounds(boundsX, boundsY, movingSpeed);
                    }
                    shape.draw(g);
                }
                canceledHistory.Clear();
                pictureBox1.Refresh();
            }
            if (e.KeyCode == Keys.Z)
            {
                Undo();
            }
            if (e.KeyCode == Keys.X)
            {
                Redo();
            }
        }

        public void Undo()
        {
            if (history.Count < 1) return;
            ICommand lastCmd = history.Pop();
            lastCmd.unexecute();
            canceledHistory.Push(lastCmd);

            g.Clear(Color.PaleTurquoise);
            foreach (IShape shape in shapes) shape.draw(g);
            pictureBox1.Refresh();
        }
        public void Redo()
        {
            if (canceledHistory.Count < 1) return;
            ICommand lastCmd = canceledHistory.Pop();
            lastCmd.execute(lastCmd.GetShape());
            history.Push(lastCmd);

            g.Clear(Color.PaleTurquoise);
            foreach (IShape shape in shapes) shape.draw(g);
            pictureBox1.Refresh();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label5.Text = e.Location.ToString();
        }

        #region ui_toolbar
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
                    ICommand cmd = new RecolorCommand(colorDialog1.Color);
                    cmd.execute(shape);
                    history.Push(cmd);
                    //shape.recolor(colorDialog1.Color);
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
        #endregion
        StreamReader sr;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "C:\\Users\\vadim\\source\\repos\\Lab7_oop\\data.txt";
            int count; IShape shape;
            try
            {
                sr = new(filename);
                count = int.Parse(sr.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    string code = sr.ReadLine();
                    shape = shapeCreator.createShapeMethod(0, 0, code);
                    if (shape != null)
                    {
                        shape.Load(sr);
                        shapes.Add(shape);
                    }                        
                }
            }
            finally
            {
                sr?.Close();
            }
            foreach (IShape c in shapes) c.draw(g);
            pictureBox1.Refresh();
        }
        StreamWriter sw;
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                sw = new("C:\\Users\\vadim\\source\\repos\\Lab7_oop\\data.txt");
                sw.WriteLine(shapes.size.ToString());
                foreach (IShape shape in shapes)
                    shape.Save(sw);
            }
            finally
            {
                sw?.Close();
            }   
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            groupBuffer = new();
            IShape group0 = new shapeGroup();
            IShape group1 = new shapeGroup();
            foreach (IShape shape in shapes)
                if (shape.IsSelected)
                    groupBuffer.Add(shape);
            deleteSelected();

            foreach (IShape shape in groupBuffer)
                ((shapeGroup)group1).AddShape(shape);

            ((shapeGroup)group0).AddShape(group1);
            group0.draw(g);
            shapes.Add(group0);
            pictureBox1.Refresh();
        }

        private void btnUngroup_Click(object sender, EventArgs e)
        {
            foreach (IShape s in shapes)
            {
                if (s.IsSelected && s is shapeGroup group)
                    group.Ungroup(shapes);
            }
        }

        
    }
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
            string[] data = sr.ReadLine().Split(' ');
            position = new(int.Parse(data[0]), int.Parse(data[1]));
            offsetX = int.Parse(data[2]); offsetY = int.Parse(data[3]);
            colorMain = Color.FromArgb(
                int.Parse(data[4]),
                int.Parse(data[5]),
                int.Parse(data[6]));

        }
    }

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

    public class triangle : IShape
    {
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

        public override void Save(StreamWriter sw)
        {
            throw new NotImplementedException();
        }

        public override void Load(StreamReader sr)
        {
            throw new NotImplementedException();
        }
    }


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
            //Console.WriteLine(distanceTop + "\t" + distanceBot + "\t" + distanceLeft + "\t" + distanceRight + "\t");
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


    public abstract class ICommand
    {
        public abstract void execute(IShape shape);
        public abstract void unexecute();
        public abstract ICommand clone();
        public abstract IShape GetShape();
    }

    public class ResizeCommand : ICommand
    {
        private IShape? _shape;
        public int _x, _y;
        public ResizeCommand(int x, int y)
        {
            _x = x;
            _y = y;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new ResizeCommand(_x, _y);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute resize");
            _shape = shape;
            _shape?.resize(_x, _y);
        }

        public override void unexecute()
        {
            Console.WriteLine("unexec resize");
            _shape?.resize(-_x, -_y);
        }

        public override IShape GetShape() => _shape;
    }

    public class MoveCommand : ICommand
    {
        private IShape? _shape;
        private int _x, _y;
        public MoveCommand(int x, int y)
        {
            _x = x;
            _y = y;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new MoveCommand(_x, _y);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute move");
            _shape = shape;
            _shape?.move(_x, _y);
        }

        public override IShape GetShape() => _shape;


        public override void unexecute()
        {
            Console.WriteLine("unexec move");
            _shape?.move(-_x, -_y);
        }
    }

    public class RecolorCommand : ICommand
    {
        private IShape? _shape;
        private Color _oldColor;
        private Color _newColor;
        public RecolorCommand(Color c)
        {
            _newColor = c;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new RecolorCommand(_newColor);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute recolor");
            _shape = shape;
            _oldColor = _shape.colorMain;
            _shape?.recolor(_newColor);
        }

        public override IShape GetShape() => _shape;

        public override void unexecute()
        {
            Console.WriteLine("unexecute recolor");
            _shape?.recolor(_oldColor);
        }
    }

    public class CreateShapeCommand : ICommand
    {
        private IShape? _shape;
        private Container<IShape> _shapes;
        public int _x, _y, _type, _index;
        Graphics _g;
        PictureBox _pb;
        public CreateShapeCommand(int x, int y, int type, int index, Container<IShape> shapes, Graphics g, PictureBox pb)
        {
            _x = x;
            _y = y;
            _type = type;
            _index = index;
            _shapes = shapes;
            _g = g;
            _pb = pb;
        }
        public override ICommand clone()
        {
            return new CreateShapeCommand(_x, _y, _type, _index, _shapes, _g, _pb);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("createShapeCommand execute");
            _shape = shapeCreator.createShapeMethod(_x, _y, _type);
            _shape.IsSelected = true;
            _shapes.Add(_shape);

            _g.Clear(Color.PaleTurquoise);
            foreach (IShape s in _shapes)
                s.draw(_g);
            _pb.Refresh();
        }

        public override IShape GetShape() => _shape;
        public override void unexecute()
        {
            Console.WriteLine("createShapeCommand unexecute");
            _shapes.RemoveAt(_index);

            _g.Clear(Color.PaleTurquoise);
            foreach (IShape s in _shapes)
                s.draw(_g);
            _pb.Refresh();
        }
    }
    public class DeleteShapeCommand : ICommand
    {
        private IShape? _shape;
        private Container<IShape> _shapes, saveBuffer, deleteBuffer;
        Graphics _g;
        PictureBox _pb;
        public DeleteShapeCommand(Container<IShape> shapes, Graphics g, PictureBox pb)
        {
            _shapes = shapes;
            _g = g;
            _pb = pb;
        }
        public override ICommand clone()
        {
            return new DeleteShapeCommand(_shapes, _g, _pb);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute delete shape cmd ");
            saveBuffer = new();
            deleteBuffer = new();
            foreach (IShape c in _shapes)
            {
                //saving all non-selected
                if (!c.IsSelected) saveBuffer.Add(c);
                else deleteBuffer.Add(c);
            }
            _shapes.Clear();
            _g.Clear(Color.PaleTurquoise);
            foreach (IShape c in saveBuffer)
                _shapes.Add(c);
            foreach (IShape c in _shapes)
                c.draw(_g);
            Console.WriteLine("deleted");
            _pb.Refresh();
        }

        public override IShape GetShape() => _shape;
        public override void unexecute()
        {
            Console.WriteLine("unexecute delete shape cmd ");
            foreach (IShape s in deleteBuffer)
            {
                _shapes.Add(s);
            }
            foreach (IShape s in _shapes)
                s.draw(_g);
            _pb.Refresh();

        }
    }

    public class shapeCreator
    {

        public static IShape createShapeMethod(int x, int y, int type)
        {
            if (type == 0) return new circle(x, y);
            if (type == 1) return new square(x, y);
            if (type == 2) return new triangle(x, y);
            if (type == 3) return new square(x, y);
            return new circle(x, y);
        }
        public static IShape createShapeMethod(int x, int y, int type, int dimensions)
        {
            if (type == 0) return new circle(x, y, dimensions);
            if (type == 1) return new square(x, y, dimensions);
            if (type == 2) return new triangle(x, y, dimensions);
            if (type == 3) return new square(x, y, dimensions);
            return new circle(x, y);
        }
        public static IShape createShapeMethod(int x, int y, string s)
        {
            if (s == "Circle") return new circle(x, y);
            if (s == "Square") return new square(x, y);
            if (s == "Triangle") return new triangle(x, y);
            if (s == "Section") return new square(x, y, 1); 
            return new circle(x, y);
        }
    }


}
