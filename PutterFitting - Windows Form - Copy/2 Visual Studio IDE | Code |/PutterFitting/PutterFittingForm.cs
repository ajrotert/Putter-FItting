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
        {"Left", "Right", "Not Applicable", "" },                          //Used in the fitting functions to display options
        {"Long", "Short", "Not Applicable", ""},
        {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"},
        {"Arcing Path", "Straight Back Straight Through", "", ""},
        {"Struggles with Alignment", "Alignment is Okay", "", ""},
        {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"},
        {"Wrist bend", "No Wrist Bend", "Not Applicable" ,""},
        {"Standard Size Grip", "Larger Grip", "Not Applicable", ""},
        {"Softer Feel", "Harder Feel", "Not Applicable", ""} };
        public string[] listNames = new string[9] {
        "Common L-R Miss",                                         //Used to display titles for the fitting functions
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

        /// <summary>
        /// Used for displaying and collecting the main data from the user. 
        /// Called after login, with the start fitting button is clicked.
        /// </summary>
        private void NextButton_Click(object sender, EventArgs e)
        {
            data[counter] = optionsList.Text;
            importance[counter] = Convert.ToInt32(importanceBox.Text);
            if (data[counter] != "" && importance[counter] > 0 && importance[counter] <= 5) //only continues if a valid option is clicked
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
            if (counter == 1)
                Controls.Add(fittingBackButton);
        }

        /// <summary>
        /// Called after the data is collected, starts the fitting.
        /// Calls the algorithm class, displays results for user
        /// </summary>
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

        /// <summary>
        /// Used by admin, used to view data in putters file, and users file
        /// Allows for a search of data, and to remove data if selected
        /// </summary>
        private void viewAll_Click(object sender, EventArgs e)
        {
            if (viewAllButton.Text == "Search")
            {
                string[] search;
                if (admin1.fileName == "putters.txt")
                {
                    search = OutBox.Text.Remove(0, 66).Split('\n');
                    showPutterSetup(); //displays options for putters
                }
                else
                {
                    search = OutBox.Text.Remove(0, 69).Split('\n');
                    showUserSetup(); //displays options for users
                }

                string[] results;
                if (search.Length == 1 && search[0] == "")
                    results = admin1.viewData(); //displays entire file
                else
                    results = admin1.viewData(search); //displays specified text
                if (results.Length == 1 && results[0] == "No results found.")
                    OutBox.Text += "\t\t\tResults: " + "0";
                else
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
                    admin1.manage = optionsList.Text.Split('|', '\n')[0]; //takes the first item in the selected line
                    admin1.Remove(); //removes data
                }
                if (admin1.fileName == "putters.txt")
                    adminPuttersSetup();
                else
                    adminUserSetup(sender, e);
            }
            else
            {
                Controls.Remove(optionsList);//Possible controls, not all are present
                Controls.Remove(addLinkBox);
                Controls.Remove(addLinkLabel);
                Controls.Remove(changeToBox);
                Controls.Remove(changeToButton);
                Controls.Remove(optionsList2);
                Controls.Remove(backButton);
                viewAllButton.Text = "Search";
                if (admin1.fileName == "putters.txt")
                {
                    OutBox.TextChanged += new System.EventHandler(Out3_TextChanged);
                    OutBox.Text = "Enter putter name, brand, or category. (Leave Blank to view all): ";
                }
                else
                    OutBox.Text = "Enter username, first name, or last name. (Leave Blank to view all): ";
                Controls.Remove(manageButton);
            }
        }

        /// <summary>
        /// Three brances, adds, removes, or searches for a putter.
        /// This is used by admin
        /// </summary>
        private void Manage_Click(object sender, EventArgs e)
        {
            if (manageButton.Text == "Add")
            {
                if (optionsList.Text != "")
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
            }
            else if (manageButton.Text == "Remove")
            {
                admin1.Remove();
                adminPuttersSetup();//resets process
            }
            else
            {
                string log = OutBox.Text;
                log = log.Remove(0, 13);
                admin1.manage = log;
                if (admin1.putterExist())
                {
                    admin1.setCharacteristic();
                    removeSetup();
                }
                else
                {
                    if (admin1.manage != null && admin1.manage != "")
                        newPutterSetup();
                }
            }
        }

        /// <summary>
        /// Checks to see if username and password match, if they do it creates an instanace
        /// If they don't match it loads a new screen to add a person
        /// </summary>
        private void Login_Click(object sender, EventArgs e)
        {
            if (Login.Text == "Login")
            {
                string username = LoginInputBox.Text;
                string password = PasswordInputBox.Text;

                if (Consumer.Login(username, password))
                {
                    SaveData saved = new SaveData("users.txt");
                    string[] information = saved.accessData('\u00AA' + username, password);
                    string[] instanceInformation = information[0].Split('\u00BB');//instance information
                    if (username == "admin")
                    {
                        admin1 = new Admin(instanceInformation[2], instanceInformation[3]);
                        adminPageSetup();
                    }
                    else
                    {
                        if (instanceInformation.Length == 7)
                            person1 = new Consumer(instanceInformation[0], instanceInformation[1], instanceInformation[4], instanceInformation[2], instanceInformation[3], instanceInformation[5]);//instance with handicap
                        else
                            person1 = new Consumer(instanceInformation[0], instanceInformation[1], instanceInformation[4], instanceInformation[2], instanceInformation[3]);//instance user
                        startPageSetup();
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
                try { birthdatTest = Convert.ToDateTime(birthdate); }
                catch { MessageBox.Show("Invalid Birthdate"); pass = false; }
                if (pass)
                {
                    try { expiration = Convert.ToDateTime(expirationString); }
                    catch { MessageBox.Show("Invalid Expiration Date"); pass = false; }
                }
                if (!(cvv2.Length == 3 && cvv2[0] >= '0' && cvv2[0] <= '9' && cvv2[1] >= '0' && cvv2[1] <= '9' && cvv2[2] >= '0' && cvv2[2] <= '9'))
                    MessageBox.Show("Invalid CVV2");
                else if (pass)
                {
                    person1 = new Consumer(username, password, birthdate, creditcardnumber, cvv2, expiration, fname, lname); //new instance
                    if (HandicapInputBox.Text != "")
                    {
                        if (person1.addNewPerson(HandicapInputBox.Text))
                            startPageSetup(); // fittingSetup();
                    }
                    else
                    {
                        if (person1.addNewPerson())
                            startPageSetup();//fittingSetup();
                    }
                }

            }
        }

        /// <summary>
        /// Checks to see what user clicked link, loads the respective options.
        /// If a person is not logged in, it allows the password to be reset.
        /// </summary>
        private void ChangePasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Users.Active)//if user instance was created
            {
                if (ChangePasswordLink.Text == "Change")
                {
                    string firstname = FNameInputBox.Text;
                    string lastname = LNameInputBox.Text;
                    string newpassword = PasswordInputBox.Text;

                    if (Admin.Active) //if user is admin
                    {
                        if (admin1.ChangePassword(firstname, lastname, newpassword))
                        {
                            ChangePasswordLink.Text = "Change Password";//resets the link
                            adminPageSetup();
                        }
                    }
                    else //user is not admin
                    {
                        if (person1.ChangePassword(firstname, lastname, newpassword, person1.username))
                        {
                            ChangePasswordLink.Text = "Change Password";//resets the link
                            startPageSetup();
                        }
                    }
                }
                else if (!Admin.Active && person1.username != "Guest")//admin is not active, consumer is
                    changePasswordSetup();
                else if (Admin.Active)
                    changePasswordSetup();
                else
                    MessageBox.Show("Cannot Change Password");
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
                    try
                    {
                        if (Users.ChangePassword(username, firstname, lastname, newpassword, Convert.ToDateTime(birthdate)))
                        { loginSetup(); }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Date");
                    }
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
            if (optionsList.SelectedIndex > 2 && optionsList.SelectedIndex < (3 + person1.results.Length))
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
            if (Admin.Active)
            {
                Admin.Active = false; //rests static fields
                admin1 = null;
            }
            else
            {
                Consumer.Active = false;
                person1 = null;
            }
            Users.Active = false;
            loginSetup();
        }

        private void link_LinkClicked(object sender, EventArgs e)
        {
            goToSiteLink.LinkVisited = true;
            try { System.Diagnostics.Process.Start(person1.fit.putter.putterLink); }
            catch { MessageBox.Show(person1.fit.putter.putterLink + " is not a valid URL"); }
        }
        private void start_Click(object sender, EventArgs e)
        {
            fittingSetup();
        }
        private void profile_Click(object sender, EventArgs e)
        {
            if (profileButton.Text == "View Profile")
                profileSetup();
            else
                startPageSetup();
        }

        /// <summary>
        /// Changes the text based on what index of teh list is selected
        /// </summary>
        private void changeTo_Click(object sender, EventArgs e)
        {
            if (changeToBox.Text == "")
            {
                MessageBox.Show("Fill text box with replacement text");
            }
            else
            {
                if (optionsList.SelectedIndex == 0)
                {
                    if (!person1.changeUserInformation(changeToBox.Text, person1.password, person1._Fname, person1._Lname, person1.birthdate.ToShortDateString()))
                        MessageBox.Show("Username Exists");
                }
                else if (optionsList.SelectedIndex == 1)
                {
                    MessageBox.Show("Password cannot be changed here.");
                }
                else if (optionsList.SelectedIndex == 2)
                {
                    person1.changeUserInformation(person1.username, person1.password, changeToBox.Text, person1._Lname, person1.birthdate.ToShortDateString());
                }
                else if (optionsList.SelectedIndex == 3)
                {
                    person1.changeUserInformation(person1.username, person1.password, person1._Fname, changeToBox.Text, person1.birthdate.ToShortDateString());
                }
                else if (optionsList.SelectedIndex == 4)
                {
                    DateTime bdate;
                    try
                    {
                        bdate = Convert.ToDateTime(changeToBox.Text);
                        person1.changeUserInformation(person1.username, person1.password, person1._Fname, person1._Lname, bdate.ToShortDateString());
                    }
                    catch { MessageBox.Show("Invalid Date"); }
                }
                else if (optionsList.SelectedIndex == 5)
                {
                    person1.changeUserInformation(person1.username, person1.password, person1._Fname, person1._Lname, person1.birthdate.ToShortDateString(), changeToBox.Text);
                }
            }
            if (optionsList.SelectedIndex < 0)
            {
                MessageBox.Show("No Item Selected.");
            }
            else
                profileSetup();
        }

        /// <summary>
        /// Called when admin wnats to change putter traits
        /// </summary>
        private void option_Click(object sender, EventArgs e)
        {
            optionsList2.Items.Clear();
            Controls.Remove(optionsList2);
            Controls.Add(optionsList2);
            Controls.Remove(changeToBox);
            if (optionsList.SelectedIndex < 5)
            {
                for (int a = 0; a < 4; a++)
                    optionsList2.Items.Add(putterList[optionsList.SelectedIndex, a]);
            }
            else if (optionsList.SelectedIndex == 5)
            {
                Controls.Remove(optionsList2);
                Controls.Add(changeToBox);
            }
        }
        private void changeTo2_Click(object sender, EventArgs e)
        {
            if (optionsList2.SelectedIndex < 0 || optionsList2.Text == "")
            {
                if (changeToBox.Text != "")
                {
                    admin1.Remove();
                    admin1.setCharacteristic(admin1.putterShape, admin1.putterBalance, admin1.putterHosel, admin1.putterWeight, admin1.putterFeel, changeToBox.Text);
                }
                else
                    MessageBox.Show("Select a valid item");
            }
            else
            {
                if (optionsList.SelectedIndex == 0)
                {
                    admin1.Remove();
                    admin1.setCharacteristic(optionsList2.Text, admin1.putterBalance, admin1.putterHosel, admin1.putterWeight, admin1.putterFeel, admin1.putterLink);
                }
                else if (optionsList.SelectedIndex == 1)
                {
                    admin1.Remove();
                    admin1.setCharacteristic(admin1.putterShape, optionsList2.Text, admin1.putterHosel, admin1.putterWeight, admin1.putterFeel, admin1.putterLink);
                }
                else if (optionsList.SelectedIndex == 2)
                {
                    admin1.Remove();
                    admin1.setCharacteristic(admin1.putterShape, admin1.putterBalance, optionsList2.Text, admin1.putterWeight, admin1.putterFeel, admin1.putterLink);
                }
                else if (optionsList.SelectedIndex == 3)
                {
                    admin1.Remove();
                    admin1.setCharacteristic(admin1.putterShape, admin1.putterBalance, admin1.putterHosel, optionsList2.Text, admin1.putterFeel, admin1.putterLink);
                }
                else if (optionsList.SelectedIndex == 4)
                {
                    admin1.Remove();
                    admin1.setCharacteristic(admin1.putterShape, admin1.putterBalance, admin1.putterHosel, admin1.putterWeight, optionsList2.Text, admin1.putterLink);
                }

            }
            if (optionsList.SelectedIndex < 0)
            {
                MessageBox.Show("No Item Selected.");
            }
            else
                adminChangePutterSetup();
        }
        private void managePutters_Click(object sender, EventArgs e)
        {
            adminPuttersSetup();
        }
        private void manageUsers_Click(object sender, EventArgs e)
        {
            adminUserSetup(sender, e);
        }

        /// <summary>
        /// Text displayed when user clicks on ? button
        /// </summary>
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
            if (Algorithm.importanceLevel[counter] == 0)
                MessageBox.Show("Recommended Importance Level: 1");
            else
                MessageBox.Show("Recommended Importance Level: " + Algorithm.importanceLevel[counter]);
        }

        /// <summary>
        /// Called when user clicks back button durning data collection for fitting
        /// </summary>
        private void fittingBack_Click(object sender, EventArgs e)
        {
            counter--;
            optionsList.Items.Clear();
            optionsTitle.Text = listNames[counter];
            for (int a = 0; a < 4; a++)
                optionsList.Items.Add(listItems[counter, a]);
            importanceBox.Text = "5";
            if (counter == 0)
                Controls.Remove(fittingBackButton);
        }

        /// <summary>
        /// creates a new instance for guest login
        /// </summary>
        private void guest_LinkClicked(object sender, EventArgs e)
        {
            person1 = new Consumer('\u00AA' + "Guest", "Guest", DateTime.Now.ToShortDateString(), "User", "Guest");
            startPageSetup();
        }
        private void home_LinkClicked(object sender, EventArgs e)
        {
            ChangePasswordLink.Text = "Change Password"; //From some screens the link is not reset, so needed to reset it here
            if (Admin.Active)
                adminPageSetup();
            else if (Consumer.Active)
                startPageSetup();
            else
                loginSetup();
        }
        private void back2_Click(object sender, EventArgs e)
        {
            putterCounter = 0;
            adminPuttersSetup();
        }

        /// <summary>
        /// For the following three events, it checks everytime the text is changed,
        /// This makes sure that a user cannot delete text
        /// </summary>
        private void Out1_TextChanged(object sender, EventArgs e)
        {
            if (OutBox.Text.Length < 14)
                OutBox.Text = "Putter Name: ";
        }
        private void Out2_TextChanged(object sender, EventArgs e)
        {
            if (OutBox.Text.Length < 70)
                OutBox.Text = "Enter username, first name, or last name. (Leave Blank to view all): ";
        }
        private void Out3_TextChanged(object sender, EventArgs e)
        {
            if (OutBox.Text.Length < 67)
                OutBox.Text = "Enter putter name, brand, or category. (Leave Blank to view all): ";
        }


        /// <summary>
        /// All the setups load the correct windows controls. 
        /// </summary>
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
            guestLink = new System.Windows.Forms.LinkLabel();

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
            LoginLabel.Text = "Username";
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

            guestLink.AutoSize = true;
            guestLink.LinkColor = System.Drawing.Color.Silver;
            guestLink.Location = new System.Drawing.Point(563, 85);
            guestLink.Name = "guestLink";
            guestLink.Size = new System.Drawing.Size(93, 13);
            guestLink.TabIndex = 22;
            guestLink.TabStop = true;
            guestLink.Text = "Login as Guest";
            guestLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(guest_LinkClicked);

            Controls.Add(PasswordInputBox);
            Controls.Add(LoginInputBox);
            Controls.Add(passwordLabel);
            Controls.Add(LoginLabel);
            Controls.Add(Login);
            Controls.Add(newUserButton);
            Controls.Add(guestLink);
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
        private void adminPuttersSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            admin1.setFile("putters.txt");

            OutBox = new System.Windows.Forms.TextBox();
            manageButton = new System.Windows.Forms.Button();
            viewAllButton = new System.Windows.Forms.Button();
            homeLink = new System.Windows.Forms.LinkLabel();

            OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox.Location = new System.Drawing.Point(25, 69);
            OutBox.Multiline = true;
            OutBox.Name = "OutBox";
            OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox.Size = new System.Drawing.Size(617, 116); //1233, 219
            OutBox.TabIndex = 0;
            OutBox.TextChanged += new System.EventHandler(Out1_TextChanged);
            OutBox.Text = "Putter Name: ";

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

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

            Controls.Add(homeLink);
            Controls.Add(manageButton);
            Controls.Add(viewAllButton);
            Controls.Add(OutBox);
        }
        private void adminUserSetup(object sender, EventArgs e)
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            admin1.setFile("users.txt");

            viewAllButton = new System.Windows.Forms.Button();
            OutBox = new System.Windows.Forms.TextBox();
            homeLink = new System.Windows.Forms.LinkLabel();

            viewAllButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            viewAllButton.ForeColor = System.Drawing.Color.MidnightBlue;
            viewAllButton.Location = new System.Drawing.Point(25, 150);
            viewAllButton.Size = new System.Drawing.Size(83, 35);
            viewAllButton.TabIndex = 21;
            viewAllButton.Text = "View Users";
            viewAllButton.UseVisualStyleBackColor = true;
            viewAllButton.Click += new System.EventHandler(viewAll_Click);
            viewAllButton.BringToFront();

            OutBox.Font = new System.Drawing.Font("Tahoma", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OutBox.ForeColor = System.Drawing.Color.MidnightBlue;
            OutBox.Location = new System.Drawing.Point(25, 69);
            OutBox.Multiline = true;
            OutBox.Name = "OutBox";
            OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            OutBox.Size = new System.Drawing.Size(617, 116); //1233, 219
            OutBox.TabIndex = 0;
            OutBox.TextChanged += new System.EventHandler(Out2_TextChanged);


            viewAll_Click(sender, e);

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

            Controls.Add(homeLink);
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
            fittingBackButton = new System.Windows.Forms.Button();
            homeLink = new System.Windows.Forms.LinkLabel();

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
            importanceBox.Location = new System.Drawing.Point(483, 200);
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
            oneToFiveLabel.Location = new System.Drawing.Point(495, 260);
            oneToFiveLabel.Name = "oneToFiveLabel";
            oneToFiveLabel.Size = new System.Drawing.Size(57, 25);
            oneToFiveLabel.TabIndex = 21;
            oneToFiveLabel.Text = "(1-5)" + Environment.NewLine + "L - H";

            NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NextButton.ForeColor = System.Drawing.Color.MidnightBlue;
            NextButton.Location = new System.Drawing.Point(360, 310);
            NextButton.Name = "NextButton";
            NextButton.Size = new System.Drawing.Size(180, 64);
            NextButton.TabIndex = 22;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += new System.EventHandler(this.NextButton_Click);

            fittingBackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            fittingBackButton.ForeColor = System.Drawing.Color.MidnightBlue;
            fittingBackButton.Location = new System.Drawing.Point(120, 310);
            fittingBackButton.Name = "fittingBackButton";
            fittingBackButton.Size = new System.Drawing.Size(180, 64);
            fittingBackButton.TabIndex = 22;
            fittingBackButton.Text = "Back";
            fittingBackButton.UseVisualStyleBackColor = true;
            fittingBackButton.Click += new System.EventHandler(fittingBack_Click);

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

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

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
            Controls.Add(homeLink);

        }
        private void newPutterSetup()
        {
            optionsList = new System.Windows.Forms.ListBox();
            addLinkLabel = new System.Windows.Forms.Label();
            addLinkBox = new System.Windows.Forms.TextBox();
            backButton = new System.Windows.Forms.Button();

            backButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            backButton.ForeColor = System.Drawing.Color.MidnightBlue;
            backButton.Location = new System.Drawing.Point(25, 190);
            backButton.Size = new System.Drawing.Size(83, 35);
            backButton.TabIndex = 21;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += new System.EventHandler(back2_Click);
            backButton.BringToFront();

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
            Controls.Add(backButton);
        }
        private void putterEndSetup()
        {
            putterCounter = 0;
            adminPuttersSetup();//resets process
        }
        private void showPutterSetup()
        {
            Controls.Remove(backButton);

            backButton = new System.Windows.Forms.Button();

            backButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            backButton.ForeColor = System.Drawing.Color.MidnightBlue;
            backButton.Location = new System.Drawing.Point(25, 190);
            backButton.Size = new System.Drawing.Size(83, 35);
            backButton.TabIndex = 21;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += new System.EventHandler(back2_Click);
            backButton.BringToFront();

            optionsList = new System.Windows.Forms.ListBox();
            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(25, 230);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(620, 104);
            optionsList.TabIndex = 3;

            Controls.Add(backButton);
            Controls.Add(optionsList);
        }
        private void showUserSetup()
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
            homeLink = new System.Windows.Forms.LinkLabel();

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

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

            Controls.Add(homeLink);
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
            homeLink = new System.Windows.Forms.LinkLabel();

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

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

            Controls.Add(FirstNameLabel);
            Controls.Add(LastNameLabel);
            Controls.Add(FNameInputBox);
            Controls.Add(LNameInputBox);
            Controls.Add(passwordLabel);
            Controls.Add(PasswordInputBox);
            Controls.Add(homeLink);
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
            BirthdateLabel.Text = "Birthdate (mm/dd/yy)";
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
            adminChangePutterSetup();
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

            if (person1.fit.putter.putterLink != null)
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
            if (person1.fit.putter.putterShape == person1.fit.putterShape)
                putterShapeBool = "Match";
            if (person1.fit.putter.putterBalance == person1.fit.putterBalance)
                putterBalanceBool = "Match";
            if (person1.fit.putter.putterHosel == person1.fit.putterHosel)
                putterHoselBool = "Match";
            if (person1.fit.putter.putterWeight == person1.fit.putterWeight)
                putterWeightBool = "Match";
            if (person1.fit.putter.putterFeel == person1.fit.putterFeel)
                putterFeelBool = "Match";

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
                Environment.NewLine + "Putter Feel: " + person1.fit.putterFeel + " (" + putterFeelBool + ")" +
                Environment.NewLine + person1.fit.Matching().ToString("0.##") + "% Matching";

            characterLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;

            Controls.Add(characterLabel);
        }
        private void startPageSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            if (person1.username == "Guest")
                fittingSetup();
            else
            {
                LoginLabel = new System.Windows.Forms.Label();
                HandicapLabel = new System.Windows.Forms.Label();
                startButton = new System.Windows.Forms.Button();
                profileButton = new System.Windows.Forms.Button();
                homeLink = new System.Windows.Forms.LinkLabel();
                signOutButton = new System.Windows.Forms.Button();

                LoginLabel.AutoSize = true;
                LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                LoginLabel.ForeColor = System.Drawing.SystemColors.Control;
                LoginLabel.Location = new System.Drawing.Point(17, 54);
                LoginLabel.Name = "LoginLabel";
                LoginLabel.TabIndex = 23;
                LoginLabel.Text = "Name: " + person1._Lname + ", " + person1._Fname;
                LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                HandicapLabel.AutoSize = true;
                HandicapLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                HandicapLabel.ForeColor = System.Drawing.SystemColors.Control;
                HandicapLabel.Location = new System.Drawing.Point(17, 95);
                HandicapLabel.Name = "HandicapLabel";
                HandicapLabel.TabIndex = 23;
                HandicapLabel.Text = "Handicap: " + person1.Handicap;
                HandicapLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                startButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                startButton.ForeColor = System.Drawing.Color.MidnightBlue;
                startButton.Location = new System.Drawing.Point(17, 155);
                startButton.Name = "startButton";
                startButton.Size = new System.Drawing.Size(110, 35);
                startButton.TabIndex = 21;
                startButton.Text = "Start Fitting";
                startButton.UseVisualStyleBackColor = true;
                startButton.Click += new System.EventHandler(start_Click);
                startButton.BringToFront();

                profileButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                profileButton.ForeColor = System.Drawing.Color.MidnightBlue;
                profileButton.Location = new System.Drawing.Point(17, 195);
                profileButton.Name = "profileButton";
                profileButton.Size = new System.Drawing.Size(110, 35);
                profileButton.TabIndex = 21;
                profileButton.Text = "View Profile";
                profileButton.UseVisualStyleBackColor = true;
                profileButton.Click += new System.EventHandler(profile_Click);
                profileButton.BringToFront();

                signOutButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                signOutButton.ForeColor = System.Drawing.Color.MidnightBlue;
                signOutButton.Location = new System.Drawing.Point(17, 235);
                signOutButton.Name = "signOutButton";
                signOutButton.Size = new System.Drawing.Size(110, 35);
                signOutButton.TabIndex = 21;
                signOutButton.Text = "Sign Out";
                signOutButton.UseVisualStyleBackColor = true;
                signOutButton.Click += new System.EventHandler(signOut_Click);

                homeLink.AutoSize = true;
                homeLink.LinkColor = System.Drawing.Color.Silver;
                homeLink.Location = new System.Drawing.Point(605, 85);
                homeLink.Name = "homeLink";
                homeLink.Size = new System.Drawing.Size(93, 13);
                homeLink.TabIndex = 22;
                homeLink.TabStop = true;
                homeLink.Text = "Home";
                homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

                Controls.Add(LoginLabel);
                Controls.Add(HandicapLabel);
                Controls.Add(startButton);
                Controls.Add(profileButton);
                Controls.Add(homeLink);
                Controls.Add(signOutButton);
            }
        }
        private void profileSetup()
        {
            startPageSetup();
            profileButton.Text = "Back";

            Controls.Remove(signOutButton);

            optionsList = new System.Windows.Forms.ListBox();
            changeToBox = new System.Windows.Forms.TextBox();
            changeToButton = new System.Windows.Forms.Button();

            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(140, 155);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(500, 175);
            optionsList.TabIndex = 3;

            optionsList.Items.Add("Username: " + person1.username);
            optionsList.Items.Add("Password: " + person1.password);
            optionsList.Items.Add("First Name: " + person1._Fname);
            optionsList.Items.Add("Last Name: " + person1._Lname);
            optionsList.Items.Add("Birthdate: " + person1.birthdate.ToShortDateString());
            if (person1.Handicap != "(None)")
                optionsList.Items.Add("Handicap: " + person1.Handicap);
            else
                optionsList.Items.Add("No handicap available");//instead add button to add handicap

            changeToButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            changeToButton.ForeColor = System.Drawing.Color.MidnightBlue;
            changeToButton.Location = new System.Drawing.Point(17, 235);
            changeToButton.Name = "changeToButton";
            changeToButton.Size = new System.Drawing.Size(110, 35);
            changeToButton.TabIndex = 21;
            changeToButton.Text = "Change Selected";
            changeToButton.UseVisualStyleBackColor = true;
            changeToButton.Click += new System.EventHandler(changeTo_Click);
            changeToButton.BringToFront();

            changeToBox.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            changeToBox.ForeColor = System.Drawing.Color.DarkBlue;
            changeToBox.Location = new System.Drawing.Point(17, 275);
            changeToBox.Multiline = false;
            changeToBox.Name = "changeToBox";
            changeToBox.Size = new System.Drawing.Size(110, 48);
            changeToBox.TabIndex = 25;

            Controls.Add(changeToBox);
            Controls.Add(changeToButton);
            Controls.Add(optionsList);
        }
        private void adminPageSetup()
        {
            Controls.Clear();
            Controls.Add(PutterTitle);
            Controls.Add(ChangePasswordLink);

            managePuttersButton = new System.Windows.Forms.Button();
            manageUsersButton = new System.Windows.Forms.Button();
            LoginLabel = new System.Windows.Forms.Label();
            HandicapLabel = new System.Windows.Forms.Label();
            signOutButton = new System.Windows.Forms.Button();
            homeLink = new System.Windows.Forms.LinkLabel();

            LoginLabel.AutoSize = true;
            LoginLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            LoginLabel.ForeColor = System.Drawing.SystemColors.Control;
            LoginLabel.Location = new System.Drawing.Point(17, 54);
            LoginLabel.Name = "LoginLabel";
            LoginLabel.TabIndex = 23;
            LoginLabel.Text = "Name: " + admin1._Lname + ", " + admin1._Fname;
            LoginLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            HandicapLabel.AutoSize = true;
            HandicapLabel.Font = new System.Drawing.Font("Segoe MDL2 Assets", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            HandicapLabel.ForeColor = System.Drawing.SystemColors.Control;
            HandicapLabel.Location = new System.Drawing.Point(17, 95);
            HandicapLabel.Name = "HandicapLabel";
            HandicapLabel.TabIndex = 23;
            HandicapLabel.Text = "Administrator";
            HandicapLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            managePuttersButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            managePuttersButton.ForeColor = System.Drawing.Color.MidnightBlue;
            managePuttersButton.Location = new System.Drawing.Point(17, 155);
            managePuttersButton.Name = "managePuttersButton";
            managePuttersButton.Size = new System.Drawing.Size(130, 35);
            managePuttersButton.TabIndex = 21;
            managePuttersButton.Text = "Manage Putters";
            managePuttersButton.UseVisualStyleBackColor = true;
            managePuttersButton.Click += new System.EventHandler(managePutters_Click);
            managePuttersButton.BringToFront();

            manageUsersButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            manageUsersButton.ForeColor = System.Drawing.Color.MidnightBlue;
            manageUsersButton.Location = new System.Drawing.Point(17, 195);
            manageUsersButton.Name = "manageUsersButton";
            manageUsersButton.Size = new System.Drawing.Size(130, 35);
            manageUsersButton.TabIndex = 21;
            manageUsersButton.Text = "Manage Users";
            manageUsersButton.UseVisualStyleBackColor = true;
            manageUsersButton.Click += new System.EventHandler(manageUsers_Click);
            manageUsersButton.BringToFront();

            signOutButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            signOutButton.ForeColor = System.Drawing.Color.MidnightBlue;
            signOutButton.Location = new System.Drawing.Point(17, 235);
            signOutButton.Name = "signOutButton";
            signOutButton.Size = new System.Drawing.Size(130, 35);
            signOutButton.TabIndex = 21;
            signOutButton.Text = "Sign Out";
            signOutButton.UseVisualStyleBackColor = true;
            signOutButton.Click += new System.EventHandler(signOut_Click);

            homeLink.AutoSize = true;
            homeLink.LinkColor = System.Drawing.Color.Silver;
            homeLink.Location = new System.Drawing.Point(605, 85);
            homeLink.Name = "homeLink";
            homeLink.Size = new System.Drawing.Size(93, 13);
            homeLink.TabIndex = 22;
            homeLink.TabStop = true;
            homeLink.Text = "Home";
            homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(home_LinkClicked);

            Controls.Add(homeLink);
            Controls.Add(LoginLabel);
            Controls.Add(HandicapLabel);
            Controls.Add(manageUsersButton);
            Controls.Add(managePuttersButton);
            Controls.Add(manageUsersButton);
            Controls.Add(signOutButton);
        }
        private void adminChangePutterSetup()
        {
            Controls.Remove(backButton);
            Controls.Remove(changeToButton);
            Controls.Remove(changeToBox);
            Controls.Remove(optionsList);
            Controls.Remove(optionsList2);

            optionsList = new System.Windows.Forms.ListBox();
            changeToButton = new System.Windows.Forms.Button();
            backButton = new System.Windows.Forms.Button();
            optionsList2 = new System.Windows.Forms.ListBox();
            changeToBox = new System.Windows.Forms.TextBox();

            optionsList.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList.FormattingEnabled = true;
            optionsList.ItemHeight = 25;
            optionsList.Location = new System.Drawing.Point(140, 190);
            optionsList.Name = "optionsList";
            optionsList.Size = new System.Drawing.Size(500, 155);
            optionsList.TabIndex = 3;

            optionsList.Items.Add("Putter Shape: " + admin1.putterShape);
            optionsList.Items.Add("Putter Balance: " + admin1.putterBalance);
            optionsList.Items.Add("Putter Hosel: " + admin1.putterHosel);
            optionsList.Items.Add("Putter Weight: " + admin1.putterWeight);
            optionsList.Items.Add("Putter Feel: " + admin1.putterFeel);
            if (admin1.putterLink != "None")
                optionsList.Items.Add("Link: " + admin1.putterLink);
            else
                optionsList.Items.Add("No Link Available");
            optionsList.Click += new System.EventHandler(option_Click);

            changeToButton.Font = new System.Drawing.Font("Tahoma", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            changeToButton.ForeColor = System.Drawing.Color.MidnightBlue;
            changeToButton.Location = new System.Drawing.Point(25, 270);
            changeToButton.Name = "changeToButton";
            changeToButton.Size = new System.Drawing.Size(83, 35);
            changeToButton.TabIndex = 21;
            changeToButton.Text = "Change Selected";
            changeToButton.UseVisualStyleBackColor = true;
            changeToButton.Click += new System.EventHandler(changeTo2_Click);
            changeToButton.BringToFront();

            changeToBox.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            changeToBox.ForeColor = System.Drawing.Color.DarkBlue;
            changeToBox.Location = new System.Drawing.Point(25, 310);
            changeToBox.Multiline = false;
            changeToBox.Name = "changeToBox";
            changeToBox.Size = new System.Drawing.Size(98, 35);
            changeToBox.TabIndex = 25;

            optionsList2.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            optionsList2.FormattingEnabled = true;
            optionsList2.ItemHeight = 25;
            optionsList2.Location = new System.Drawing.Point(25, 310);
            optionsList2.Name = "optionsList";
            optionsList2.Size = new System.Drawing.Size(98, 35);//83
            optionsList2.TabIndex = 3;

            backButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            backButton.ForeColor = System.Drawing.Color.MidnightBlue;
            backButton.Location = new System.Drawing.Point(25, 190);
            backButton.Size = new System.Drawing.Size(83, 35);
            backButton.TabIndex = 21;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += new System.EventHandler(back2_Click);
            backButton.BringToFront();

            manageButton.Location = new System.Drawing.Point(25, 230);

            Controls.Add(backButton);
            Controls.Add(changeToButton);
            Controls.Add(optionsList);
            Controls.Add(optionsList2);
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
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button profileButton;
        private System.Windows.Forms.TextBox changeToBox;
        private System.Windows.Forms.Button changeToButton;
        private System.Windows.Forms.Button managePuttersButton;
        private System.Windows.Forms.Button manageUsersButton;
        private System.Windows.Forms.Button fittingBackButton;
        private System.Windows.Forms.LinkLabel guestLink;
        private System.Windows.Forms.LinkLabel homeLink;
        private System.Windows.Forms.ListBox optionsList2;
        
    }

}