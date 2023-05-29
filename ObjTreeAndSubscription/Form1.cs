using Lab7_oop.commands;
using Lab7_oop.utilities;
using System.Drawing.Drawing2D;

namespace Lab7_oop
{

    public partial class Form1 : Form
    {
        readonly string path = "C:\\Users\\vadim\\source\\repos\\Lab7_oop\\data.txt";
        private Bitmap bm;
        private Graphics g;
        private List<IShape> shapes;
        private int mouseX, mouseY;
        private int toolbarHeight = 100;
        SelectedShape sh;
        private int boundsX, boundsY;
        private int desiredDimension = 25;
        //mouse
        private bool multi, joints, createNew;
        //settings
        private int movingSpeed = 1;
        private int sizingSpeed = 1;
        private StreamWriter sw;
        private Stack<ICommand> history = new();
        private Stack<ICommand> canceledHistory = new();
        private Dictionary<Keys, ICommand> commands = new();
        public Form1()
        {
            InitializeComponent();
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.PaleTurquoise);
            pictureBox1.Image = bm;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DoubleBuffered = true;
            shapes = new();

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

            commands[Keys.Delete] = new DeleteShapeCommand(shapes);

            UpdateBoundsForShapes();
        }


        public void UpdateBoundsForShapes()
        {
            foreach (var s in shapes)
            {
                s.updBounds(boundsX, boundsY);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            multi = (Form.ModifierKeys == Keys.Control);
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
                ICommand cmd = new CreateShapeCommand(mouseX, mouseY, (int)sh, shapes);
                cmd.execute(s);
                history.Push(cmd);
                s = cmd.GetShape();
                
                if (sh == SelectedShape.Section)
                    s.resize(0, -(desiredDimension - 1));
            }
            foreach (IShape c in shapes)
                c.draw(g);

            pictureBox1.Refresh();
            UpdateBoundsForShapes();

        }


        public void deleteSelected()
        {
            ICommand cmd = new DeleteShapeCommand(shapes);
            cmd.execute(null);
            history.Push(cmd);
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width, ClientSize.Height - toolbarHeight);
            boundsX = pictureBox1.Size.Width;
            boundsY = pictureBox1.Size.Height;
            UpdateBoundsForShapes();
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            pictureBox1.Image = bm;
            foreach (IShape c in shapes)
            {
                c.draw(g);
                c.checkOrFixOutOfBounds();
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
            UpdateBoundsForShapes();

        }



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
                        shape.checkOrFixOutOfBounds();
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
            lastCmd.unexecute(); // 
            canceledHistory.Push(lastCmd);

            g.Clear(Color.PaleTurquoise);
            foreach (IShape shape in shapes) shape.draw(g);
            pictureBox1.Refresh();
        }
        public void Redo()
        {
            if (canceledHistory.Count < 1) return;
            ICommand lastCmd = canceledHistory.Pop();
            lastCmd.execute(lastCmd.GetShape()); //
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShapeArray array = new();
            wadimShapeFactory f = new();
            array.LoadShapes(path, f);
            shapes = array.GetShapes();

            foreach (IShape c in shapes) c.draw(g);
            pictureBox1.Refresh();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                sw = new(path);
                sw.WriteLine(shapes.Count.ToString());
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
            GroupUp();
            UpdateBoundsForShapes();
        }
        private void btnUngroup_Click(object sender, EventArgs e)
        {
            UnGroup();
            UpdateBoundsForShapes();
        }
        public void UnGroup()
        {
            List<IShape> b = new();
            ICommand cmd = new UnGroupCommand(shapes);
            foreach (IShape s in shapes)
                if (s is shapeGroup && s.IsSelected)
                    b.Add(s);
            foreach (IShape s in b)
            {
                cmd.execute(s);
                history.Push(cmd);
            }
        }

        public void GroupUp()
        {
            shapeGroup g = new();
            ICommand cmd = new GroupCommand(shapes);
            //List<ICommand> cmdList = new();
            cmd.execute(g);
            //cmdList.Add(cmd);
            history.Push(cmd);
        }

       
    }



    enum SelectedShape
    {
        Circle,
        Square,
        Triangle,
        Section
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
