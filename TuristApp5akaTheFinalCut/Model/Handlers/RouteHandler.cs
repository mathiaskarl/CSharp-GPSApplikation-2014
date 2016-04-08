using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using TuristApp5akaTheFinalCut.Common;
using TuristApp5akaTheFinalCut.Model.Route;

namespace TuristApp5akaTheFinalCut.Model.Handlers
{
    class RouteHandler
    {
        private List<Node> nodeData = new List<Node>();
        private List<Edge> edgeData = new List<Edge>();
        private List<Node> nodesLoadState = new List<Node>();
        private List<Edge> edgesLoadState = new List<Edge>();

        public List<Node> NodeList;
        public TrackPoint trackPoint;
        public double TotalDistance;

        public RouteHandler(List<Node> nodeInput, List<Edge> edgeInput)
        {
            initiateLists(nodeInput, edgeInput, true);
            CreateRoutes(nodeInput, edgeInput);
        }

        private void CreateRoutes(List<Node> nodeInput, List<Edge> edgeInput)
        {
            NodeList = new List<Node>();
            foreach (Node obj in nodeInput)
            {
                foreach (KeyValuePair<int, double> objs in obj.initNeighbors)
                {
                    Node currentNode = nodeInput.FirstOrDefault(data => data.Id == objs.Key);
                    Neighbor tempNeighbor = new Neighbor();
                    tempNeighbor.Node = currentNode;
                    tempNeighbor.Distance = objs.Value;
                    obj.Neighbors.Add(tempNeighbor);

                    Route.Path tempPath = new Route.Path();
                    tempPath.NodeList = new List<Node>() { currentNode };
                    tempPath.TotalDistance = objs.Value;
                    obj.PathList.Add(objs.Key, tempPath);
                }

                NodeList.Add(obj);
            }

            CreateEdges(NodeList, edgeInput);

            bool whileCheck = true;
            do
            {
                foreach (Node obj in NodeList)
                {
                    if (obj.PathList.Count < NodeList.Count - 1)
                        obj.Visited = false;
                    CheckNode(obj);
                }
                if (!(NodeList.Any(data => data.PathList.Count < NodeList.Count - 1)))
                    whileCheck = false;

            } while (whileCheck);
        }

        private void CreateEdges(List<Node> passedNodes, List<Edge> passedEdges)
        {
            foreach (Node obj in passedNodes)
                obj.EdgeList.Clear();

            foreach (Edge obj in passedEdges)
            {

                List<Point> reversedList = new List<Point>(obj.Lines);
                reversedList.Reverse();
                List<Edge> foreachList = new List<Edge>()
                {
                    new Edge(obj.Id, obj.NodeA, obj.NodeB, obj.Lines),
                    new Edge(obj.Id, obj.NodeB, obj.NodeA, reversedList)
                };


                foreach (Edge objs in foreachList)
                {
                    Node NodeA = passedNodes.FirstOrDefault(data => data.Id == objs.NodeA);
                    Node NodeB = passedNodes.FirstOrDefault(data => data.Id == objs.NodeB);

                    if (objs.Lines.Count < 1 || !(objs.Lines[0].Equals(new Point(NodeA.Coordinates.X, NodeA.Coordinates.Y))) && !(objs.Lines[objs.Lines.Count - 1].Equals(new Point(NodeB.Coordinates.X, NodeB.Coordinates.Y))))
                    {
                        objs.Lines.Insert(0, new Point(NodeA.Coordinates.X, NodeA.Coordinates.Y));
                        objs.Lines.Add(new Point(NodeB.Coordinates.X, NodeB.Coordinates.Y));
                    }

                    passedNodes[passedNodes.FindIndex(data => data.Id == objs.NodeA)].EdgeList.Add(objs);
                }
            }
        }

