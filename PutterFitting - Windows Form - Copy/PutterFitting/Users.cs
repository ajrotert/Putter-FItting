using System;
using System.Collections.Generic;
using System.Text;

namespace PutterFitting
{
    public struct node
    {
        public int importance;
        public string putterTrait; //used for heaps, stores the user data in levels of importance
    };

    public class Users
	{
        protected string _Fname;
        protected string _Lname;
		protected DateTime _Birthdate;

		public Users(string Fname, string Lname)
		{
            _Fname = Fname;
            _Lname = Lname;
		}
        public void ChangePassword(string username)
        {
            throw new NotImplementedException();
        }
    }
}