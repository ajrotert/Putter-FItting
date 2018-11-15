using System;
using System.Collections.Generic;
using System.Text;

namespace PutterFitting    // d putterVal, changed putterLink, to  putterFits array
{  //added PutterLength and PutterGrip and putterCharacteristics
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
            deleteHeap(heap, ref l, ref PutterLength); 
            deleteHeap(heap, ref l, ref PutterGrip);

            while (l >= 0)
            {
                putterCharacteristics[a] = "";
                deleteHeap(heap, ref l, ref putterCharacteristics[a]); //*****First two should be len and grip, not sent to putter data
                a++;
            }
        }
        public string PutterLength;
        public string PutterGrip;
		public string[] putterFits;
		public string[] putterCharacteristics = new string[5];
        SaveData putters = new SaveData("putters.txt");


        public void GetPutter()
        {
            string[] data = putters.accessData(putterCharacteristics);
            putterFits = new string[data.Length];
            for(int a =0; a<data.Length; a++)
            {
                string[] temp = data[a].Split('\u00BB');
                putterFits[a] = temp[0];
            }
        }
	}
}