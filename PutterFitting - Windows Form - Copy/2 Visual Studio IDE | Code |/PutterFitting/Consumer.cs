using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting 
{

    public class Consumer : Users
	{
        public Consumer(string username, string password, string birtdate, string Fname, string Lname):base(Fname, Lname)
        {//User already exists without handicap
            _username = username;
            this.password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
            Handicap = "(None)";
        }
        public Consumer(string username, string password, string birtdate, string Fname, string Lname, string handicap) : base(Fname, Lname)
        {//User already exists with handicap
            _username = username;
            this.password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
            Handicap = handicap;
        }
        public Consumer(string username, string password, string birtdate, string CrediCardNumber, string cvv2, DateTime expirationDate, string Fname, string Lname) : base(Fname, Lname)
        { //Constructor for new user
            this.username = username;
            this.password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
            _UserCard = new CreditCard(CrediCardNumber, cvv2, expirationDate);
            _Handicap = "(None)";
        }
        public Consumer(Consumer c) : base(c._Fname, c._Lname) //used if the consumer restarts, creates a new instance that copies over
        {
            username = c.username;
            password = c.password;
            _Birthdate = c._Birthdate;
            _UserCard = c._UserCard;
            _Handicap = c.Handicap;
        }
        string _username;
		string _password;
		string _Handicap;
        public string[] results;
        public Algorithm fit;
        CreditCard _UserCard;
        private static SaveData save = new SaveData("users.txt");//called without instance of user for login

        public string username //since username from file has a speical char, this will add or remove it
        {
            get { return _username.Remove(0,1); }
            private set { _username = '\u00AA' + value; }
        }
        public string password
        {
            get { return _password; }
            private set { _password = value; }
        }
        public string Handicap
        {
            get { return _Handicap; }
            private set { _Handicap = value; }
        }
        /// <summary>
        /// static so that a instance is not necessary to check if user exists in file
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Login(string username, string password) 
        {
            return save.verify('\u00AA' + username, password);
        }

        /// <summary>
        /// Saves person, and charges them, also allows for handicap to be added
        /// </summary>
        /// <param name="handicap"></param>
        /// <returns></returns>
        public bool addNewPerson(string handicap = null)
        {
            if (!UserSave.verify('\u00AA' + username) && username!="Guest")//if username does not exist then it will continue
            {
                if (_UserCard.MakePayment(5.99))
                {
                    if (handicap != null)
                    {
                        Handicap = handicap;
                        return UserSave.save(_username, password, _Fname, _Lname, _Birthdate.ToShortDateString(), handicap);//UserSave is inherted from users
                    }
                    else
                        return UserSave.save(_username, password, _Fname, _Lname, _Birthdate.ToShortDateString());
                }
                return false;
            }
            else
            {
                MessageBox.Show("Username Exists");
                return false;
            }
        }

        /// <summary>
        /// Adds a new person without all the checks, used for change password, or other information
        /// </summary>
        /// <param name="makeNew"></param>
        /// <returns></returns>
        private bool addNewPerson(bool makeNew)
        {
            if (makeNew)
            {
                if (!save.verify(_username))
                {
                    if (Handicap != "(None)")
                        return UserSave.save(_username, password, _Fname, _Lname, _Birthdate.ToShortDateString(), Handicap);
                    return UserSave.save(_username, password, _Fname, _Lname, _Birthdate.ToShortDateString());
                }
                else
                    return false;

            }
            return false;
        }

        /// <summary>
        /// Takes in the collected data from user, and sends it to algorithm class
        /// </summary>
        /// <param name="UserData"></param>
        /// <param name="UserImportance"></param>
        public void startFit(string[] UserData, int[] UserImportance)
        {
            fit = new Algorithm(UserData, UserImportance);
            results = fit.FindPutter();
            fit.setCharacteristic();
        }

        /// <summary>
        /// Removes person, changes all the information, then adds them to file agian
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="birthdate"></param>
        /// <param name="handicap"></param>
        /// <returns></returns>
        public bool changeUserInformation(string username, string password, string fname, string lname, string birthdate, string handicap = null)
        {
            bool check = username == this.username;
            if (check || !save.verify(username))
            {
                bool removed = UserSave.remove(this.username);
                this.username = username;
                this.password = password;
                _Fname = fname;
                _Lname = lname;
                _Birthdate = Convert.ToDateTime(birthdate);
                if (handicap != null)
                    Handicap = handicap;

                return addNewPerson(removed);
            }
            return false;
        }

        /// <summary>
        /// calls base function, then adds new information to file.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="newPassword"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool ChangePassword(string firstName, string lastName, string newPassword, string username)
        {
            if (base.ChangePassword(username, firstName, lastName))
            {
                password = newPassword;
                return addNewPerson(true);
            }
            else
            {
                MessageBox.Show("Credentials don't match");
                return false;
            }
        }
        ~Consumer()
        {
            //Active = false;
        }
    }
}
//add function that gets credit card number, may store in seperate file