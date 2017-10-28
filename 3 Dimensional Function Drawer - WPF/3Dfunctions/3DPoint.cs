using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;

namespace _3Dfunctions
{
    public class _3DPoint
    {
        public float x;
        public float y;
        public List<_3DPoint> proximity = new List<_3DPoint>();
        public float z;
        public static double inMiddle;
        public Color c;

        public _3DPoint()
        { }
        public _3DPoint(_3DPoint p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
            this.c = p.c;
        }
    }
}
