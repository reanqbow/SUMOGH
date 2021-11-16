using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SumoGH
{
    class TimeParser
    {
            public IEnumerable<XElement> Reader(string FilePath)
            {
                // Return the Timestep child of the tree
                XElement xelement = XElement.Load(FilePath);
                IEnumerable<XElement> timesteps = xelement.Elements();
                return timesteps;
            }
    }
}
