using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    class BallTarget : Ball
    {
        int live;

        public BallTarget(int pRadius, Vec2 pPosition, int pLive = 3) : base(pRadius, pPosition)
        {
            live = pLive;

            UpdateColor();
        }

        private void Update()
        {
            CollisionCheck();
        }

        private void CollisionCheck()
        {
            if(collided)
            {
                if (live > 1)
                {
                    live--;
                    UpdateColor();
                }
                else
                {
                    MyGame myGame = (MyGame)game;
                    Destroy();
                    myGame.RemoveBall(this);
                }
                collided = false;
            }
        }

        private void UpdateColor()
        {
            switch (live)
            {
                case 1:
                    {
                        Fill(255, 255, 255);
                        Stroke(0, 0, 0);
                        Ellipse(radius, radius, 2 * radius, 2 * radius);
                        break;
                    }
                case 2:
                    {
                        Fill(0, 255, 0);
                        Stroke(0, 0, 0);
                        Ellipse(radius, radius, 2 * radius, 2 * radius);
                        break;
                    }
                case 3:
                    {
                        Fill(0, 0, 255);
                        Stroke(0, 0, 0);
                        Ellipse(radius, radius, 2 * radius, 2 * radius);
                        break;
                    }
                case 4:
                    {
                        Fill(255, 0, 255);
                        Stroke(0, 0, 0);
                        Ellipse(radius, radius, 2 * radius, 2 * radius);
                        break;
                    }
                case 5:
                    {
                        Fill(255, 255, 0);
                        Stroke(0, 0, 0);
                        Ellipse(radius, radius, 2 * radius, 2 * radius);
                        break;
                    }
            }
        }
    }
}
