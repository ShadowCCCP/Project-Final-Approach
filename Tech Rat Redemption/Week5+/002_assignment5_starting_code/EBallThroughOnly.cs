using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class EBallThroughOnly : Square
    {
        public EBallThroughOnly(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("BlueLazerS.png", 5, 1)
        {

        }

        void Update()
        {
            SetCycle(0, 5);
            Animate(0.05f);
        }
    }
}
