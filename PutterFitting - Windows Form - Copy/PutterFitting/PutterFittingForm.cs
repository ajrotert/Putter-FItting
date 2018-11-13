using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PutterFitting//added admin function, start, postlogin
{
    public partial class PutterFittingSoftware : Form
    {
        private System.Windows.Forms.ListBox optionsList;
        private System.Windows.Forms.TextBox importanceBox;
        private System.Windows.Forms.Label oneToFiveLabel;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button manageButton;
        public string[] data = new string[9];
        public int[] importance = new int[9];
        Consumer person1;
        Admin admin1;
        public string[] putter = new string[5];
        public string[,] putterList = new string[5, 4]
        {
            { "Normal Putter Head", "Wide Putter Head", "", "" },
            { "Toe Weighted", "Face Balanced", "", "" },
            { "Offset Shaft", "Straight Shaft", "", "" },
            { "Lighter Weight", "Heavier Weight", "Standard Weight", "" },
            { "Softer Feel", "Harder Feel", "", "" }
        };
        public int putterCounter =0;
        public string[,] listItems = new string[9, 4] {
        {"", "", "", "" },
        {"Long", "Short", "Not Applicable", ""},
        {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"},
        {"Arcing Path", "Straight Back Straight Through", "", ""},
        {"Struggles with Alignment", "Alignment is Okay", "", ""},
        {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"},
        {"Wrist bend", "No Wrist Bend", "Not Applicable" ,""},
        {"Standard Size Grip", "Larger Grip", "Not Applicable", ""},
        {"Softer Feel", "Harder Feel", "Not Applicable", ""} };
        public string[] listNames = new string[9] {
        "",
        "Common Distance Miss",
        "Dominant Eye",
        "Swing Path",
        "Alignment",
        "Height",
        "Putter Head Movement",
        "Grip Perefrence",
        "Feel"};
        public int counter = 0;
        public PutterFittingSoftware()
        {
            InitializeComponent();
        }
        private void PutterFittingSoftware_Load(object sender, EventArgs e)
        {
            OutBox.Text = "Login: " + Environment.NewLine + "Password: "+Environment.NewLine;
        }
        private void SwingPathImportance_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            data[counter] = optionsList.Text;
            importance[counter] = Convert.ToInt32(importanceBox.Text);
            optionsList.Items.Clear();
            counter++;
            if (counter == 9)
            {
                Controls.Remove(optionsList);
                Controls.Remove(NextButton);
                Controls.Remove(optionsTitle);
                Controls.Remove(importanceBox);
                Controls.Remove(oneToFiveLabel);
                Controls.Remove(importanceTitle);
                start();
            }
            else
            {
                optionsTitle.Text = listNames[counter];
                for (int a = 0; a < 4; a++)
                    optionsList.Items.Add(listItems[counter, a]);
                importanceBox.Text = "5";
            }

        }//next button
        private void start()
        {
            string[] results = person1.startFit(data, importance);
            OutBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.Size = new System.Drawing.Size(616, 325);
            OutBox.Text = "Best Fitting Putters: " + Environment.NewLine;
            for (int a = 0; a < results.Length; a++)
                OutBox.Text += results[a] + Environment.NewLine;
        }
        private void Manage_Click(object sender, EventArgs e)
        {
            if(manageButton.Text == "Add")
            {
                putter[putterCounter] = optionsList.Text;
                optionsList.Items.Clear();
                putterCounter++;
                if (putterCounter == 5)
                {
                    Controls.Remove(optionsList);
                    Controls.Remove(manageButton);
                    admin1.AddNewPutter(putter);
                }
                else
                    for (int a = 0; a < 4; a++)
                        optionsList.Items.Add(putterList[putterCounter, a]);
                

            }
            else if(manageButton.Text == "Remove")
            {
                admin1.RemovePutter();
            }
            else
            {
                string log = OutBox.Text;
                log = log.Remove(0, 13);
                admin1.managePutter = log;
                if (admin1.putterExist())
                    manageButton.Text = "Remove";
                else
                {
                    optionsList = new System.Windows.Forms.ListBox();

                    manageButton.Text = "Add";
                    optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    optionsList.FormattingEnabled = true;
                    optionsList.ItemHeight = 25;
                    optionsList.Location = new System.Drawing.Point(120, 230);
                    optionsList.Name = "optionsList";
                    optionsList.Size = new System.Drawing.Size(306, 104);
                    optionsList.TabIndex = 3;

                    Controls.Add(optionsList);

                    for (int a = 0; a < 4; a++)
                        optionsList.Items.Add(putterList[putterCounter, a]);
                }
            }
        }
        private void admin()
        {
            OutBox.Text = "Putter Name: ";
            Controls.Remove(Login);
            manageButton = new System.Windows.Forms.Button();
            manageButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            manageButton.ForeColor = System.Drawing.Color.MidnightBlue;
            manageButton.Location = new System.Drawing.Point(560, 150);
            manageButton.Size = new System.Drawing.Size(83, 35);
            manageButton.TabIndex = 21;
            manageButton.Text = "Manage";
            manageButton.UseVisualStyleBackColor = true;
            manageButton.Click += new System.EventHandler(Manage_Click);
            Controls.Add(manageButton);
            manageButton.BringToFront();

        }
        private void postLogin()
        {
            Controls.Remove(Login);
            optionsList = new System.Windows.Forms.ListBox();
            importanceBox = new System.Windows.Forms.TextBox();
            importanceTitle = new System.Windows.Forms.Label();
            oneToFiveLabel = new System.Windows.Forms.Label();
            NextButton = new System.Windows.Forms.Button();
            // 
            // optionsTitle
            // 
            optionsTitle.AutoSize = true;
            optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsTitle.Location = new System.Drawing.Point(120, 200); // 442, 393
            optionsTitle.Name = "optionsTitle";
            optionsTitle.Size = new System.Drawing.Size(193, 27);
            optionsTitle.TabIndex = 2;
            optionsTitle.Text = "Common L-R Miss";
            // 
            // optionsList
            // 
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Not Applicable",
            " "});
            optionsList.Location = new System.Drawing.Point(120,230);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(306, 104);
            optionsList.TabIndex = 3;
            // 
            // importanceBox
            // 
            importanceBox.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            importanceBox.Location = new System.Drawing.Point(483, 240);
            importanceBox.Multiline = true;
            importanceBox.Name = "importanceBox";
            importanceBox.Size = new System.Drawing.Size(58, 54);
            importanceBox.TabIndex = 14;
            importanceBox.Text = "5";
            importanceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            importanceBox.TextChanged += new System.EventHandler(this.SwingPathImportance_TextChanged);
            // 
            // importanceTitle
            // 
            importanceTitle.AutoSize = true;
            importanceTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            importanceTitle.Location = new System.Drawing.Point(455, 200);
            importanceTitle.Name = "importanceTitle";
            importanceTitle.Size = new System.Drawing.Size(176, 25);
            importanceTitle.TabIndex = 20;
            importanceTitle.Text = "Importance Level";
            // 
            // oneToFiveLabel
            // 
            oneToFiveLabel.AutoSize = true;
            oneToFiveLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            oneToFiveLabel.Location = new System.Drawing.Point(495, 300);
            oneToFiveLabel.Name = "oneToFiveLabel";
            oneToFiveLabel.Size = new System.Drawing.Size(57, 25);
            oneToFiveLabel.TabIndex = 21;
            oneToFiveLabel.Text = "(1-5)";
            // 
            // NextButton
            // 
            NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NextButton.ForeColor = System.Drawing.Color.MidnightBlue;
            NextButton.Location = new System.Drawing.Point(120, 340);
            NextButton.Name = "NextButton";
            NextButton.Size = new System.Drawing.Size(422, 64);
            NextButton.TabIndex = 22;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += new System.EventHandler(this.NextButton_Click);

            Controls.Add(optionsList);
            Controls.Add(importanceBox);
            Controls.Add(importanceTitle);
            Controls.Add(oneToFiveLabel);
            Controls.Add(NextButton);
        }
        private void Login_Click(object sender, EventArgs e)
        {
            if (Login.Text == "Login")
            {
                string log = OutBox.Text;
                string[] credentials;
                credentials = log.Split('\n');
                credentials[0] = credentials[0].Remove(0, 7);
                credentials[1] = credentials[1].Remove(0, 10);

                credentials[0] = credentials[0].Remove(credentials[0].Length-1);//removes nextLine
                credentials[1] = credentials[1].Remove(credentials[1].Length-1);

                if (Consumer.Login(credentials[0], credentials[1]))
                {
                    SaveData saved = new SaveData("users.txt");
                    string[] information = saved.accessData(credentials[0], credentials[1]);
                    string[] ii = information[0].Split('\u00BB');//instance information
                    if (credentials[0] == "admin")
                    {
                        admin1 = new Admin(ii[2], ii[3]);
                        admin();
                    }
                    else
                    {
                        person1 = new Consumer(ii[0], ii[1], ii[2], ii[3], ii[4]);//instance user
                        postLogin();
                    }

                }
                else
                { 
                    OutBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    OutBox.Text += "First Name: " + Environment.NewLine + "Last Name: " + Environment.NewLine + "Handicap: " + Environment.NewLine + "Credit Card Number: " + Environment.NewLine + "CVV2: " + Environment.NewLine + "Expiration Date: "+Environment.NewLine;   
                    Login.Text = "Create";
                }
            }
            else
            {
                string log = OutBox.Text;
                string[] userInfo;
                userInfo = log.Split('\n');
                userInfo[0] = userInfo[0].Remove(0, 7);
                userInfo[0] = userInfo[0].Remove(userInfo[0].Length-1);
                userInfo[1] = userInfo[1].Remove(0, 10);
                userInfo[1] = userInfo[1].Remove(userInfo[1].Length - 1);
                userInfo[2] = userInfo[2].Remove(0, 12);
                userInfo[2] = userInfo[2].Remove(userInfo[2].Length - 1);
                userInfo[3] = userInfo[3].Remove(0, 11);
                userInfo[3] = userInfo[3].Remove(userInfo[3].Length - 1);
                userInfo[4] = userInfo[4].Remove(0, 10);
                userInfo[4] = userInfo[4].Remove(userInfo[4].Length - 1);
                userInfo[5] = userInfo[5].Remove(0, 20);
                userInfo[5] = userInfo[5].Remove(userInfo[5].Length - 1);
                userInfo[6] = userInfo[6].Remove(0, 6);
                userInfo[6] = userInfo[6].Remove(userInfo[6].Length - 1);
                userInfo[7] = userInfo[7].Remove(0, 17);
                userInfo[7] = userInfo[7].Remove(userInfo[7].Length - 1);
                person1 = new Consumer(userInfo[0], userInfo[1], userInfo[4], userInfo[5], userInfo[6], userInfo[7], userInfo[2], userInfo[3]); //new instance
                person1.addNewPerson();
                postLogin();
            }
        }
    }
    
}


