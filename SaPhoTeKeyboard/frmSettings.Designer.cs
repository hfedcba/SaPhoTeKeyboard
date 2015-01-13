namespace SaPhoTeKeyboard
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.rdbGerman = new System.Windows.Forms.RadioButton();
            this.rdbEnglish = new System.Windows.Forms.RadioButton();
            this.Label4 = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(12, 12);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(117, 17);
            this.chkAutostart.TabIndex = 13;
            this.chkAutostart.Text = "Start with Windows";
            this.chkAutostart.UseVisualStyleBackColor = true;
            this.chkAutostart.CheckedChanged += new System.EventHandler(this.chkAutostart_CheckedChanged);
            // 
            // rdbGerman
            // 
            this.rdbGerman.AutoSize = true;
            this.rdbGerman.Location = new System.Drawing.Point(77, 57);
            this.rdbGerman.Name = "rdbGerman";
            this.rdbGerman.Size = new System.Drawing.Size(62, 17);
            this.rdbGerman.TabIndex = 18;
            this.rdbGerman.Text = "German";
            this.rdbGerman.UseVisualStyleBackColor = true;
            this.rdbGerman.CheckedChanged += new System.EventHandler(this.rdbGerman_CheckedChanged);
            // 
            // rdbEnglish
            // 
            this.rdbEnglish.AutoSize = true;
            this.rdbEnglish.Checked = true;
            this.rdbEnglish.Location = new System.Drawing.Point(12, 57);
            this.rdbEnglish.Name = "rdbEnglish";
            this.rdbEnglish.Size = new System.Drawing.Size(59, 17);
            this.rdbEnglish.TabIndex = 17;
            this.rdbEnglish.TabStop = true;
            this.rdbEnglish.Text = "English";
            this.rdbEnglish.UseVisualStyleBackColor = true;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(9, 41);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(86, 13);
            this.Label4.TabIndex = 16;
            this.Label4.Text = "Keyboard layout:";
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.Location = new System.Drawing.Point(205, 52);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(67, 23);
            this.OK_Button.TabIndex = 19;
            this.OK_Button.Text = "Close";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 87);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.rdbGerman);
            this.Controls.Add(this.rdbEnglish);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.chkAutostart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SaPhoTeKeyboard - Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkAutostart;
        internal System.Windows.Forms.RadioButton rdbGerman;
        internal System.Windows.Forms.RadioButton rdbEnglish;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button OK_Button;
    }
}

