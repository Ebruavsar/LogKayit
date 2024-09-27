namespace LOG
{
    partial class Form1
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
            this.open = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.sunucu = new System.Windows.Forms.Button();
            this.güncel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Raporla = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(132, 21);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(75, 23);
            this.open.TabIndex = 2;
            this.open.Text = "Dosya Aç";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click_1);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // sunucu
            // 
            this.sunucu.Location = new System.Drawing.Point(896, 21);
            this.sunucu.Name = "sunucu";
            this.sunucu.Size = new System.Drawing.Size(100, 23);
            this.sunucu.TabIndex = 3;
            this.sunucu.Text = "Log Sunucu";
            this.sunucu.UseVisualStyleBackColor = true;
            this.sunucu.Click += new System.EventHandler(this.button2_Click);
            // 
            // güncel
            // 
            this.güncel.Location = new System.Drawing.Point(24, 21);
            this.güncel.Name = "güncel";
            this.güncel.Size = new System.Drawing.Size(102, 23);
            this.güncel.TabIndex = 49;
            this.güncel.Text = "Güncel dosya aç";
            this.güncel.UseVisualStyleBackColor = true;
            this.güncel.Click += new System.EventHandler(this.güncel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(24, 70);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(972, 478);
            this.tabControl1.TabIndex = 50;
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
            // 
            // Raporla
            // 
            this.Raporla.Location = new System.Drawing.Point(213, 21);
            this.Raporla.Name = "Raporla";
            this.Raporla.Size = new System.Drawing.Size(75, 23);
            this.Raporla.TabIndex = 51;
            this.Raporla.Text = "Raporla";
            this.Raporla.UseVisualStyleBackColor = true;
            this.Raporla.Click += new System.EventHandler(this.Raporla_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(354, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 52;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 560);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Raporla);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.güncel);
            this.Controls.Add(this.sunucu);
            this.Controls.Add(this.open);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button open;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button sunucu;
        private System.Windows.Forms.Button güncel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button Raporla;
        private System.Windows.Forms.Button button1;
    }
}

