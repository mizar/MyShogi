namespace MyShogi.View.Win2Dx
{
    partial class BoardForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPos1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPos2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPos3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normal1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverse1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normal2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverse2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normal3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverse3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normal4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverse4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normal5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverse5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.handOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigBLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigBRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigULToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pieceCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneCharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twoCharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.coordCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nONEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nORMALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aRABICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cHESSLIKEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1333, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setPos1ToolStripMenuItem,
            this.setPos2ToolStripMenuItem,
            this.setPos3ToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // setPos1ToolStripMenuItem
            // 
            this.setPos1ToolStripMenuItem.Name = "setPos1ToolStripMenuItem";
            this.setPos1ToolStripMenuItem.Size = new System.Drawing.Size(160, 30);
            this.setPos1ToolStripMenuItem.Text = "SetPos1";
            this.setPos1ToolStripMenuItem.Click += new System.EventHandler(this.setPos1);
            // 
            // setPos2ToolStripMenuItem
            // 
            this.setPos2ToolStripMenuItem.Name = "setPos2ToolStripMenuItem";
            this.setPos2ToolStripMenuItem.Size = new System.Drawing.Size(160, 30);
            this.setPos2ToolStripMenuItem.Text = "SetPos2";
            this.setPos2ToolStripMenuItem.Click += new System.EventHandler(this.setPos2);
            // 
            // setPos3ToolStripMenuItem
            // 
            this.setPos3ToolStripMenuItem.Name = "setPos3ToolStripMenuItem";
            this.setPos3ToolStripMenuItem.Size = new System.Drawing.Size(160, 30);
            this.setPos3ToolStripMenuItem.Text = "SetPos3";
            this.setPos3ToolStripMenuItem.Click += new System.EventHandler(this.setPos3);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraToolStripMenuItem,
            this.handOrderToolStripMenuItem,
            this.pieceCharsetToolStripMenuItem,
            this.coordCharsetToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // cameraToolStripMenuItem
            // 
            this.cameraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normal1ToolStripMenuItem,
            this.reverse1ToolStripMenuItem,
            this.normal2ToolStripMenuItem,
            this.reverse2ToolStripMenuItem,
            this.normal3ToolStripMenuItem,
            this.reverse3ToolStripMenuItem,
            this.normal4ToolStripMenuItem,
            this.reverse4ToolStripMenuItem,
            this.normal5ToolStripMenuItem,
            this.reverse5ToolStripMenuItem});
            this.cameraToolStripMenuItem.Name = "cameraToolStripMenuItem";
            this.cameraToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.cameraToolStripMenuItem.Text = "camera";
            // 
            // normal1ToolStripMenuItem
            // 
            this.normal1ToolStripMenuItem.Name = "normal1ToolStripMenuItem";
            this.normal1ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.normal1ToolStripMenuItem.Text = "Normal1";
            this.normal1ToolStripMenuItem.Click += new System.EventHandler(this.viewNormal1);
            // 
            // reverse1ToolStripMenuItem
            // 
            this.reverse1ToolStripMenuItem.Name = "reverse1ToolStripMenuItem";
            this.reverse1ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.reverse1ToolStripMenuItem.Text = "Reverse1";
            this.reverse1ToolStripMenuItem.Click += new System.EventHandler(this.viewReverse1);
            // 
            // normal2ToolStripMenuItem
            // 
            this.normal2ToolStripMenuItem.Name = "normal2ToolStripMenuItem";
            this.normal2ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.normal2ToolStripMenuItem.Text = "Normal2";
            this.normal2ToolStripMenuItem.Click += new System.EventHandler(this.viewNormal2);
            // 
            // reverse2ToolStripMenuItem
            // 
            this.reverse2ToolStripMenuItem.Name = "reverse2ToolStripMenuItem";
            this.reverse2ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.reverse2ToolStripMenuItem.Text = "Reverse2";
            this.reverse2ToolStripMenuItem.Click += new System.EventHandler(this.viewReverse2);
            // 
            // normal3ToolStripMenuItem
            // 
            this.normal3ToolStripMenuItem.Name = "normal3ToolStripMenuItem";
            this.normal3ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.normal3ToolStripMenuItem.Text = "Normal3";
            this.normal3ToolStripMenuItem.Click += new System.EventHandler(this.viewNormal3);
            // 
            // reverse3ToolStripMenuItem
            // 
            this.reverse3ToolStripMenuItem.Name = "reverse3ToolStripMenuItem";
            this.reverse3ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.reverse3ToolStripMenuItem.Text = "Reverse3";
            this.reverse3ToolStripMenuItem.Click += new System.EventHandler(this.viewReverse3);
            // 
            // normal4ToolStripMenuItem
            // 
            this.normal4ToolStripMenuItem.Name = "normal4ToolStripMenuItem";
            this.normal4ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.normal4ToolStripMenuItem.Text = "Normal4";
            this.normal4ToolStripMenuItem.Click += new System.EventHandler(this.viewNormal4);
            // 
            // reverse4ToolStripMenuItem
            // 
            this.reverse4ToolStripMenuItem.Name = "reverse4ToolStripMenuItem";
            this.reverse4ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.reverse4ToolStripMenuItem.Text = "Reverse4";
            this.reverse4ToolStripMenuItem.Click += new System.EventHandler(this.viewReverse4);
            // 
            // normal5ToolStripMenuItem
            // 
            this.normal5ToolStripMenuItem.Name = "normal5ToolStripMenuItem";
            this.normal5ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.normal5ToolStripMenuItem.Text = "Normal5";
            this.normal5ToolStripMenuItem.Click += new System.EventHandler(this.viewNormal5);
            // 
            // reverse5ToolStripMenuItem
            // 
            this.reverse5ToolStripMenuItem.Name = "reverse5ToolStripMenuItem";
            this.reverse5ToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.reverse5ToolStripMenuItem.Text = "Reverse5";
            this.reverse5ToolStripMenuItem.Click += new System.EventHandler(this.viewReverse5);
            // 
            // handOrderToolStripMenuItem
            // 
            this.handOrderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bigBLToolStripMenuItem,
            this.bigBRToolStripMenuItem,
            this.bigULToolStripMenuItem,
            this.bigURToolStripMenuItem});
            this.handOrderToolStripMenuItem.Name = "handOrderToolStripMenuItem";
            this.handOrderToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.handOrderToolStripMenuItem.Text = "handOrder";
            // 
            // bigBLToolStripMenuItem
            // 
            this.bigBLToolStripMenuItem.Name = "bigBLToolStripMenuItem";
            this.bigBLToolStripMenuItem.Size = new System.Drawing.Size(144, 30);
            this.bigBLToolStripMenuItem.Text = "BigBL";
            this.bigBLToolStripMenuItem.Click += new System.EventHandler(this.BigBL);
            // 
            // bigBRToolStripMenuItem
            // 
            this.bigBRToolStripMenuItem.Name = "bigBRToolStripMenuItem";
            this.bigBRToolStripMenuItem.Size = new System.Drawing.Size(144, 30);
            this.bigBRToolStripMenuItem.Text = "BigBR";
            this.bigBRToolStripMenuItem.Click += new System.EventHandler(this.BigBR);
            // 
            // bigULToolStripMenuItem
            // 
            this.bigULToolStripMenuItem.Name = "bigULToolStripMenuItem";
            this.bigULToolStripMenuItem.Size = new System.Drawing.Size(144, 30);
            this.bigULToolStripMenuItem.Text = "BigUL";
            this.bigULToolStripMenuItem.Click += new System.EventHandler(this.BigUL);
            // 
            // bigURToolStripMenuItem
            // 
            this.bigURToolStripMenuItem.Name = "bigURToolStripMenuItem";
            this.bigURToolStripMenuItem.Size = new System.Drawing.Size(144, 30);
            this.bigURToolStripMenuItem.Text = "BigUR";
            this.bigURToolStripMenuItem.Click += new System.EventHandler(this.BigUR);
            // 
            // pieceCharsetToolStripMenuItem
            // 
            this.pieceCharsetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oneCharToolStripMenuItem,
            this.twoCharToolStripMenuItem});
            this.pieceCharsetToolStripMenuItem.Name = "pieceCharsetToolStripMenuItem";
            this.pieceCharsetToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.pieceCharsetToolStripMenuItem.Text = "pieceCharset";
            // 
            // oneCharToolStripMenuItem
            // 
            this.oneCharToolStripMenuItem.Name = "oneCharToolStripMenuItem";
            this.oneCharToolStripMenuItem.Size = new System.Drawing.Size(162, 30);
            this.oneCharToolStripMenuItem.Text = "oneChar";
            this.oneCharToolStripMenuItem.Click += new System.EventHandler(this.pieceOneChar);
            // 
            // twoCharToolStripMenuItem
            // 
            this.twoCharToolStripMenuItem.Name = "twoCharToolStripMenuItem";
            this.twoCharToolStripMenuItem.Size = new System.Drawing.Size(162, 30);
            this.twoCharToolStripMenuItem.Text = "twoChar";
            this.twoCharToolStripMenuItem.Click += new System.EventHandler(this.pieceTwoChar);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 35);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1333, 640);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // coordCharsetToolStripMenuItem
            // 
            this.coordCharsetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nONEToolStripMenuItem,
            this.nORMALToolStripMenuItem,
            this.aRABICToolStripMenuItem,
            this.cHESSLIKEToolStripMenuItem});
            this.coordCharsetToolStripMenuItem.Name = "coordCharsetToolStripMenuItem";
            this.coordCharsetToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.coordCharsetToolStripMenuItem.Text = "coordCharset";
            // 
            // nONEToolStripMenuItem
            // 
            this.nONEToolStripMenuItem.Name = "nONEToolStripMenuItem";
            this.nONEToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.nONEToolStripMenuItem.Text = "NONE";
            this.nONEToolStripMenuItem.Click += new System.EventHandler(this.coordNone);
            // 
            // nORMALToolStripMenuItem
            // 
            this.nORMALToolStripMenuItem.Name = "nORMALToolStripMenuItem";
            this.nORMALToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.nORMALToolStripMenuItem.Text = "NORMAL";
            this.nORMALToolStripMenuItem.Click += new System.EventHandler(this.coordNormal);
            // 
            // aRABICToolStripMenuItem
            // 
            this.aRABICToolStripMenuItem.Name = "aRABICToolStripMenuItem";
            this.aRABICToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.aRABICToolStripMenuItem.Text = "ARABIC";
            this.aRABICToolStripMenuItem.Click += new System.EventHandler(this.coordArabic);
            // 
            // cHESSLIKEToolStripMenuItem
            // 
            this.cHESSLIKEToolStripMenuItem.Name = "cHESSLIKEToolStripMenuItem";
            this.cHESSLIKEToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.cHESSLIKEToolStripMenuItem.Text = "CHESSLIKE";
            this.cHESSLIKEToolStripMenuItem.Click += new System.EventHandler(this.coordChesslike);
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 675);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "BoardForm";
            this.Text = "BoardForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPos1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPos2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPos3ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normal1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverse1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normal2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverse2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normal3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverse3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normal4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverse4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normal5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverse5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem handOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigBLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigBRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigULToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pieceCharsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneCharToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twoCharToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordCharsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nONEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nORMALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aRABICToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cHESSLIKEToolStripMenuItem;
    }
}
