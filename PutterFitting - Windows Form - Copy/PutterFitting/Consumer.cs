using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting 
{

    public class Consumer : Users
	{
        public Consumer(string username, string password, string birtdate, string Fname, string Lname):base(Fname, Lname)
        {
            _username = username;
            _password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
            _Handicap = "(None)";
        }
        public Consumer(string username, string password, string birtdate, string Fname, string Lname, string handicap) : base(Fname, Lname)
        {
            _username = username;
            _password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
            _Handicap = handicap;
        }
        public Consumer(string username, string password, string birtdate, string CrediCardNumber, string cvv2, DateTime expirationDate, string Fname, string Lname) : base(Fname, Lname)
        {
             _username = username;
             _password = password;
             _Birthdate = Convert.ToDateTime(birtdate);
             _UserCard = new CreditCard(CrediCardNumber, cvv2, expirationDate);
            _Handicap = "(None)";
        }
        public Consumer(Consumer c) : base(c._Fname, c._Lname) //used if the consumer restarts, creates a new instance that copies over
        {
            _username = c.username;
            _password = c.password;
            _Birthdate = c._Birthdate;
            _UserCard = c._UserCard;
            _Handicap = c.Handicap;
        }
        string _username; //not in superclass, because admin has set username, and password
		string _password;
		string _Handicap;
        public string[] results;
        public Algorithm fit;
        CreditCard _UserCard;
        private static SaveData save = new SaveData("users.txt");//called without instance of user for login

        public string username
        {
            get { return _username; }
            private set { _username = value; }
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
        public static bool Login(string username, string password) //static so that a instance is not necessary to check if user login exists
        {
            return save.verify(username, password);
        }
        public bool addNewPerson(string handicap = null)//not static, allow for a new instance ot be created to save
        {
            if (!UserSave.verify(username))//if username does not exist then it will continue
            {
                if (_UserCard.MakePayment(5.99))
                {
                    if (handicap != null)
                    {
                        Handicap = handicap;
                        return UserSave.save(username, password, _Fname, _Lname, _Birthdate.ToShortDateString(), handicap);//UserSave is inherted from users
                    }
                    else
                        return UserSave.save(username, password, _Fname, _Lname, _Birthdate.ToShortDateString());
                }
                return false;
            }
            else
            {
                MessageBox.Show("Username Exists");
                return false;
            }
        }
        private bool addNewPerson(bool makeNew)//the user logs in, they dont have a cc active, this overloads the first one that makes a payment
        {
            return UserSave.save(username, password, _Fname, _Lname, _Birthdate.ToShortDateString());
        }
        public void startFit(string[] UserData, int[] UserImportance)
        {
            fit = new Algorithm(UserData, UserImportance);
            results = fit.FindPutter();
            fit.setCharacteristic();
        }
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
    }
}
//add function that gets credit card number, may store in seperate file