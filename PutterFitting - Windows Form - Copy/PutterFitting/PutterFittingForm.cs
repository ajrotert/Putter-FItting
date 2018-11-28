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
        "Putter Head Feel"};
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
            person1.startFit(data, importance);
            string[] results = person1.results;
            resultsSetup();
            optionsList.Items.Add(" Putters Found: ( " + results.Length + " )");
            optionsList.Items.Add("");
            for (int a = 0; a < results.Length; a++)
                optionsList.Items.Add(results[a]);
            optionsList.Items.Add("");
            optionsList.Items.Add("Length: " + person1.fit.putter.PutterLength);
            optionsList.Items.Add("Grip: " + person1.fit.putter.PutterGrip);
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
                Controls.Remove(OutBox2);
                Controls.Remove(addLinkBox);
                Controls.Remove(addLinkLabel);
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
                    if (addLinkBox.Text != "")
                        admin1.setCharacteristic(putter[0], putter[1], putter[2], putter[3], putter[4], addLinkBox.Text);
                    else
                        admin1.setCharacteristic(putter[0], putter[1], putter[2], putter[3], putter[4]);
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
                {
                    removeSetup();
                }
                else
                    newPutterSetup();
            }
        }
        private void viewButton_Click(object sender, EventArgs e)
        {
            Controls.Add(OutBox2);
            Controls.Remove(viewButton);
            OutBox2.Text += Environment.NewLine + "Putter Shape: " + admin1.putterShape;
            OutBox2.Text += Environment.NewLine + "Putter Balance: " + admin1.putterBalance;
            OutBox2.Text += Environment.NewLine + "Putter Hosel: " + admin1.putterHosel;
            OutBox2.Text += Environment.NewLine + "Putter Weight: " + admin1.putterWeight;
            OutBox2.Text += Environment.NewLine + "Putter Feel: " + admin1.putterFeel;
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
                string expirationString = ExpirationInputBox.Text;

                bool pass = true;
                DateTime birthdatTest;
                DateTime expiration = DateTime.Now; //had to initialize to pass to construcotr
                try{ birthdatTest = Convert.ToDateTime(birthdate); }
                catch{ MessageBox.Show("Invalid Birthdate"); pass = false; }
                if (pass)
                {
                    try { expiration = Convert.ToDateTime(expirationString); }
                    catch { MessageBox.Show("Invalid Expiration Date"); pass = false; }
                }
                if (!(cvv2.Length == 3 && cvv2[0] >= '0' && cvv2[0] <= '9' && cvv2[1] >= '0' && cvv2[1] <= '9' && cvv2[2] >= '0' && cvv2[2] <= '9'))
                    MessageBox.Show("Invalid CVV2");
                else if(pass)
                {
                    person1 = new Consumer(username, password, birthdate, creditcardnumber, cvv2, expiration, fname, lname); //new instance
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
        private void backButton_Click(object sender, EventArgs e)
        {
            loginSetup();
        }
        private void newUser_Click(object sender, EventArgs e)
        {
            newUserSetup();
        }
        private void showMore_Click(object sender, EventArgs e)
        {
            Controls.Remove(characterLabel);
            if (optionsList.SelectedIndex >2 && optionsList.SelectedIndex < (3+person1.results.Length))
                showMoreSetup();
        }
        private void showMyDetails_Click(object sender, EventArgs e)
        {
            Controls.Remove(characterLabel);
            if (optionsList.SelectedIndex >= 0 && optionsList.SelectedIndex > 2 && optionsList.SelectedIndex < (3 + person1.results.Length))
                showMyDetails2Setup();
            else
                showMyDetailsSetup();
        }
        private void startOver_Click(object sender, EventArgs e)
        {
            fittingSetup();
            Consumer temp = new Consumer(person1);
            person1 = temp;//resets all the non-static fields for classes
        }
        private void signOut_Click(object sender, EventArgs e)
        {
            loginSetup();
        }
        private void link_LinkClicked(object sender, EventArgs e)
        {
            goToSiteLink.LinkVisited = true;
            System.Diagnostics.Process.Start(person1.fit.putter.putterLink);
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
            if(Algorithm.importanceLevel[counter] == 0)
                MessageBox.Show("Recommended Importance Level: 1");
            else
                MessageBox.Show("Recommended Importance Level: " + Algorithm.importanceLevel[counter]);
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
            newUserButton = new System.Windows.Forms.Button();

            Login.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Login.ForeColor = System.Drawing.Color.MidnightBlue;
            Login.Location = new System.Drawing.Point(560, 144);
            Login.Name = "Login";
            Login.Size = new System.Drawing.Size(82, 35);
            Login.TabIndex = 21;
            Login.Text = "Login";
            Login.UseVisualStyleBackColor = true;
            Login.Click += new System.EventHandler(Login_Click);

            newUserButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            newUserButton.ForeColor = System.Drawing.Color.MidnightBlue;
            newUserButton.Location = new System.Drawing.Point(560, 104);
            newUserButton.Name = "newUserButton";
            newUserButton.Size = new System.Drawing.Size(82, 35);
            newUserButton.TabIndex = 21;
            newUserButton.Text = "Create";
            newUserButton.UseVisualStyleBackColor = true;
            newUserButton.Click += new System.EventHandler(newUser_Click);

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
            PasswordInputBox.PasswordChar = '\u25CF';

            Controls.Add(PasswordInputBox);
            Controls.Add(LoginInputBox);
            Controls.Add(passwordLabel);
            Controls.Add(LoginLabel);
            Controls.Add(Login);
            Controls.Add(newUserButton);
        }
        private void newUserSetup()
        {
            Controls.Remove(newUserButton);

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
            backButton = new System.Windows.Forms.Button();


            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginInputBox.Size = new System.Drawing.Size(147, 24);
            LoginInputBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PasswordInputBox.Size = new System.Drawing.Size(147, 24);
            PasswordInputBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            passwordLabel.Location = new System.Drawing.Point(17, 95);
            PasswordInputBox.Location = new System.Drawing.Point(224, 95);
            Login.Location = new System.Drawing.Point(560, 300);


            backButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            backButton.ForeColor = System.Drawing.Color.MidnightBlue;
            backButton.Location = new System.Drawing.Point(560, 265);
            backButton.Name = "backButton";
            backButton.Size = new System.Drawing.Size(82, 35);
            backButton.TabIndex = 21;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += new System.EventHandler(backButton_Click);

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
            BirthdateLabel.Text = "Birthdate (mm/dd/yy)";
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
            ExpirationLabel.Text = "Expiration (mm/yyyy)";
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
            Controls.Add(backButton);
            Login.Text = "Create";
        }
        private void adminSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            OutBox = new System.Windows.Forms.TextBox();
            manageButton = new System.Windows.Forms.Button();
            viewAllButton = new System.Windows.Forms.Button();

            OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox.Location = new System.Drawing.Point(25, 69);
            OutBox.Multiline = true;
            OutBox.Name = "OutBox";
            OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox.Size = new System.Drawing.Size(617, 116); //1233, 219
            OutBox.TabIndex = 0;
            OutBox.Text = "Putter Name: ";

            /*Controls.Remove(Login);
            Controls.Remove(LoginLabel);//should be username
            Controls.Remove(passwordLabel);
            Controls.Remove(LoginInputBox);
            Controls.Remove(PasswordInputBox);*/

            manageButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            manageButton.ForeColor = System.Drawing.Color.MidnightBlue;
            manageButton.Location = new System.Drawing.Point(560, 150);
            manageButton.Size = new System.Drawing.Size(83, 35);
            manageButton.TabIndex = 21;
            manageButton.Text = "Manage";
            manageButton.UseVisualStyleBackColor = true;
            manageButton.Click += new System.EventHandler(Manage_Click);
            manageButton.BringToFront();
            
            viewAllButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            viewAllButton.ForeColor = System.Drawing.Color.MidnightBlue;
            viewAllButton.Location = new System.Drawing.Point(25, 150);
            viewAllButton.Size = new System.Drawing.Size(83, 35);
            viewAllButton.TabIndex = 21;
            viewAllButton.Text = "View Putters";
            viewAllButton.UseVisualStyleBackColor = true;
            viewAllButton.Click += new System.EventHandler(viewAll_Click);
            viewAllButton.BringToFront();

            

            Controls.Add(manageButton);
            Controls.Add(viewAllButton);
            Controls.Add(OutBox);
        }
        private void fittingSetup()
        {
            counter = 0;
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
            optionsTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            importanceTitle.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            oneToFiveLabel.Text = "(1-5)" + Environment.NewLine+"L - H";
            
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
            infoButton.Size = new System.Drawing.Size(37, 37);
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
            info2Button.Size = new System.Drawing.Size(37, 37);
            info2Button.TabIndex = 23;
            info2Button.Text = "?";
            info2Button.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            info2Button.UseVisualStyleBackColor = true;
            info2Button.Click += new System.EventHandler(this.info2Button_Click);

            Controls.Add(optionsList);
            Controls.Add(importanceBox);
            Controls.Add(importanceTitle);
            Controls.Add(oneToFiveLabel);
            Controls.Add(optionsTitle);
            Controls.Add(NextButton);
            Controls.Add(infoButton);
            Controls.Add(info2Button);
            Controls.Add(LoginLabel);
            Controls.Add(HandicapLabel);

        }
        private void newPutterSetup()
        {
            optionsList = new System.Windows.Forms.ListBox();
            addLinkLabel = new System.Windows.Forms.Label();
            addLinkBox = new System.Windows.Forms.TextBox();

            manageButton.Text = "Add";
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(90, 230);
            optionsList.Name = "optionsList";//optionsList is like a class, creating new instances
            optionsList.Size = new System.Drawing.Size(306, 104);
            optionsList.TabIndex = 3;
            for (int a = 0; a < 4; a++)
                optionsList.Items.Add(putterList[putterCounter, a]);

            addLinkLabel.AutoSize = true;
            addLinkLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            addLinkLabel.ForeColor = System.Drawing.SystemColors.Control;
            addLinkLabel.Location = new System.Drawing.Point(400, 230);
            addLinkLabel.Name = "addLinkLabel";
            addLinkLabel.TabIndex = 24;
            addLinkLabel.Text = "Add Link to Putter (Optional)";
            addLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            addLinkBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            addLinkBox.ForeColor = System.Drawing.Color.DarkBlue;
            addLinkBox.Location = new System.Drawing.Point(420, 250);
            addLinkBox.Multiline = false;
            addLinkBox.Name = "HandicapInputBox";
            addLinkBox.Size = new System.Drawing.Size(147, 24);
            addLinkBox.TabIndex = 25;

            Controls.Add(optionsList);
            Controls.Add(addLinkLabel);
            Controls.Add(addLinkBox);
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
            optionsList = new System.Windows.Forms.ListBox();
            showMoreButton = new System.Windows.Forms.Button();
            showMyDetailsButton = new System.Windows.Forms.Button();
            startOverButton = new System.Windows.Forms.Button();
            signOutButton = new System.Windows.Forms.Button();

            optionsList.Size = new System.Drawing.Size(616, 116);
            optionsList.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ForeColor = System.Drawing.Color.MidnightBlue;
            optionsList.Location = new System.Drawing.Point(25, 69);
            optionsList.Name = "optionsList";
            optionsList.TabIndex = 3;
            optionsList.Items.Add("Best Fitting Putters: ");

            showMoreButton.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            showMoreButton.ForeColor = System.Drawing.Color.MidnightBlue;
            showMoreButton.Location = new System.Drawing.Point(540, 183);
            showMoreButton.Name = "showMoreButton";
            showMoreButton.Size = new System.Drawing.Size(102, 35);
            showMoreButton.TabIndex = 21;
            showMoreButton.Text = "Show Details";
            showMoreButton.UseVisualStyleBackColor = true;
            showMoreButton.Click += new System.EventHandler(showMore_Click);

            showMyDetailsButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            showMyDetailsButton.ForeColor = System.Drawing.Color.MidnightBlue;
            showMyDetailsButton.Location = new System.Drawing.Point(540, 233);
            showMyDetailsButton.Name = "showMyDetailsButton";
            showMyDetailsButton.Size = new System.Drawing.Size(102, 35);
            showMyDetailsButton.TabIndex = 21;
            showMyDetailsButton.Text = "My Details";
            showMyDetailsButton.UseVisualStyleBackColor = true;
            showMyDetailsButton.Click += new System.EventHandler(showMyDetails_Click);

            startOverButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            startOverButton.ForeColor = System.Drawing.Color.MidnightBlue;
            startOverButton.Location = new System.Drawing.Point(540, 283);
            startOverButton.Name = "startOverButton";
            startOverButton.Size = new System.Drawing.Size(102, 35);
            startOverButton.TabIndex = 21;
            startOverButton.Text = "Start Over";
            startOverButton.UseVisualStyleBackColor = true;
            startOverButton.Click += new System.EventHandler(startOver_Click);

            signOutButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            signOutButton.ForeColor = System.Drawing.Color.MidnightBlue;
            signOutButton.Location = new System.Drawing.Point(540, 333);
            signOutButton.Name = "signOutButton";
            signOutButton.Size = new System.Drawing.Size(102, 35);
            signOutButton.TabIndex = 21;
            signOutButton.Text = "Sign Out";
            signOutButton.UseVisualStyleBackColor = true;
            signOutButton.Click += new System.EventHandler(signOut_Click);

            Controls.Add(optionsList);
            Controls.Add(showMoreButton);
            Controls.Add(showMyDetailsButton);
            Controls.Add(startOverButton);
            Controls.Add(signOutButton);
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
        private void removeSetup()
        {
            manageButton.Text = "Remove";

            viewButton = new System.Windows.Forms.Button();
            OutBox2 = new System.Windows.Forms.TextBox();

            viewButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            viewButton.ForeColor = System.Drawing.Color.MidnightBlue;
            viewButton.Location = new System.Drawing.Point(560, 185);
            viewButton.Name = "viewButton";
            viewButton.Size = new System.Drawing.Size(82, 35);
            viewButton.TabIndex = 21;
            viewButton.Text = "View";
            viewButton.UseVisualStyleBackColor = true;
            viewButton.Click += new System.EventHandler(viewButton_Click);
            viewButton.BringToFront();

            OutBox2 = new System.Windows.Forms.TextBox();
            OutBox2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox2.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox2.Location = new System.Drawing.Point(25, 185);
            OutBox2.Multiline = true;
            OutBox2.Name = "OutBox2";
            OutBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox2.Size = new System.Drawing.Size(617, 115); //1233, 219
            OutBox2.TabIndex = 0;
            OutBox2.Text = OutBox.Text;

            Controls.Add(viewButton);
        }
        private void showMoreSetup()
        {
            Controls.Remove(goToSiteLink);

            string selected = optionsList.Text;
            person1.fit.putter.setCharacteristic(selected);

            characterLabel = new System.Windows.Forms.Label();
            goToSiteLink = new System.Windows.Forms.LinkLabel();

            characterLabel.AutoSize = true;
            characterLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            characterLabel.ForeColor = System.Drawing.SystemColors.Control;
            characterLabel.Location = new System.Drawing.Point(17, 175);
            characterLabel.Name = "characterLabel";
            characterLabel.TabIndex = 24;
            characterLabel.Text = "Putter: " + selected + 
                Environment.NewLine + "Putter Head Shape: " + person1.fit.putter.putterShape + 
                Environment.NewLine + "Putter Balance: " + person1.fit.putter.putterBalance +
                Environment.NewLine + "Putter Hosel: " + person1.fit.putter.putterHosel +
                Environment.NewLine + "Putter Weight: " + person1.fit.putter.putterWeight +
                Environment.NewLine + "Putter Feel: " + person1.fit.putter.putterFeel;
            characterLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            if(person1.fit.putter.putterLink != null)
            {
                goToSiteLink.AutoSize = true;
                goToSiteLink.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                goToSiteLink.LinkColor = System.Drawing.Color.Silver;
                goToSiteLink.Location = new System.Drawing.Point(17, 320);
                goToSiteLink.Name = "goToSiteLink";
                goToSiteLink.TabIndex = 22;
                goToSiteLink.TabStop = true;
                goToSiteLink.Text = "View Putter";
                goToSiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(link_LinkClicked);
                Controls.Add(goToSiteLink);
            }

            Controls.Add(characterLabel);

        }
        private void showMyDetailsSetup()
        {
            Controls.Remove(goToSiteLink);

            characterLabel = new System.Windows.Forms.Label();

            characterLabel.AutoSize = true;
            characterLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            characterLabel.ForeColor = System.Drawing.SystemColors.Control;
            characterLabel.Location = new System.Drawing.Point(17, 175);
            characterLabel.Name = "characterLabel";
            characterLabel.TabIndex = 24;
            characterLabel.Text = "Name: " + person1._Fname + " " + person1._Lname +
                Environment.NewLine + "Putter Head Shape: " + person1.fit.putterShape +
                Environment.NewLine + "Putter Balance: " + person1.fit.putterBalance +
                Environment.NewLine + "Putter Hosel: " + person1.fit.putterHosel +
                Environment.NewLine + "Putter Weight: " + person1.fit.putterWeight +
                Environment.NewLine + "Putter Feel: " + person1.fit.putterFeel;
            characterLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Controls.Add(characterLabel);
        }
        private void showMyDetails2Setup()
        {
            Controls.Remove(goToSiteLink);

            string selected = optionsList.Text;
            person1.fit.putter.setCharacteristic(selected);
            characterLabel = new System.Windows.Forms.Label();

            string putterShapeBool = "*No Match"; 
            string putterBalanceBool = "*No Match";
            string putterHoselBool = "*No Match"; 
            string putterWeightBool = "*No Match";
            string putterFeelBool = "*No Match";
            double matchingC = 0;
            if (person1.fit.putter.putterShape == person1.fit.putterShape)
            {
                matchingC++;
                putterShapeBool = "Match";
            }
            if (person1.fit.putter.putterBalance == person1.fit.putterBalance)
            {
                matchingC++;
                putterBalanceBool = "Match";
            }
            if (person1.fit.putter.putterHosel == person1.fit.putterHosel)
            {
                matchingC++;
                putterHoselBool = "Match";
            }
            if (person1.fit.putter.putterWeight == person1.fit.putterWeight)
            {
                matchingC++;
                putterWeightBool = "Match";
            }
            if(person1.fit.putter.putterFeel == person1.fit.putterFeel)
            {
                matchingC++;
                putterFeelBool = "Match";
            }

            characterLabel.AutoSize = true;
            characterLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            characterLabel.ForeColor = System.Drawing.SystemColors.Control;
            characterLabel.Location = new System.Drawing.Point(17, 175);
            characterLabel.Name = "characterLabel";
            characterLabel.TabIndex = 24;
            characterLabel.Text = "Name: " + person1._Fname + " " + person1._Lname +
                Environment.NewLine + "Putter Head Shape: " + person1.fit.putterShape + " (" + putterShapeBool + ")" +
                Environment.NewLine + "Putter Balance: " + person1.fit.putterBalance + " (" + putterBalanceBool + ")" +
                Environment.NewLine + "Putter Hosel: " + person1.fit.putterHosel + " (" + putterHoselBool + ")" +
                Environment.NewLine + "Putter Weight: " + person1.fit.putterWeight + " (" + putterWeightBool + ")" +
                Environment.NewLine + "Putter Feel: " + person1.fit.putterFeel + " (" + putterFeelBool + ")"+
                Environment.NewLine + (matchingC /5 *100).ToString() + "% Matching";

            characterLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Controls.Add(characterLabel);
        }


        private System.Windows.Forms.TextBox OutBox;
        private System.Windows.Forms.TextBox OutBox2;
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
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button newUserButton;
        private System.Windows.Forms.Button showMoreButton;
        private System.Windows.Forms.Button showMyDetailsButton;
        private System.Windows.Forms.Button signOutButton;
        private System.Windows.Forms.Button startOverButton;
        private System.Windows.Forms.Label characterLabel;
        private System.Windows.Forms.LinkLabel goToSiteLink;
        private System.Windows.Forms.Label addLinkLabel;
        private System.Windows.Forms.TextBox addLinkBox;


    }

}
/*
 * 

 */
