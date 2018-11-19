using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting //removed save data //add remove person? added start fit //changed name to consumer//added changepassword override
{

    public class Consumer : Users
	{
        public Consumer(string username, string password, string birtdate, string Fname, string Lname):base(Fname, Lname)
        {
            _username = username;
            _password = password;
            _Birthdate = Convert.ToDateTime(birtdate);
        }
        public Consumer(string username, string password, string birtdate, string CrediCardNumber, string cvv2, DateTime expirationDate, string Fname, string Lname) : base(Fname, Lname)
        {
             _username = username;
             _password = password;
             _Birthdate = Convert.ToDateTime(birtdate);
             UserCard = new CreditCard(CrediCardNumber, cvv2, expirationDate);
        }
        string _username; //not in superclass, because admin has set username, and password
		string _password;
		string _Handicap;
        public Algorithm fit;
        public CreditCard UserCard;
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
        public static bool Login(string username, string password) //static so that a instance is not necessary to check if user login exists
        {
            SaveData save = new SaveData("users.txt");//called without instance of user
            return save.verify(username, password);
        }
		public bool addNewPerson(string handicap = null)//not static, allow for a new instance ot be created to save
		{
            if (handicap != null)
            {
                _Handicap = handicap;
                return UserSave.save(username, password, _Fname, _Lname, _Birthdate.ToShortDateString(), handicap);
            }
            else
                return UserSave.save(username, password, _Fname, _Lname, _Birthdate.ToShortDateString());
        }
        public string[] startFit(string[] UserData, int[] UserImportance)
        {
            fit = new Algorithm(UserData, UserImportance);
            string[] results = fit.FindPutter();
            //MessageBox.Show(fit.putter.PutterLength);
            return results;//also should only keep name, and add lenght and grip
        }
        public override bool ChangePassword(string firstName, string lastName, string newPassword, string username)
        {
            if (base.ChangePassword(username, firstName, lastName))
            {
                password = newPassword;
                addNewPerson();
                return true;
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