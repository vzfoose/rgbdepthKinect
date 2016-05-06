/* Image Header.cs
 * 
 * Class for setting and storing the parameters for the header of an image in a .rgbdepth file
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbgdepthKinect
{
    class ImageHeader
    {
        private int _version;
        private int _frameNo;
        private int _images_in_frame;
        private int _image_index_within_frame;
        private int _nrows;
        private int _ncols;
        private int _nlayers;
        private int _bytes_per_datum;
        private int _imageflag;		//0 for depth,rgb;1 for BGR;2 for IR;3 for UV
        private int _imagetype;     //1 for color, 2 for depth, 4 for IR, 8 for UV
        private int _invalid;
       // private double _time;
        private int _data_type;     //1 for in2, 2 for uint, 3 for float

        public ImageHeader(int imagetype, int frame)
        {
            _frameNo = frame;

            switch (imagetype)
            {
                case 1: //color
                    _version=3;
			        _images_in_frame=3;
			        _image_index_within_frame=0;
			        _nrows=1080;
			        _ncols=1920;
			        _nlayers=3;
			        _bytes_per_datum=1;
			        _imageflag=1;
			        _imagetype=1;
			        _invalid = 32001;
			        //_time = 0.0;
			        _data_type = 2;
			        break;

                case 2: //depth
                    _version = 3;
                    _images_in_frame = 3;
                    _image_index_within_frame = 1;
                    _nrows = 424;
                    _ncols = 512;
                    _nlayers = 1;
                    _bytes_per_datum = 2;
                    _imageflag = 0;
                    _imagetype = 2;
                    _invalid = 32001;
                   // _time = 0.0;
                    _data_type = 2;
                    break;
              
                case 4: //IR
                    _version = 4;
                    _images_in_frame = 3;
                    _image_index_within_frame = 2;
                    _nrows = 424;
                    _ncols = 512;
                    _nlayers = 1;
                    _bytes_per_datum = 2;
                    _imageflag = 2;
                    _imagetype = 4;
                    _invalid = -1;
                    // _time = 0.0;
                    _data_type = 2;
                    break;
            }

        }

        //returns all paraemers as a list to be written to file
        public List<int> parameters()
        {
            List<int> paramsList = new List<int>();
            paramsList.Add(_version);
            paramsList.Add(_frameNo);
            paramsList.Add(_images_in_frame);
            paramsList.Add(_image_index_within_frame);
            paramsList.Add(_nrows);
            paramsList.Add(_ncols);
            paramsList.Add(_nlayers);
            paramsList.Add(_data_type);
            paramsList.Add(_bytes_per_datum);
            paramsList.Add(_imageflag);
            paramsList.Add(_imagetype);
            paramsList.Add(_invalid);

            return paramsList;
        }

        //TODO
        //public int headerSize()
        //{
        //    return 56;
        //}
    }
}
