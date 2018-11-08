using System;
using System.Collections.Generic;
using System.Text;

namespace Putter_Fitting
{
	public class CreditCard : UserInformation
	{
        public CreditCard(string Fname, string Lname):base(Fname, Lname)
        {
        _cvv2 = 0;
        _CreditCardNumber = null;
            _expirationDate = DateTime.Now;
        }
        public CreditCard(string CrediCardNumber, int cvv2, DateTime expirationDate, string Fname, string Lname):base(Fname, Lname)
        {
        _CreditCardNumber = CrediCardNumber;
        _cvv2 = cvv2;
        _expirationDate = expirationDate;
        }
		string _CreditCardNumber;
		int _cvv2;
		DateTime _expirationDate;

		public bool MakePayment(int total)
		{
            return true;
		}
	}
}
