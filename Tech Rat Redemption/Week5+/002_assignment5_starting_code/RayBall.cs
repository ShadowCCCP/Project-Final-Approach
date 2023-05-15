using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class RayBall : BallNew
    {
        public int maxCollisions;
        public int collisionCount;
        public Vec2 first;
        public Vec2 second;

        int lastTime = Time.now;
        int startTimer = Time.now;

        public RayBall(Vec2 pPosition, Vec2 pVelocity, int pMaxCollisions) : base("Empty.png", 1, 1)
        {
            maxCollisions = pMaxCollisions;
            SetOrigin(width / 2, height / 2);
            position = pPosition;
            velocity = pVelocity;
            UpdatePosition();
            radius = 128;
        }

        protected override void BlockCollision()
        {
            int cTime = Time.now;
            if (cTime - startTimer > 45)
            {
                for (int i = 0; i < myGame.NumberOfSquares(); i++)
                {
                    if (myGame.GetSquare(i).isPlayer == false && !(myGame.GetSquare(i) is BulletThroughOnly))
                    {
                        Vec2 check = new Vec2(x, y);

                        // left edge
                        if (x < myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2)
                        {
                            check.x = myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2;
                        }
                        // right edge
                        else if (x > myGame.GetSquare(i).x + myGame.GetSquare(i).width / 2)
                        {
                            check.x = myGame.GetSquare(i).x + myGame.GetSquare(i).width / 2;
                        }

                        // top edge
                        if (y < myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2)
                        {
                            check.y = myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2;
                        }
                        // bottom edge
                        else if (y > myGame.GetSquare(i).y + myGame.GetSquare(i).height / 2)
                        {
                            check.y = myGame.GetSquare(i).y + myGame.GetSquare(i).height / 2;
                        }

                        Vec2 dist = new Vec2(position.x - check.x, position.y - check.y);
                        float distance = dist.Length();
                        if (distance <= radius)
                        {
                            collisionCount++;
                            // Check whether the calculated point is on a corner or not
                            if (check.Approximate(myGame.GetSquare(i).topLeftCorner) || check.Approximate(myGame.GetSquare(i).topRightCorner) ||
                                check.Approximate(myGame.GetSquare(i).bottomLeftCorner) || check.Approximate(myGame.GetSquare(i).bottomRightCorner))
                            {
                                Vec2 normal = (position - check).Normalized();
                                float overlap = radius - distance;
                                position += normal * overlap;
                                velocity.Reflect(normal);
                                test(position);
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
                                        test(position);
                                    }
                                    else
                                    {
                                        // bottom
                                        float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height / 2 + radius + 1);

                                        position.y = impactY;
                                        velocity.y *= -1;
                                        test(position);
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
                                        test(position);
                                    }
                                    else
                                    {
                                        // right
                                        float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                        position.x = impactX;
                                        velocity.x *= -1;
                                        test(position);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        protected override void LineSegmentTopOrBottom(bool bottom, Vec2 difference, int i)
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
                    collisionCount++;
                    test(poi);
                    collisions.Add(new CollisionInfo(bottomOrTop.Normal(), null, toi));
                }
            }
        }

        protected override void BallCollision()
        {

        }

        protected override void Update()
        {
            base.Update();

            int currentTime = Time.now;
            if (currentTime - lastTime > 3000)
            {
                Destroy();
            }
        }

        public void test(Vec2 pos)
        {
            switch (collisionCount)
            {
                case 1:
                    {
                        myGame.mfirst = pos;
                        break;
                    }
                case 2:
                    {
                        myGame.msecond = pos;
                        break;
                    }
            }
        }

        private void UpdatePosition()
        {
            x = position.x;
            y = position.y;
        }

        protected override void ResolveCollision(CollisionInfo col)
        {
            base.ResolveCollision(col);
            collisionCount++;
        }
    }
}