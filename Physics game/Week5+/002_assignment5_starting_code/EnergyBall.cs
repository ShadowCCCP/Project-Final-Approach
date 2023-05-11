using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class EnergyBall : BallNew
    {
        float gravity;

        public EnergyBall(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows)
        {
            if (obj != null)
            {
                gravity = obj.GetFloatProperty("gravity", 0.5f);
            }

            acceleration = new Vec2(0, gravity);
        }
    }
}
