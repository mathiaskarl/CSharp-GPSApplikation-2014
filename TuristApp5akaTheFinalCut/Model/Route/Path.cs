using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace TuristApp5akaTheFinalCut.Model.Route
{
    public class Path
    {
        public List<Node> NodeList { get; set; }
        public double TotalDistance { get; set; }
    }
}
