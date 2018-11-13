using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace PutterFitting       //changed relativeImportance to public static, changed find arr val to private, added const int heap size
{
    /*public struct node
    {
        public int importance;
        public string putterTrait;
    }; */  //already include in namespace
    public class Algorithm
	    {
            public Algorithm(string[] data, int[] userImportance)
            {
                _data = data;
                _userImportance = userImportance;

            }
    		string[] _data;
    		int[] _userImportance;
            public static int[] relativeImportance = {3, 2, 1, 5, 3, 0, 4, 0, 1};
            public const int HEAP_SIZE = 7;
            public int last = -1;
            node[] heap = new node[HEAP_SIZE];
            PutterData putter;

            private void FindArrVal()
    		{
                for (int a = 0; a < 9; a++)
                {
                    relativeImportance[a] = relativeImportance[a] * _userImportance[a];
                }
            }

            private void reheapUp(node[] heap, int child)
            {
                if (child != 0)
                {
                    int parent = (child - 1) / 2;
                    if (heap[parent].importance < heap[child].importance)
                    {
                        node temp = heap[parent];
                        heap[parent] = heap[child];
                        heap[child] = temp;
                        reheapUp(heap, parent);
                }
            }
            }
            private void insertHeap(node[] heap, ref int last, node data)
            {
                if (last != HEAP_SIZE - 1)
                {
                    last++;
                    heap[last] = data;
                    reheapUp(heap, last);
                }
            }
            public string[] FindPutter()//node[] original idea to return heap, not necessary by calling the putterdata class
            {
                FindArrVal();
                node temp;
                //Alignment - 2
                if(_data[4] == "Struggles with Alignment")
                {
                    temp.importance = relativeImportance[4];
                    temp.putterTrait = "Wide Putter Head";
                    insertHeap(heap, ref last, temp);
                }
                else
                {
                    temp.importance = relativeImportance[4];
                    temp.putterTrait = "Normal Putter Head";
                    insertHeap(heap, ref last, temp);
                }

                //Toe Hang vs Face Balanced Checks - 4
                if ((_data[0] == "Left" && _data[3] == "Arcing Path" )|| ( _data[0] == "Not Applicable" && _data[3] == "Arcing Path"))
                {
                    if (relativeImportance[0] > relativeImportance[3])
                        temp.importance = relativeImportance[0];
                    else
                        temp.importance = relativeImportance[3];
                    temp.putterTrait = "Toe Weighted";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[0] == "Right" && _data[3] == "Arcing Path")
                {
                    if (relativeImportance[0] > relativeImportance[3])
                    {
                        temp.importance = relativeImportance[0];
                        temp.putterTrait = "Face Balanced";
                        insertHeap(heap, ref last, temp);
                    }
                    else
                    {
                        temp.importance = relativeImportance[3];
                        temp.putterTrait = "Toe Weighted";
                        insertHeap(heap, ref last, temp);
                    }

                }
                else if ((_data[0] == "Right" && _data[3] == "Straight Back Straight Through")|| (_data[0] == "Not Applicable" && _data[3] == "Straight Back Straight Through"))
                {
                    if (relativeImportance[0] > relativeImportance[3])
                        temp.importance = relativeImportance[0];
                    else
                        temp.importance = relativeImportance[3];
                    temp.putterTrait = "Face Balanced";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[0] == "Left" && _data[3] == "Straight Back Straight Through")
                {
                    if (relativeImportance[0] > relativeImportance[3])
                    {
                        temp.importance = relativeImportance[0];
                        temp.putterTrait = "Toe Weighted";
                        insertHeap(heap, ref last, temp);
                    }
                    else
                    {
                        temp.importance = relativeImportance[3];
                        temp.putterTrait = "Face Balanced";
                        insertHeap(heap, ref last, temp);
                    }
                }

                //Offset - 2
                if(_data[2] == "Right Handed, Right Eye" || _data[2] == "Left Handed, Left Eye")
                {
                    temp.importance = relativeImportance[2];
                    temp.putterTrait = "Offset Shaft";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[2] == "Right Handed, Left Eye" || _data[2] == "Left Handed, Right Eye")
                {
                    temp.importance = relativeImportance[2];
                    temp.putterTrait = "Straight Shaft";
                    insertHeap(heap, ref last, temp);
                }

                //Length - 4
                if(_data[5] == "Greather than 6ft 6in")
                {
                    temp.importance = 27;
                    temp.putterTrait = "36in";
                    insertHeap(heap, ref last, temp);
                }
                else if(_data[5] == "Greater than 6ft")
                {
                    temp.importance = 27;
                    temp.putterTrait = "35in";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[5] == "Less than 6ft")
                {
                    temp.importance = 27;
                    temp.putterTrait = "34in";
                    insertHeap(heap, ref last, temp);
                }
                else if(_data[5] == "Less than 5ft 5in")
                {
                    temp.importance = 27;
                    temp.putterTrait = "33in";
                    insertHeap(heap, ref last, temp);
                }

                //Weight -3
                if(_data[1] == "Long")
                {
                    temp.importance = relativeImportance[1];
                    temp.putterTrait = "Lighter Weight";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[1] == "Short")
                {
                    temp.importance = relativeImportance[1];
                    temp.putterTrait = "Heavier Weight";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[1] == "Not Applicable")
                {
                    temp.importance = relativeImportance[1];
                    temp.putterTrait = "Standard Weight";
                    insertHeap(heap, ref last, temp);
                }

                //Grip -3X3
                if(_data[6] == "Wrist bend" && _data[7] == "Standard Size Grip")
                {
                    if (relativeImportance[6] > relativeImportance[7])
                    {
                        temp.importance = 26;
                        temp.putterTrait = "Larger Grip";
                    }
                    else{
                        temp.importance = 26;
                        temp.putterTrait = "Standard Grip";
                    }
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "Wrist bend" && _data[7] == "Larger Grip")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Larger Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "Wrist bend" && _data[7] == "Not Applicable")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Larger Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "No Wrist Bend" && _data[7] == "Standard Size Grip")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Standard Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "No Wrist Bend" && _data[7] == "Larger Grip")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Larger Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "No Wrist Bend" && _data[7] == "Not Applicable")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Standard Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "Not Applicable" && _data[7] == "Standard Size Grip")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Standard Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "Not Applicable" && _data[7] == "Larger Grip")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Larger Grip";
                    insertHeap(heap, ref last, temp);
                }
                else if (_data[6] == "Not Applicable" && _data[7] == "Not Applicable")
                {
                    temp.importance = 26;
                    temp.putterTrait = "Standard Grip";
                    insertHeap(heap, ref last, temp);
                }

                //Feel-2
                if(_data[8] == "Softer Feel")
                {
                    temp.importance = relativeImportance[8];
                    temp.putterTrait = "Softer Feel";
                    insertHeap(heap, ref last, temp);
                }
                else
                {
                    temp.importance = relativeImportance[8];
                    temp.putterTrait = "Harder Feel";
                    insertHeap(heap, ref last, temp);
                }

            putter = new PutterData(heap, last);
            putter.GetPutter();
            return putter.putterFits; //string of fit putters
            //return heap;
            }
        }
}
 /*
Common miss {"Left", "Right", "Not Applicable"}; 0
Common miss {"Long", "Short", "Not Applicable"}; 1
Dominant Eye {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"}; 2
Swing path {"Arcing Path", "Straight Back Straight Through"}; 3
Alignment {"Struggles with Alignment", "Alignment is Okay"}; 4
Height {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"}; 5
head movement {"Wrist bend", "No Wrist Bend", "Not Applicable"}; 6
Grip perefrence {"Standard Size Grip", "Larger Grip", "Not Applicable"}; 7
Feel {"Softer Feel", "Harder Feel", "Not Applicable"}; 8


1. Alignment-alignment 
2. Toe hang-common miss, and swing path
3. Offset-eye
4. Length - height
5. Weight - common miss
6. Grip Style/Size - head movement, prefernce
7. Face Material/Texture - feel
 */