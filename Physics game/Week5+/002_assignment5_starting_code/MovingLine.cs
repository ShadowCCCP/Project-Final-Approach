using System;
using System.Drawing;
using System.Threading.Tasks;
using GXPEngine.Core;
using GXPEngine.Managers;
using GXPEngine.OpenGL;

namespace GXPEngine
{
    /// <summary>
    /// Implements an OpenGL line
    /// </summary>
    public class MovingLine : LineSegment
    {
        bool doOnce;

        MyGame myGame;
        float lineCurrentDistance;
        float boundaryLeft;
        float boundaryRight;

        float rotationSpeed = 5;
        ShootingBall sB;

        Vec2 velocity = new Vec2(0, 0);
        //float acceleration = 0.8f;
        //float friction = 0.1f;

        public MovingLine(Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1) : base(pStart, pEnd, pColor = 0xffffffff, pLineWidth = 1)
        {
            myGame = (MyGame)game;
            boundaryRight = myGame.boundaryRight;
            boundaryLeft = myGame.boundaryLeft;

            start = pStart;
            end = pEnd;

            color = pColor;
            lineWidth = pLineWidth;
        }

        private void Rotate(float degrees)
        {
            // Midpoint
            Vec2 midpoint = new Vec2((start.x + end.x) / 2, (start.y + end.y) / 2);

            // Set to rotation
            start = start.RotateAroundDegrees(end, degrees);
            end = end.RotateAroundDegrees(end, degrees);

            startBall.position = start;
            endBall.position = end;
        }

        private float GetRotation()
        {
            Vec2 line = start - end;
            return line.GetAngleDegrees();
        }

        private void Update()
        {
            Shoot();
            Movement();
            UpdateBalls();
        }

        private void Shoot()
        {
            if (Input.GetMouseButton(0) && !doOnce) //left click
            {
                sB = new ShootingBall(15, start + (start - end).Normalized() * 25, (start - end).Normalized() * 8, false);
                myGame._ballsOld.Add(sB);
                myGame.AddChild(sB);
                sB.shot = true;
                sB.rotation = GetRotation() + 90;
                doOnce = true;
            }

            if (sB != null)
            {
                if (sB.y > myGame.height + sB.radius + 50)
                {
                    doOnce = false;
                    sB.LateDestroy();
                }
            }
        }

        private void CheckBoundaries()
        {
            UpdateCurrentDistance();

            if(end.x > boundaryRight)
            {
                velocity = new Vec2(-velocity.x, 0);
                end.x = boundaryRight;
                start.x = end.x - lineCurrentDistance;
            }
            else if(start.x < boundaryLeft)
            {
                velocity = new Vec2(-velocity.x, 0);
                start.x = boundaryLeft;
                end.x = start.x + lineCurrentDistance;
            }
            else if(start.x > boundaryRight)
            {
                velocity = new Vec2(-velocity.x, 0);
                start.x = boundaryRight;
                end.x = start.x + lineCurrentDistance;
            }
            else if(end.x < boundaryLeft)
            {
                velocity = new Vec2(-velocity.x, 0);
                end.x = boundaryLeft;
                start.x = end.x - lineCurrentDistance;
            }
        }

        private void UpdateCurrentDistance()
        {
            lineCurrentDistance = end.x - start.x;
        }

        private void Movement()
        {
            CheckBoundaries();
            FollowTarget();
        }

        private void UpdateBalls()
        {
            // collision balls with radius 0
            startBall.position = start;
            endBall.position = end;

            // shooting ball
            //sB.UpdateNormal(start, end);
            //sB.UpdateRotation(GetRotation()+90);
        }

        private void FollowTarget()
        {
            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);

            Vec2 midpoint = new Vec2((start.x + end.x) / 2, (start.y + end.y) / 2);
            Vec2 cPos = mousePos - end;
            float targetRotation = cPos.GetAngleDegrees() - (GetRotation() );

            // 1. Get the difference between the two rotations
            // 2. + 540 to make it always positive
            // 3. % 360 to remove values bigger than 360
            // 4. - 180 to ensure that the value is between -180 and 180
            float distance = (targetRotation - rotation + 540) % 360 - 180;
            
            if(distance > rotationSpeed)
            {
                Rotate(rotationSpeed);
            }
            else if(distance < -rotationSpeed)
            {
                Rotate(-rotationSpeed);
            }
            else
            {
                Rotate(distance);
            }
        }


        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        override protected void RenderSelf(GLContext glContext)
        {
            if (game != null)
            {
                Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
            }
        }
    }
}

