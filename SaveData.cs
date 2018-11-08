using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Putter_Fitting //added verify, also changed fileName to data, removed fileData field, added remove data function
{
	public class SaveData
	{
        public SaveData(string fileName)
        {
        this.fileName = fileName;
        }
		string fileName;
		//string fileData;

		public bool save(params string[] data)                  //returns true if data was sucessfully saved
        {
            if (fileExist(fileName))
            {
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    for (int a = 0; a < data.Length; a++)
                        sw.Write("" + data[a] + " ");
                    sw.WriteLine();
                    return true;
                }
            }
            return false;
        }
        public bool remove(params string[] data)            //returns bool value if successful, removes line of code.
        {
            string[] theWholeFile = (File.ReadAllLines(fileName));
            for (int a = 0; a < theWholeFile.Length; a++)
            {
                if (theWholeFile[a].Contains(data[0]))
                {
                    theWholeFile[a] = "";
                    File.WriteAllLines(fileName, theWholeFile);
                    return true;
                }
            }
            return false;
        }
        public bool verify(string username, string password) //returns bool value for matching username, password
        {
            string[] check = accessData(username, password);
            if (check[0].Contains(username + " " + password))
                return true;
            return false;
        }
       
        public string[] accessData(params string[] data) //returns a string of matching information
		{
            // Open the file to read from.
            List<string> dataList = new List<string> ();
            using (StreamReader sr = File.OpenText(fileName))
            {
                 string fileData = "";
                 while ((fileData = sr.ReadLine()) != null)
                 {
                    if(fileData.Contains(data[0]))
                        dataList.Add(fileData);
                 }
            }

            for (int a = 1; a < data.Length; a++)
            {
                for (int b = 0; b < dataList.Count && dataList.Count>1; b++)//not perfect, if there are not perfect matches then it limits it to last option presented, future versions could show all the non perfect matches
                {                                                           //change the deleting to track the ones that need deleting then if greater than 1 delete
                    if (!dataList[b].Contains(data[a]))
                    {
                        dataList.RemoveAt(b);
                        b--;
                    }
                }
            }

            return dataList.ToArray();
        }

		public static bool fileExist(string fileName) //static may be necessary in management to check data, true or false if file exists
		{
            return File.Exists(fileName);
   		}
	}
}
