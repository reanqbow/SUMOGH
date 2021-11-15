using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace SumoGH
{
    public class SumoGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "SumoGH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("72255604-592e-4e4b-8b98-9c9f0dbb2915");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
