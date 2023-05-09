using GXPEngine.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    class ShootingBall : Ball
    {
        private MyGame myGame;
        private Vec2 lineEnd;
        private Vec2 lineStart;
        private Vec2 line;
        private float lineRotation;

        public bool doOnce;
        public bool shot;
            
        public ShootingBall(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius, pPosition, pVelocity)
        {
            myGame = (MyGame)game;
        }

        private void Update()
        {
            if (!shot)
            {
                UpdateGizmo();
                UpdateBallOnLine();
                if (Input.GetKeyDown(Key.ENTER))
                {
                    shot = true;
                }
            }
            else if(shot && !doOnce)
            {
                velocity = line.Normal() * 8;
                doOnce = true;
            }

            CheckBoundaries();
        }

        public void UpdateRotation(float degrees)
        {
            lineRotation = degrees;
        }

        public void UpdateNormal(Vec2 start, Vec2 end)
        {
            lineEnd = end;
            lineStart = start;

            line = start - end;
        }

        private void UpdateBallOnLine()
        {
            //position = new Vec2(lineEnd.x + line.x / 2 + line.Normal().x * 20, lineEnd.y + line.y / 2 + line.Normal().y * 20);
            position = lineStart;
            rotation = lineRotation;
        }

        private void CheckBoundaries()
        {
            if(y > myGame.height + radius + 50)
            {
                shot = false;
                doOnce = false;
                velocity = new Vec2(0, 0);
            }
        }

        private void UpdateGizmo()
        {
            Gizmos.DrawArrow(x, y, line.Normal().x * 50, line.Normal().y * 50);
            // Pauls line
            Stroke(200, 0, 0);
            Line(0, height / 2, width, height / 2);
        }
    }
}
