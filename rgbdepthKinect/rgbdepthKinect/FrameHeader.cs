/* FileHeader.cs
 * 
 * Class for setting and storing the parameters for the header of a frame in a .rgbdepth file
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbgdepthKinect
{

    class FrameHeader
    {
        private int _version;
        private int _images_in_frame;
        private int _camID; //camID will be zero on ordinary non-network kinect apps
        public int _frameNo { get; set; }
        public int _shotNo {get; set; }

        public FrameHeader(int frame, int shot)
        {
            _version = 3;
            _images_in_frame = 3;
            _camID = 0;
            _frameNo = frame;
            _shotNo = shot;
        }


        //returns all paraemers as a list to be written to file
        public List<int> parameters()
        {
            List<int> paramsList = new List<int>();
            paramsList.Add(_version);
            paramsList.Add(_frameNo);
            paramsList.Add(_images_in_frame);
            paramsList.Add(_camID);
            paramsList.Add(_shotNo);
            return paramsList;
        }


    }
}
