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
            if (!putterExist()) //automatically adds putter if the name is not found
            {
                if (putterLink != "None")
                    AddNewPutter(putterShape, putterBalance, putterHosel, putterWeight, putterFeel, putterLink);
                else
                    AddNewPutter(putterShape, putterBalance, putterHosel, putterWeight, putterFeel);
            }
        }

        /// <summary>
        /// if the putter name exists in file, then it returns true
        /// </summary>
        /// <returns></returns>
        public bool putterExist()
        {
            bool exist = dataSave.verify(manage);
            if(manage != null && manage != "")
                return exist;
            return false;
        }

        /// <summary>
        /// Uses a list to add the putter name to the beginning, then saves putter
        /// </summary>
        /// <param name="putterData"></param>
        public void AddNewPutter(params string[] putterData)
        {
            List<string> complete = new List<string>();
            complete.Add(manage);
            for (int a = 0; a < putterData.Length; a++)
                complete.Add(putterData[a]);
            dataSave.save(complete.ToArray());
        }

        /// <summary>
        /// removes the data stored in manage, usally putter name, or username 
        /// </summary>
        public void Remove()
        {
            bool success;
            success = dataSave.remove(manage);
            if (success == false)
                MessageBox.Show(manage + " not found in file");
        }

        /// <summary>
        /// Shows all data in the selected file
        /// </summary>
        /// <returns></returns>
        public string[] viewData()
        {
            string[] matching = dataSave.accessData(('\u00BB').ToString());
            for(int a = 0; a<matching.Length; a++)
            {
                matching[a] = matching[a].Replace('\u00BB', '|');
            }
            return matching;
        }

        /// <summary>
        /// Shows only matching data in selected file
        /// Also checks for a '+' combines for multiple paramaters
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string[] viewData(string[] data)
        {
            List<string> combined = new List<string>();
            List<string> tempData = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                string[] matching;

                if (data[i].Contains('\r'))
                {
                    if (data[i].Length == 1)
                        data[i] = " ";
                    else
                        data[i] = data[i].Remove('\r');
                }

                if (data[i] == "")
                {
                    matching = viewData();
                }
                else
                {
                    //if (data[i][data[i].Length - 1] == '\r')
                      //  data[i] = data[i].Remove(data[i].Length - 1);

                    if (data[i].Contains(" + "))
                        tempData = data[i].Split('+', ' ').ToList();
                    else
                        tempData = data[i].Split('+').ToList();

                    for (int a = 0; a < tempData.Count; a++)
                        if (tempData[a] == "" || tempData[a] == null || tempData[a] == '\r'.ToString())
                            tempData.RemoveAt(a);
                    matching = dataSave.accessData(tempData.ToArray());
                }
                for (int a = 0; a < matching.Length; a++)
                {
                        matching[a] = matching[a].Replace('\u00BB', '|');
                        combined.Add(matching[a]);
                }
                if (matching.Length == 1 && (!matching[0].Contains(data[i]) && !matching[0].Contains(tempData[0])))//could use the verify function, but that will not be as efficient
                    combined[i] = " ";
            }
            
            return combined.ToArray();
        }

        /// <summary>
        /// Calles base function, if a user is removed, then a new user is added to file
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="newPassword"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool ChangePassword(string firstName, string lastName, string newPassword, string username = "admin")
        {
            if(base.ChangePassword('\u00AA' + username, firstName, lastName))
            {
                UserSave.save('\u00AA' + username, newPassword, _Fname, _Lname);
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
            //Active = false;
        }
    }
}