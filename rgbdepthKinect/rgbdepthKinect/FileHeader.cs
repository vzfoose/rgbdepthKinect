/* FileHeader.cs
 * 
 * Class for setting and storing the parameters for the file header of a .rgbdepth file
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace rbgdepthKinect
{

  
    [Serializable()]
    class parameters
    {
        public int _version = 5;
        public int _IDtag;
        public int _camID;
        public int _framesPerShot;
        public double _curTime = 0.0;
    }
    
    class FileHeader
    {
        
        //private int _version = 5;
        //private int _IDtag;
        //private int _camID;
        //private int _framesPerShot;
        //private double _curTime = 0.0;
        private parameters _parameters = new parameters();

        private int _headerSize = 24; //4*4ints(4bytes each) + 1 double (8bytes)

        public FileHeader(int shotframes)
        {
            _parameters._IDtag = 12010013;
            _parameters._camID = 0;
            _parameters._framesPerShot = shotframes;
        }
        

        //returns all paraemers as a list to be written to file
        public List<int> parameters()
        {
            List<int> paramsList = new List<int>();
            paramsList.Add(_parameters._version);
            paramsList.Add(_parameters._IDtag);
            paramsList.Add(_parameters._camID);
            paramsList.Add(_parameters._framesPerShot);

            return paramsList;
        }



        //size of this header in bytes 
        //TODO: calculate - hardcoded for now
        public int headersize()
        {
            return _headerSize;
        }
        


    }
}
