using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace SumoGH
{
    public class SumoGHComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SumoGHComponent()
          : base("SumoGH", "SumoGH",
              "A parser and visualizer of SUMO dataset in Grasshopper",
              "Sumo", "Reader")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Time", "T", "The Timestep of an Xml output.", GH_ParamAccess.item, 1);
            pManager.AddTextParameter("FilePath", "FP", "The Xml file path of SUMO output.", GH_ParamAccess.item, "");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("TotalTime", "TT", "The total timestep count.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("VehiclePosition", "vPos", "The plane list of vehicle positions", GH_ParamAccess.list);
            pManager.AddPlaneParameter("BikePosition", "bPos", "The plane list of bike positions", GH_ParamAccess.list);
            pManager.AddPlaneParameter("PedestrianPosition", "pPos", "The plane list of pedestrian positions", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Initialize input.
            int time = 0;
            string filePath = "";

            DA.GetData("Time", ref time);
            DA.GetData("FilePath", ref filePath);
            
            //Instantiate parsers.
            TimeParser t = new TimeParser();
            ParseAgent p = new ParseAgent();
            Vector3d n = new Vector3d(0, 0, 1);

            IEnumerable<XElement> xe = t.Reader(filePath);
            int totalTime = p.TimestepCounter(xe);

            //Parse Xml to a dictionary.
            Dictionary<string, Agent> res = p.Parse(xe);

            List<Plane> vehlst = new List<Plane>();
            List<Plane> pedlst = new List<Plane>();
            List<Plane> bikelst = new List<Plane>();

            //Loop the dictionary to get coordinates according to time.
            foreach (KeyValuePair<string, Agent> kvp in res)
            {
                if (kvp.Value.PositionDict.ContainsKey(time))
                {
                    if (kvp.Key[0] == 'v')
                    {
                        double x = kvp.Value.GetPos(kvp.Value, time)[0];
                        double y = kvp.Value.GetPos(kvp.Value, time)[1];
                        double angle = kvp.Value.GetPos(kvp.Value, time)[2];
                        Point3d pt_v = new Point3d(x, y, 0);
                        Plane pl_v = new Plane(pt_v, n);
                        pl_v.Rotate(angle, n);
                        vehlst.Add(pl_v);
                    }
                    else if (kvp.Key[0] == 'b')
                    {
                        double x = kvp.Value.GetPos(kvp.Value, time)[0];
                        double y = kvp.Value.GetPos(kvp.Value, time)[1];
                        double angle = kvp.Value.GetPos(kvp.Value, time)[2];
                        Point3d pt_b = new Point3d(x, y, 0);
                        Plane pl_b = new Plane(pt_b, n);
                        pl_b.Rotate(angle, n);
                        bikelst.Add(pl_b);
                    }
                    else
                    {
                        double x = kvp.Value.GetPos(kvp.Value, time)[0];
                        double y = kvp.Value.GetPos(kvp.Value, time)[1];
                        double angle = kvp.Value.GetPos(kvp.Value, time)[2];
                        Point3d pt_p = new Point3d(x, y, 0);
                        Plane pl_p = new Plane(pt_p, n);
                        pl_p.Rotate(angle, n);
                        pedlst.Add(pl_p);
                    }
                }
            }

            DA.SetData("TotalTime", totalTime);
            DA.SetDataList("VehiclePosition", vehlst);
            DA.SetDataList("BikePosition", bikelst);
            DA.SetDataList("PedestrianPosition", pedlst);
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("77248f6d-3b83-4aa5-b8b8-1f3495452eda"); }
        }
    }
}
