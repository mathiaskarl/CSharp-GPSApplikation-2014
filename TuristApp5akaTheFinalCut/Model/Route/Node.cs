using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml.Media;

namespace TuristApp5akaTheFinalCut.Model.Route
{
    [DataContract]
    public class Node
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Point Coordinates { get; set; }
        public Boolean Visited { get; set; }

        [DataMember]
        public Dictionary<int, Path> PathList = new Dictionary<int, Path>();
        [DataMember]
        public List<Edge> EdgeList = new List<Edge>();
        [DataMember]
        public List<Neighbor> Neighbors = new List<Neighbor>();

        // XML Initiate / Storage
        [DataMember]
        public Dictionary<int, double> initNeighbors = new Dictionary<int, double>();

        public Node() { }
        

        public Node(int Id, string Name, Point Coordinates, bool Visited = false)
        {
            this.Id = Id;
            this.Name = Name;
            this.Coordinates = Coordinates;
            PathList = new Dictionary<int, Path>();
            EdgeList = new List<Edge>();
            Neighbors = new List<Neighbor>();
            initNeighbors = new Dictionary<int, double>();
        }

        public Node(Node node)
        {
            this.Id = node.Id;
            this.Name = node.Name;
            this.Coordinates = node.Coordinates;
            Dictionary<int, double> newList = new Dictionary<int, double>();
            foreach(var obj in node.initNeighbors)
                newList.Add(obj.Key, obj.Value);
            this.initNeighbors = newList;
            PathList = new Dictionary<int, Path>();
            EdgeList = new List<Edge>();
            Neighbors = new List<Neighbor>();
        }
    }
}
