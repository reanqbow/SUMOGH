using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SumoGH
{
    class Agent
    {
        public string Type;
        public string Id;
        public Dictionary<int, Tuple<double, double, double, string>> PositionDict;

        public Agent(string Type, string Id, Dictionary<int, Tuple<double, double, double, string>> PositionDict)
        {
            this.Type = Type;
            this.Id = Id;
            this.PositionDict = PositionDict;
        }

        public double[] GetPos(Agent a, int time)
        {
            double[] Position = new double[3];
            Position[0] = a.PositionDict[time].Item1;
            Position[1] = a.PositionDict[time].Item2;
            Position[2] = a.PositionDict[time].Item3;

            return Position;
        }
    }
}
