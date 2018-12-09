using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace PutterFitting
{
    public class Algorithm : Iputter //implements traits for ideal putter
    {
            public Algorithm(string[] data, int[] userImportance)
            {
                _data = data;
                _userImportance = userImportance;
            }

    		private string[] _data;
    		private int[] _userImportance;
            public static int[] importanceLevel = { 3, 2, 1, 5, 3, 0, 4, 0, 1 };
            private int[] relativeImportance = new int[importanceLevel.Length];
            public const int HEAP_SIZE = 7;
            private int last = -1;
            private node[] heap = new node[HEAP_SIZE];
            public PutterData putter;

            private node temp; //used for the length and grip
            private node _putterShape;
            private node _putterBalance;
            private node _putterHosel;
            private node _putterWeight;
            private node _putterFeel;

            //Iputter Interface implementation
            public string putterShape { get; set; }
            public string putterBalance { get; set; }
            public string putterHosel { get; set; }
            public string putterWeight { get; set; }
            public string putterFeel { get; set; }
            public string putterLink { get; set; } //implementation not needed for finding matching putter
            public void setCharacteristic(params string[] data)
            {
                putterShape = _putterShape.putterTrait;
                putterBalance = _putterBalance.putterTrait;
                putterHosel = _putterHosel.putterTrait;
                putterWeight = _putterWeight.putterTrait;
                putterFeel = _putterFeel.putterTrait;
                putterLink = null;
            }
            
            /// <summary>
            /// finds the relative importance levels to be compared, multiplies user values, against my ratings
            /// </summary>
            private void FindArrVal()
    		{
                for (int a = 0; a < 9; a++)
                {
                    relativeImportance[a] = importanceLevel[a] * _userImportance[a];
                }
            }

            /// <summary>
            /// Called after a putter is found, calculates the level of "matching" based on importance rankings
            /// </summary>
            /// <returns></returns>
            public double Matching()
            {
                double total = (_putterShape.importance + _putterBalance.importance + _putterHosel.importance + _putterWeight.importance + _putterFeel.importance);
                double matchingC = 0;
                if (putter.putterShape == putterShape)
                {
                    matchingC+= _putterShape.importance;
                }
                if (putter.putterBalance == putterBalance)
                {
                    matchingC += _putterBalance.importance;
                }
                if (putter.putterHosel == putterHosel)
                {
                    matchingC+= _putterHosel.importance;
                }
                if (putter.putterWeight == putterWeight)
                {
                    matchingC+= _putterWeight.importance;
                }
                if (putter.putterFeel == putterFeel)
                {
                    matchingC+= _putterFeel.importance;
                }

                return (matchingC / total) *100;
            }
            
            /// <summary>
            /// makes sure everything is in order
            /// </summary>
            /// <param name="heap"></param>
            /// <param name="child"></param>
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
            /// <summary>
            /// Addeds stuff to heap
            /// </summary>
            /// <param name="heap"></param>
            /// <param name="last"></param>
            /// <param name="data"></param>
            private void insertHeap(node[] heap, ref int last, node data)
            {
                if (last != HEAP_SIZE - 1)
                {
                    last++;
                    heap[last] = data;
                    reheapUp(heap, last);
                }
            }
            
            /// <summary>
            /// Checks each putterTrait, and finds matching characteristics
            /// Then inserts into heap
            /// </summary>
            /// <returns></returns>
            public string[] FindPutter()
            {
                FindArrVal();
                //Alignment - 2
                if(_data[4] == "Struggles with Alignment")
                {
                    _putterShape.importance = relativeImportance[4];
                    _putterShape.putterTrait = "Wide Putter Head";
                    insertHeap(heap, ref last, _putterShape);
                }
                else
                {
                    _putterShape.importance = relativeImportance[4];
                    _putterShape.putterTrait = "Normal Putter Head";
                    insertHeap(heap, ref last, _putterShape);
                }

                //Toe Hang vs Face Balanced Checks - 4
                if ((_data[0] == "Left" && _data[3] == "Arcing Path" )|| ( _data[0] == "Not Applicable" && _data[3] == "Arcing Path"))
                {
                    if (relativeImportance[0] > relativeImportance[3])
                        _putterBalance.importance = relativeImportance[0];
                    else
                        _putterBalance.importance = relativeImportance[3];
                    _putterBalance.putterTrait = "Toe Weighted";
                    insertHeap(heap, ref last, _putterBalance);
                }
                else if (_data[0] == "Right" && _data[3] == "Arcing Path")
                {
                    if (relativeImportance[0] > relativeImportance[3])
                    {
                        _putterBalance.importance = relativeImportance[0];
                        _putterBalance.putterTrait = "Face Balanced";
                        insertHeap(heap, ref last, _putterBalance);
                    }
                    else
                    {
                        _putterBalance.importance = relativeImportance[3];
                        _putterBalance.putterTrait = "Toe Weighted";
                        insertHeap(heap, ref last, _putterBalance);
                    }

                }
                else if ((_data[0] == "Right" && _data[3] == "Straight Back Straight Through")|| (_data[0] == "Not Applicable" && _data[3] == "Straight Back Straight Through"))
                {
                    if (relativeImportance[0] > relativeImportance[3])
                        _putterBalance.importance = relativeImportance[0];
                    else
                        _putterBalance.importance = relativeImportance[3];
                    _putterBalance.putterTrait = "Face Balanced";
                    insertHeap(heap, ref last, _putterBalance);
                }
                else if (_data[0] == "Left" && _data[3] == "Straight Back Straight Through")
                {
                    if (relativeImportance[0] > relativeImportance[3])
                    {
                        _putterBalance.importance = relativeImportance[0];
                        _putterBalance.putterTrait = "Toe Weighted";
                        insertHeap(heap, ref last, _putterBalance);
                    }
                    else
                    {
                        _putterBalance.importance = relativeImportance[3];
                        _putterBalance.putterTrait = "Face Balanced";
                        insertHeap(heap, ref last, _putterBalance);
                    }
                }

                //Offset - 2
                if(_data[2] == "Right Handed, Right Eye" || _data[2] == "Left Handed, Left Eye")
                {
                    _putterHosel.importance = relativeImportance[2];
                    _putterHosel.putterTrait = "Offset Shaft";
                    insertHeap(heap, ref last, _putterHosel);
                }
                else if (_data[2] == "Right Handed, Left Eye" || _data[2] == "Left Handed, Right Eye")
                {
                    _putterHosel.importance = relativeImportance[2];
                    _putterHosel.putterTrait = "Straight Shaft";
                    insertHeap(heap, ref last, _putterHosel);
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
                    _putterWeight.importance = relativeImportance[1];
                    _putterWeight.putterTrait = "Lighter Weight";
                    insertHeap(heap, ref last, _putterWeight);
                }
                else if (_data[1] == "Short")
                {
                    _putterWeight.importance = relativeImportance[1];
                    _putterWeight.putterTrait = "Heavier Weight";
                    insertHeap(heap, ref last, _putterWeight);
                }
                else if (_data[1] == "Not Applicable")
                {
                    _putterWeight.importance = relativeImportance[1];
                    _putterWeight.putterTrait = "Standard Weight";
                    insertHeap(heap, ref last, _putterWeight);
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
                    _putterFeel.importance = relativeImportance[8];
                    _putterFeel.putterTrait = "Softer Feel";
                    insertHeap(heap, ref last, _putterFeel);
                }
                else
                {
                    _putterFeel.importance = relativeImportance[8];
                    _putterFeel.putterTrait = "Harder Feel";
                    insertHeap(heap, ref last, _putterFeel);
                }

            putter = new PutterData(heap, last);
            putter.GetPutter();
            return putter.putterFits; //string of fit putters
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