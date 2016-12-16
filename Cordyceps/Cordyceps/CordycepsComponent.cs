using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Text;

namespace Cordyceps
{
    public class CordycepsComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CordycepsComponent()
            : base("Cordyceps", "Cordyceps",
                "Reading the module/Cordycepting",
                "Cordyceps", "Cordyceps")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Open the port", "Open", "Opens the port", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Starts the reading", "Start", "Starts reading the values from the Cordyceps", GH_ParamAccess.item, false);
            pManager.AddTextParameter("COMport", "COMport", "Choose which port to read.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MaxValue", "MaxValue", "Adds a max value for all knobs.", GH_ParamAccess.item, 1000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Messages", "msg", "Read your messages!", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Knob1", "I", "Your 1st value.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Knob2", "II", "Your 2nd value.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Knob3", "III", "Your 3rd value.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Knob4", "IV", "Your 4th value.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Slider", "Slider", "Connect the Slider to the Camera component.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Open = default(bool);
            bool Start = default(bool);
            string COMport = default(string);
            int maxValue = default(int);

            DA.GetData(0, ref Open);
            DA.GetData(1, ref Start);
            DA.GetData(2, ref COMport);
            DA.GetData(3, ref maxValue);

            List<double> resultA = new List<double>();
            List<double> resultB = new List<double>();
            List<double> resultC = new List<double>();
            List<double> resultD = new List<double>();
            List<double> resultE = new List<double>();

            List<string> msgList = new List<string>();

            //List<bool> testList = new List<bool>();

            System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort(COMport, 9600); 

            if (Open == true)
            {
                port.RtsEnable = true;
                port.DtrEnable = true;
                port.Open();

                msgList.Add("Fuck yes mate, port is open. Set Start to True.");

                if (Start == true & Open == true & port.IsOpen == true)
                {
                    _timer();
                    string resA = port.ReadLine();    //Read the respective line from the port.
                    string resB = port.ReadLine();
                    string resC = port.ReadLine();
                    string resD = port.ReadLine();
                    string resE = port.ReadLine();

                    double valA = System.Double.Parse(resA.Trim());   //Converting the strings to doubles
                    double valB = System.Double.Parse(resB.Trim());
                    double valC = System.Double.Parse(resC.Trim());
                    double valD = System.Double.Parse(resD.Trim());
                    double valE = System.Double.Parse(resE.Trim());

                    double mappedA = (Math.Round((valA / 1000) * maxValue, 5));   //Remapping the numbers
                    double mappedB = (Math.Round((valB / 1000) * maxValue, 5));   //if user asks for it
                    double mappedC = (Math.Round((valC / 1000) * maxValue, 5));
                    double mappedD = (Math.Round((valD / 1000) * maxValue, 5));
                    double mappedE = (Math.Round((valE / 1000) * maxValue, 5));
                    //testList.Add(mappedA);

                    if (maxValue == 1000)
                    {
                        resultA.Add(valA);      //If user doesn't ask for remap
                        resultB.Add(valB);      //the string values are output
                        resultC.Add(valC);
                        resultD.Add(valD);
                        resultE.Add((Math.Round((valE / 1000) * 360, 5)));
                    }
                    else
                    {
                        resultA.Add(mappedA);   //Remapped numbers if maxValue != 0
                        resultB.Add(mappedB);
                        resultC.Add(mappedC);
                        resultD.Add(mappedD);
                        resultE.Add(mappedE);
                    }
                    msgList.Add("Feel the flow, lol.");
                }
            }

            else
            {
                port.Close();
                msgList.Add("Too bad mate,  port is closed.");
            }

            DA.SetDataList(1, resultA);
            DA.SetDataList(2, resultB);
            DA.SetDataList(3, resultC);
            DA.SetDataList(4, resultD);
            DA.SetDataList(5, resultE);
            DA.SetDataList(0, msgList);
            //test = maxValue;
            port.Close();

        }

        GH_Document GHdoc;

        void _timer()
        {
            GHdoc = OnPingDocument();
            GHdoc.ScheduleSolution(50, CallBack);
        }
        public void CallBack(GH_Document GHdoc)
        {
            ExpireSolution(false);
        }
        public void expire(bool recompute)
        {
            recompute = true;
        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Cordyceps.Properties.Resources.new_cordyceps;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{6ed6f83e-a7cf-43bd-9c32-48b65fa76d97}"); }
        }
    }
}

namespace GetPorts
{
    public class GetPortsComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetPortsComponent()
            : base("GetPorts", "GetPorts",
                "This component will check which ports are open",
                "Cordyceps", "Port Access")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("getPorts", "getPorts", "Set this to True to get the available ports.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ports available", "Ports", "The list shows the available ports. Use the List Item component to choose one.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool getport = default(bool);

            if (!DA.GetData(0, ref getport)) return;

            List<string> ports = new List<string>();
            if (getport == true)
            {
                _timer();
                ports.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            }
            else
            {
                ports.Add("Set getPorts to True mate");
            }
            DA.SetDataList(0, ports);

        }


        GH_Document GHdoc;
        void _timer()
        {
            GHdoc = OnPingDocument();
            GHdoc.ScheduleSolution(50, CallBack);
        }
        public void CallBack(GH_Document GHdoc)
        {
            ExpireSolution(false);
        }
        public void Expire(bool recompute)
        {
            recompute = true;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;                
                return Cordyceps.Properties.Resources.new_usb;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{f8946ba8-dab5-49c7-abb8-1372a7292f0f}"); }
        }
    }
}

