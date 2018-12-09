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

        /// <summary>
        /// Checks to make sure user is not a guest, then removes the user
        /// virtual to allow for subclasses to be override
        /// </summary>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual bool ChangePassword(string username, string firstName, string lastName, string newPassword = null)
        {
            if (username == "Guest")
                return false;
            if (firstName == _Fname && lastName == _Lname)
            {
                UserSave.remove(username);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Second version used to reset password, this is static because the user is not logged in
        /// There for there is not an instance for them
        /// </summary>
        /// <param name="username"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="newPassword"></param>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        internal static bool ChangePassword(string username, string firstname, string lastname, string newPassword, DateTime birthdate)
        {
            SaveData UserSave = new SaveData("users.txt"); //need here because instance of user does not exist
            if (UserSave.verify(username) && UserSave.verify(firstname, lastname, birthdate.ToShortDateString()))
            {
                string handicap = UserSave.accessData('\u00AA' + username)[0].Split('\u00BB')[5];
                UserSave.remove(username);
                if(handicap!="")
                    return UserSave.save('\u00AA' + username, newPassword, firstname, lastname, birthdate.ToShortDateString(), handicap);
                return UserSave.save('\u00AA' + username, newPassword, firstname, lastname, birthdate.ToShortDateString());
            }
            else
            {
                MessageBox.Show("Could not verify credentials");
                return false;
            }
        }
        ~Users()
        {
            //Active = false;
        }
    }
}