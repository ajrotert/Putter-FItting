using System;
using System.Collections.Generic;
using System.Text;

namespace Putter_Fitting //removed save data //add remove person?
{

    public class User : UserInformation
	{
        public User(string username, string password, string Handicap, string Fname, string Lname):base(Fname, Lname)
        {
            _username = username;
            _password = password;
            _Handicap = Handicap;
            CreditCard UserCard = new CreditCard(Fname, Lname);
        }
        public User(string username, string password, string Handicap, string CrediCardNumber, int cvv2, DateTime expirationDate, string Fname, string Lname) : base(Fname, Lname)
        {
             _username = username;
             _password = password;
             _Handicap = Handicap;
            CreditCard UserCard = new CreditCard(CrediCardNumber, cvv2, expirationDate, Fname, Lname); //try public static or public
        }
        string _username;
		string _password;
		string _Handicap;
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
            SaveData saved = new SaveData("users.txt");
            return saved.verify(username, password);
        }

		public void ChangePassword(string username)
		{
			throw new NotImplementedException();
		}

		public void addNewPerson()//not static, allow for a new instance ot be created to save
		{
            SaveData save = new SaveData("users.txt");
            save.save(username, password, _Handicap);
		}

	}
}
