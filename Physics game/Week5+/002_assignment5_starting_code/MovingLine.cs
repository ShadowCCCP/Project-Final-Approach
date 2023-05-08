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
        float lineCurrentDistance;
        float boundaryLeft;
        float boundaryRight;

        bool followTarget;
        float rotationSpeed = 5;
        ShootingBall sB;

        Vec2 velocity = new Vec2(0, 0);
        float acceleration = 0.8f;
        float friction = 0.1f;

        public MovingLine(Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1) : base(pStart, pEnd, pColor = 0xffffffff, pLineWidth = 1)
        {
            MyGame myGame = (MyGame)game;
            boundaryRight = myGame.boundaryRight;
            boundaryLeft = myGame.boundaryLeft;

            // Shooting ball
            Vec2 line = end - start;
            sB = new ShootingBall(15,start /*new Vec2(end.x + line.x / 2 + line.Normal().x * 20, end.y + line.y / 2 + line.Normal().y * 20)*/, new Vec2(0, 0), false);
            myGame._balls.Add(sB);

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
            start = start.RotateAroundDegrees(midpoint, degrees);
            end = end.RotateAroundDegrees(midpoint, degrees);

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
            Movement();
            ApplyVelocity();
            UpdateBalls();
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
           // ToggleFollowTarget();
            MovingInput();
            CheckBoundaries();
            FollowTarget();
            /*if (followTarget)
            {
                FollowTarget();
            }
            else
            {
                RotationInput();
            }*/
        }

        /*private void ToggleFollowTarget()
        {
            if(Input.GetKeyDown(Key.ZERO))
            {
                followTarget = !followTarget;
            }
        }*/

        /*private void RotationInput()
        {
            if (Input.GetKey(Key.D))
            {
                Rotate(rotationSpeed);
            }
            else if (Input.GetKey(Key.A))
            {
                Rotate(-rotationSpeed);
            }
        }*/

        private void MovingInput()
        {
            if (Input.GetKey(Key.RIGHT))
            {
                velocity.x += acceleration;
            }
            else if (Input.GetKey(Key.LEFT))
            {
                velocity.x -= acceleration;
            }

            velocity *= (1 - friction);
        }

        private void UpdateBalls()
        {
            // collision balls with radius 0
            startBall.position = start;
            endBall.position = end;

            // shooting ball
            sB.UpdateNormal(start, end);
            sB.UpdateRotation(GetRotation());
        }

        private void ApplyVelocity()
        {
            start += velocity;
            end += velocity;
        }

        private void FollowTarget()
        {
            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);

            Vec2 midpoint = new Vec2((start.x + end.x) / 2, (start.y + end.y) / 2);
            Vec2 cPos = mousePos - midpoint;
            float targetRotation = cPos.GetAngleDegrees() - (GetRotation());

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

