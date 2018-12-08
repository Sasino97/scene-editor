using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasinosoft.SampMapEditor.IPL
{
    public class IPLElement
    {
        public int ModelID { get; set; }
        public string ModelName { get; set; }
        public int Unknown1 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double RX { get; set; }
        public double RY { get; set; }
        public double RZ { get; set; }
        public double Scale { get; set; }
        public int Unknown2 { get; set; }
    }
}
