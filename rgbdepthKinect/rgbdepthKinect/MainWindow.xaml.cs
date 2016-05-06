using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//using LightBuzz.Vitruvius; 

namespace rbgdepthKinect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        Mode _mode = Mode.Color;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;

        Boolean _newShot = false;

        int FramesToCollect = 0; //frames left to be written
        int frameIndex = 0; //will increment everytime a frame is written
        int shotIndex = 0; //will increment with each shot taken for a particular file

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        private void CollectAllData(MultiSourceFrame reference, string filename)
        {
            FramesToCollect--;

            var colorFrame = reference.ColorFrameReference.AcquireFrame();
            var depthFrame = reference.DepthFrameReference.AcquireFrame();
            var irFrame = reference.InfraredFrameReference.AcquireFrame();

            int framesPer = Int32.Parse(frameBox.Text);

            //set up the writer object
            BinaryWriter writer;

            //check to see if file exists - append if so, create if not
            if (File.Exists(filename + ".rgbdepth"))
            {
                writer = new BinaryWriter(File.Open(filename + ".rgbdepth", FileMode.Append));

                if (_newShot) { shotIndex++; }
            }
            else
            {
                //reset frame index and shot index
                frameIndex = 0;
                shotIndex = 0;
                writer = new BinaryWriter(File.Open(filename + ".rgbdepth", FileMode.Create));

                //set up the file header
                FileHeader fileheader = new FileHeader(framesPer);

                //write fileheader parameters
                foreach (int item in fileheader.parameters())
                {
                    writer.Write(item);
                }
                writer.Write(0.0);//TODO get current time
            }

            //set up frameheader
            FrameHeader frameheader = new FrameHeader(frameIndex, shotIndex);

            //write frameheader parameters
            foreach (int item in frameheader.parameters())
            {
                writer.Write(item);
            }
            writer.Write(0.0);//TODO get current time


            // Color
            using (colorFrame)
            {
                //colorFrame.Save(filename+"_color.png"); //used for png shapshot

                ImageHeader colorHeader = new ImageHeader(1, frameIndex);

                //write colorheader parameters
                foreach (int item in colorHeader.parameters())
                {
                    writer.Write(item);
                }
                writer.Write(0.0);//TODO get current time

                //write color image data
                writer.Write(colorFrame.rawColor());
                int cframeSize = 3 * 1920 * 1080;

                //write size of color image + color header data
                writer.Write(cframeSize + 56);//headersize = 56

            }

            // Depth
            using (depthFrame)
            {
                // depthFrame.Save(filename + "_depth.png");

                ImageHeader depthHeader = new ImageHeader(2, frameIndex);

                //write depthheader parameters
                foreach (int item in depthHeader.parameters())
                {
                    writer.Write(item);
                }
                writer.Write(0.0);//TODO get current time

                //write raw depth (ushort) array
                ushort[] sArray = depthFrame.rawDepth();
                for (int i = 0; i < sArray.Length; i++)
                {
                    writer.Write(sArray[i]);
                }

                int dframeSize = 2 * 512 * 424;
                writer.Write(dframeSize + 56);

            }

            // IR
            using (irFrame)
            {
                ////irFrame.Save(filename + "_ir.png");

                ImageHeader irHeader = new ImageHeader(4, frameIndex);

                //write depthheader parameters
                foreach (int item in irHeader.parameters())
                {
                    writer.Write(item);
                }
                writer.Write(0.0);//TODO get current time

                //write raw ir (ushort) array
                ushort[] sArray = irFrame.rawIR();
                for (int i = 0; i < sArray.Length; i++)
                {
                    writer.Write(sArray[i]);
                }

                int dframeSize = 2 * 512 * 424;
                writer.Write(dframeSize + 56);
            }

            writer.Close();
            shotConfimedLabel.Content = "Shot: " + shotIndex + " successfully written to " + filename + ".rgbdepth";
            _newShot = false;

        }


        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {

            var reference = e.FrameReference.AcquireFrame();

            if ((_newShot || FramesToCollect != 0) && reference.ColorFrameReference.AcquireFrame() != null)
            {
                CollectAllData(reference, fileBox.Text);
            }

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();

                    }
                }
            }

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Infrared)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Depth;
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Infrared;
        }

        private void Collect_Click(object sender, RoutedEventArgs e)
        {
            _newShot = true;
            FramesToCollect = Int32.Parse(frameBox.Text);
        }

        #endregion



    }

    public enum Mode
    {
        Color,
        Depth,
        Infrared
    }
}
