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
    class AngledLine : AnimationSprite
    {
        int variant = 1;

        public Vec2 start;
        public Vec2 end;

        protected Ball startBall;
        protected Ball endBall;

        public new uint color = 0xffffffff;
        public uint lineWidth = 1;

        public AngledLine(string filename = "", int cols = 0, int rows = 0, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {
            MyGame myGame = (MyGame)game;

            if (obj != null)
            {
                variant = obj.GetIntProperty("variant", 1);
            }

            switch (variant)
            {
                case 1:
                    {
                        // Top right - Bottom left
                        start = new Vec2(0, 0);
                        end = new Vec2(256, 256);
                        break;
                    }
                case 2:
                    {
                        // Top left - Bottom right
                        start = new Vec2(256, 0);
                        end = new Vec2(0, 256);
                        break;
                    }
            }

            startBall = new Ball(50, new Vec2(start.x, start.y), moving: true);
            endBall = new Ball(50, new Vec2(end.x, end.y), moving: true);
            myGame._balls.Add(startBall);
            myGame._balls.Add(endBall);
        }

        override protected void RenderSelf(GLContext glContext)
        {
            if (game != null)
            {
                Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
            }
        }
    }
}
