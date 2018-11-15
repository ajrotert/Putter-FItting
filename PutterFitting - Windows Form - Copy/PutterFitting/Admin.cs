using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PutterFitting //admin inherts, cc does not//added changepassword override added active
{
    public class Admin : Users
    {
        public Admin(string Fname, string Lname) : base(Fname, Lname)
        {
            _Fname = Fname;
            _Lname = Lname;
            Active = true;
        }
        public string managePutter;
        new public static bool Active = false;
        SaveData putterSave = new SaveData("putters.txt");
        public bool putterExist()
        {
            return putterSave.verify(managePutter);
        }
        public void AddNewPutter(params string[] putterData)
        {
            List<string> complete = new List<string>();
            complete.Add(managePutter);
            for (int a = 0; a < putterData.Length; a++)
                complete.Add(putterData[a]);
            putterSave.save(complete.ToArray());
        }

        public void RemovePutter()
        {
            bool success;
            success = putterSave.remove(managePutter);
            if (success == false)
                MessageBox.Show(managePutter + " not found in file");
        }
        public override bool ChangePassword(string firstName, string lastName, string newPassword, string username = "admin")
        {
            if(base.ChangePassword(username, firstName, lastName))
            {
                UserSave.save(username, newPassword, _Fname, _Lname);
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
