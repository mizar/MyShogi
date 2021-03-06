﻿namespace MyShogi.View.Win2D
{
    partial class EngineConsiderationMainControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EngineConsiderationMainControl));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.engineConsiderationControl1 = new MyShogi.View.Win2D.EngineConsiderationControl();
            this.engineConsiderationControl2 = new MyShogi.View.Win2D.EngineConsiderationControl();
            this.toolStrip1 = new MyShogi.View.Win2D.ToolStripEx();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.miniShogiBoard1 = new MyShogi.View.Win2D.MiniShogiBoard();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer2.Panel2.Controls.Add(this.miniShogiBoard1);
            this.splitContainer2.Size = new System.Drawing.Size(763, 235);
            this.splitContainer2.SplitterDistance = 571;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 1;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            this.splitContainer2.Resize += new System.EventHandler(this.splitContainer2_Resize);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.engineConsiderationControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.engineConsiderationControl2);
            this.splitContainer1.Size = new System.Drawing.Size(571, 235);
            this.splitContainer1.SplitterDistance = 126;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // engineConsiderationControl1
            // 
            this.engineConsiderationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineConsiderationControl1.EngineName = "";
            this.engineConsiderationControl1.Location = new System.Drawing.Point(0, 0);
            this.engineConsiderationControl1.Margin = new System.Windows.Forms.Padding(1);
            this.engineConsiderationControl1.Name = "engineConsiderationControl1";
            this.engineConsiderationControl1.RootSfen = null;
            this.engineConsiderationControl1.Size = new System.Drawing.Size(571, 126);
            this.engineConsiderationControl1.SortRanking = false;
            this.engineConsiderationControl1.TabIndex = 0;
            this.engineConsiderationControl1.TabStop = false;
            // 
            // engineConsiderationControl2
            // 
            this.engineConsiderationControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineConsiderationControl2.EngineName = "";
            this.engineConsiderationControl2.Location = new System.Drawing.Point(0, 0);
            this.engineConsiderationControl2.Margin = new System.Windows.Forms.Padding(1);
            this.engineConsiderationControl2.Name = "engineConsiderationControl2";
            this.engineConsiderationControl2.RootSfen = null;
            this.engineConsiderationControl2.Size = new System.Drawing.Size(571, 107);
            this.engineConsiderationControl2.SortRanking = false;
            this.engineConsiderationControl2.TabIndex = 0;
            this.engineConsiderationControl2.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ClickAction = MyShogi.View.Win2D.ClickActionEnum.MA_NOACTIVATE;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 210);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(190, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "閉";
            this.toolStripButton5.ToolTipText = "このミニ盤面を閉じます。";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "◀";
            this.toolStripButton1.ToolTipText = "最初の局面に戻る";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "◁";
            this.toolStripButton2.ToolTipText = "一手戻る";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "▷";
            this.toolStripButton3.ToolTipText = "一手進む";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "▶";
            this.toolStripButton4.ToolTipText = "最後の局面に進む";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "転";
            this.toolStripButton6.ToolTipText = "盤面を反転します。";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // miniShogiBoard1
            // 
            this.miniShogiBoard1.BoardData = null;
            this.miniShogiBoard1.Location = new System.Drawing.Point(2, 10);
            this.miniShogiBoard1.Margin = new System.Windows.Forms.Padding(1);
            this.miniShogiBoard1.Name = "miniShogiBoard1";
            this.miniShogiBoard1.Size = new System.Drawing.Size(185, 202);
            this.miniShogiBoard1.TabIndex = 1;
            this.miniShogiBoard1.TabStop = false;
            // 
            // EngineConsiderationMainControl
            // 
            this.Controls.Add(this.splitContainer2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EngineConsiderationMainControl";
            this.Size = new System.Drawing.Size(763, 235);
            this.Resize += new System.EventHandler(this.EngineConsiderationDialog_Resize);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MyShogi.View.Win2D.EngineConsiderationControl engineConsiderationControl1;
        private MyShogi.View.Win2D.EngineConsiderationControl engineConsiderationControl2;
        private MyShogi.View.Win2D.MiniShogiBoard miniShogiBoard1;
        private MyShogi.View.Win2D.ToolStripEx toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
    }
}
