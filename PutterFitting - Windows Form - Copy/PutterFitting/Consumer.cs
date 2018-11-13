using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting //removed save data //add remove person? added start fit //changed name to consumer
{

    public class Consumer : Users
	{
        public Consumer(string username, string password, string Handicap, string Fname, string Lname):base(Fname, Lname)
        {
            _username = username;
            _password = password;
            _Handicap = Handicap;
            //CreditCard UserCard = new CreditCard(Fname, Lname);
        }
        public Consumer(string username, string password, string Handicap, string CrediCardNumber, string cvv2, string expirationDate, string Fname, string Lname) : base(Fname, Lname)
        {
             _username = username;
             _password = password;
             _Handicap = Handicap;
            CreditCard UserCard = new CreditCard(CrediCardNumber, cvv2, expirationDate, Fname, Lname); //try public static or public
        }
        string _username;
		string _password;
		string _Handicap;
        SaveData save = new SaveData("users.txt");
        Algorithm fit;
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
            SaveData save = new SaveData("users.txt");
            return save.verify(username, password);
        }
		public bool addNewPerson()//not static, allow for a new instance ot be created to save
		{
            return save.save(username, password, _Handicap, _Fname, _Lname);
        }
        public string[] startFit(string[] UserData, int[] UserImportance)
        {
            fit = new Algorithm(UserData, UserImportance);
            string[] results = fit.FindPutter();
            return results;//also should only keep name, and add lenght and grip
        }

    }
}
//add function that gets credit card number, may store in seperate file