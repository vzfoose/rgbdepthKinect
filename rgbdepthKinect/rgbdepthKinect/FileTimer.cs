using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgbdepthKinect
{
    class FileTimer
    {


        public FileTimer() { } //empty constructor

        public double time()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long msSinceEpoch = (long)t.TotalMilliseconds;

            double secondsSinceEpoch = msSinceEpoch / 1000.00;

            return secondsSinceEpoch;

        }
    }
}
