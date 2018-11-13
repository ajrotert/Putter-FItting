using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PutterFitting //added verify, also changed fileName to data, removed fileData field, added remove data function
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
                        sw.Write("" + data[a] + '\u00BB');
                    sw.WriteLine();
                    return true;
                }
            }//                        sw.Write("" + data[a] +  " " + '\u00BB' + " ");

            return false;
        }
        public bool remove(params string[] data)            //returns bool value if successful, removes line of code.
        {
            if (fileExist(fileName))
            {
                string[] theWholeFile = (File.ReadAllLines(fileName));
                List<string> allData = new List<string>();
                for (int a = 0; a < theWholeFile.Length; a++)
                {
                    if (!theWholeFile[a].Contains(data[0]))
                        allData.Add(theWholeFile[a]);
                }
                File.WriteAllLines(fileName, allData.ToArray());
                return true;
            }
            return false;
        }
        public bool verify(params string[] data) //returns bool value for matching username, password
        {
            if (fileExist(fileName))
            {
                string[] check = accessData(data);
                string compares = "";
                for (int a = 0; a < data.Length; a++)
                {
                    compares += data[a] + '\u00BB';
                }
                if (check.Length > 0 && check[0].Contains(compares))
                    return true;
            }
            return false;
        }

        public string[] accessData(params string[] data) //returns a string of matching information
        {            // Open the file to read from.
            if (fileExist(fileName))
            {
                List<string> dataList = new List<string>();
                string fileData = "";
                using (StreamReader sr = File.OpenText(fileName))
                {
                    while ((fileData = sr.ReadLine()) != null)//saves all file data that contains the first parameter
                    {
                        if (fileData.Contains(data[0]))
                        {
                            dataList.Add(fileData);
                        }
                    }
                }

                if (dataList.Count == 0) //insures that there will always be one item returned
                {
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        fileData = sr.ReadLine();
                        if (fileData != null)
                            dataList.Add(fileData);
                        else
                            dataList.Add("empty list");
                    }
                }

                for (int a = 1; a < data.Length; a++) //the number of parameters sent in
                {
                    for (int b = 0;( b < dataList.Count && dataList.Count > 1); b++)//not perfect, if there are not perfect matches then it limits it to last option presented, future versions could show all the non perfect matches
                    {                                                           //change the deleting to track the ones that need deleting then if greater than 1 delete
                        if (!dataList[b].Contains(data[a]))
                        {
                            dataList.RemoveAt(b);
                            b--;
                        }
                    }
                }
                string[] array = dataList.ToArray();
                return array;
            }
            return null;
        }

		public static bool fileExist(string fileName) //static may be necessary in management to check data, true or false if file exists
		{
            return File.Exists(fileName);
   		}
	}
}
