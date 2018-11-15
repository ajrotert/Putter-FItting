namespace PutterFitting
{
    partial class PutterFittingSoftware
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
            this.OutBox = new System.Windows.Forms.TextBox();
            this.PutterTitle = new System.Windows.Forms.Label();
            this.optionsTitle = new System.Windows.Forms.Label();
            this.importanceTitle = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.Button();
            this.ChangePasswordLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // OutBox
            // 
            this.OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            this.OutBox.Location = new System.Drawing.Point(49, 137);
            this.OutBox.Multiline = true;
            this.OutBox.Name = "OutBox";
            this.OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutBox.Size = new System.Drawing.Size(1233, 219);
            this.OutBox.TabIndex = 0;
            this.OutBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // PutterTitle
            // 
            this.PutterTitle.AutoSize = true;
            this.PutterTitle.Font = new System.Drawing.Font("MV Boli", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PutterTitle.Location = new System.Drawing.Point(28, 9);
            this.PutterTitle.Name = "PutterTitle";
            this.PutterTitle.Size = new System.Drawing.Size(1254, 125);
            this.PutterTitle.TabIndex = 1;
            this.PutterTitle.Text = "Putter Fitting Application";
            this.PutterTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // optionsTitle
            // 
            this.optionsTitle.AutoSize = true;
            this.optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsTitle.Location = new System.Drawing.Point(442, 393);
            this.optionsTitle.Name = "optionsTitle";
            this.optionsTitle.Size = new System.Drawing.Size(0, 27);
            this.optionsTitle.TabIndex = 2;
            // 
            // importanceTitle
            // 
            this.importanceTitle.AutoSize = true;
            this.importanceTitle.Location = new System.Drawing.Point(759, 396);
            this.importanceTitle.Name = "importanceTitle";
            this.importanceTitle.Size = new System.Drawing.Size(0, 25);
            this.importanceTitle.TabIndex = 20;
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Login.Location = new System.Drawing.Point(1119, 287);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(163, 69);
            this.Login.TabIndex = 21;
            this.Login.Text = "Login";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // ChangePasswordLink
            // 
            this.ChangePasswordLink.AutoSize = true;
            this.ChangePasswordLink.LinkColor = System.Drawing.Color.Silver;
            this.ChangePasswordLink.Location = new System.Drawing.Point(1095, 137);
            this.ChangePasswordLink.Name = "ChangePasswordLink";
            this.ChangePasswordLink.Size = new System.Drawing.Size(187, 25);
            this.ChangePasswordLink.TabIndex = 22;
            this.ChangePasswordLink.TabStop = true;
            this.ChangePasswordLink.Text = "Change Password";
            this.ChangePasswordLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChangePasswordLink_LinkClicked);
            // 
            // PutterFittingSoftware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1328, 694);
            this.Controls.Add(this.ChangePasswordLink);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.importanceTitle);
            this.Controls.Add(this.optionsTitle);
            this.Controls.Add(this.PutterTitle);
            this.Controls.Add(this.OutBox);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.MaximizeBox = false;
            this.Name = "PutterFittingSoftware";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Putter Fitting Software";
            this.Load += new System.EventHandler(this.PutterFittingSoftware_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OutBox;
        private System.Windows.Forms.Label PutterTitle;
        private System.Windows.Forms.Label optionsTitle;
        private System.Windows.Forms.Label importanceTitle;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.LinkLabel ChangePasswordLink;
    }
}

