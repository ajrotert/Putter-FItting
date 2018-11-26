using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutterFitting
{
    interface Iputter
    {
        string putterShape { get; set; }
        string putterBalance { get; set; }
        string putterHosel { get; set; }
        string putterWeight { get; set; }
        string putterFeel { get; set; }
        void setCharacteristic(params string[] data);
        //add link to website for purchase?
    }
}
