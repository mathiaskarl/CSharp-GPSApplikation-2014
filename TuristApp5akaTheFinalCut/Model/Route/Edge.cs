using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace TuristApp5akaTheFinalCut.Model.Route
{
    [DataContract]
    public class Edge
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int NodeA { get; set; }
        [DataMember]
        public int NodeB { get; set; }
        [DataMember]
        public List<Point> Lines { get; set; }

        public Edge() { }

        public Edge(int Id, int NodeA, int NodeB, List<Point> Lines = null)
        {
            this.Id = Id;
            this.NodeA = NodeA;
            this.NodeB = NodeB;
            this.Lines = (Lines != null ? Lines : new List<Point>());
        }

        public Edge(Edge edge)
        {
            this.Id = edge.Id;
            this.NodeA = edge.NodeA;
            this.NodeB = edge.NodeB;
            this.Lines = new List<Point>(edge.Lines);
        }
    }
}