/*
        Common miss {"Left", "Right", "Not Applicable"};
        Common miss {"Long", "Short", "Not Applicable"};
        Dominant Eye {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"};
        Swing path {"Arcing Path", "Straight Back Straight Through"};
        Alignment {"Struggles with Alignment", "Alignment is Okay"};
        Height {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"};
        head movement {"Wrist bend", "No Wrist Bend", "Not Applicable"};
        Grip perefrence {"Standard Size Grip", "Larger Grip", "Not Applicable"};
        Feel {"Softer Feel", "Harder Feel", "Not Applicable"};


     private System.Windows.Forms.Label optionsTitle;
        private System.Windows.Forms.ListBox optionsList;
        private System.Windows.Forms.TextBox importanceBox;
        private System.Windows.Forms.Label importanceTitle;
        private System.Windows.Forms.Label oneToFiveLabel;
        private System.Windows.Forms.Button NextButton;

            this.OutBox = new System.Windows.Forms.TextBox();
            this.PutterTitle = new System.Windows.Forms.Label();
            this.optionsTitle = new System.Windows.Forms.Label();
            this.optionsList = new System.Windows.Forms.ListBox();
            this.importanceBox = new System.Windows.Forms.TextBox();
            this.importanceTitle = new System.Windows.Forms.Label();
            this.oneToFiveLabel = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();


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
            this.OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            this.OutBox.Location = new System.Drawing.Point(49, 137);
            this.OutBox.Multiline = true;
            this.OutBox.Name = "OutBox";
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
            this.PutterTitle.Click += new System.EventHandler(this.PutterTitle_Click);
            // 
            // optionsTitle
            // 
            this.optionsTitle.AutoSize = true;
            this.optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsTitle.Location = new System.Drawing.Point(442, 393);
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
            this.optionsList.Location = new System.Drawing.Point(447, 424);
            this.optionsList.Name = "optionsList";
            this.optionsList.Size = new System.Drawing.Size(306, 104);
            this.optionsList.TabIndex = 3;
            this.optionsList.SelectedIndexChanged += new System.EventHandler(this.PathList_SelectedIndexChanged);
            // 
            // importanceBox
            // 
            this.importanceBox.Font = new System.Drawing.Font("Microsoft Tai Le", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importanceBox.Location = new System.Drawing.Point(811, 439);
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
            this.importanceTitle.Location = new System.Drawing.Point(759, 396);
            this.importanceTitle.Name = "importanceTitle";
            this.importanceTitle.Size = new System.Drawing.Size(176, 25);
            this.importanceTitle.TabIndex = 20;
            this.importanceTitle.Text = "Importance Level";
            this.importanceTitle.Click += new System.EventHandler(this.label2_Click);
            // 
            // oneToFiveLabel
            // 
            this.oneToFiveLabel.AutoSize = true;
            this.oneToFiveLabel.Location = new System.Drawing.Point(812, 496);
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
            this.NextButton.Location = new System.Drawing.Point(447, 574);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(422, 64);
            this.NextButton.TabIndex = 22;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
 */
