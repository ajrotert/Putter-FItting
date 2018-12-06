using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting
{

    public class Users
	{
        protected internal string _Fname;
        protected internal string _Lname;
		protected DateTime _Birthdate;
        public DateTime birthdate
        {
            get { return _Birthdate; }
            private set { _Birthdate = value; }
        }
        public static bool Active = false;
        public SaveData UserSave = new SaveData("users.txt");

		public Users(string Fname, string Lname)
		{
            _Fname = Fname;
            _Lname = Lname;
            Active = true;
		}
        public virtual bool ChangePassword(string username, string firstName, string lastName, string newPassword = null)
        {
            if (_Fname == "Guest")
                return false;
            if (firstName == _Fname && lastName == _Lname)
            {
                UserSave.remove(username);
                return true;
            }
            return false;
        }
        internal static bool ChangePassword(string username, string firstname, string lastname, string newPassword, DateTime birthdate)
        {
            SaveData UserSave = new SaveData("users.txt"); //need here because instance of user does not exist
            if (UserSave.verify(username) && UserSave.verify(firstname, lastname, birthdate.ToShortDateString()))
            {
                UserSave.remove(username);
                UserSave.save(username, newPassword, firstname, lastname, birthdate.ToShortDateString());
                return true;
            }
            else
            {
                MessageBox.Show("Could not verify credentials");
                return false;
            }
        }
        ~Users()
        {
            Active = false;
        }
    }
}