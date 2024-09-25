namespace LOG
{
    partial class Raporla
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
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonRaporla = new System.Windows.Forms.Button();
            this.textBoxDizin = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Location = new System.Drawing.Point(249, 79);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFormat.TabIndex = 0;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(309, 148);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            // 
            // buttonRaporla
            // 
            this.buttonRaporla.Location = new System.Drawing.Point(309, 198);
            this.buttonRaporla.Name = "buttonRaporla";
            this.buttonRaporla.Size = new System.Drawing.Size(75, 23);
            this.buttonRaporla.TabIndex = 2;
            this.buttonRaporla.Text = "Raporla";
            this.buttonRaporla.UseVisualStyleBackColor = true;
            // 
            // textBoxDizin
            // 
            this.textBoxDizin.Location = new System.Drawing.Point(138, 150);
            this.textBoxDizin.Name = "textBoxDizin";
            this.textBoxDizin.Size = new System.Drawing.Size(100, 20);
            this.textBoxDizin.TabIndex = 3;
            // 
            // Raporla
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 349);
            this.Controls.Add(this.textBoxDizin);
            this.Controls.Add(this.buttonRaporla);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.comboBoxFormat);
            this.Name = "Raporla";
            this.Text = "Raporla";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonRaporla;
        private System.Windows.Forms.TextBox textBoxDizin;
    }
}