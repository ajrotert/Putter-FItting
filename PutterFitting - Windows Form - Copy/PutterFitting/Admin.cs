using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PutterFitting
{
    public class Admin : Users, Iputter //implements traits for actual putter to be added
    {
        public Admin(string Fname, string Lname) : base(Fname, Lname)
        {
            _Fname = Fname;
            _Lname = Lname;
            Active = true;
        }
        public string manage =null;
        public string fileName;
        new public static bool Active = false;
        SaveData dataSave; 

        public void setFile(string fileName)
        {
            this.fileName = fileName;
            dataSave = new SaveData(fileName);
        }

        //Iputter Interface implementation
        public string putterShape { get; set; }
        public string putterBalance { get; set; }
        public string putterHosel { get; set; }
        public string putterWeight { get; set; }
        public string putterFeel { get; set; }
        public string putterLink { get; set; }
        public void setCharacteristic(params string[] data)
        {
            if (putterExist())
            {
                List<string> dataChar = new List<string>();
                dataChar = dataSave.accessData(manage)[0].Split('\u00BB').ToList();
                dataChar.RemoveAt(0);
                data = dataChar.ToArray();
            }
            putterShape = data[0];
            putterBalance = data[1];
            putterHosel = data[2];
            putterWeight = data[3];
            putterFeel = data[4];
            if (data.Length >= 6)
                putterLink = data[5];
            else
                putterLink = "None";
            if (!putterExist())
            {
                if (putterLink != "None")
                    AddNewPutter(putterShape, putterBalance, putterHosel, putterWeight, putterFeel, putterLink);
                else
                    AddNewPutter(putterShape, putterBalance, putterHosel, putterWeight, putterFeel);
            }
        }

        public bool putterExist()
        {
            bool exist = dataSave.verify(manage);
            if(manage != null && manage != "")
                return exist;
            return false;
        }
        public void AddNewPutter(params string[] putterData)
        {
            List<string> complete = new List<string>();
            complete.Add(manage);
            for (int a = 0; a < putterData.Length; a++)
                complete.Add(putterData[a]);
            dataSave.save(complete.ToArray());
        }

        public void RemovePutter()
        {
            bool success;
            success = dataSave.remove(manage);
            if (success == false)
                MessageBox.Show(manage + " not found in file");
        }
        public string[] viewData()
        {
            string[] matching = dataSave.accessData(('\u00BB').ToString());
            for(int a = 0; a<matching.Length; a++)
            {
                matching[a] = matching[a].Replace('\u00BB', '|');
            }
            return matching;
        }
        public string[] viewData(string[] data)
        {
            List<string> combined = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                string[] matching;
                if (data[i] == "")
                {
                    matching = viewData();
                }
                else
                {
                    if (data[i][data[i].Length - 1] == '\r')
                        data[i] = data[i].Remove(data[i].Length - 1);
                    matching = dataSave.accessData(data[i]);
                }
                for (int a = 0; a < matching.Length; a++)
                {
                        matching[a] = matching[a].Replace('\u00BB', '|');
                        combined.Add(matching[a]);
                }
                if (matching.Length == 1 && !matching[0].Contains(data[i]))//could use the verify function, but that will not be as efficient
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
        ~Admin()
        {
            Active = false;
        }
    }
}