        static void CheckNode(Node node)
        {
            if (!node.Visited)
            {
                node.Visited = true;
                foreach (Neighbor obj in node.Neighbors)
                {
                    CheckNode(obj.Node);
                    foreach (int key in obj.Node.PathList.Keys)
                    {
                        if (key != node.Id)
                        {
                            List<Node> nodeList = new List<Node>();
                            double neighborDistance = obj.Node.PathList[key].TotalDistance;
                            if (node.PathList.ContainsKey(key))
                            {
                                double currentDistance = node.PathList[key].TotalDistance;
                                if (neighborDistance + obj.Distance < currentDistance)
                                {
                                    nodeList.AddRange(obj.Node.PathList[key].NodeList);
                                    nodeList.Insert(0, obj.Node);
                                    node.PathList[key].NodeList = nodeList;
                                    node.PathList[key].TotalDistance = neighborDistance + obj.Distance;
                                }
                            }
                            else
                            {
                                nodeList.AddRange(obj.Node.PathList[key].NodeList);
                                nodeList.Insert(0, obj.Node);

                                Route.Path path = new Route.Path()
                                {
                                    NodeList = nodeList,
                                    TotalDistance = obj.Distance + neighborDistance
                                };
                                node.PathList.Add(key, path);
                            }
                        }
                    }
                }
            }
        }

        public void EdgePoint(Point point, Point point2)
        {
            CreateEdges(nodeData, edgeData);
            Node currentNode = ClosestNode(point);
            ClosestEdge(currentNode, point);
            CreateNode();
            trackPoint = null;

            CreateEdges(nodeData, edgeData);
            Node currentNode2 = ClosestNode(point2);
            ClosestEdge(currentNode2, point2);
            CreateNode();
            CreateRoutes(nodeData, edgeData);
            initiateLists(nodesLoadState, edgesLoadState);
        }

        private void CreateNode()
        {
            int Id = 1;
            List<int> IdList = nodeData.Select(data => data.Id).ToList();
            while (IdList.Contains(Id))
            {
                Id++;
            }
            trackPoint.Id = Id;

            Node newNode = new Node(Id, Id.ToString(), trackPoint.Point, false);
            List<Node> currentNodes = nodeData.Where(data => data.Id == trackPoint.Edge.NodeA || data.Id == trackPoint.Edge.NodeB).ToList();


            foreach (Node obj in nodeData)
            {
                if (!(currentNodes.Any(data => data.Id == obj.Id)))
                    continue;

                foreach (var objss in currentNodes)
                    obj.initNeighbors.Remove(objss.Id);

                if (!obj.initNeighbors.Any(data => data.Key == Id))
                    obj.initNeighbors.Add(Id, 2);
                newNode.initNeighbors.Add(obj.Id, 2);
            }
            TrackPoint TrackA = new TrackPoint(trackPoint);
            TrackA.Edge.Lines.Insert(TrackA.EdgeLine-1, TrackA.Point);
            TrackPoint TrackB = new TrackPoint(trackPoint);
            TrackB.Edge.Lines.Insert(TrackB.EdgeLine - 1, TrackB.Point);

            edgeData.RemoveAll(data => data.Id == trackPoint.Edge.Id);
            edgeData.Add(new Edge(edgeData.Count, trackPoint.Edge.NodeA, Id, (trackPoint.TotalLines > 2 ? TrackA.Edge.Lines.GetRange(0, trackPoint.EdgeLine) : null)));
            edgeData.Add(new Edge(edgeData.Count + 1, Id, trackPoint.Edge.NodeB, (trackPoint.TotalLines > 2 ? TrackB.Edge.Lines.GetRange(trackPoint.EdgeLine-1, TrackB.Edge.Lines.Count-(trackPoint.EdgeLine-1)) : null)));
            nodeData.Add(newNode);
        }

        private Node ClosestNode(Point point)
        {
            List<KeyValuePair<double, Node>> NodesDistance = new List<KeyValuePair<double, Node>>();
            foreach (Node obj in nodeData)
            {
                double result = Math.Pow((point.X - obj.Coordinates.X), 2) + Math.Pow((point.Y - obj.Coordinates.Y), 2);
                NodesDistance.Add(new KeyValuePair<double, Node>(result, obj));
            }
            var key = NodesDistance.Min(data => data.Key);
            return NodesDistance.FirstOrDefault(data => data.Key == key).Value;
        }