namespace SetCamera
{
    public class SetCameraComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SetCameraComponent()
            : base("SetCamera", "SetCamera",
                "This component will set the view in Perspective window",
                "Cordyceps", "Viewport")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Activate the camera", "Activate", "Sets the view in Persective window.", GH_ParamAccess.item, false);
            pManager.AddPointParameter("The target of camera", "Target", "Becomes the target of the view.", GH_ParamAccess.item, new Point3d(0, 0, 0));
            pManager.AddPointParameter("The point of view", "POV", "Becomes the point of view.", GH_ParamAccess.item, new Point3d(100, 100, 100));
            pManager.AddIntegerParameter("Camera Slider", "Slider", "The input value will rotate the camera around the Target-Point.", GH_ParamAccess.item, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Messages", "msg", "Read your messages!", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Activate = false;
            Point3d Target = new Point3d(0, 0, 0);
            Point3d POV = new Point3d(100, 100, 100);
            int Slider = 0;

            if (!DA.GetData(0, ref Activate)) return;
            if (!DA.GetData(1, ref Target)) return;
            if (!DA.GetData(2, ref POV)) return;
            if (!DA.GetData(3, ref Slider)) return;


            List<string> msgList = new List<string>();
            double degrees = Slider * (Math.PI / 180);
            if (Activate == true)
            {
                Rhino.Display.RhinoViewport vp;

                if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.IsPerspectiveProjection == true)
                {
                    vp = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;
                    Transform rotate = Transform.Rotation(degrees, Target);
                    POV.Transform(rotate);
                    vp.SetCameraLocations(Target, POV);
                    //Print("Camera rollin");
                    msgList.Add("Camera rollin'");
                }
                else
                {
                    //Print("Go to Perspective view and set to True again!");
                    msgList.Add("Go to Perspective view and try again mate!");
                }
            }
            else
            {
                //Print("Set Activate to True mate");
                msgList.Add("Set Active to True, lol");
            }

            DA.SetDataList(0, msgList);

        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;                
                return Cordyceps.Properties.Resources.new_videocamera;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{24c4904d-8f93-4ef9-b9e6-1efc1b4e3d4c}"); }
        }
    }
}

namespace getCapture
{
    public class GetCaptureComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetCaptureComponent()
            : base("GetCapture", "GetCapture",
                "This component will capture a screenshot of your current active view.",
                "Cordyceps", "Viewport")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Captures a screenshot!", "Capture", "Connect with Button. Capture a screenshot of current active window.", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Set the width of the screenshot", "Width", "Sets the width of your final screenshot.", GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Set the height of the screenshot", "Height", "Sets the height of your final screenshot.", GH_ParamAccess.item, 700);
            pManager.AddTextParameter("Set the filepath of the screenshot", "Path", "Final path of your screenshot. Remember to end the path with a /.", GH_ParamAccess.item, "desktop/");
            pManager.AddTextParameter("Set the name of the screenshot", "Filename", "Decides the name of your screenshot.", GH_ParamAccess.item, "Screenshot_1");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Messages", "msg", "Read your messages!", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Capture = false;
            int Width = 1000;
            int Height = 700;
            string Path = "desktop/";
            string Filename = "Capture1";

            if (!DA.GetData(0, ref Capture)) return;
            if (!DA.GetData(1, ref Width)) return;
            if (!DA.GetData(2, ref Height)) return;
            if (!DA.GetData(3, ref Path)) return;
            if (!DA.GetData(4, ref Filename)) return;

            List<string> msgList = new List<string>();

            if (Capture == true)
            {
                System.Drawing.Bitmap screen = new System.Drawing.Bitmap(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.CaptureToBitmap(new System.Drawing.Size(Width, Height)));
                screen.Save(Path + Filename + ".png");
                msgList.Add("Screenshot saved to " + Path + Filename + ".png" + " mate!");
            }
            else
            {
                msgList.Add("Set the height, the width, the path and the filename. When you're ready, set Start to True mate!");
            }

            DA.SetDataList(0, msgList);

        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;                
                return Cordyceps.Properties.Resources.new_camera;

            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("FC62E1FB-DB0B-4D69-86A9-E2B72E0D0E44"); }

        }
    }
}

namespace checkLicense
{
    public class LicenseComponent : GH_AssemblyPriority
    {
        public LicenseComponent()
        {
        }
        
    
    public override GH_LoadingInstruction PriorityLoad()
    {
    DateTime EndLicense = new DateTime(2017, 07, 15);
    DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        if(dateNow<=EndLicense){
            Rhino.RhinoApp.WriteLine("Cordyceps loaded!");
            Rhino.RhinoApp.WriteLine("License ends " + EndLicense.ToString("dd.MM.yyyy") + ". Written by Zakaria Djebbara.");
            return GH_LoadingInstruction.Proceed;
        }
        else{
            Rhino.RhinoApp.WriteLine("Cordyceps failed to load.");
            Rhino.RhinoApp.WriteLine("Contact developer for renewal of license. License ended " + EndLicense.ToString("dd.MM.yyyy") + ".");
            Rhino.UI.Dialogs.ShowMessageBox("Cordyceps not loaded..." + Environment.NewLine + "Contact developer for renewal of license. License ended " + EndLicense.ToString("dd.MM.yyyy") + ".", "Cordyceps Plugin");
            return GH_LoadingInstruction.Abort;
        }
}
}
    }