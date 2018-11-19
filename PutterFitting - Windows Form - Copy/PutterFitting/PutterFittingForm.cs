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
        private System.Windows.Forms.Button viewAllButton;
        private System.Windows.Forms.Button infoButton;

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
                Controls.Remove(infoButton);
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
            OutBox.Text += Environment.NewLine + "Length: " + person1.fit.putter.PutterLength + " Grip: " + person1.fit.putter.PutterGrip;
        }
        private void viewAll_Click(object sender, EventArgs e)
        {
            if (viewAllButton.Text == "Search")
            {
                string[] search = OutBox.Text.Remove(0, 66).Split('\n');
                string[] results;

                optionsList = new System.Windows.Forms.ListBox();
                optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                optionsList.FormattingEnabled = true;
                optionsList.ItemHeight = 25;
                optionsList.Location = new System.Drawing.Point(25, 230);
                optionsList.Name = "optionsList";
                optionsList.Size = new System.Drawing.Size(620, 104);
                optionsList.TabIndex = 3;
                Controls.Add(optionsList);

                if (search.Length == 1 && search[0] == "")
                    results = admin1.viewPutter();
                else
                    results = admin1.viewPutter(search);
                for (int a = 0; a < results.Length; a++)
                {
                    optionsList.Items.Add(results[a]);
                }
                viewAllButton.Text = "Remove / Restart";
                viewAllButton.Location = new System.Drawing.Point(560, 150);
            }
            else if (viewAllButton.Text == "Remove / Restart")
            {
                if (optionsList.SelectedIndex>=0)
                {
                    admin1.managePutter = optionsList.Text.Split('|', '\n')[0];
                    admin1.RemovePutter();
                }
                Controls.Remove(optionsList);
                Controls.Remove(viewAllButton);
                admin();//rests process
            }
            else
            {
                viewAllButton.Text = "Search";
                OutBox.Text = "Enter putter name, brand, or category. (Leave Blank to view all): ";
                Controls.Remove(manageButton);
            }
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
                    Controls.Remove(viewAllButton);
                    admin1.AddNewPutter(putter);
                    putterCounter = 0;
                    admin();//resets process

                }
                else
                    for (int a = 0; a < 4; a++)
                        optionsList.Items.Add(putterList[putterCounter, a]);

            }
            else if(manageButton.Text == "Remove")
            {
                admin1.RemovePutter();
                admin();//resets process
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
            //
            //viewAllButton
            //
            viewAllButton = new System.Windows.Forms.Button();
            viewAllButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            viewAllButton.ForeColor = System.Drawing.Color.MidnightBlue;
            viewAllButton.Location = new System.Drawing.Point(25, 150);
            viewAllButton.Size = new System.Drawing.Size(83, 35);
            viewAllButton.TabIndex = 21;
            viewAllButton.Text = "View Putters";
            viewAllButton.UseVisualStyleBackColor = true;
            viewAllButton.Click += new System.EventHandler(viewAll_Click);
            Controls.Add(viewAllButton);
            viewAllButton.BringToFront();

        }
        private void postLogin()
        {
            Controls.Remove(Login);
            optionsList = new System.Windows.Forms.ListBox();
            importanceBox = new System.Windows.Forms.TextBox();
            importanceTitle = new System.Windows.Forms.Label();
            oneToFiveLabel = new System.Windows.Forms.Label();
            NextButton = new System.Windows.Forms.Button();
            infoButton = new System.Windows.Forms.Button();

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
            // 
            // infoButton
            // 
            infoButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            infoButton.ForeColor = System.Drawing.Color.MidnightBlue;
            infoButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            infoButton.Location = new System.Drawing.Point(75, 230);
            infoButton.Name = "infoButton";
            infoButton.Size = new System.Drawing.Size(37, 90);
            infoButton.TabIndex = 23;
            infoButton.Text = "?";
            infoButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            infoButton.UseVisualStyleBackColor = true;
            infoButton.Click += new System.EventHandler(this.infoButton_Click);

            Controls.Add(optionsList);
            Controls.Add(importanceBox);
            Controls.Add(importanceTitle);
            Controls.Add(oneToFiveLabel);
            Controls.Add(NextButton);
            Controls.Add(infoButton);
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
                    string[] instanceInformation = information[0].Split('\u00BB');//instance information
                    if (credentials[0] == "admin")
                    {
                        admin1 = new Admin(instanceInformation[2], instanceInformation[3]);
                        admin();
                    }
                    else
                    {
                        person1 = new Consumer(instanceInformation[0], instanceInformation[1], instanceInformation[4], instanceInformation[2], instanceInformation[3]);//instance user
                        postLogin();
                    }

                }
                else
                { 
                    OutBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    OutBox.Text += "First Name: " + Environment.NewLine + "Last Name: " + Environment.NewLine + "Birthdate(xx/xx/xxxx): " + Environment.NewLine + "Credit Card Number: " + Environment.NewLine + "CVV2: " + Environment.NewLine + "Expiration Date: "+Environment.NewLine;   
                    Login.Text = "Create";
                }
            }
            else
            {
                string log = OutBox.Text;
                string[] userInfo;
                userInfo = log.Split('\n');
                userInfo[0] = userInfo[0].Remove(0, 7);//username
                userInfo[0] = userInfo[0].Remove(userInfo[0].Length-1);
                userInfo[1] = userInfo[1].Remove(0, 10);//password
                userInfo[1] = userInfo[1].Remove(userInfo[1].Length - 1);
                userInfo[2] = userInfo[2].Remove(0, 12);//first name
                userInfo[2] = userInfo[2].Remove(userInfo[2].Length - 1);
                userInfo[3] = userInfo[3].Remove(0, 11);//last name
                userInfo[3] = userInfo[3].Remove(userInfo[3].Length - 1);
                userInfo[4] = userInfo[4].Remove(0, 23);//birthdate
                userInfo[4] = userInfo[4].Remove(userInfo[4].Length - 1);
                userInfo[5] = userInfo[5].Remove(0, 20);//cc num
                userInfo[5] = userInfo[5].Remove(userInfo[5].Length - 1);
                userInfo[6] = userInfo[6].Remove(0, 6);//cvv2 num
                userInfo[6] = userInfo[6].Remove(userInfo[6].Length - 1);
                userInfo[7] = userInfo[7].Remove(0, 17);//experation date
                userInfo[7] = userInfo[7].Remove(userInfo[7].Length - 1);
                person1 = new Consumer(userInfo[0], userInfo[1], userInfo[4], userInfo[5], userInfo[6], Convert.ToDateTime(userInfo[7]), userInfo[2], userInfo[3]); //new instance
                if (person1.UserCard.MakePayment(5.99))
                {
                    person1.addNewPerson();
                    postLogin();
                }
            }
        }

        private void ChangePasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Users.Active)//if user clicked login
            {
                if (ChangePasswordLink.Text == "Change")
                {
                    string log = OutBox.Text;
                    string[] credentials;
                    credentials = log.Split('\n');
                    credentials[0] = credentials[0].Remove(0, 12);//firstname
                    credentials[1] = credentials[1].Remove(0, 11);//lastname
                    credentials[2] = credentials[2].Remove(0, 14);//newpassword
                    credentials[0] = credentials[0].Remove(credentials[0].Length - 1);
                    credentials[1] = credentials[1].Remove(credentials[1].Length - 1);
                    credentials[2] = credentials[2].Remove(credentials[2].Length - 1);

                    if (Admin.Active) //if user is admin
                    {
                        if (admin1.ChangePassword(credentials[0], credentials[1], credentials[2]))
                            OutBox.Text = "Done.";
                    }
                    else
                    {
                        if (person1.ChangePassword(credentials[0], credentials[1], credentials[2], person1.username))
                            OutBox.Text = "Done.";
                    }
                    ChangePasswordLink.Text = "Change Password";
                }
                else
                {
                    ChangePasswordLink.Text = "Change";
                    OutBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    OutBox.Text = "First Name: " + Environment.NewLine + "Last Name: " + Environment.NewLine + "New Password: " + Environment.NewLine;
                }
            }
            else
            {
                if (ChangePasswordLink.Text == "Reset")
                {
                    string log = OutBox.Text;
                    string[] credentials;
                    credentials = log.Split('\n');
                    credentials[0] = credentials[0].Remove(0, 10);//username
                    credentials[1] = credentials[1].Remove(0, 12);//firstname
                    credentials[2] = credentials[2].Remove(0, 11);//lastname
                    credentials[3] = credentials[3].Remove(0, 14);//newpassword
                    credentials[4] = credentials[4].Remove(0, 11);//birthdate
                    credentials[0] = credentials[0].Remove(credentials[0].Length - 1);
                    credentials[1] = credentials[1].Remove(credentials[1].Length - 1);
                    credentials[2] = credentials[2].Remove(credentials[2].Length - 1);
                    credentials[3] = credentials[3].Remove(credentials[3].Length - 1);
                    credentials[4] = credentials[4].Remove(credentials[4].Length - 1);
                    if (Users.ChangePassword(credentials[0], credentials[1], credentials[2], credentials[3], Convert.ToDateTime(credentials[4])))
                        OutBox.Text = "Done.";
                    else
                        MessageBox.Show("Could not verify credentials");
                    ChangePasswordLink.Text = "Change Password";
                }
                else
                {
                    ChangePasswordLink.Text = "Reset";
                    OutBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    OutBox.Text = "Username: " + Environment.NewLine + "First Name: " + Environment.NewLine + "Last Name: " + Environment.NewLine + "New Password: " + Environment.NewLine + "Birthdate: " + Environment.NewLine;
                }

            }
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            string[] infoHelp = { "If you have a consistent miss, left or right, select the appropriate option, otherwise select \"Not Applicable\". If you are not sure or your misses are not  consistent select \"Not Applicable\".",
            "If you have a consistent miss, short or long, select the appropriate option, otherwise select \"Not Applicable\". If you are not sure or your misses are not  consistent select \"Not Applicable\".",
            "The dominate eye affects alignment, select the appropriate option, if you are not sure what your dominate eye is, select \"Right Hand, Right Eye\".",
            "An arcing swing path is when the putter head arcs around the target line, straight back and straight through swing path is when the putter head stays square on the target line.",
            "If you struggle with alignment to the hole, select the appropriate option. Putter shapes may help with alignment. ",
            "The length is loosely based on height. Selcet the closet option.",
            "Bending wrists causes extra putter head movement. This can happen by leading the stroke with the hands. Select the appropriate options, if you are not sure select \"Not Applicable\".",
            "If you have a preference on grips, select the appropriate option. If you are not sure select \"Not Applicable\".",
            "Softer feeling putters will have less feedback, harder feeling putters will have more feedback. If you are not sure select \"Not Applicable\"."};
            if (counter >0)
                MessageBox.Show(listNames[counter] + Environment.NewLine + infoHelp[counter]);
            else
                MessageBox.Show("Common Left to Right Miss" + Environment.NewLine + infoHelp[counter]);
        }
    }
    
}