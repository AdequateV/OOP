
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab8_oop
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            checkBoxJoints = new CheckBox();
            label4 = new Label();
            label5 = new Label();
            colorDialog1 = new ColorDialog();
            toolStrip1 = new ToolStrip();
            toolStripButton2 = new ToolStripDropDownButton();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripButton1 = new ToolStripDropDownButton();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            unselectShapesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            circleellipseToolStripMenuItem = new ToolStripMenuItem();
            squarerectangleToolStripMenuItem = new ToolStripMenuItem();
            triangleToolStripMenuItem = new ToolStripMenuItem();
            sectionToolStripMenuItem = new ToolStripMenuItem();
            toolStripButton3 = new ToolStripButton();
            toolStripButton4 = new ToolStripDropDownButton();
            helpToolStripMenuItem = new ToolStripMenuItem();
            sendFeedbackToolStripMenuItem = new ToolStripMenuItem();
            goToSourceToolStripMenuItem = new ToolStripMenuItem();
            btnGroup = new Button();
            btnUngroup = new Button();
            checkBox1 = new CheckBox();
            treeView1 = new TreeView();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.PaleTurquoise;
            pictureBox1.Dock = DockStyle.Bottom;
            pictureBox1.Location = new Point(0, 114);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(834, 375);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(503, 36);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 2;
            label1.Text = "|Ctrl|";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(72, 25);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // checkBoxJoints
            // 
            checkBoxJoints.AutoSize = true;
            checkBoxJoints.Checked = true;
            checkBoxJoints.CheckState = CheckState.Checked;
            checkBoxJoints.Location = new Point(148, 57);
            checkBoxJoints.Name = "checkBoxJoints";
            checkBoxJoints.Size = new Size(134, 19);
            checkBoxJoints.TabIndex = 4;
            checkBoxJoints.TabStop = false;
            checkBoxJoints.Text = "select all at joints [Z]";
            checkBoxJoints.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 25);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 11;
            label4.Text = "label4";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(134, 25);
            label5.Name = "label5";
            label5.Size = new Size(38, 15);
            label5.TabIndex = 13;
            label5.Text = "label5";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton2, toolStripButton1, toolStripSeparator1, toolStripDropDownButton1, toolStripButton3, toolStripButton4 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(834, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.ShowDropDownArrow = false;
            toolStripButton2.Size = new Size(29, 22);
            toolStripButton2.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(121, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(121, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(121, 22);
            saveAsToolStripMenuItem.Text = "Save as...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DropDownItems.AddRange(new ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem, clearToolStripMenuItem, unselectShapesToolStripMenuItem });
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.ShowDropDownArrow = false;
            toolStripButton1.Size = new Size(31, 22);
            toolStripButton1.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.Size = new Size(158, 22);
            undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.Size = new Size(158, 22);
            redoToolStripMenuItem.Text = "Redo";
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(158, 22);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // unselectShapesToolStripMenuItem
            // 
            unselectShapesToolStripMenuItem.Name = "unselectShapesToolStripMenuItem";
            unselectShapesToolStripMenuItem.Size = new Size(158, 22);
            unselectShapesToolStripMenuItem.Text = "Unselect shapes";
            unselectShapesToolStripMenuItem.Click += unselectShapesToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { circleellipseToolStripMenuItem, squarerectangleToolStripMenuItem, triangleToolStripMenuItem, sectionToolStripMenuItem });
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(94, 22);
            toolStripDropDownButton1.Text = "Choose shape";
            // 
            // circleellipseToolStripMenuItem
            // 
            circleellipseToolStripMenuItem.Name = "circleellipseToolStripMenuItem";
            circleellipseToolStripMenuItem.Size = new Size(170, 22);
            circleellipseToolStripMenuItem.Text = "Circle (ellipse)";
            circleellipseToolStripMenuItem.Click += circleellipseToolStripMenuItem_Click;
            // 
            // squarerectangleToolStripMenuItem
            // 
            squarerectangleToolStripMenuItem.Name = "squarerectangleToolStripMenuItem";
            squarerectangleToolStripMenuItem.Size = new Size(170, 22);
            squarerectangleToolStripMenuItem.Text = "Square (rectangle)";
            squarerectangleToolStripMenuItem.Click += squarerectangleToolStripMenuItem_Click;
            // 
            // triangleToolStripMenuItem
            // 
            triangleToolStripMenuItem.Name = "triangleToolStripMenuItem";
            triangleToolStripMenuItem.Size = new Size(170, 22);
            triangleToolStripMenuItem.Text = "Triangle";
            triangleToolStripMenuItem.Click += triangleToolStripMenuItem_Click;
            // 
            // sectionToolStripMenuItem
            // 
            sectionToolStripMenuItem.Name = "sectionToolStripMenuItem";
            sectionToolStripMenuItem.Size = new Size(170, 22);
            sectionToolStripMenuItem.Text = "Section";
            sectionToolStripMenuItem.Click += sectionToolStripMenuItem_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(74, 22);
            toolStripButton3.Text = "Recolor to...";
            toolStripButton3.Click += toolStripButton3_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton4.DropDownItems.AddRange(new ToolStripItem[] { helpToolStripMenuItem, sendFeedbackToolStripMenuItem, goToSourceToolStripMenuItem });
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.ShowDropDownArrow = false;
            toolStripButton4.Size = new Size(39, 22);
            toolStripButton4.Text = "More";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(151, 22);
            helpToolStripMenuItem.Text = "Help";
            // 
            // sendFeedbackToolStripMenuItem
            // 
            sendFeedbackToolStripMenuItem.Name = "sendFeedbackToolStripMenuItem";
            sendFeedbackToolStripMenuItem.Size = new Size(151, 22);
            sendFeedbackToolStripMenuItem.Text = "Send feedback";
            // 
            // goToSourceToolStripMenuItem
            // 
            goToSourceToolStripMenuItem.Name = "goToSourceToolStripMenuItem";
            goToSourceToolStripMenuItem.Size = new Size(151, 22);
            goToSourceToolStripMenuItem.Text = "Go to source";
            // 
            // btnGroup
            // 
            btnGroup.Location = new Point(188, 28);
            btnGroup.Name = "btnGroup";
            btnGroup.Size = new Size(94, 23);
            btnGroup.TabIndex = 16;
            btnGroup.Text = "Group selected";
            btnGroup.UseVisualStyleBackColor = true;
            btnGroup.Click += btnGroup_Click;
            // 
            // btnUngroup
            // 
            btnUngroup.Location = new Point(290, 28);
            btnUngroup.Name = "btnUngroup";
            btnUngroup.Size = new Size(108, 23);
            btnUngroup.TabIndex = 18;
            btnUngroup.Text = "Ungroup selected";
            btnUngroup.UseVisualStyleBackColor = true;
            btnUngroup.Click += btnUngroup_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(315, 57);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(90, 19);
            checkBox1.TabIndex = 19;
            checkBox1.Text = "arrow mode";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(701, 12);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(121, 80);
            treeView1.TabIndex = 20;
            treeView1.BeforeSelect += treeView1_BeforeSelect;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // button1
            // 
            button1.Location = new Point(404, 28);
            button1.Name = "button1";
            button1.Size = new Size(93, 23);
            button1.TabIndex = 21;
            button1.Text = "remove arrows";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 489);
            Controls.Add(button1);
            Controls.Add(treeView1);
            Controls.Add(checkBox1);
            Controls.Add(btnUngroup);
            Controls.Add(btnGroup);
            Controls.Add(toolStrip1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(checkBoxJoints);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            KeyPreview = true;
            Name = "Form1";
            Text = "Form1";
            ResizeEnd += Form1_ResizeEnd;
            KeyDown += Form1_KeyDown;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private CheckBox checkBoxJoints;
        private Label label4;
        private Label label5;
        private ColorDialog colorDialog1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripButton2;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripDropDownButton toolStripButton1;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripButton toolStripButton3;
        private ToolStripMenuItem circleellipseToolStripMenuItem;
        private ToolStripMenuItem squarerectangleToolStripMenuItem;
        private ToolStripMenuItem triangleToolStripMenuItem;
        private ToolStripMenuItem sectionToolStripMenuItem;
        private ToolStripMenuItem unselectShapesToolStripMenuItem;
        private ToolStripDropDownButton toolStripButton4;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem sendFeedbackToolStripMenuItem;
        private ToolStripMenuItem goToSourceToolStripMenuItem;
        private Button btnGroup;
        private Button btnUngroup;
        private CheckBox checkBox1;
        private TreeView treeView1;
        private Button button1;
    }
}