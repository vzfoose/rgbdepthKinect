Kinect rgbdepth

*******************************************************************
INSTALLATION:

Install Microsoft Kinect SDK 2.0

Start Visual Studio.

The prebuilt solution should work fine. 
s
If it does not, follow these steps:

Create a new project
	under other languages/visual c# - select WPF Application
Name the project rgbdepthKinect and click ok

********************************
NOTE: If you give the project a different name, you must change the namespace in all of the included files
eg. if you name the proejct "test" line 14 of fileHeader.cs must be "namespace test"


********************************

In the solution explorer:

right click on the project:
	choose add->existing item:
		add the files:
			Extensions.cs
			FileHeader.cs
			FrameHeader.cs
			ImageHeader.cs

right click on references(under the project in the solution explorer):
	choose add reference:
		add:
			Microsoft.Kinect.dll 
(C:\Program Files\Microsoft SDKs\Kinect\v2.0-DevPreview1311\Assemblies\Microsoft.Kinect.dll)
			System.Drawing.dll 
(C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5)

make the code in the following files match the repo:

App.xaml
App.xaml.cs
MainWindow.xaml
MainWindow.xaml.cs

For some reason, Visual Studio doesn't like it if you try to import these files,
so you will probably need to paste in the code.

As before, if you used a name other than "rgbdepthKinect" you will have to change all references of this name
to the name you chose.

As long as all of the namespace references are consistant with the project name 
AND you have added the appropriate dll references, the project should work fine

*****************************************************************

USE:

View and collect color, depth, and ir images with a kinect 2.0 camera.

Press the buttons to view the desired stream.

Pressing the 'collect' button will add a shot with the desired number of frames (from the frames per shot box)
to the file specified by the filename box (filename.rgbdepth) if the file does not exist, it will be created, otherwise the shot will be appended to the file.

Green test will appear to the right of the frames per shot box to indicate a shot was successfully written.

The file will appear in the solution's debug directory by default

*******************************************************************


