using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PutterFitting
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
        public string[] viewPutter()
        {
            string[] matching = putterSave.accessData(('\u00BB').ToString());
            for(int a = 0; a<matching.Length; a++)
            {
                matching[a] = matching[a].Replace('\u00BB', '|');
            }
            return matching;
        }
        public string[] viewPutter(string[] data)
        {
            List<string> combined = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                string[] matching;
                if (data[i] == "")
                {
                    matching = viewPutter();
                }
                else
                {
                    if (data[i][data[i].Length - 1] == '\r')
                        data[i] = data[i].Remove(data[i].Length - 1);
                    matching = putterSave.accessData(data[i]);
                }
                for (int a = 0; a < matching.Length; a++)
                {
                        matching[a] = matching[a].Replace('\u00BB', '|');
                        combined.Add(matching[a]);
                }
                if (matching.Length == 1 && !matching[0].Contains(data[i]))
                    combined[i] = " ";
            }
            
            return combined.ToArray();
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