        private void ClosestEdge(Node currentNode, Point point)
        {
            List<Edge> tempEdges = currentNode.EdgeList;
            foreach (Edge obj in tempEdges)
            {
                int NumberOfSteps = 2;
                for (int i = 0; i < obj.Lines.Count; i++)
                {
                    if (obj.Lines.Count > i + 1)
                    {
                        List<Point> tempPoints = LinePoints(obj.Lines[i], obj.Lines[i + 1]);
                        foreach (Point objPoint in tempPoints)
                        {
                            double result = Math.Pow((point.X - objPoint.X), 2) + Math.Pow((point.Y - objPoint.Y), 2);
                            if (trackPoint == null || result < trackPoint.Distance)
                            {
                                trackPoint = new TrackPoint(objPoint, result, obj, NumberOfSteps, obj.Lines.Count);
                            }
                        }
                    }
                    NumberOfSteps++;
                }
            }
            trackPoint.EdgeLine = trackPoint.EdgeLine > 0 ? trackPoint.EdgeLine : 1;
        }

        public List<Point> LinePoints(Point a, Point b)
        {
            List<Point> tempList = new List<Point>();

            bool steep = Math.Abs(b.Y - a.Y) > Math.Abs(b.X - a.X);
            if (steep)
            {
                double temp;
                temp = a.X; a.X = a.Y; a.Y = temp;
                temp = b.X; b.X = b.Y; b.Y = temp;
            }
            if (a.X > b.X)
            {
                double temp;
                temp = a.X; a.X = b.X; b.X = temp;
                temp = a.Y; a.Y = b.Y; b.Y = temp;
            }

            double dx = b.X - a.X;
            double dy = Math.Abs(b.Y - a.Y);
            double error = dx / 2;
            double ystep = (a.Y < b.Y) ? 1 : -1;
            double y = a.Y;

            for (double x = a.X; x <= b.X; x++)
            {
                tempList.Add(new Point((steep ? y : x), (steep ? x : y)));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            return tempList;
        }

        public List<Polyline> AddRoute(int x, int y)
        {
            List<Polyline> tempList = new List<Polyline>();
            Node routeNode = NodeList.FirstOrDefault(data => data.Id == x);
            List<Node> routePath = routeNode.PathList.FirstOrDefault(data => data.Key == y).Value.NodeList;
            routePath.Insert(0, routeNode);
            for (int i = 0; i < routePath.Count; i++)
            {
                if (i != routePath.Count - 1)
                {
                    int key = (routePath.Count > 1 ? i + 1 : 0);
                    Node currentNode = routePath[i];
                    Edge currentEdge = currentNode.EdgeList.FirstOrDefault(data => data.NodeB == routePath[key].Id);
                    Polyline poly = new Polyline()
                    {
                        StrokeThickness = 10,
                        Points = Tools.ToPointCollection(currentEdge.Lines),
                        Stroke = new SolidColorBrush() { Color = Colors.Black },
                        Opacity = 0.7,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round 
                    };
                    
                    tempList.Add(poly);
                }
            }
            TotalDistance = routeNode.PathList.FirstOrDefault(data => data.Key == y).Value.TotalDistance;
            return tempList;
        }

        #region initiate
        private void initiateLists(IEnumerable<Node> nodeInput, IEnumerable<Edge> edgeInput, bool loadstate = false)
        {
            if (nodeData != null && nodeData.Count > 0)
                nodeData = new List<Node>();
            if (edgeData != null && edgeData.Count > 0)
                edgeData = new List<Edge>();

            foreach (Node obj in nodeInput.ToList())
            {
                nodeData.Add(new Node(obj));

                if (loadstate)
                    nodesLoadState.Add(new Node(obj));
            }

            foreach (Edge obj in edgeInput.ToList())
            {
                edgeData.Add(new Edge(obj));

                if (loadstate)
                    edgesLoadState.Add(new Edge(obj));
            }
        }
        #endregion
    }

    public class TrackPoint
    {
        public Point Point;
        public double Distance;
        public Edge Edge;
        public int EdgeLine;
        public int TotalLines;
        public int Id;

        public TrackPoint(Point Point, double Distance, Edge Edge, int EdgeLine, int TotalLines)
        {
            this.Point = Point;
            this.Distance = Distance;
            this.Edge = Edge;
            this.EdgeLine = EdgeLine;
            this.TotalLines = TotalLines;
        }

        public TrackPoint(TrackPoint trackpoint)
        {
            this.Point = new Point(trackpoint.Point.X, trackpoint.Point.Y);
            this.Distance = trackpoint.Distance;
            this.Edge = new Edge(trackpoint.Edge);
            this.EdgeLine = trackpoint.EdgeLine;
            this.TotalLines = trackpoint.TotalLines;
        }
    }
}
