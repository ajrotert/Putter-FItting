using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PutterFitting //admin inherts, cc does not
{
    public class Admin : Users
    {
        public Admin(string Fname, string Lname) : base(Fname, Lname)
        {
        }
        public string managePutter;
        SaveData save = new SaveData("putters.txt"); //do this for all classes
        public bool putterExist()
        {
            return save.verify(managePutter);
        }
        public void AddNewPutter(params string[] putterData)
        {
            List<string> complete = new List<string>();
            complete.Add(managePutter);
            for (int a = 0; a < putterData.Length; a++)
                complete.Add(putterData[a]);
            save.save(complete.ToArray());
        }

        public void RemovePutter()
        {
            bool success;
            success = save.remove(managePutter);
            if (success == false)
                MessageBox.Show(managePutter + " not found in file");
        }
    }
}
