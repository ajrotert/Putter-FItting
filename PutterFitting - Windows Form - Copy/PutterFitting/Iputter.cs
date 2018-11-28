using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutterFitting
{
    public struct node
    {
        public int importance;
        public string putterTrait; //used for heaps, stores the user data in levels of importance
    };

    interface Iputter
    {
        string putterShape { get; set; }
        string putterBalance { get; set; }
        string putterHosel { get; set; }
        string putterWeight { get; set; }
        string putterFeel { get; set; }
        string putterLink { get; set; }
        void setCharacteristic(params string[] data);
    }
}
