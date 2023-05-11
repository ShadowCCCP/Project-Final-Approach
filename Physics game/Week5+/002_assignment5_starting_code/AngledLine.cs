using GXPEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class AngledLine : AnimationSprite
    {
        bool posFix;

        int variant = 1;

        public Vec2 start;
        public Vec2 end;

        protected BallNew startBall;
        protected BallNew endBall;

        public AngledLine(string filename = "", int cols = 0, int rows = 0, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {

            if (obj != null)
            {
                variant = obj.GetIntProperty("variant", 1);
            }
        }

        public void Update()
        {
            PositionFix();
        }

        private void PositionFix()
        {
            if (!posFix)
            {
                MyGame myGame = (MyGame)game;

                switch (variant)
                {
                    case 1:
                        {
                            // Top right - Bottom left
                            start = new Vec2(x + width / 2, y - height / 2);
                            end = new Vec2(x - width / 2, y + height / 2);
                            break;
                        }
                    case 2:
                        {
                            // Top left - Bottom right
                            start = new Vec2(x - width / 2, y - height / 2);
                            end = new Vec2(x + width / 2, y + height / 2);
                            break;
                        }
                }

                startBall = new BallNew(50, new Vec2(start.x, start.y), default, true, moving: true);
                endBall = new BallNew(50, new Vec2(end.x, end.y), default, true, moving: true);
                myGame.AddBall(startBall);
                myGame.AddBall(endBall);
                myGame.AddAngle(this);

                posFix = true;
            }
        }
    }
}
