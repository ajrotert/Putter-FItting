using System;
using System.Collections.Generic;
using System.Text;

namespace Putter_Fitting    //changed methods to static // removed putterVal, changed putterLink, to  putterFits array
{
   /*public struct node
    {
        public int importance;
        public string putterTrait;
    }; */  //already include in namespace

    public class PutterData
	{

        private void reheapDown(node[] heap, int loc, int last)
        {//check for children, if one is larger then swap
            int leftKey, rightKey, largestIndex;
            node hold;
            if (loc * 2 + 1 <= last)
            {
                leftKey = heap[loc * 2 + 1].importance;
                if (loc * 2 + 2 <= last)
                    rightKey = heap[loc * 2 + 2].importance;
                else
                    rightKey = leftKey - 1; // make so rightKey is always smaller than leftKey
                if (rightKey > leftKey)
                    largestIndex = 2 * loc + 2;
                else
                    largestIndex = 2 * loc + 1;
                if (heap[loc].importance < heap[largestIndex].importance)
                {
                    hold = heap[loc];
                    heap[loc] = heap[largestIndex];
                    heap[largestIndex] = hold;
                    reheapDown(heap, largestIndex, last);
                }
            }
        }
        private void deleteHeap(node[] heap, ref int last, ref string dataOut)
        {
            if (last >= 0)
            {
                dataOut = heap[0].putterTrait;
                heap[0] = heap[last];
                last--;
                reheapDown(heap, 0, last);
            }
        }
        public PutterData(node[] heap, int last)
        {
            int a = 0, l = last;
            while (l >= 0)
            {
                deleteHeap(heap, ref l, ref putterCharacteristics[a]); //*****First two should be len and grip, not sent to putter data
                a++;
            }
        }
		public string[] putterFits;
		public string[] putterCharacteristics;

		public static void AddNewPutter(params string[] putterData)
		{
            SaveData save = new SaveData("putters.txt");
            /*
             * lenghts and grips are not included as they can be changed easy
                putter name: 
                "Larger Putter Head" "Normal Putter Head"
                "Toe Weighted" "Face Balanced"
                "Offset Shaft" "Straight Shaft"
                "Lighter Weight" "Heavier Weight" "Standard Weight"
                "Softer Feel" "Harder Feel"
             */
            save.save(putterData);
		}

		public static void RemovePutter(string putter)
		{
            bool success;
            SaveData remove = new SaveData("putters.txt");
            success = remove.remove(putter);
            if (success == false)
                Console.WriteLine("" + putter + " not found in file");
		}
        public void FindPutter()
        {
            //need to send a string of data, and get array of putters back
            SaveData putters = new SaveData("putters.txt");
            putterFits = putters.accessData(putterCharacteristics);
        }
	}
}