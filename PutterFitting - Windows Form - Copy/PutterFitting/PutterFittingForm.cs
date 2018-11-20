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
        public string[] data = new string[9];       //Used to store data selected from the user
        public int[] importance = new int[9];       //Used to store the importance data from the user
        Consumer person1;                           //Creates person var, initialized durning the login fucntions
        Admin admin1;                               //Only initialized if the username is admin
        public string[] putter = new string[5];     //Used in the manage function to store putter traits
        public string[,] putterList = new string[5, 4]
        {                                           //Used in the manage function to display options
            { "Normal Putter Head", "Wide Putter Head", "", "" },
            { "Toe Weighted", "Face Balanced", "", "" },
            { "Offset Shaft", "Straight Shaft", "", "" },
            { "Lighter Weight", "Heavier Weight", "Standard Weight", "" },
            { "Softer Feel", "Harder Feel", "", "" }
        };
        public int putterCounter = 0;               //Used in manage function to track which trait
        public string[,] listItems = new string[9, 4] {
        {"", "", "", "" },                          //Used in the fitting functions to display options
        {"Long", "Short", "Not Applicable", ""},
        {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"},
        {"Arcing Path", "Straight Back Straight Through", "", ""},
        {"Struggles with Alignment", "Alignment is Okay", "", ""},
        {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"},
        {"Wrist bend", "No Wrist Bend", "Not Applicable" ,""},
        {"Standard Size Grip", "Larger Grip", "Not Applicable", ""},
        {"Softer Feel", "Harder Feel", "Not Applicable", ""} };
        public string[] listNames = new string[9] {
        "",                                         //Used to display titles for the fitting functions
        "Common Distance Miss",
        "Dominant Eye",
        "Swing Path",
        "Alignment",
        "Height",
        "Putter Head Movement",
        "Grip Perefrence",
        "Feel"};
        public int counter = 0;                     //Track the display options for fitting
        public PutterFittingSoftware()
        {
            InitializeComponent();
        }
        private void PutterFittingSoftware_Load(object sender, EventArgs e)
        {
            loginSetup();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            data[counter] = optionsList.Text;
            importance[counter] = Convert.ToInt32(importanceBox.Text);
            if (data[counter] != "" && importance[counter] > 0 && importance[counter]<=5)
            {
                optionsList.Items.Clear();
                counter++;
                if (counter == 9) //ends the data collection, starts the putter search
                    start();
                else              //Continues the data collection, changes the options for the user to select
                {
                    optionsTitle.Text = listNames[counter];
                    for (int a = 0; a < 4; a++)
                        optionsList.Items.Add(listItems[counter, a]);
                    importanceBox.Text = "5";
                }
            }
        }

        private void start()
        {
            string[] results = person1.startFit(data, importance);
            resultsSetup();
            OutBox.Text += " ( " + results.Length + " )" + Environment.NewLine + Environment.NewLine;
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

                showPutterSetup();

                if (search.Length == 1 && search[0] == "")
                    results = admin1.viewPutter();
                else
                    results = admin1.viewPutter(search);
                OutBox.Text += "\t\t\tResults: " + results.Length;
                for (int a = 0; a < results.Length; a++)
                    optionsList.Items.Add(results[a]);
                viewAllButton.Text = "Remove / Restart";
                viewAllButton.Location = new System.Drawing.Point(560, 150);
            }
            else if (viewAllButton.Text == "Remove / Restart")
            {
                if (optionsList.SelectedIndex >= 0)
                {
                    admin1.managePutter = optionsList.Text.Split('|', '\n')[0];
                    admin1.RemovePutter();
                }
                adminSetup();
            }
            else
            {
                Controls.Remove(optionsList);
                viewAllButton.Text = "Search";
                OutBox.Text = "Enter putter name, brand, or category. (Leave Blank to view all): ";
                Controls.Remove(manageButton);
            }
        }

        private void Manage_Click(object sender, EventArgs e)
        {
            if (manageButton.Text == "Add")
            {
                putter[putterCounter] = optionsList.Text;
                optionsList.Items.Clear();
                putterCounter++;
                if (putterCounter == 5)
                {
                    admin1.AddNewPutter(putter);
                    putterEndSetup();
                }
                else
                    for (int a = 0; a < 4; a++)
                        optionsList.Items.Add(putterList[putterCounter, a]);
            }
            else if (manageButton.Text == "Remove")
            {
                admin1.RemovePutter();
                adminSetup();//resets process
            }
            else
            {
                string log = OutBox.Text;
                log = log.Remove(0, 13);
                admin1.managePutter = log;
                if (admin1.putterExist())
                    manageButton.Text = "Remove";
                else
                    newPutterSetup();
            }
        }
        
        private void Login_Click(object sender, EventArgs e)
        {
            if (Login.Text == "Login")
            {
                string username = LoginInputBox.Text;
                string password = PasswordInputBox.Text;

                if (Consumer.Login(username, password))
                {
                    SaveData saved = new SaveData("users.txt");
                    string[] information = saved.accessData(username, password);
                    string[] instanceInformation = information[0].Split('\u00BB');//instance information
                    if (username == "admin")
                    {
                        admin1 = new Admin(instanceInformation[2], instanceInformation[3]);
                        adminSetup();
                    }
                    else
                    {
                        if(instanceInformation.Length == 7)
                            person1 = new Consumer(instanceInformation[0], instanceInformation[1], instanceInformation[4], instanceInformation[2], instanceInformation[3], instanceInformation[5]);//instance with handicap
                        else
                            person1 = new Consumer(instanceInformation[0], instanceInformation[1], instanceInformation[4], instanceInformation[2], instanceInformation[3]);//instance user
                        fittingSetup();
                    }
                }
                 else
                    newUserSetup();
            }
             else
             {
                string username = LoginInputBox.Text;
                string password = PasswordInputBox.Text;
                string fname = FNameInputBox.Text;
                string lname = LNameInputBox.Text;
                string birthdate = BirthdateInputBox.Text;
                string creditcardnumber = CreditCardInputBox.Text;
                string cvv2 = CVV2InputBox.Text;
                string expiration = ExpirationInputBox.Text;
                if (!(birthdate[2] == '/' && birthdate[5] == '/' && birthdate.Length == 8))
                    MessageBox.Show("Invalid Birthdate");
                else if (!(expiration[2] == '/' && expiration[5] == '/' && expiration.Length == 8))
                    MessageBox.Show("Invalid Expiration Date");
                else if (!(cvv2.Length == 3 && cvv2[0] >= '0' && cvv2[0] <= '9' && cvv2[1] >= '0' && cvv2[1] <= '9' && cvv2[2] >= '0' && cvv2[2] <= '9'))
                    MessageBox.Show("Invalid CVV2");
                else
                {
                    person1 = new Consumer(username, password, birthdate, creditcardnumber, cvv2, Convert.ToDateTime(expiration), fname, lname); //new instance
                    if (HandicapInputBox.Text != "")
                    {
                        if (person1.addNewPerson(HandicapInputBox.Text))
                            fittingSetup();
                    }
                    else
                    {
                        if (person1.addNewPerson())
                            fittingSetup();
                    }
                }
             }
        }
        
        private void ChangePasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Users.Active)//if user instance was created
            {
                if (ChangePasswordLink.Text == "Change")
                {
                    string firstname = FNameInputBox.Text;
                    string lastname = LNameInputBox.Text;
                    string newpassword = PasswordInputBox.Text;

                    if (Admin.Active) //if user is admin
                    {
                        if (admin1.ChangePassword(firstname, lastname, newpassword)) { adminSetup(); }
                    }
                    else //user is not admin
                    {
                        if (person1.ChangePassword(firstname, lastname, newpassword, person1.username)) { fittingSetup(); }
                    }
                    ChangePasswordLink.Text = "Change Password";//resets the link
                }
                else
                    changePasswordSetup();
            }
            else //user is not logged in
            {
                if (ChangePasswordLink.Text == "Reset")
                {
                    string username = LoginInputBox.Text;
                    string firstname = FNameInputBox.Text;
                    string lastname = LNameInputBox.Text;
                    string newpassword = PasswordInputBox.Text;
                    string birthdate = BirthdateInputBox.Text;
                    if (Users.ChangePassword(username, firstname, lastname, newpassword, Convert.ToDateTime(birthdate))) { loginSetup(); }
                    ChangePasswordLink.Text = "Change Password";
                }
                else
                    resetPasswordSetup();

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
            if (counter > 0)
                MessageBox.Show(listNames[counter] + Environment.NewLine + infoHelp[counter]);
            else
                MessageBox.Show("Common Left to Right Miss" + Environment.NewLine + infoHelp[counter]);
        }
        private void info2Button_Click(object sender, EventArgs e)
        {
            if(Algorithm.relativeImportance[counter] == 0)
                MessageBox.Show("Recommended Importance Level: 1");
            else
                MessageBox.Show("Recommended Importance Level: " + Algorithm.relativeImportance[counter]);
        }

        private void loginSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            Login = new System.Windows.Forms.Button();
            LoginLabel = new System.Windows.Forms.Label();
            passwordLabel = new System.Windows.Forms.Label();
            LoginInputBox = new System.Windows.Forms.TextBox();
            PasswordInputBox = new System.Windows.Forms.TextBox();
             
            Login.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Login.ForeColor = System.Drawing.Color.MidnightBlue;
            Login.Location = new System.Drawing.Point(560, 144);
            Login.Name = "Login";
            Login.Size = new System.Drawing.Size(82, 35);
            Login.TabIndex = 21;
            Login.Text = "Login";
            Login.UseVisualStyleBackColor = true;
            Login.Click += new System.EventHandler(this.Login_Click);
             
            LoginLabel.AutoSize = true;
            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginLabel.ForeColor = System.Drawing.SystemColors.Control;
            LoginLabel.Location = new System.Drawing.Point(17, 69);
            LoginLabel.Name = "LoginLabel";
            LoginLabel.TabIndex = 23;
            LoginLabel.Text = "Login";
            LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
              
            passwordLabel.AutoSize = true;
            passwordLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.ForeColor = System.Drawing.SystemColors.Control;
            passwordLabel.Location = new System.Drawing.Point(17, 130);
            passwordLabel.Name = "passwordLabel";
            //passwordLabel.Size = new System.Drawing.Size(364, 96);
            passwordLabel.TabIndex = 24;
            passwordLabel.Text = "Password";
            passwordLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
             
            LoginInputBox.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            LoginInputBox.Location = new System.Drawing.Point(224, 69);
            LoginInputBox.Multiline = true;
            LoginInputBox.Name = "LoginInputBox";
            LoginInputBox.Size = new System.Drawing.Size(296, 48);
            LoginInputBox.TabIndex = 25;
            
            PasswordInputBox.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PasswordInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            PasswordInputBox.Location = new System.Drawing.Point(224, 130);
            PasswordInputBox.Multiline = true;
            PasswordInputBox.Name = "PasswordInputBox";
            PasswordInputBox.Size = new System.Drawing.Size(295, 48);
            PasswordInputBox.TabIndex = 26;
            Controls.Add(PasswordInputBox);
            Controls.Add(LoginInputBox);
            Controls.Add(passwordLabel);
            Controls.Add(LoginLabel);
            Controls.Add(Login);
        }
        private void newUserSetup()
        {
            FirstNameLabel = new System.Windows.Forms.Label();
            LastNameLabel = new System.Windows.Forms.Label();
            FNameInputBox = new System.Windows.Forms.TextBox();
            LNameInputBox = new System.Windows.Forms.TextBox();
            BirthdateLabel = new System.Windows.Forms.Label();
            CreditCardLabel = new System.Windows.Forms.Label();
            BirthdateInputBox = new System.Windows.Forms.TextBox();
            CreditCardInputBox = new System.Windows.Forms.TextBox();
            CVV2Label = new System.Windows.Forms.Label();
            ExpirationLabel = new System.Windows.Forms.Label();
            CVV2InputBox = new System.Windows.Forms.TextBox();
            ExpirationInputBox = new System.Windows.Forms.TextBox();
            HandicapLabel = new System.Windows.Forms.Label();
            HandicapInputBox = new System.Windows.Forms.TextBox();

            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginInputBox.Size = new System.Drawing.Size(147, 24);
            LoginInputBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PasswordInputBox.Size = new System.Drawing.Size(147, 24);
            PasswordInputBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.Location = new System.Drawing.Point(17, 95);
            PasswordInputBox.Location = new System.Drawing.Point(224, 95);
            Login.Location = new System.Drawing.Point(560, 300);


            HandicapLabel.AutoSize = true;
            HandicapLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            HandicapLabel.ForeColor = System.Drawing.SystemColors.Control;
            HandicapLabel.Location = new System.Drawing.Point(17, 135);
            HandicapLabel.Name = "HandicapLabel";
            HandicapLabel.TabIndex = 24;
            HandicapLabel.Text = "Handicap (Optional)";
            HandicapLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            HandicapInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            HandicapInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            HandicapInputBox.Location = new System.Drawing.Point(223, 135);
            HandicapInputBox.Multiline = true;
            HandicapInputBox.Name = "HandicapInputBox";
            HandicapInputBox.Size = new System.Drawing.Size(147, 24);
            HandicapInputBox.TabIndex = 25;

            FirstNameLabel.AutoSize = true;
            FirstNameLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FirstNameLabel.ForeColor = System.Drawing.SystemColors.Control;
            FirstNameLabel.Location = new System.Drawing.Point(17, 175);
            FirstNameLabel.Name = "FirstNameLabel";
            FirstNameLabel.TabIndex = 23;
            FirstNameLabel.Text = "First Name";
            FirstNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            LastNameLabel.AutoSize = true;
            LastNameLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LastNameLabel.ForeColor = System.Drawing.SystemColors.Control;
            LastNameLabel.Location = new System.Drawing.Point(17, 200);
            LastNameLabel.Name = "LastNameLabel";
            LastNameLabel.TabIndex = 24;
            LastNameLabel.Text = "Last Name";
            LastNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            FNameInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FNameInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            FNameInputBox.Location = new System.Drawing.Point(223, 175);
            FNameInputBox.Multiline = true;
            FNameInputBox.Name = "FNameInputBox";
            FNameInputBox.Size = new System.Drawing.Size(147, 24);
            FNameInputBox.TabIndex = 25;

            LNameInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LNameInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            LNameInputBox.Location = new System.Drawing.Point(223, 200);
            LNameInputBox.Multiline = true;
            LNameInputBox.Name = "LNameInputBox";
            LNameInputBox.Size = new System.Drawing.Size(147, 24);
            LNameInputBox.TabIndex = 26;

            BirthdateLabel.AutoSize = true;
            BirthdateLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            BirthdateLabel.ForeColor = System.Drawing.SystemColors.Control;
            BirthdateLabel.Location = new System.Drawing.Point(17, 225);
            BirthdateLabel.Name = "BirthdateLabel";
            //BirthdateLabel.Size = new System.Drawing.Size(222, 96);
            BirthdateLabel.TabIndex = 23;
            BirthdateLabel.Text = "Birthdate (xx/xx/xx)";
            BirthdateLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            CreditCardLabel.AutoSize = true;
            CreditCardLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CreditCardLabel.ForeColor = System.Drawing.SystemColors.Control;
            CreditCardLabel.Location = new System.Drawing.Point(17, 250);
            CreditCardLabel.Name = "CreditCardLabel";
            //CreditCardLabel.Size = new System.Drawing.Size(364, 96);
            CreditCardLabel.TabIndex = 24;
            CreditCardLabel.Text = "Credit Card Number";
            CreditCardLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            BirthdateInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            BirthdateInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            BirthdateInputBox.Location = new System.Drawing.Point(223, 225);
            BirthdateInputBox.Multiline = true;
            BirthdateInputBox.Name = "BirthdateInputBox";
            BirthdateInputBox.Size = new System.Drawing.Size(147, 24);
            BirthdateInputBox.TabIndex = 25;

            CreditCardInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CreditCardInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            CreditCardInputBox.Location = new System.Drawing.Point(223, 250);
            CreditCardInputBox.Multiline = true;
            CreditCardInputBox.Name = "CreditCardInputBox";
            CreditCardInputBox.Size = new System.Drawing.Size(147, 24);
            CreditCardInputBox.TabIndex = 26;

            CVV2Label.AutoSize = true;
            CVV2Label.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CVV2Label.ForeColor = System.Drawing.SystemColors.Control;
            CVV2Label.Location = new System.Drawing.Point(17, 275);
            CVV2Label.Name = "CVV2Label";
            //CVV2Label.Size = new System.Drawing.Size(222, 96);
            CVV2Label.TabIndex = 23;
            CVV2Label.Text = "CVV2";
            CVV2Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            ExpirationLabel.AutoSize = true;
            ExpirationLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ExpirationLabel.ForeColor = System.Drawing.SystemColors.Control;
            ExpirationLabel.Location = new System.Drawing.Point(17, 300);
            ExpirationLabel.Name = "ExpirationLabel";
            ExpirationLabel.TabIndex = 24;
            ExpirationLabel.Text = "Expiration (xx/xx/xx)";
            ExpirationLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            CVV2InputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CVV2InputBox.ForeColor = System.Drawing.Color.DarkBlue;
            CVV2InputBox.Location = new System.Drawing.Point(223, 275);
            CVV2InputBox.Multiline = true;
            CVV2InputBox.Name = "CVV2InputBox";
            CVV2InputBox.Size = new System.Drawing.Size(147, 24);
            CVV2InputBox.TabIndex = 25;

            ExpirationInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ExpirationInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            ExpirationInputBox.Location = new System.Drawing.Point(223, 300);
            ExpirationInputBox.Multiline = true;
            ExpirationInputBox.Name = "ExpirationInputBox";
            ExpirationInputBox.Size = new System.Drawing.Size(147, 24);
            ExpirationInputBox.TabIndex = 26;
            Controls.Add(FirstNameLabel);
            Controls.Add(LastNameLabel);
            Controls.Add(FNameInputBox);
            Controls.Add(LNameInputBox);
            Controls.Add(BirthdateLabel);
            Controls.Add(CreditCardLabel);
            Controls.Add(BirthdateInputBox);
            Controls.Add(CreditCardInputBox);
            Controls.Add(CVV2Label);
            Controls.Add(ExpirationLabel);
            Controls.Add(CVV2InputBox);
            Controls.Add(ExpirationInputBox);
            Controls.Add(HandicapLabel);
            Controls.Add(HandicapInputBox);
            Login.Text = "Create";
        }
        private void adminSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);
            OutBox = new System.Windows.Forms.TextBox();
            OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox.Location = new System.Drawing.Point(25, 69);
            OutBox.Multiline = true;
            OutBox.Name = "OutBox";
            OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox.Size = new System.Drawing.Size(617, 115); //1233, 219
            OutBox.TabIndex = 0;
            OutBox.Text = "Putter Name: ";
            Controls.Add(OutBox);
            Controls.Remove(Login);
            Controls.Remove(LoginLabel);//should be username
            Controls.Remove(passwordLabel);
            Controls.Remove(LoginInputBox);
            Controls.Remove(PasswordInputBox);
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
        private void fittingSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);
            optionsList = new System.Windows.Forms.ListBox();
            importanceBox = new System.Windows.Forms.TextBox();
            importanceTitle = new System.Windows.Forms.Label();
            oneToFiveLabel = new System.Windows.Forms.Label();
            NextButton = new System.Windows.Forms.Button();
            infoButton = new System.Windows.Forms.Button();
            info2Button = new System.Windows.Forms.Button();
            LoginLabel = new System.Windows.Forms.Label();
            HandicapLabel = new System.Windows.Forms.Label();

            LoginLabel.AutoSize = true;
            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginLabel.ForeColor = System.Drawing.SystemColors.Control;
            LoginLabel.Location = new System.Drawing.Point(17, 69);
            LoginLabel.Name = "LoginLabel";
            LoginLabel.TabIndex = 23;
            LoginLabel.Text = "Name: " + person1._Lname + ", " + person1._Fname;
            LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            HandicapLabel.AutoSize = true;
            HandicapLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            HandicapLabel.ForeColor = System.Drawing.SystemColors.Control;
            HandicapLabel.Location = new System.Drawing.Point(17, 110);
            HandicapLabel.Name = "HandicapLabel";
            HandicapLabel.TabIndex = 23;
            HandicapLabel.Text = "Handicap: " + person1.Handicap;
            HandicapLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            optionsTitle.AutoSize = true;
            optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsTitle.Location = new System.Drawing.Point(120, 170); // 442, 393
            optionsTitle.Name = "optionsTitle";
            optionsTitle.Size = new System.Drawing.Size(193, 27);
            optionsTitle.TabIndex = 2;
            optionsTitle.Text = "Common L-R Miss";
           
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Not Applicable",
            ""});
            optionsList.Location = new System.Drawing.Point(120, 200);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(306, 104);
            optionsList.TabIndex = 3;
            
            importanceBox.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            importanceBox.Location = new System.Drawing.Point(483, 210);
            importanceBox.Multiline = true;
            importanceBox.Name = "importanceBox";
            importanceBox.Size = new System.Drawing.Size(58, 54);
            importanceBox.TabIndex = 14;
            importanceBox.Text = "5";
            importanceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
             
            importanceTitle.AutoSize = true;
            importanceTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            importanceTitle.Location = new System.Drawing.Point(455, 170);
            importanceTitle.Name = "importanceTitle";
            importanceTitle.Size = new System.Drawing.Size(176, 25);
            importanceTitle.TabIndex = 20;
            importanceTitle.Text = "Importance Level";
              
            oneToFiveLabel.AutoSize = true;
            oneToFiveLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            oneToFiveLabel.Location = new System.Drawing.Point(495, 270);
            oneToFiveLabel.Name = "oneToFiveLabel";
            oneToFiveLabel.Size = new System.Drawing.Size(57, 25);
            oneToFiveLabel.TabIndex = 21;
            oneToFiveLabel.Text = "(1-5)";
            
            NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NextButton.ForeColor = System.Drawing.Color.MidnightBlue;
            NextButton.Location = new System.Drawing.Point(120, 310);
            NextButton.Name = "NextButton";
            NextButton.Size = new System.Drawing.Size(422, 64);
            NextButton.TabIndex = 22;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += new System.EventHandler(this.NextButton_Click);
            
            infoButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            infoButton.ForeColor = System.Drawing.Color.MidnightBlue;
            infoButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            infoButton.Location = new System.Drawing.Point(75, 200);
            infoButton.Name = "infoButton";
            infoButton.Size = new System.Drawing.Size(37, 90);
            infoButton.TabIndex = 23;
            infoButton.Text = "?";
            infoButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            infoButton.UseVisualStyleBackColor = true;
            infoButton.Click += new System.EventHandler(this.infoButton_Click);

            info2Button.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            info2Button.ForeColor = System.Drawing.Color.MidnightBlue;
            info2Button.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            info2Button.Location = new System.Drawing.Point(550, 200);
            info2Button.Name = "info2Button";
            info2Button.Size = new System.Drawing.Size(37, 90);
            info2Button.TabIndex = 23;
            info2Button.Text = "?";
            info2Button.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            info2Button.UseVisualStyleBackColor = true;
            info2Button.Click += new System.EventHandler(this.info2Button_Click);

            Controls.Add(optionsList);
            Controls.Add(importanceBox);
            Controls.Add(importanceTitle);
            Controls.Add(oneToFiveLabel);
            Controls.Add(NextButton);
            Controls.Add(infoButton);
            Controls.Add(info2Button);
            Controls.Add(LoginLabel);
            Controls.Add(HandicapLabel);

        }
        private void newPutterSetup()
        {
            optionsList = new System.Windows.Forms.ListBox();
            manageButton.Text = "Add";
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(120, 230);
            optionsList.Name = "optionsList";//optionsList is like a class, creating new instances
            optionsList.Size = new System.Drawing.Size(306, 104);
            optionsList.TabIndex = 3;
            Controls.Add(optionsList);
            for (int a = 0; a < 4; a++)
                optionsList.Items.Add(putterList[putterCounter, a]);
        }
        private void putterEndSetup()
        {
            putterCounter = 0;
            adminSetup();//resets process
        }
        private void showPutterSetup()
        {
            optionsList = new System.Windows.Forms.ListBox();
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(25, 230);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(620, 104);
            optionsList.TabIndex = 3;
            Controls.Add(optionsList);
        }
        private void resultsSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);
            OutBox = new System.Windows.Forms.TextBox();
            Controls.Remove(LoginLabel);
            Controls.Remove(LoginInputBox);
            Controls.Remove(passwordLabel);
            Controls.Remove(PasswordInputBox);
            OutBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.Size = new System.Drawing.Size(616, 325);
            OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox.Location = new System.Drawing.Point(25, 69);
            OutBox.Multiline = true;
            OutBox.Name = "OutBox";
            OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox.TabIndex = 0;
            Controls.Add(OutBox);
            OutBox.Text = "Best Fitting Putters: ";
        }
        private void changePasswordSetup()
        {
            ChangePasswordLink.Text = "Change";
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);
            FirstNameLabel = new System.Windows.Forms.Label();
            LastNameLabel = new System.Windows.Forms.Label();
            FNameInputBox = new System.Windows.Forms.TextBox();
            LNameInputBox = new System.Windows.Forms.TextBox();
            passwordLabel = new System.Windows.Forms.Label();
            PasswordInputBox = new System.Windows.Forms.TextBox();

            FirstNameLabel.AutoSize = true;
            FirstNameLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FirstNameLabel.ForeColor = System.Drawing.SystemColors.Control;
            FirstNameLabel.Location = new System.Drawing.Point(17, 95);
            FirstNameLabel.Name = "FirstNameLabel";
            FirstNameLabel.TabIndex = 23;
            FirstNameLabel.Text = "First Name";
            FirstNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            LastNameLabel.AutoSize = true;
            LastNameLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LastNameLabel.ForeColor = System.Drawing.SystemColors.Control;
            LastNameLabel.Location = new System.Drawing.Point(17, 120);
            LastNameLabel.Name = "LastNameLabel";
            LastNameLabel.TabIndex = 24;
            LastNameLabel.Text = "Last Name";
            LastNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            FNameInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FNameInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            FNameInputBox.Location = new System.Drawing.Point(223, 95);
            FNameInputBox.Multiline = true;
            FNameInputBox.Name = "FNameInputBox";
            FNameInputBox.Size = new System.Drawing.Size(147, 24);
            FNameInputBox.TabIndex = 25;

            LNameInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LNameInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            LNameInputBox.Location = new System.Drawing.Point(223, 120);
            LNameInputBox.Multiline = true;
            LNameInputBox.Name = "LNameInputBox";
            LNameInputBox.Size = new System.Drawing.Size(147, 24);
            LNameInputBox.TabIndex = 26;

            passwordLabel.AutoSize = true;
            passwordLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.ForeColor = System.Drawing.SystemColors.Control;
            passwordLabel.Location = new System.Drawing.Point(17, 145);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.TabIndex = 24;
            passwordLabel.Text = "New Password";
            passwordLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            PasswordInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PasswordInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            PasswordInputBox.Location = new System.Drawing.Point(223, 145);
            PasswordInputBox.Multiline = true;
            PasswordInputBox.Name = "PasswordInputBox";
            PasswordInputBox.Size = new System.Drawing.Size(147, 24);
            PasswordInputBox.TabIndex = 25;

            Controls.Add(FirstNameLabel);
            Controls.Add(LastNameLabel);
            Controls.Add(FNameInputBox);
            Controls.Add(LNameInputBox);
            Controls.Add(passwordLabel);
            Controls.Add(PasswordInputBox);
        }
        private void resetPasswordSetup()
        {
            changePasswordSetup();
            ChangePasswordLink.Text = "Reset";

            LoginLabel = new System.Windows.Forms.Label();
            LoginInputBox = new System.Windows.Forms.TextBox();
            BirthdateLabel = new System.Windows.Forms.Label();
            BirthdateInputBox = new System.Windows.Forms.TextBox();

            BirthdateLabel.AutoSize = true;
            BirthdateLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            BirthdateLabel.ForeColor = System.Drawing.SystemColors.Control;
            BirthdateLabel.Location = new System.Drawing.Point(17, 170);
            BirthdateLabel.Name = "BirthdateLabel";
            BirthdateLabel.TabIndex = 23;
            BirthdateLabel.Text = "Birthdate (xx/xx/xx)";
            BirthdateLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            BirthdateInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            BirthdateInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            BirthdateInputBox.Location = new System.Drawing.Point(223, 170);
            BirthdateInputBox.Multiline = true;
            BirthdateInputBox.Name = "BirthdateInputBox";
            BirthdateInputBox.Size = new System.Drawing.Size(147, 24);
            BirthdateInputBox.TabIndex = 25;

            LoginLabel.AutoSize = true;
            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginLabel.ForeColor = System.Drawing.SystemColors.Control;
            LoginLabel.Location = new System.Drawing.Point(17, 70);
            LoginLabel.Name = "LoginLabel";
            LoginLabel.TabIndex = 23;
            LoginLabel.Text = "Username";
            LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            LoginInputBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginInputBox.ForeColor = System.Drawing.Color.DarkBlue;
            LoginInputBox.Location = new System.Drawing.Point(223, 70);
            LoginInputBox.Multiline = true;
            LoginInputBox.Name = "LoginInputBox";
            LoginInputBox.Size = new System.Drawing.Size(147, 24);
            LoginInputBox.TabIndex = 25;

            Controls.Add(BirthdateLabel);
            Controls.Add(BirthdateInputBox);
            Controls.Add(LoginLabel);
            Controls.Add(LoginInputBox);
        }

        private System.Windows.Forms.TextBox OutBox;
        private System.Windows.Forms.ListBox optionsList;
        private System.Windows.Forms.TextBox importanceBox;
        private System.Windows.Forms.Label oneToFiveLabel;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button manageButton;
        private System.Windows.Forms.Button viewAllButton;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.Label FirstNameLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.TextBox FNameInputBox;
        private System.Windows.Forms.TextBox LNameInputBox;
        private System.Windows.Forms.Label BirthdateLabel;
        private System.Windows.Forms.Label CreditCardLabel;
        private System.Windows.Forms.TextBox BirthdateInputBox;
        private System.Windows.Forms.TextBox CreditCardInputBox;
        private System.Windows.Forms.Label CVV2Label;
        private System.Windows.Forms.Label ExpirationLabel;
        private System.Windows.Forms.TextBox CVV2InputBox;
        private System.Windows.Forms.TextBox ExpirationInputBox;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.Label LoginLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox LoginInputBox;
        private System.Windows.Forms.TextBox PasswordInputBox;
        private System.Windows.Forms.Button info2Button;
        private System.Windows.Forms.TextBox HandicapInputBox;
        private System.Windows.Forms.Label HandicapLabel;
    }

}