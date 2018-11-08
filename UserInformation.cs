using System;
using System.Collections.Generic;
using System.Text;

namespace Putter_Fitting
{
    public struct node
    {
        public int importance;
        public string putterTrait; //used for heaps, stores the user data in levels of importance
    };

    public class UserInformation
	{
        protected string _Fname;
        protected string _Lname;
		protected DateTime _Birthdate;

		public UserInformation(string Fname, string Lname)
		{
            _Fname = Fname;
            _Lname = Lname;
		}
	}
}