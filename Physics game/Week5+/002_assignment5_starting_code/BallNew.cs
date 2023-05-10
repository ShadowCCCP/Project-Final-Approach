using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class BallNew : AnimationSprite
    {
        public bool lineBall;
        public bool IsBullet = false;

        bool timer = false;

        public Vec2 velocity;
        public Vec2 position;

        public float _density = 1;

        private float bouncyPlatformVelocity = 1.5f;
        private Vec2 oldVelocity;

        public float Mass
        {
            get
            {
                return radius * radius * _density;
            }
        }

        public readonly int radius;
        public readonly bool moving;

        bool posFix;
        MyGame myGame;
        Vec2 _oldPosition;
        Arrow _velocityIndicator;

        protected List<CollisionInfo> collisions;

        public BallNew(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {
            myGame = (MyGame)game;
            myGame.AddBall(this);

            radius = 128;

            velocity = new Vec2(-10, -10);
        }

        public BallNew(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool pLineBall = false, bool moving = true) : base("DebugBall.png", 1, 1, -1, false, false)
        {
            myGame = (MyGame)game;
            lineBall = true;
            myGame.AddBall(this);
        }

        private CollisionInfo FindEarliestCollision()
        {
            collisions = new List<CollisionInfo>();

            BallCollision();
            LineSegmentCollision();
            BlockCollision();

            return EarliestCollision();

        }

        public void Update()
        {
            PositionFix();
            _oldPosition = position;
            position += velocity;

            UpdateScreenPosition();

            CollisionInfo firstCollision = FindEarliestCollision();
            if (firstCollision != null)
            {
                ResolveCollision(firstCollision);
            }

            timerBouncy();
            animationDestroy();
        }

        bool ballDestroyed = false; //this should become true when the energy orb breaks
        int counter = 0;
        int frame = 0;
        void animationDestroy()
        {
            if (ballDestroyed )
            {
                counter++;

                if (counter > 10) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 8)
                    {
                        frame = 0;
                        myGame.RemoveBallNew(this);
                    }
                }
            }
            SetFrame(frame);
        }

        private void ResolveCollision(CollisionInfo col)
        {
            position = _oldPosition + col.timeOfImpact * velocity;
            if (col.other != null)
            {
                BallNew other = col.other as BallNew;
                if (other.lineBall)
                {
                    velocity.Reflect(col.normal);
                }
                else
                {
                    velocity.Reflect(col.normal, col.velCOM);
                    other.velocity.Reflect(col.normal, col.velCOM);
                }
            }
            else
            {
                velocity.Reflect(col.normal);
            }
        }

        private void PositionFix()
        {
            if(!posFix)
            {
                position.x = x;
                position.y = y;

                posFix = true;
            }
        }

        private void UpdateScreenPosition()
        {
            x = position.x;
            y = position.y;
        }

        private void BallCollision()
        {
            for (int i = 0; i < myGame.NumberOfBalls(); i++)
            {
                BallNew mover = myGame.GetBall(i);
                if (mover != this)
                {
                    Vec2 u = _oldPosition - mover.position;
                    float a = velocity.Length() * velocity.Length();
                    float b = 2 * u.Dot(velocity);
                    float c = u.Length() * u.Length() - (radius + mover.radius) * (radius + mover.radius);

                    if (c < 0)
                    {
                        if (b < 0)
                        {
                            Vec2 velCOM = (Mass * velocity + mover.Mass * mover.velocity) / (Mass + mover.Mass);
                            collisions.Add(new CollisionInfo(u.Normalized(), mover, 0, velCOM));
                        }
                    }

                    if (a > 0.00001f)
                    {
                        float D = b * b - 4 * a * c;

                        if (D > 0)
                        {
                            float toi = (-b - Mathf.Sqrt(D)) / (2 * a);
                            if (0 <= toi && toi < 1)
                            {
                                Vec2 velCOM = (Mass * velocity + mover.Mass * mover.velocity) / (Mass + mover.Mass);
                                collisions.Add(new CollisionInfo(u.Normalized(), mover, toi, velCOM));
                            }
                        }
                    }
                }
            }
        }

        private CollisionInfo EarliestCollision()
        {
            foreach (CollisionInfo col in collisions)
            {
                bool earliest = true;
                foreach (CollisionInfo other in collisions)
                {
                    if (col != other)
                    {
                        if (col.timeOfImpact > other.timeOfImpact)
                        {
                            earliest = false;
                        }
                    }
                }
                if (earliest)
                {
                    return col;
                }
            }

            return null;
        }

        private void LineSegmentCollision()
        {
            for (int i = 0; i < myGame.NumberOfAngles(); i++)
            {
                Vec2 difference = position - myGame.GetAngle(i).end;

                // Bottom
                LineSegmentTopOrBottom(true, difference, i);
                // Top
                LineSegmentTopOrBottom(false, difference, i);
            }
        }

        private void LineSegmentTopOrBottom(bool bottom, Vec2 difference, int i)
        {
            Vec2 bottomOrTop;

            if (bottom)
            {
                bottomOrTop = myGame.GetAngle(i).end - myGame.GetAngle(i).start;
            }
            else
            {
                bottomOrTop = myGame.GetAngle(i).start - myGame.GetAngle(i).end;
            }

            float toi = 2;
            float a = difference.Dot(bottomOrTop.Normal()) - radius;
            float b = (_oldPosition - position).Dot(bottomOrTop.Normal());

            if (b > 0 && a >= 0)
            {
                toi = a / b;
            }
            else if (b > 0 && a >= -radius)
            {
                toi = 0;
            }

            if (toi >= 0 && toi <= 1)
            {
                Vec2 poi = _oldPosition + velocity * toi;
                Vec2 poiDifference;
                if (bottom)
                {
                    poiDifference = poi - myGame.GetAngle(i).start;
                }
                else
                {
                    poiDifference = poi - myGame.GetAngle(i).end;
                }
                float lineDistance = poiDifference.Dot(bottomOrTop.Normalized());
                if (lineDistance >= 0 && lineDistance <= bottomOrTop.Length())
                {
                    collisions.Add(new CollisionInfo(bottomOrTop.Normal(), null, toi));
                }
            }
        }

        private void BlockCollision()
        {
            for (int i = 0; i < myGame.NumberOfSquares(); i++)
            {
                if((myGame.GetSquare(i) is EBallThroughOnly && !IsBullet) || (myGame.GetSquare(i) is BulletThroughOnly && IsBullet))
                {
                    break;
                }

                Vec2 check = new Vec2(x, y);

                // left edge
                if (x < myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2)
                {
                    check.x = myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2;
                }
                // right edge
                else if (x > myGame.GetSquare(i).x + myGame.GetSquare(i).width/2)
                {
                    check.x = myGame.GetSquare(i).x + myGame.GetSquare(i).width / 2;
                }

                // top edge
                if (y < myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2)
                {
                    check.y = myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2;
                }
                // bottom edge
                else if (y > myGame.GetSquare(i).y + myGame.GetSquare(i).height/2)
                {
                    check.y = myGame.GetSquare(i).y + myGame.GetSquare(i).height / 2;
                }

                Vec2 dist = new Vec2(position.x - check.x, position.y - check.y);
                float distance = dist.Length();
                if (distance <= radius)
                {
                    // Check whether the calculated point is on a corner or not
                    if (check.Approximate(myGame.GetSquare(i).topLeftCorner) || check.Approximate(myGame.GetSquare(i).topRightCorner) ||
                        check.Approximate(myGame.GetSquare(i).bottomLeftCorner) || check.Approximate(myGame.GetSquare(i).bottomRightCorner))
                    {
                        Vec2 normal = (position - check).Normalized();
                        float overlap = radius - distance;
                        position += normal * overlap;
                        velocity.Reflect(normal);
                    }
                    else
                    {
                        if (Math.Abs(dist.x) < Math.Abs(dist.y))
                        {
                            if (y < myGame.GetSquare(i).y)
                            {
                                //top
                                float impactY = myGame.GetSquare(i).y - (myGame.GetSquare(i).height / 2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -1;
                            }
                            else
                            {
                                // bottom
                                float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height/2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -1;
                            }
                        }
                        else
                        {
                            if (x < myGame.GetSquare(i).x)
                            {
                                // left
                                float impactX = myGame.GetSquare(i).x - (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -1;
                            }
                            else
                            {
                                // right
                                float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -1;
                            }
                        }
                        objectchecker(i);
                        

                    }
                }
            }
        }

        void objectchecker(int i)
        {
            if (myGame.GetSquare(i) is BouncyPlatform)
            {
                myGame.BouncyPlatformAnim = true;
                oldVelocity = velocity;
                velocity = velocity * bouncyPlatformVelocity;
                timer = true;
            }
            if (myGame.GetSquare(i) is ButtonPlatform)
            {
                myGame.ButtonPressed = true;
                Console.WriteLine("Button pressed");
            }
            else
            {
                myGame.ButtonPressed = false;
                Console.WriteLine("Button off");
            }
            if (myGame.GetSquare(i) is EnergyReceptor)
            {
                //NEXT LEVEL
                Console.WriteLine("Next level");
            }
        }

        int time;
        bool timerStarted = false;

        void timerBouncy()
        {
             if (timerStarted == false)
             {
                 time = Time.time;
             }
             if (timer == true) //start timer
             {
                  timerStarted = true;

                  if (Time.time - time > 1500) //timer ends
                  {
                        velocity = oldVelocity;
                        timer = false;
                        timerStarted = false;
                  }
             }
            
        }
    }
}