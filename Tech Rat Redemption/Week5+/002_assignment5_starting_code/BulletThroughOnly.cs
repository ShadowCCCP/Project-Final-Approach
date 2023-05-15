using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class BulletThroughOnly : Square
    {
        public BulletThroughOnly(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows)
        {

        }
    }
}
