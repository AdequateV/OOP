
using Lab4_mvc_oop.Properties;

namespace Lab4_mvc_oop
{
    public partial class Form1 : Form
    {
        Model model;
        public Form1()
        {
            InitializeComponent();
            model = new Model();

            //when observer invokes, notify me (thru UpdateFromModel)
            model.observer += new EventHandler(UpdateFromModel);
            Application.ApplicationExit += Application_AppExit;
            UpdateFromModel(default, default);
        }

        private void Application_AppExit(object? sender, EventArgs e)
        {
            model.Save();
        }

        private void UpdateFromModel(object sender, EventArgs e)
        {
            int a = model.getA();
            int b = model.getB();
            int c = model.getC();

            textBox1.Text = a.ToString();
            textBox2.Text = b.ToString();
            textBox3.Text = c.ToString();
            numericUpDown1.Value = a;
            numericUpDown2.Value = b;
            numericUpDown3.Value = c;
            trackBar1.Value = a;
            trackBar2.Value = b;
            trackBar3.Value = c;
            
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            model.SetFirst(Int32.Parse(textBox1.Text));
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            model.SetSecond(Int32.Parse(textBox2.Text));
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            model.SetThird(Int32.Parse(textBox3.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                model.getA().ToString() + " " +
                model.getB().ToString() + " " +
                model.getC().ToString());
        }
        #region trackbar + numeric ValueChanged
        private void trackBar1_ValueChanged(object sender, EventArgs e) =>
            model.SetFirst((Int32)trackBar1.Value);

        private void trackBar2_ValueChanged(object sender, EventArgs e) =>
            model.SetSecond((Int32)trackBar2.Value);

        private void trackBar3_ValueChanged(object sender, EventArgs e) =>
            model.SetThird((Int32)trackBar3.Value);


        private void numericUpDown1_ValueChanged(object sender, EventArgs e) =>
            model.SetFirst((Int32)numericUpDown1.Value);        

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) =>
            model.SetSecond((Int32)numericUpDown2.Value);
        

        private void numericUpDown3_ValueChanged(object sender, EventArgs e) =>
            model.SetThird((Int32)numericUpDown3.Value);
        #endregion
    }
    public class Model
    {
        private int First;
        private int Second;
        private int Third;
        public readonly int MAX_VAL = 100;
        public readonly int MIN_VAL = 0;
        public Model()
        {
            First  = Settings.Default.valueA;
            Second = Settings.Default.valueB;
            Third  = Settings.Default.valueC;

        }
        public EventHandler observer;
        public int getA() { return First; }
        public int getB() { return Second; }
        public int getC() { return Third; }

        public void SetFirst(int v)
        {
            First = v;
            if (MIN_VAL >= v)
            {
                First = MIN_VAL;
                v = First;
            }
            if (v >= MAX_VAL)
            {
                First = MAX_VAL;
                v = First;
            }
            
            if (v > Second)
            {
                if (v > Third)
                    Second = (Third = First);
                else
                    Second = v;
            }
            
            observer.Invoke(this, null);
        }
        public void SetSecond(int v)
        {
            if (MIN_VAL >= v)
            {
                observer.Invoke(this, null); return;
            }
                
            if (v >= MAX_VAL)
            {
                observer.Invoke(this, null); return;
            }

            if (First <= v && v <= Third)
                Second = v;
            else
            {
                if (v > Third) Second = Third;
                if (v < First) Second = First;
            }
            observer.Invoke(this, null);
        }
        public void SetThird(int v)
        {
            Third = v;
            if (MIN_VAL >= v)            
                v = Third = MIN_VAL;
            
            if (v >= MAX_VAL)            
                v = Third = MAX_VAL;          

            if (Second > v)
            {
                if (First > v)
                    First = (Second = Third);
                else
                    Second = Third;
                
            }
            observer.Invoke(this, null);
        }
        public void Save()
        {
            Settings.Default.valueA = First;
            Settings.Default.valueB = Second;
            Settings.Default.valueC = Third;
            Settings.Default.Save();
        }

    }
}