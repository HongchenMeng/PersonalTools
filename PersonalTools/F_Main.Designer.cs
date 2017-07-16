namespace PersonalTools
{
    partial class F_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Main));
            this.mS_Main = new System.Windows.Forms.MenuStrip();
            this.splitC = new System.Windows.Forms.SplitContainer();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitC)).BeginInit();
            this.splitC.SuspendLayout();
            this.SuspendLayout();
            // 
            // mS_Main
            // 
            this.mS_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mS_Main.Location = new System.Drawing.Point(0, 0);
            this.mS_Main.Name = "mS_Main";
            this.mS_Main.Size = new System.Drawing.Size(942, 24);
            this.mS_Main.TabIndex = 3;
            this.mS_Main.Text = "menuStrip2";
            // 
            // splitC
            // 
            this.splitC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitC.Location = new System.Drawing.Point(0, 24);
            this.splitC.Name = "splitC";
            // 
            // splitC.Panel1
            // 
            this.splitC.Panel1.BackgroundImage = global::PersonalTools.Properties.Resources.Background__01;
            this.splitC.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitC.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitC_Panel1_Paint);
            // 
            // splitC.Panel2
            // 
            this.splitC.Panel2.BackgroundImage = global::PersonalTools.Properties.Resources.Background__02;
            this.splitC.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitC.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitC_Panel2_Paint);
            this.splitC.Panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.splitC_Panel2_MouseClick);
            this.splitC.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitC_Panel2_MouseMove);
            this.splitC.Size = new System.Drawing.Size(942, 590);
            this.splitC.SplitterDistance = 312;
            this.splitC.TabIndex = 4;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(167, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(167, 20);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(942, 614);
            this.Controls.Add(this.splitC);
            this.Controls.Add(this.mS_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "F_Main";
            this.Text = "梦红尘~密码管理";
            this.Load += new System.EventHandler(this.F_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitC)).EndInit();
            this.splitC.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mS_Main;
        private System.Windows.Forms.SplitContainer splitC;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

