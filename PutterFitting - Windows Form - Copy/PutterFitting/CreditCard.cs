using System;
using System.Collections.Generic;
using System.Text;

namespace PutterFitting
{
	public class CreditCard //changed everything to strings
	{
        public CreditCard(string Fname, string Lname)
        {
        _cvv2 = "0";
        _CreditCardNumber = null;
            _expirationDate = DateTime.Now.ToString();
        }
        public CreditCard(string CrediCardNumber, string cvv2, string expirationDate, string Fname, string Lname)
        {
        _CreditCardNumber = CrediCardNumber;
        _cvv2 = cvv2;
        _expirationDate = expirationDate;
        }
		string _CreditCardNumber;
		string _cvv2;
		string _expirationDate;

		public bool MakePayment(int total)
		{
            return true;
		}
	}
}
