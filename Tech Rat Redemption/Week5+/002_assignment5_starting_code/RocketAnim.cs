using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class RocketAnim : AnimationSprite
    {
        public RocketAnim(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("FinalAnimation.png", 6, 4)
        {
            SetFrame(0);
        }

        void Update()
        {
            SetCycle(0, 22);
            if (currentFrame < 21)
            {

                Animate(0.08f);
            }

        }
    }
}
