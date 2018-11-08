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
            this.optionsList = new System.Windows.Forms.ListBox();
            this.importanceBox = new System.Windows.Forms.TextBox();
            this.importanceTitle = new System.Windows.Forms.Label();
            this.oneToFiveLabel = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OutBox
            // 
            this.OutBox.Location = new System.Drawing.Point(12, 91);
            this.OutBox.Multiline = true;
            this.OutBox.Name = "OutBox";
            this.OutBox.Size = new System.Drawing.Size(776, 142);
            this.OutBox.TabIndex = 0;
            this.OutBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // PutterTitle
            // 
            this.PutterTitle.AutoSize = true;
            this.PutterTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PutterTitle.Location = new System.Drawing.Point(101, 9);
            this.PutterTitle.Name = "PutterTitle";
            this.PutterTitle.Size = new System.Drawing.Size(594, 79);
            this.PutterTitle.TabIndex = 1;
            this.PutterTitle.Text = "Putter Fitting Tool";
            this.PutterTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PutterTitle.Click += new System.EventHandler(this.PutterTitle_Click);
            // 
            // optionsTitle
            // 
            this.optionsTitle.AutoSize = true;
            this.optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsTitle.Location = new System.Drawing.Point(162, 259);
            this.optionsTitle.Name = "optionsTitle";
            this.optionsTitle.Size = new System.Drawing.Size(193, 27);
            this.optionsTitle.TabIndex = 2;
            this.optionsTitle.Text = "Common L-R Miss";
            this.optionsTitle.Click += new System.EventHandler(this.Path_Click);
            // 
            // optionsList
            // 
            this.optionsList.FormattingEnabled = true;
            this.optionsList.ItemHeight = 25;
            this.optionsList.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Not Applicable",
            " "});
            this.optionsList.Location = new System.Drawing.Point(167, 290);
            this.optionsList.Name = "optionsList";
            this.optionsList.Size = new System.Drawing.Size(306, 104);
            this.optionsList.TabIndex = 3;
            this.optionsList.SelectedIndexChanged += new System.EventHandler(this.PathList_SelectedIndexChanged);
            // 
            // importanceBox
            // 
            this.importanceBox.Font = new System.Drawing.Font("Microsoft Tai Le", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importanceBox.Location = new System.Drawing.Point(531, 305);
            this.importanceBox.Multiline = true;
            this.importanceBox.Name = "importanceBox";
            this.importanceBox.Size = new System.Drawing.Size(58, 54);
            this.importanceBox.TabIndex = 14;
            this.importanceBox.Text = "5";
            this.importanceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.importanceBox.TextChanged += new System.EventHandler(this.SwingPathImportance_TextChanged);
            // 
            // importanceTitle
            // 
            this.importanceTitle.AutoSize = true;
            this.importanceTitle.Location = new System.Drawing.Point(479, 262);
            this.importanceTitle.Name = "importanceTitle";
            this.importanceTitle.Size = new System.Drawing.Size(176, 25);
            this.importanceTitle.TabIndex = 20;
            this.importanceTitle.Text = "Importance Level";
            this.importanceTitle.Click += new System.EventHandler(this.label2_Click);
            // 
            // oneToFiveLabel
            // 
            this.oneToFiveLabel.AutoSize = true;
            this.oneToFiveLabel.Location = new System.Drawing.Point(532, 362);
            this.oneToFiveLabel.Name = "oneToFiveLabel";
            this.oneToFiveLabel.Size = new System.Drawing.Size(57, 25);
            this.oneToFiveLabel.TabIndex = 21;
            this.oneToFiveLabel.Text = "(1-5)";
            this.oneToFiveLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // NextButton
            // 
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.ForeColor = System.Drawing.Color.MidnightBlue;
            this.NextButton.Location = new System.Drawing.Point(167, 440);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(422, 64);
            this.NextButton.TabIndex = 22;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PutterFittingSoftware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(800, 546);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.oneToFiveLabel);
            this.Controls.Add(this.importanceTitle);
            this.Controls.Add(this.importanceBox);
            this.Controls.Add(this.optionsList);
            this.Controls.Add(this.optionsTitle);
            this.Controls.Add(this.PutterTitle);
            this.Controls.Add(this.OutBox);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "PutterFittingSoftware";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Putter Fitting Software";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OutBox;
        private System.Windows.Forms.Label PutterTitle;
        private System.Windows.Forms.Label optionsTitle;
        private System.Windows.Forms.ListBox optionsList;
        private System.Windows.Forms.TextBox importanceBox;
        private System.Windows.Forms.Label importanceTitle;
        private System.Windows.Forms.Label oneToFiveLabel;
        private System.Windows.Forms.Button NextButton;
    }
}

