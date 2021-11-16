using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SumoGH
{
    class ParseAgent
    {
        public int TimestepCounter(IEnumerable<XElement> xe)
        {
            int i = 0;
            foreach(XElement x in xe)
            {
                i++;
            }
            return i;
        }

        public static Agent MakeVeh(XElement veh, string id, int counter)
        {
            string type = veh.Name.ToString();
            double x = Convert.ToDouble(veh.Attribute("x").Value);
            double y = Convert.ToDouble(veh.Attribute("y").Value);
            double degree = Convert.ToDouble(veh.Attribute("angle").Value);
            string laneId = veh.Attribute("lane").Value;

            Dictionary<int, Tuple<double, double, double, string>> dict =
               new Dictionary<int, Tuple<double, double, double, string>>();

            dict.Add(counter, Tuple.Create(x, y, degree, laneId));
            Agent a = new Agent(type, id, dict);

            return a;
        }

        public static Agent MakePed(XElement veh, string id, int counter)
        {
            string type = veh.Name.ToString();
            double x = Convert.ToDouble(veh.Attribute("x").Value);
            double y = Convert.ToDouble(veh.Attribute("y").Value);
            double degree = Convert.ToDouble(veh.Attribute("angle").Value);
            string laneId = veh.Attribute("edge").Value;

            Dictionary<int, Tuple<double, double, double, string>> dict =
               new Dictionary<int, Tuple<double, double, double, string>>();

            dict.Add(counter, Tuple.Create(x, y, degree, laneId));
            Agent a = new Agent(type, id, dict);

            return a;
        }

        public Dictionary<string, Agent> Parse(IEnumerable<XElement> xe)
        {
            Dictionary<string, Agent> res = new Dictionary<string, Agent>();
            int counter = 0;
            foreach (XElement timestep in xe)
            {
                IEnumerable<XElement> de = timestep.Descendants();
                foreach (XElement v in de)
                {
                    string id = v.Attribute("id").Value;
                    if (v.Name.ToString() != "person" && v.Name.ToString() != "bike")
                    {
                        if (!res.ContainsKey(id))
                        {
                            Agent a = MakeVeh(v, id, counter);
                            res.Add(id, a);
                        }
                        else
                        {
                            double x = Convert.ToDouble(v.Attribute("x").Value);
                            double y = Convert.ToDouble(v.Attribute("y").Value);
                            double degree = Convert.ToDouble(v.Attribute("angle").Value);
                            string laneId = v.Attribute("lane").Value;
                            res[id].PositionDict.Add(counter, Tuple.Create(x, y, degree, laneId));
                        }
                    }
                    else
                    {
                        if (!res.ContainsKey(id))
                        {
                            Agent a = MakePed(v, id, counter);
                            res.Add(id, a);
                        }
                        else
                        {
                            double x = Convert.ToDouble(v.Attribute("x").Value);
                            double y = Convert.ToDouble(v.Attribute("y").Value);
                            double degree = Convert.ToDouble(v.Attribute("angle").Value);
                            string laneId = v.Attribute("edge").Value;
                            res[id].PositionDict.Add(counter, Tuple.Create(x, y, degree, laneId));
                        }
                    }
                }
                counter++;
            }

            return res;
        }
    }
}
