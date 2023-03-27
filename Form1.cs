namespace Lab4_wforms_oop
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        List<mcircle> mcircles;
        List<mcircle> deleteBuffer;
        int mouseX, mouseY;
        int radius = 50;
        public Form1()
        {
            InitializeComponent();
            bm = new(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.PaleTurquoise);
            pictureBox1.Image = bm;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            mcircles = new List<mcircle>();
            
        }
        
        Pen pen = new(Color.Black, 1);

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
		
            bool multi = checkBoxMulti.Checked && Form.ModifierKeys == Keys.Control;
            bool joints = checkBoxJoints.Checked;
            bool createNew = true;
            foreach(mcircle c in mcircles)
            {
                if(c.check(mouseX, mouseY))
                {
                    createNew = false;
                    if (multi || joints)
                    {
                        if (!multi)
                            foreach (mcircle c2 in mcircles)
                                if (!c2.check(mouseX, mouseY))
                                    c2.IsSelected = false;
                        
                        c.IsSelected = true;
                        continue;
                    }

                    foreach (mcircle c1 in mcircles)
                        c1.IsSelected = false;
                    c.IsSelected = true;
                }
            }
            if (createNew)
            {
                if (!multi)
                    foreach (mcircle c in mcircles)
                        c.IsSelected = false;

                var circle = new mcircle(mouseX, mouseY, radius, g);
                circle.IsSelected = true;
                mcircles.Add(circle);
            }
            foreach (mcircle c in mcircles)
                c.draw();
            
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
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                label1.Text += "del";
                deleteBuffer = new List<mcircle>();

                foreach (mcircle c in mcircles)
                    if (!c.IsSelected)
                        deleteBuffer.Add(c); //saved all non-selected
                mcircles.Clear();
                g.Clear(Color.PaleTurquoise);
                foreach (mcircle c in deleteBuffer)
                    mcircles.Add(c);

                foreach (mcircle c in mcircles)
                    c.draw();
                label1.Text += "j";
                pictureBox1.Refresh();
            }
            if(Form.ModifierKeys == Keys.Control)
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
                foreach(mcircle c in mcircles)
                {
                    c.IsSelected = false;
                    c.draw();
                }
                pictureBox1.Refresh();
            }
            if(e.KeyCode == Keys.Z)
            {
                checkBoxJoints.Checked = !checkBoxJoints.Checked;
            }
            if(e.KeyCode == Keys.X)
            {
                checkBoxMulti.Checked = !checkBoxMulti.Checked;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        { 
            
        }

        

        
        
    }
    public class mcircle
    {
        public Point position { get; set; }
        public int radius { get; set; }
        public bool IsSelected { get; set; } = false;
        
        Graphics g;
        public mcircle(int x, int y, int r, Graphics _g)
        {
            position = new Point(x, y);
            radius = r; 
            g = _g;
        }
        public void draw()
        {
            var pen = new Pen(Color.Yellow, 2);
            var brush = new SolidBrush(Color.FromArgb(128, 255, 204, 153));

            if (IsSelected)
            {
                pen = new Pen(Color.Black, 2);
                brush = new SolidBrush(Color.FromArgb(128, 166, 166, 166)); ;
            }
            g.DrawEllipse(pen,   position.X - radius, position.Y - radius, radius * 2, radius * 2);
            g.FillEllipse(brush, position.X - radius, position.Y - radius, radius * 2, radius * 2);
        }
        public bool check(int x, int y)
        {
            if ((Math.Pow(x - position.X, 2) + Math.Pow(y - position.Y, 2)) < radius * radius)
            {
                IsSelected = true;
                return true;
            }
            return false;
        }
    }
    public class mcircles
    {
        public List<mcircle> CCircles;
        public mcircles()
        {
            CCircles = new List<mcircle>();
        }
        public void UnSelectAll()
        {
            foreach(mcircle c in CCircles)
            {
                c.IsSelected = false;
            }
        }
        public void DrawAll()
        {
            foreach (mcircle c in CCircles)
                c.draw();
        }

}
}